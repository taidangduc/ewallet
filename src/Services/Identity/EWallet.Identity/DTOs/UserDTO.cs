namespace EWallet.Identity.DTOs;

public class UserDTO
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public List<string> Roles { get; set; }
}