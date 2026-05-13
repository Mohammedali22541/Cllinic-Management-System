using ClinicApp.Business.Abstractions;
using ClinicApp.Business.Dtos.Authentication;
using ClinicApp.Data.DatabaseContract;
using ClinicApp.Data.Entites;
using Microsoft.AspNetCore.Identity;

namespace ClinicApp.Business.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> LoginAsync(LoginDto dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return false;

            var res = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, dto.RememberMe, false);
            return res.Succeeded;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            if (dto is null ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password) ||
                string.IsNullOrWhiteSpace(dto.ConfirmPassword) ||
                string.IsNullOrWhiteSpace(dto.FullName) ||
                string.IsNullOrWhiteSpace(dto.Phone) ||
                dto.Age <= 0)
            {
                return false;
            }

            var userExist = await _userManager.FindByEmailAsync(dto.Email);

            if (userExist is not null || dto.Password != dto.ConfirmPassword)
                return false;

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                PhoneNumber = dto.Phone
            };

            var createUser = await _userManager.CreateAsync(user, dto.Password);

            if (!createUser.Succeeded)
            {
                Console.WriteLine(string.Join(",", createUser.Errors.Select(x => x.Description)));
                return false;
            }

            var patient = new Patient
            {
                Name = dto.FullName,
                Phone = dto.Phone,
                Age = dto.Age,
                ApplicationUserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork
                .GetRepository<Patient, int>()
                .AddAsync(patient);

            var patientCreated = await _unitOfWork.SaveChangesAsync() > 0;

            if (!patientCreated)
            {
                await _userManager.DeleteAsync(user);
                return false;
            }

            return true;
        }
    }
}