using System;

namespace pumpk1n_backend.Settings
{
    public class JwtSettings
    {
        public String Secret { get; set; }
        public Int32 Expiry { get; set; }
    }
}