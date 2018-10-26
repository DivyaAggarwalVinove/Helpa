using Helpa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services
{
    public interface ILoginServices
    {
        Task<LoginErrorResponseModel> Login(string email, string pwd);
    }
}
