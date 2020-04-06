using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Models.DatabaseContexts;

namespace pumpk1n_backend.Services.InternalStuffs
{
    public class InternalService : IInternalService
    {
        private readonly DatabaseContext _context;

        public InternalService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task ChangeAccountRole(long accountId, UserType roleId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == accountId);

                    user.UserType = roleId;
                    _context.Users.Update(user);
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
    }
}