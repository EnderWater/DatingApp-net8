using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController(DataContext context) : BaseApiController
{
    [HttpPost("register")] //account/register
    public async Task<ActionResult<AppUser>> Register(string username, string password)
    {
        //var newAppUser = new AppUser{Id = id, UserName = username, PasswordHash=[], PasswordSalt=[]};

        using var hmac = new HMACSHA512();

        var user = new AppUser{
            UserName = username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
            PasswordSalt = hmac.Key
        };
        
        // context.User.Add(newAppUser);
        await context.SaveChangesAsync();

        return Ok();
    }
}
