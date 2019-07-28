using System.Threading.Tasks;
using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Services.InternalStuffs
{
    public interface IInternalService
    {
        Task ChangeAccountRole(long accountId, UserType roleId);
    }
}