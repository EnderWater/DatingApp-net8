using System.ComponentModel.DataAnnotations;
using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


public class UsersController(DataContext context) : BaseApiController
{
    [AllowAnonymous]
    [HttpGet] // GET to /api/users
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await context.User.ToListAsync();

        return Ok(users);
    }

    [Authorize]
    [HttpGet("{id:int}")] // GET to /api/users/USERID (1,2,3,12, etc)
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await context.User.FindAsync(id);

        if (user == null) return NotFound();
        
        return Ok(user);
    }

    [HttpPost] // POST to /api/users/
    public async Task<ActionResult<AppUser>> AddUser(int id, string username, string password, string passwordSalt)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] passwordSaltBytes = Encoding.UTF8.GetBytes(passwordSalt);

        var newAppUser = new AppUser{Id = id, UserName = username, PasswordHash= passwordBytes, PasswordSalt= passwordSaltBytes };
        
        context.User.Add(newAppUser);
        await context.SaveChangesAsync();

        return Ok();
    }
}