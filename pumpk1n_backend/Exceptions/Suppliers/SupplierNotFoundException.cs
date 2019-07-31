using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Suppliers
{
    public class SupplierNotFoundException : CustomException
    {
        public SupplierNotFoundException()
        {
            Code = ErrorCode.SupplierNotFoundException;
        }
    }
}