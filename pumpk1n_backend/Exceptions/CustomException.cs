using System;
using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions
{
    public class CustomException : Exception
    {
        public ErrorCode Code { get; set; }
    }
}
