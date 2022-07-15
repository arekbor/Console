using System.ComponentModel.DataAnnotations;

namespace Console.ModelDto;

public class LoginUserDto
{
    [Required]
    [MaxLength(25)]
    [Display(Name = "User name")]
    public string Username { get; set; }
    [Required]
    [Display(Name = "Password")]
    public string Password { get; set; }
}