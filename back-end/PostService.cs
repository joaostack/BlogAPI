using back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace back_end;

public interface IPostService
{
    Task<Post> CreatePost(Post post);
    Task<List<Post>> GetPosts();
    Task<object> DeletePost(Guid guid);
    Task<object> UpdatePost(Guid guid, Post newPost);
}

public class PostService : IPostService
{
    private readonly DataContext _context;

    public PostService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Post>> GetPosts()
    {
        var posts = await _context.Posts.ToListAsync();
        return posts;
    }

    public async Task<Post> CreatePost(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task<object> DeletePost(Guid guid)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(post => post.Id == guid);
        if (post is null)
            return "Post inexistente!";

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return "Post deletado com sucesso!";
    }

    public async Task<object> UpdatePost(Guid guid, Post newPost)
    {
        var post = await _context.Posts.FindAsync(guid);
        if (post is null)
            return "Post não encontrado!";

        post.Title = newPost.Title;
        post.Content = newPost.Content;
        await _context.SaveChangesAsync();
        return "Post atualizado!";
    }
}
