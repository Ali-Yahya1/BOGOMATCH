﻿using System;
using System.ComponentModel.DataAnnotations;


public class Users
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
    public string Role { get; set; }
    public string Email { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenxEpiryTime { get; set; }
    public string ResetPasswordToken { get; set; }
    public DateTime ResetPasswordExpiry { get; set; }
}
