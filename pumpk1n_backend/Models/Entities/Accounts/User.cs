using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pumpk1n_backend.Models.Entities.Accounts
{
    [Table("User")]
    public class User
    {
        [Key]
        public Int64 Id { get; set; }
        public String GoogleOAuthProfileId { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public DateTime? PhoneNumberConfirmedDate { get; set; }
        public String FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Nonce { get; set; }
        public String HashedPassword { get; set; }
        
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Boolean UserProfileCompleted { get; set; }
        
        public String UserActivationCode { get; set; }
        public DateTime? UserActivationCodeIssuedDate { get; set; }
        public DateTime? ActivatedDate { get; set; }
        
        public String PasswordResetCode { get; set; }
        public DateTime? PasswordResetCodeIssuedDate { get; set; }
        public DateTime? PasswordResetCodeUsedDate { get; set; }
    }
}