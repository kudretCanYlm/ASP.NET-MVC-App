using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportStore.WebUI.Controllers;
using SportStore.WebUI.Infrastructure.Abstrack;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SportStore.UnitTests
{
    [TestClass]
   public class AdminSecurityTest
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            //arrange -create mock
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(x => x.Authenticate("admin", "secret")).Returns(true);

            //arrange -create the view model
            LoginViewModel model = new LoginViewModel()
            {
                UserName="admin",
                Password="secret"
            };
            //arrange - create controller 
            AccountController target = new AccountController(mock.Object);
            //act -authenticate using valid credentials
            ActionResult result = target.Login(model, "/MyURL");
            //assert
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyURL",((RedirectResult)result).Url);
        }
        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentails()
        {
            //arrange -create mock
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(x => x.Authenticate("badUser", "badPass")).Returns(false);
            //arrange -createview model
            LoginViewModel model = new LoginViewModel()
            {
                UserName="badUser",
                Password="badPass"
            };

            //arrange -create the controller
            AccountController target = new AccountController(mock.Object);
            //act -
            ActionResult result = target.Login(model, "/MyUrl");
            //assert
            Assert.IsInstanceOfType(result,typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
