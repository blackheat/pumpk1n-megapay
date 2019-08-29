using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Exceptions.Chain;
using pumpk1n_backend.Models.ChainTransferModels.Accounts;
using pumpk1n_backend.Models.DatabaseContexts;
using pumpk1n_backend.Services.Accounts;

namespace pumpk1n_backend.Services.InternalStuffs
{
    public class InternalService : IInternalService
    {
        private readonly DatabaseContext _context;
        private readonly IAccountChainService _accountChainService;

        public InternalService(DatabaseContext context, IAccountChainService accountChainService)
        {
            _context = context;
            _accountChainService = accountChainService;
        }

        public async Task ChangeAccountRole(long accountId, UserType roleId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == accountId);
                    
                    var accountChainInfo = await _accountChainService.GetAccount(user.Id);
                    if (accountChainInfo == null)
                        throw new DataNotFoundInChainException();
                    if (long.Parse(accountChainInfo.Hash) != user.ComputeHash())
                        throw new ChainCodeDataNotInSyncException();
                    
                    user.UserType = roleId;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    
                    var chainModel = new ChainAccountTransferModel
                    {
                        Id = user.Id.ToString(),
                        CreatedDate = user.RegisteredDate.ToBinary().ToString(),
                        Hash = user.ComputeHash().ToString()
                    };
                    await _accountChainService.AddAccount(chainModel);
                    transaction.Commit();
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