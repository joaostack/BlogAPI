namespace back_end.Models;

public class User
{
    public Guid Id { get; init; } = new Guid();
    public required string Name { get; set; }
    public required string Password { get; set; }
    public string Role { get; set; } = string.Empty;
}
