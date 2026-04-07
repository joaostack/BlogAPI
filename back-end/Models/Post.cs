using System.ComponentModel.DataAnnotations;

namespace back_end.Models;

public class Post
{
    public Guid Id { get; init; } = new Guid();

    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public required string Title { get; set; }
    public required string Content { get; set; }
}
