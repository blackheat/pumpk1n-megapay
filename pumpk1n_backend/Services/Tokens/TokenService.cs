using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Exceptions.Accounts;
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

        public TokenService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserTokenBalanceModel> GetUserBalance(long userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new UserDoesNotExistException();
            var userTokenBalanceModel = _mapper.Map<User, UserTokenBalanceModel>(user);
            return userTokenBalanceModel;
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