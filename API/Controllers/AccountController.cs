using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")] // POST to /account/register
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
    {
        //var newAppUser = new AppUser{Id = id, UserName = username, PasswordHash=[], PasswordSalt=[]};

        if (await UserExists(registerDTO.UserName))
        {
            return BadRequest("Username is already taken");
        }

        using var hmac = new HMACSHA512();

        var user = new User
        {
            UserName = registerDTO.UserName,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
            PasswordSalt = hmac.Key
        };

        context.User.Add(user);
        await context.SaveChangesAsync();

        return new UserDTO
        {
            UserName = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }

    [AllowAnonymous]
    [HttpPost("login")] // POST to /account/login
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
    {
        // This line searches the database and returns the ONLY first entry that matches the condition.
        // If a user is found, it will be returned. If not, null is returned.
        //var user = await context.User.SingleOrDefaultAsync(x => x.UserName == loginDTO.Username);

        // For now, we will stick with using FirstOrDefaultAsync to get the user from the DB
        var user = await context.User.FirstOrDefaultAsync(x =>
            x.UserName.ToLower() == loginDTO.UserName.ToLower());

        if (user == null)
        {
            return Unauthorized("Invalid Username");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid Password");
            }
        }

        return new UserDTO
        {
            UserName = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }

    private async Task<bool> UserExists(string username)
    {
        return await context.User.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }

    [HttpGet("check-username")]
    public async Task<bool> CheckUserName(string username)
    {
        return await UserExists(username);
    }
}
