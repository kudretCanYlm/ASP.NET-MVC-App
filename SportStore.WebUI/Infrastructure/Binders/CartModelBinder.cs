using SportStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportStore.WebUI.Infrastructure.Binders
{
    //bunu global.asax ta çağırcaz
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // get the Cart from the session
            Cart cart = null;
            if(controllerContext.HttpContext.Session!=null)
            {
                cart= (Cart)controllerContext.HttpContext.Session[sessionKey];
            }
            // create the Cart if there wasn't one in the session data
            if (cart is null)
            {
                cart = new Cart();
                if (controllerContext.HttpContext.Session!=null)
                {
                    controllerContext.HttpContext.Session[sessionKey]=cart;
                }
            }
            return cart;
        }
    }
}