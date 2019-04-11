using AutoMapper;
using pumpk1n_backend.Models.DatabaseContexts;

namespace pumpk1n_backend.Services.Accounts
{
    public class InternalAccountService : IInternalAccountService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public InternalAccountService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}