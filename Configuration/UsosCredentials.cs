using System.ComponentModel.DataAnnotations;

namespace UsosPotwierdzanieWnioskow.Configuration;

public sealed class UsosCredentials
{
    /// <summary>
    /// ENV USOS__Username
    /// </summary>
    [Required]
    public required string Username { get; init; }
    /// <summary>
    /// ENV USOS__Password
    /// </summary>
    [Required]
    public required string Password { get; init; }
}