using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportStore.WebUI.Controllers
{
    [Authorize(Roles ="")]
    public class AdminController:Controller
    {
        IProductRepository repository;

        public AdminController(IProductRepository repository)
        {
            this.repository = repository;
        }
        public ViewResult Index()
        {
            return View(repository.Products);
        }
        public ViewResult Edit(int productId)
        {
            Product product = repository.Products.FirstOrDefault(x => x.ProductID == productId);
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product product,HttpPostedFileBase image=null)
        {
            if (ModelState.IsValid)
            {
                if (image!=null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                repository.SaveProduct(product);
                TempData["message"] = string.Format("{0} has been saved",product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }
        public ViewResult Create()
        {
            return View("Edit",new Product());
        }
        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product product = repository.DeleteProduct(productId);
            if (product != null)
            {
                TempData["message"] = string.Format("{0} was deleted",product.Name);
            }
            return RedirectToAction("Index");
        }
    }
}