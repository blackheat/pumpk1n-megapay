namespace pumpk1n_backend.Enumerations
{
    public enum ErrorCode
    {
        // Unhandled Exception
        UnknownException = 1001,

        // NotModified Exception
        NotModifiedException = 1002,
        
        // Pagination Data Exception
        InvalidPaginationDataException = 1003,
        
        // Accounts
        InvalidCredentialsException = 2000,
        UserNotActivatedException = 2001,
        UserAlreadyExistsException = 2002,
        
        // Products
        ProductNotFoundException = 3001,
        
        // Suppliers
        SupplierNotFoundException = 4001,
        
        // Inventories
        InventoryItemAlreadyExportedException = 5001,
        InventoryItemNotFoundException = 5002
    }
}
