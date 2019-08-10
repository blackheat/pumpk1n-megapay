using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Exceptions.Accounts;
using pumpk1n_backend.Exceptions.Others;
using pumpk1n_backend.Exceptions.Tokens;
using pumpk1n_backend.Helpers.Tokens;
using pumpk1n_backend.Models;
using pumpk1n_backend.Models.DatabaseContexts;
using pumpk1n_backend.Models.Entities.Accounts;
using pumpk1n_backend.Models.Entities.Tokens;
using pumpk1n_backend.Models.ReturnModels.Tokens;
using pumpk1n_backend.Models.TransferModels.Tokens;

namespace pumpk1n_backend.Services.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly ITokenHelper _tokenHelper;

        private const float TokenRate = 1;

        public TokenService(DatabaseContext context, IMapper mapper, ITokenHelper tokenHelper)
        {
            _context = context;
            _mapper = mapper;
            _tokenHelper = tokenHelper;
        }

        public async Task<UserTokenBalanceModel> GetUserBalance(long userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new UserDoesNotExistException();
            var userTokenBalanceModel = _mapper.Map<User, UserTokenBalanceModel>(user);
            return userTokenBalanceModel;
        }

        public async Task<UserTokenTransactionModel> CreateTokenPurchaseRequest(long userId,
            TokenTransactionInsertModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var pendingTokenPurchaseRequestCount = await _context.UserTokenTransactions.Where(utt =>
                        utt.ConfirmedDate < utt.AddedDate && utt.CancelledDate < utt.AddedDate).LongCountAsync();
                    if (pendingTokenPurchaseRequestCount > 0)
                        throw new PendingTokenTransactionExistsException();
                    
                    var currentDate = DateTime.UtcNow;
                    var tokenTransaction = _mapper.Map<TokenTransactionInsertModel, UserTokenTransaction>(model);
                    tokenTransaction.CustomerId = userId;
                    tokenTransaction.AddedDate = currentDate;
                    tokenTransaction.TransactionType = TokenTransactionType.Add;

                    _context.UserTokenTransactions.Add(tokenTransaction);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return _mapper.Map<UserTokenTransaction, UserTokenTransactionModel>(tokenTransaction);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<UserTokenTransactionModel> CancelTokenPurchaseRequest(long requestId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var tokenPurchaseRequest =
                        await _context.UserTokenTransactions.FirstOrDefaultAsync(utt => utt.Id == requestId);
                    tokenPurchaseRequest.CancelledDate = DateTime.UtcNow;
                    _context.UserTokenTransactions.Update(tokenPurchaseRequest);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return _mapper.Map<UserTokenTransaction, UserTokenTransactionModel>(tokenPurchaseRequest);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<UserTokenTransactionModel> GetTokenPurchaseRequest(long txId)
        {
            var tokenTransaction = await _context.UserTokenTransactions.Include(tt => tt.TokenBillings)
                .FirstOrDefaultAsync(tt => tt.Id == txId && tt.TransactionType == TokenTransactionType.Add);
            var tokenTransactionModel = _mapper.Map<UserTokenTransaction, UserTokenTransactionModel>(tokenTransaction);
            return tokenTransactionModel;
        }
        
        public async Task<UserTokenTransactionModel> GetUserTokenPurchaseRequest(long userId, long txId)
        {
            var tokenTransaction = await _context.UserTokenTransactions.Include(tt => tt.TokenBillings)
                .FirstOrDefaultAsync(tt => tt.Id == txId && tt.TransactionType == TokenTransactionType.Add && tt.CustomerId == userId);
            var tokenTransactionModel = _mapper.Map<UserTokenTransaction, UserTokenTransactionModel>(tokenTransaction);
            return tokenTransactionModel;
        }

        public async Task<CustomList<UserTokenTransactionModel>> GetUserTokenPurchaseRequests(long userId, int count = 10, int page = 1)
        {
            if (page <= 0 || count <= 0)
                throw new InvalidPaginationDataException();
            
            var startAt = (page - 1) * count;
            var tokenPurchaseRequests = await _context.UserTokenTransactions
                .Where(tt => tt.TransactionType == TokenTransactionType.Add && tt.CustomerId == userId)
                .OrderByDescending(tt=> tt.AddedDate)
                .Skip(startAt)
                .Take(count).ToListAsync();
            
            var totalCount = await _context.UserTokenTransactions.CountAsync();
            var totalPages = totalCount / count + (totalCount / count > 0 ? totalCount % count : 1);

            var supplierReturnModels =
                _mapper.Map<List<UserTokenTransaction>, CustomList<UserTokenTransactionModel>>(tokenPurchaseRequests);
            supplierReturnModels.CurrentPage = page;
            supplierReturnModels.TotalPages = totalPages;
            supplierReturnModels.TotalItems = totalCount;
            supplierReturnModels.IsListPartial = true;

            return supplierReturnModels;
        }

        public async Task<CoinGateBillModel> CreateUserBilling(long userId, long txId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var tokenTransaction = await _context.UserTokenTransactions
                        .Include(tt => tt.TokenBillings)
                        .FirstOrDefaultAsync(tt => tt.Id == txId && tt.CustomerId == userId);

                    if (tokenTransaction.TokenBillings.Where(tb => tb.InvoiceFullyPaid).LongCount() > 0 &&
                        tokenTransaction.ConfirmedDate >= tokenTransaction.AddedDate)
                        throw new TokenTransactionAlreadyConfirmedException();
                    
                    if (tokenTransaction.TokenBillings.Where(tb => tb.CancelledDate < tb.CreatedDate && 
                                                                   !tb.InvoiceFullyPaid).LongCount() > 0)
                        throw new PendingTokenBillingExistsException();

                    if (tokenTransaction.CancelledDate >= tokenTransaction.AddedDate)
                        throw new TokenTransactionAlreadyCancelledException();

                    var coinGateInvoiceModel = new CoinGateInvoiceTransferModel
                    {
                        OrderId = tokenTransaction.Id.ToString(),
                        PriceAmount = tokenTransaction.Amount * TokenRate,
                        PriceCurrency = "USD",
                        ReceiveCurrency = "BTC",
                        Title = "Pumpk1n Token Purchase Invoice",
                        Description = $"Receipt for purchase of {tokenTransaction.Amount} PKN"
                    };

                    var coinGateInvoiceReturnModel = await _tokenHelper.GenerateInvoice(coinGateInvoiceModel);
                    var tokenBilling =
                        _mapper.Map<CoinGateInvoiceReturnModel, TokenBilling>(coinGateInvoiceReturnModel);
                    tokenBilling.CreatedDate = DateTime.UtcNow;
                    tokenBilling.UserTokenTransactionId = tokenTransaction.Id;
                    tokenBilling.Name = coinGateInvoiceModel.Title;
                    tokenBilling.Description = coinGateInvoiceModel.Description;

                    _context.TokenBillings.Add(tokenBilling);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    var tokenBillingModel = _mapper.Map<TokenBilling, CoinGateBillModel>(tokenBilling);
                    return tokenBillingModel;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task ProcessCoinGateHook(CoinGateHookTransferModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var tokenBilling = await _context.TokenBillings.Include(tb => tb.UserTokenTransaction)
                        .ThenInclude(utt => utt.Customer)
                        .FirstOrDefaultAsync(tb =>
                            tb.UserTokenTransactionId == model.OrderId && tb.GatewayInvoiceSecret.Equals(model.Token,
                                StringComparison.InvariantCultureIgnoreCase));
                    
                    if (tokenBilling == null)
                        throw new TokenBillingNotFoundException();

                    if (tokenBilling.UserTokenTransaction.ConfirmedDate >=
                        tokenBilling.UserTokenTransaction.AddedDate && tokenBilling.UserTokenTransaction.CancelledDate <
                        tokenBilling.UserTokenTransaction.AddedDate)
                        return;
                        
                    tokenBilling.GatewayStatus = model.Status;
                    tokenBilling.ReceivedAmount = model.ReceiveAmount;

                    if (model.Status.Equals("paid", StringComparison.InvariantCultureIgnoreCase))
                    {
                        tokenBilling.InvoiceFullyPaid = true;
                        tokenBilling.UserTokenTransaction.ConfirmedDate = DateTime.UtcNow;
                        _context.UserTokenTransactions.Update(tokenBilling.UserTokenTransaction);

                        tokenBilling.UserTokenTransaction.Customer.Balance += tokenBilling.UserTokenTransaction.Amount;
                        _context.Users.Update(tokenBilling.UserTokenTransaction.Customer);
                    }

                    _context.TokenBillings.Update(tokenBilling);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<UserTokenTransactionModel> CreateTokenTransaction(long userId,
            TokenTransactionInsertModel model, TokenTransactionType transactionType)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var tokenTransaction = _mapper.Map<TokenTransactionInsertModel, UserTokenTransaction>(model);
                    tokenTransaction.CustomerId = userId;
                    tokenTransaction.AddedDate = tokenTransaction.ConfirmedDate = DateTime.UtcNow;
                    tokenTransaction.TransactionType = transactionType;

                    _context.UserTokenTransactions.Add(tokenTransaction);

                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    if (user == null)
                        throw new UserDoesNotExistException();
                    if (transactionType == TokenTransactionType.Add)
                        user.Balance += model.Amount;
                    else
                        user.Balance -= model.Amount;

                    _context.Users.Update(user);

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return _mapper.Map<UserTokenTransaction, UserTokenTransactionModel>(tokenTransaction);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}