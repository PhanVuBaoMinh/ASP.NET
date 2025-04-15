using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using PhanVuBaoMinh.Data;

namespace PhanVuBaoMinh.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _environment = environment;
        }

        [TempData]
        public string? StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }

        public class InputModel
        {
            [Phone]
            public string? PhoneNumber { get; set; }

            [Display(Name = "Avatar")]
            public IFormFile? AvatarImage { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("Unable to load user.");

            Email = user.Email;
            AvatarUrl = string.IsNullOrEmpty(user.AvatarUrl) ? "/images/default-avatar.png" : user.AvatarUrl;
            Input.PhoneNumber = user.PhoneNumber;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("Unable to load user.");

            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // giữ lại thông tin
                return Page();
            }

            // Update phone number
            if (Input.PhoneNumber != user.PhoneNumber)
            {
                user.PhoneNumber = Input.PhoneNumber;
            }

            // Upload avatar
            if (Input.AvatarImage != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "avatars");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Input.AvatarImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.AvatarImage.CopyToAsync(stream);
                }

                user.AvatarUrl = $"/images/avatars/{uniqueFileName}";
            }

            await _userManager.UpdateAsync(user);

            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
