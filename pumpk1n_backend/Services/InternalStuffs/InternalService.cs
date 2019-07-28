using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Models.DatabaseContexts;

namespace pumpk1n_backend.Services.InternalStuffs
{
    public class InternalService : IInternalService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public InternalService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task ChangeAccountRole(long accountId, UserType roleId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == accountId);
            user.UserType = roleId;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}