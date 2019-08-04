using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Inventories
{
    public class InventoryItemWithSameUniqueIdentifierExistsException : CustomException
    {
        public InventoryItemWithSameUniqueIdentifierExistsException()
        {
            Code = ErrorCode.InventoryItemWithSameUniqueIdentifierExistsException;
        }
    }
}