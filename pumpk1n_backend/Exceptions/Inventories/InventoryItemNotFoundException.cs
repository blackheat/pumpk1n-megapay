using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Inventories
{
    public class InventoryItemNotFoundException : CustomException
    {
        public InventoryItemNotFoundException()
        {
            Code = ErrorCode.InventoryItemNotFoundException;
        }
    }
}