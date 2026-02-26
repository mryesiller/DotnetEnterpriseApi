using System.ComponentModel.DataAnnotations;

namespace EnterpriseAPI.Application.DTOs;

public class RegisterDto
{
    [Required, EmailAddress]
    public required string Email { get; set; }

    [Required, DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6)]
    public required string Password { get; set; }

    public string? FirstName { get; set; }  // Nullable (zorunlu değil)
    public string? LastName { get; set; }   // Nullable
}

public class LoginDto
{
    [Required, EmailAddress]
    public required string Email { get; set; }

    [Required, DataType(DataType.Password)]
    public required string Password { get; set; }
}