using System.ComponentModel.DataAnnotations;
using Console.Data;

namespace Console.ModelDto;

public class RegisterUserDto
{
    [Required]
    [MaxLength(25)]
    [Display(Name = "User name")]
    public string Username { get; set; }
    [Required]
    [Display(Name = "Password")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = Message.Code01)]
    public string Password { get; set; }
    [Required]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }
}