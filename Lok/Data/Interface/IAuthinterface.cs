using Lok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Data.Interface
{
   public  interface IAuthinterface
    {
        Task<bool> IsUserExists(string Email);
        Task<Login> Login(string Email, string Password);
          Task<Login> ChangePass(Login Login, string password);
        Task<Login> GetUser(string Email);

    }
}
