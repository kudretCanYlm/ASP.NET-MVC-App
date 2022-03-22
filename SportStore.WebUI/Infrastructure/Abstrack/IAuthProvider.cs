using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportStore.WebUI.Infrastructure.Abstrack
{
    public interface IAuthProvider
    {
        bool Authenticate(string username, string password);

    }
}