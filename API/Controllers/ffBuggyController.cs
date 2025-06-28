using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Controllers;

namespace API;

class BuggyController(DataContext context) : BaseApiController
{
    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetAuth()
    {
        return "secret text";
    }

    [HttpGet("not-found")]
    public ActionResult<User> GetNotFound()
    {
        var thing = context.User.Find(-1);

        if (thing == null) return NotFound();
        else return thing;
    }

    [HttpGet("server-error")]
    public ActionResult<User> GetServerError()
    {
        var thing = context.User.Find(-1) ?? throw new Exception("A bad thing has happened.");
        return thing;
    }

    [HttpGet("bad-request")]
    public ActionResult<User> GetBadRequest()
    {
        return BadRequest("This was not a good request.");
    }

}