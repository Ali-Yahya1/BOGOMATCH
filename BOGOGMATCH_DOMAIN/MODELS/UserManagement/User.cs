﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOGOGMATCH_DOMAIN.MODELS.UserManagement
{
    [Table("BOGO_USERS")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public bool IsGoogleAuthenticated { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? refreshToken { get; set; }
        public DateTime? refreshTokenxEpiryTime { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordExpiry { get; set; }
    }
}
