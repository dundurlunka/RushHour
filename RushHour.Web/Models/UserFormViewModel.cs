namespace RushHour.Web.Models
{
    using AutoMapper;
    using Data.Models;
    using Common.Mapping;
    using System.ComponentModel.DataAnnotations;

    public class UserFormViewModel : IMapFrom<User>, IHaveCustomMapping
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Username")]
        public string UserName { get; set; }
    
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [StringLength(14, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile
                .CreateMap<User, UserFormViewModel>()
                .ForMember(u => u.Password, cfg => cfg.UseValue(""))
                .ForMember(u => u.ConfirmPassword, cfg => cfg.UseValue(""));
        }
    }
}
