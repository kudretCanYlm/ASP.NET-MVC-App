using SportStore.WebUI.Infrastructure.Abstrack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SportStore.WebUI.Infrastructure.Concreate
{
    public class FormsAuthProvider : IAuthProvider
    {
        [Obsolete]
        public bool Authenticate(string username, string password)
        {
            bool result = FormsAuthentication.Authenticate(username, password);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, false);
            }
            return result;
        }
    }
}