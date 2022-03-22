using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.WebUI.Controllers;
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
   public class CartTest
    {
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //arrange
            //create mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product{ ProductID=1,Name="P1",Category="Apples"}
            });
            //arrange 
            //create a cart
            Cart cart = new Cart();
            //arrange
            //create new controler
            CartController target = new CartController(mock.Object,null);
            //act
            //add a product to the cart
            target.AddToCart(cart,1,null);
            ///assert
            Assert.IsTrue(cart.Lines.Count() == 1);
            Assert.IsTrue(cart.Lines.ToArray()[0].Product.ProductID == 1);
        }
        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            //arrange
            //create mock
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product{ ProductID=1,Name="P1",Category="Apples"}
            }.AsQueryable());
            //arrange
            //create cart
            Cart cart = new Cart();
            //arrange
            //create new controller
            CartController target = new CartController(mock.Object,null);
            RedirectToRouteResult result = target.AddToCart(cart, 2, "MyUrl");
            //assert
            Assert.IsTrue(result.RouteValues["action"].ToString()=="Index");
            Assert.IsTrue(result.RouteValues["returnUrl"].ToString() == "MyUrl");
        }
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            //arrange --create a cart
            Cart cart = new Cart();
            //arrange -- create the controller
            CartController target = new CartController(null,null);
            //act --call the Index action method
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;
            //assert
            Assert.AreSame(result.Cart, cart);
            Assert.IsTrue(result.ReturnUrl == "myUrl");
        }
        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            //arange create mock
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            //arrange create an empty card
            Cart cart = new Cart();
            //bir eşya alırsa
            cart.AddItem(new Product(), 1);
            //arrange -create shipping details
            ShippingDetails shippingDetails = new ShippingDetails();
            //arrange -create an instance of the controller
            CartController target = new CartController(null, mock.Object);
            //arrange --add an error to model
            target.ModelState.AddModelError("error","error");
            //act
            ViewResult result = target.Checkout(cart,new ShippingDetails());
            //assert -check that the order hasnt beenpassed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(),It.IsAny<ShippingDetails>()),Times.Never());
            //assert -check that the methodis returning the default view 
            Assert.IsTrue("" == result.ViewName);
            //assert -check that I am passing an invalid model to the view
            Assert.IsTrue(false==result.ViewData.ModelState.IsValid);
        }

    }
}
