using AutoMapper;
using pumpk1n_backend.Models.DatabaseContexts;

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
    }
}