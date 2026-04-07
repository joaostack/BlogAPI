using back_end.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back_end.ApiEndpoints;

public static class UsersEndpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/users").WithTags("API Users");

        group.MapPost(
            "/authentication",
            [AllowAnonymous]
        async ([FromBody] User userData, DataContext context) =>
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Name == userData.Name);
                if (user is null)
                    return Results.Unauthorized();

                // comparar senha especificada com a senha hasheada
                bool verifyPassword = BCrypt.Net.BCrypt.Verify(userData.Password, user.Password);
                if (!verifyPassword)
                    return Results.Unauthorized();

                var token = JwtService.CreateToken(user);
                return Results.Ok(new { Token = token });
            }
        );

        group.MapPost(
            "/newAccount",
            [AllowAnonymous]
        async ([FromBody] User userData, UsersService usersService) =>
            {
                var result = await usersService.NewAccount(userData);
                return Results.Ok(new { Message = result });
            }
        );

        group
            .MapDelete(
                "/deleteUser/{guid}",
                async (Guid guid, UsersService usersService) =>
                {
                    var result = await usersService.DeleteAccount(guid);
                    return Results.Ok(new { Message = result });
                }
            )
            .RequireAuthorization();

        group
            .MapPut(
                "/updateUser/{guid}",
                async ([FromBody] User newUser, Guid guid, UsersService usersService) =>
                {
                    var result = await usersService.UpdateAccount(guid, newUser);
                    return Results.Ok(new { Message = result });
                }
            )
            .RequireAuthorization();
    }
}
