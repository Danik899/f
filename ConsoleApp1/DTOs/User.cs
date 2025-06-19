namespace ConsoleApp1.DTOs;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string FullName { get; set; }
    public string GroupNumber { get; set; }
    public string Email { get; set; }
}