namespace RushHour.Web.Controllers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common;
    using Data.Models;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service.Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;
        private const string HomeControllerString = "Home";
        private const string IndexAction = nameof(HomeController.Index);

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.emailSender = emailSender;
        }

        private RedirectToActionResult RedirectToHome()
        {
            return RedirectToAction(IndexAction, HomeControllerString, new { area = "" });
        }

        [HttpGet]
        [Authorize(Roles = CommonConstants.AdministratorRole)]
        public IActionResult Index()
        {            
            List<UserListViewModel> users = userManager
                .Users
                .ProjectTo<UserListViewModel>(mapper.ConfigurationProvider)
                .ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData.AddWarningMessage("You already have an account!");

                return RedirectToHome();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserFormViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData.AddWarningMessage("You already have an account!");

                return RedirectToHome();
            }

            if (!ModelState.IsValid)
            {
                TempData.AddWarningMessage("Failed to register. Try again!");

                return View(model);
            }

            User user = new User { Email = model.Email, UserName = model.UserName };

            if(model.PhoneNumber != "")
            {
                user.PhoneNumber = model.PhoneNumber;
            }

            IdentityResult result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            await signInManager.SignInAsync(user, false);
            await emailSender.SendEmailAsync(model.Email, "Sign up", "You successfully signed up");

            TempData.AddSuccessMessage("You successfully registered!");
            return RedirectToHome();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData.AddWarningMessage("You are already logged in!");

                return RedirectToHome();
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                TempData.AddWarningMessage("Failed to log in. Try again!");

                return View(model);
            }

            Microsoft.AspNetCore.Identity.SignInResult identityResult = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

            if (!identityResult.Succeeded)
            {
               TempData.AddErrorMessage("Failed to log in. Try again!");

                return View(model);
            }

            TempData.AddSuccessMessage("You successfully logged in!");
            return RedirectToLocal(returnUrl);
        }
        
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            TempData.AddSuccessMessage("You logged out!");
            return RedirectToHome();
        }

        [HttpGet]
        [Authorize]        
        public async Task<IActionResult> Update(int id)
        {
            User userToEdit = await userManager.FindByIdAsync(id.ToString());

            if(userToEdit == null)
            {
                TempData.AddErrorMessage("User does not exist!");
                return RedirectToHome();
            }
            
            User currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser.Id != id && !await userManager.IsInRoleAsync(currentUser, CommonConstants.AdministratorRole))
            {
                TempData.AddErrorMessage("You don't have permission for this operation!");
                return RedirectToHome();
            }

            UserFormViewModel userFormViewModel = mapper.Map<User, UserFormViewModel>(userToEdit);

            return View(userFormViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(UserFormViewModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddWarningMessage("Failed to edit user. Try again!");
                return View(model);
            }

            User currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser.Id != id && !await userManager.IsInRoleAsync(currentUser, CommonConstants.AdministratorRole))
            {
                TempData.AddErrorMessage("You don't have permission for this operation!");
                return RedirectToHome();
            }

            User userToUpdate = await userManager.FindByIdAsync(id.ToString());
            if(userToUpdate == null)
            {
                TempData.AddErrorMessage("User not found!");
                return RedirectToHome();
            }            

            userToUpdate.Email = model.Email;
            userToUpdate.UserName = model.UserName;
            userToUpdate.PhoneNumber = model.PhoneNumber;

            IdentityResult updateUserResult = await userManager.UpdateAsync(userToUpdate);
            if (!updateUserResult.Succeeded)
            {
                AddErrors(updateUserResult);
                return View(model);
            }

            string token = await userManager.GeneratePasswordResetTokenAsync(userToUpdate);
            IdentityResult resetPasswordResult = await userManager.ResetPasswordAsync(userToUpdate, token, model.Password);
            if (!resetPasswordResult.Succeeded)
            {
                AddErrors(resetPasswordResult);
                return RedirectToHome();
            }            

            if(currentUser.Id == id)
                await signInManager.SignInAsync(userToUpdate, false);


            TempData.AddSuccessMessage("User updated successfully!");
            return RedirectToHome();
        }

        [HttpGet]
        [Authorize(Roles = CommonConstants.AdministratorRole)]
        public async Task<IActionResult> Delete(int id)
        {
            User userToDelete = await userManager.FindByIdAsync(id.ToString());
            if(userToDelete == null)
            {
                TempData.AddErrorMessage("User not found!");
                return RedirectToAction(nameof(Index));
            }

            IdentityResult result = await userManager.DeleteAsync(userToDelete);

            if (!result.Succeeded)
                AddErrors(result);
            else
                TempData.AddSuccessMessage("User deleted successfully!");


            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            TempData.AddErrorMessage("You do not have permission for this operation!");
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToHome();
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                TempData.AddErrorMessage(error.Description);
            }
        }
    }
}