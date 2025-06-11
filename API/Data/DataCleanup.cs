using API.Controllers;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataController(DataContext context) : BaseApiController
{
    [HttpPost("camelcase")] // POST to /data/camelcase
    public void CamelCaseName()
    {
        string updateQuery = @"
        UPDATE ""User""
        SET UserName = UPPER(SUBSTR(UserName, 1, 1)) || SUBSTR(UserName, 2)
        ";
        context.Database.ExecuteSqlRaw(updateQuery);
        return;
    }
}
 