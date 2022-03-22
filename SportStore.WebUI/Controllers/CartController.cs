using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportStore.WebUI.Controllers
{
    public class CartController:Controller
    {
        private IProductRepository repository;
        private IOrderProcessor orderProcessor;
        public CartController(IProductRepository repository,IOrderProcessor orderProcessor)
        {
            this.repository = repository;
            this.orderProcessor = orderProcessor;
        }
        //Cartı card model binden sonra ekledik
        public ViewResult Index(Cart cart,string returnUrl)
        {
            return View(new CartIndexViewModel()
            {
                ReturnUrl = returnUrl,
                // Cart = GetCart()
                //bunun yerine bind edilen metotu kullancaz
                Cart = cart
            });
        }
        //Cartı card model binden sonra ekledik
        public RedirectToRouteResult AddToCart(Cart cart, int productId,string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(x => x.ProductID == productId);
            if (product != null)
            {
                // GetCart().AddItem(product, 1);
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        //Cartı card model binden sonra ekledik
        public RedirectToRouteResult RemoveFromCart(Cart cart,int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(x => x.ProductID == productId);
            if (product != null)
            {
                //yeerine bind gelecek
                //GetCart().RemoveLine(product);
                cart.RemoveLine(product);
            }  
            return RedirectToAction("Index", new { returnUrl });
        }
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }
        [HttpPost]
        public ViewResult Checkout(Cart cart,ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count()==0)
            {
                ModelState.AddModelError("","Sorry, your cartis empty !");
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart,shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }
        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if(cart==null)
            {
                cart= new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}