using System.Text.RegularExpressions;
using back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace back_end;

public interface IUsersService
{
    Task<object> NewAccount(User user);
    Task<object> DeleteAccount(Guid guid);
    Task<object> UpdateAccount(Guid guid, User newUser);
}

public class UsersService : IUsersService
{
    private readonly DataContext _context;

    public UsersService(DataContext context)
    {
        _context = context;
    }

    public async Task<object> NewAccount(User user)
    {
        // filtros
        var trimmedUserName = user.Name.Trim();
        var checkSpecialChars = Regex.Match(trimmedUserName, "[^a-zA-Z1-9]");
        if (string.IsNullOrEmpty(user.Name) || checkSpecialChars.Success)
            return "Username is not suitable.";

        // remove special chars
        var splitSpecialChars = Regex.Replace(trimmedUserName, "[^a-zA-Z0-9]", "");
        user.Name = splitSpecialChars;

        if (user.Name.Length >= 20)
            return "Username is too large (<20).";
        if (user.Password.Length < 8)
            return "This is a bad password (recommended >= 8).";

        // verificar se o username já está cadastrado
        var userExists = await _context.Users.FirstOrDefaultAsync(u => u.Name == user.Name);
        if (userExists is not null)
            return "Username already taken!";

        // gerar hash para a senha especificada
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, 12);
        user.Password = hashedPassword;
        user.Role = "user"; // role padrão
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<object> DeleteAccount(Guid guid)
    {
        var user = await _context.Users.FindAsync(guid);
        if (user is null)
            return "Não foi possível deletar a conta!";

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return "Usuário deletado com sucesso!";
    }

    public async Task<object> UpdateAccount(Guid guid, User newUser)
    {
        var user = await _context.Users.FindAsync(guid);
        if (user is null)
            return "Não foi possível atualizar a conta!";

        user.Name = newUser.Name;
        if (!string.IsNullOrEmpty(newUser.Password))
            user.Password = newUser.Password;

        await _context.SaveChangesAsync();
        return "Usuário atualizado com sucesso!";
    }
}
