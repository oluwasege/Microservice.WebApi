using MicroCore.Utils;
using MicroCore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAccount.Core.Enum;
using UserAccount.Entities.DataAccess;
using UserAccount.Entities.Models;
using UserAccount.Entities.ViewModels;
using UserAccount.Service.Interfaces;

namespace UserAccount.Service
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserServices(UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }
        public async Task<ResultModel<PaginatedList<UserVM>>> GetAllOnboardedUsers(BaseSearchViewModel model)
        {
            var resultModel = new ResultModel<PaginatedList<UserVM>>();
            var query = GetAllUsers().Where(x=>x.PhoneNumberConfirmed==true&&x.Activated==true);
            if (query == null)
            {
                resultModel.AddError("No existing user");
                return resultModel;
            }
            var usersPaged = query.ToPaginatedList((int)model.PageIndex, (int)model.PageSize);
            var usersVms = usersPaged.Select(x => (UserVM)x).ToList();
            var data = new PaginatedList<UserVM>(usersVms, (int)model.PageIndex, (int)model.PageSize, usersPaged.TotalCount);
            resultModel.Data = data;
            resultModel.Message = $"Found {usersPaged.Count} users";
            return resultModel;
        }

        public async Task<ResultModel<UserVM>> GetUserAsync(string email)
        {
            var resultModel = new ResultModel<UserVM>();
            var user = await GetUser(email);
            if (user == null)
            {
                resultModel.AddError("User not found");
                return resultModel;
            };

            if (user.PhoneNumberConfirmed == false)
            {
                resultModel.AddError("User not oboarded completely");
                return resultModel;
            };
            resultModel.Data = user;
            resultModel.Message = "User retrieved";
            return resultModel;

        }

        public async Task<ResultModel<string>> RegisterUser(RegisterUserVM model)
        {
            var result = new ResultModel<string>();
            try
            {
                if(model.ConfirmPassword!=model.Password)
                {
                    result.Message = "Password must be the same with Confirm password";
                    result.AddError("Password must be the same with Confirm password");
                    result.Data = null;
                    return result;
                }
                var existingUser = await GetUser(model.Email);

                if (existingUser != null)
                {
                    result.Message = "Account exists";
                    result.AddError("Account exists");
                    result.Data = null;
                    return result;
                }
                //var lga = new LGA
                //{
                //    Name = model.LGA
                //};

                var state = new State
                {
                    //LGAId = lga.Id,
                    Name = model.State,
                    LGA = model.LGA
                };
                await _context.States.AddAsync(state);
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email.ToLower(),
                    EmailConfirmed = false,
                    Activated = false,
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberConfirmed = false,
                    StateId=state.Id,
                };
                
                var role = await _roleManager.FindByNameAsync(AppRoles.Customer);

                if (role == null)
                {
                    result.AddError("Account not created");
                    result.Message = "Account not created";
                    return result;
                }
                var response = await _userManager.CreateAsync(user, model.Password);
                // AN ERROR OCCURED WHILE CREATING USER
                if (!response.Succeeded)
                {
                    foreach (var error in response.Errors)
                    {
                        result.AddError(error.Description);
                    }
                    return result;
                };

                await _userManager.AddToRoleAsync(user, role.Name);
                var code = await Generate2FA(user, "Phone");
                result.Data = $"To complete account creation please use this otp code {code}";
                result.Message = "success";
                return result;
            }
            catch (Exception ex)
            {
                result.AddError(ex.InnerException.Message);
                return result;
            }
        }

        public async Task<ResultModel<string>> ResendOtp(ResendOtpVM model)
        {
            var result = new ResultModel<string>();
            try
            {
                var user = await GetUser(model.Email);
                if (user != null)
                {
                    result.Message = "Account exists";
                    result.AddError("Account exists");
                    result.Data = null;
                    return result;
                }

                if(user.PhoneNumberConfirmed==true&&user.Activated==true)
                {
                    result.AddError("Account activated already");
                    result.Data = null;
                    return result;

                }
                var code = await Generate2FA(user, "Phone");
                result.Data = $"To complete account creation please use this otp code {code}";
                result.Message = "success";
                return result;
            }
            catch (Exception ex)
            {
                result.AddError(ex.InnerException.Message);
                return result;
            }
        }

        private IQueryable<ApplicationUser>GetAllUsers() => _context.Users.Include(x=>x.State).AsQueryable();

        public async Task<ResultModel<bool>> ActivateUser(ActivateUserVM model)
        {
            var result=new ResultModel<bool>();
            try
            {
                var user=await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {

                }
                var success=await _userManager.VerifyTwoFactorTokenAsync(user, "Phone", model.Code);
                if(!success)
                {
                    result.AddError("Incorrect otp code");
                    return result;
                }
                user.PhoneNumberConfirmed = true;
                user.Activated = true;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                result.Data=true;
                result.Message="success";
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<ApplicationUser> GetUser(string emailAddress)
            => await _context.Users.AsNoTracking().Include(x=>x.State).FirstOrDefaultAsync(x=>x.Email.ToLower()==emailAddress.ToLower());
        private async Task<string> Generate2FA(ApplicationUser user, string provider)
            =>await _userManager.GenerateTwoFactorTokenAsync(user, provider);
        
    }
}
