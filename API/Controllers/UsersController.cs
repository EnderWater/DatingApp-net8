using System.ComponentModel.DataAnnotations;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] // /api/users
public class UsersController(DataContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await context.User.ToListAsync();

        return Ok(users);
    }


    [HttpGet("{id:int}")] // /api/users/id
    public async Task<ActionResult<AppUser>> GetUsers(int id)
    {
        var user = await context.User.FindAsync(id);

        if (user == null) return NotFound();
        
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<AppUser>> AddUser(int id, string username)
    {
        var newAppUser = new AppUser{Id = id, UserName = username};
        
        context.User.Add(newAppUser);
        await context.SaveChangesAsync();

        return Ok();
    }
}