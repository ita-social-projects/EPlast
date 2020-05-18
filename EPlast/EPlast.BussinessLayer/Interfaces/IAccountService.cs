using EPlast.BussinessLayer.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BussinessLayer.Interfaces
{
    public interface IAccountService
    {
        LoginViewModel LoginAsyncGet(string returnUrl);
    }
}
