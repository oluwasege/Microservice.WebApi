using System;
using System.Collections.Generic;
using System.Text;

namespace Banks.Service
{
    public class ResponseVM
    {
        public Result[] Result { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError { get; set; }
    }

    public class Result
    {
        public string BankName { get; set; }
        public string BankCode { get; set; }
    }
}
