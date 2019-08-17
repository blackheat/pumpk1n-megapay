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
        UserDoesNotExistException = 2003,
        
        // Products
        ProductNotFoundException = 3001,
        
        // Suppliers
        SupplierNotFoundException = 4001,
        
        // Inventories
        InventoryItemAlreadyExportedException = 5001,
        InventoryItemNotFoundException = 5002,
        InventoryItemWithSameUniqueIdentifierExistsException = 5003,
        
        // Tokens
        TokenBillingNotFoundException = 6001,
        TokenTransactionAlreadyCancelledException = 6002,
        TokenTransactionAlreadyConfirmedException = 6003,
        TokenTransactionNotFoundException = 6004,
        PendingTokenTransactionExistsException = 6005,
        PendingTokenBillingExistsException = 6006,
        InsufficientBalanceException = 6007,
        
        // Orders
        CartAlreadyExistsException = 7001,
        CartNotFoundException = 7002,
        OrderNotFoundException = 7003,
        CartItemNotFoundException = 7004,
        OrderAlreadyCancelledOrCheckedoutException = 7005,
    }
}
