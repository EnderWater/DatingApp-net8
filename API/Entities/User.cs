﻿namespace API.Entities;

/// <summary>
/// Summary description for AppUser
/// </summary>
public class User
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
}