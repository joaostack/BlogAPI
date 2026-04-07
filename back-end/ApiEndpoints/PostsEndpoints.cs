using back_end.Models;
using Microsoft.AspNetCore.Mvc;

namespace back_end.ApiEndpoints;

public static class PostsEndpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/posts").WithTags("API Posts");

        group.MapGet(
            "/getPosts",
            async (PostService postService) =>
            {
                var posts = await postService.GetPosts();
                return Results.Ok(new { Message = posts });
            }
        );

        group
            .MapPost(
                "/addPost",
                async ([FromBody] Post post, PostService postService) =>
                {
                    var result = await postService.CreatePost(post);
                    return Results.Ok(result);
                }
            )
            .RequireAuthorization();

        group
            .MapDelete(
                "/deletePost/{guid}",
                async (Guid guid, PostService postService) =>
                {
                    await postService.DeletePost(guid);
                    return Results.NoContent();
                }
            )
            .RequireAuthorization();

        group
            .MapPut(
                "/updatePost/{guid}",
                async ([FromBody] Post newPost, Guid guid, PostService postService) =>
                {
                    var result = await postService.UpdatePost(guid, newPost);
                    return Results.Ok(new { Message = result });
                }
            )
            .RequireAuthorization();
    }
}
