using MicroCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Banks.Service.Interface
{
    public interface IBankService
    {
        Task<ResultModel<List<Result>>> GetAllBanks();
    }
}
