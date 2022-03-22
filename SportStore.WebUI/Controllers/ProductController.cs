using Moq;
using SportStore.Domain.Abstract;
using SportStore.Domain.Concrete;
using SportStore.Domain.Entities;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository repository;
        public int pagesize=4;
        public ProductController(IProductRepository productRepository)
        {
            
            repository = productRepository;
        }

        // GET: Product
   
        public ViewResult List(string category,int page=1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                .Where(x => x.Category == category || category == null)
                .OrderBy(p => p.ProductID)
                .Skip((page - 1) * pagesize)
                .Take(pagesize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pagesize,
                    TotalItems = category == null ? repository.Products.Count() : repository.Products.Where(x => x.Category == category).Count(),
                },
                CurrentCategory=category
            };
            
            return View(model);
        }
        public FileContentResult GetImage(int productId)
        {
            Product prod = repository.Products.Where(x => x.ProductID == productId).FirstOrDefault();
            if (prod != null)
                return File(prod.ImageData, prod.ImageMimeType);
            else
                return null;
        }
     

    }
}