using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Inventories
{
    public class InventoryItemAlreadyExportedException : CustomException
    {
        public InventoryItemAlreadyExportedException()
        {
            Code = ErrorCode.InventoryItemAlreadyExportedException;
        }
    }
}