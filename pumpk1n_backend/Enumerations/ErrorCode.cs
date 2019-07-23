namespace pumpk1n_backend.Enumerations
{
    public enum ErrorCode
    {
        // Unhandled Exception
        UnknownException = 1001,

        // NotModified Exception
        NotModifiedException = 1002,
        
        // Accounts
        InvalidCredentialsException = 2000,
        UserNotActivatedException = 2001,
        UserAlreadyExistsException = 2002,
        
        // Products
        ProductNotFoundException = 3001
    }
}
