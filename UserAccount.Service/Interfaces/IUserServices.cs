using MicroCore.Utils;
using MicroCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserAccount.Entities.ViewModels;

namespace UserAccount.Service.Interfaces
{
    public interface IUserServices
    {
        Task<ResultModel<string>> RegisterUser(RegisterUserVM model);
        Task<ResultModel<bool>> ActivateUser(ActivateUserVM model);
        Task<ResultModel<PaginatedList<UserVM>>> GetAllOnboardedUsers(BaseSearchViewModel model);
        Task<ResultModel<UserVM>> GetUserAsync(string email);
        Task<ResultModel<string>> ResendOtp(ResendOtpVM model);
    }
}
