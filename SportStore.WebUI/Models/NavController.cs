using SportStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportStore.WebUI.Models
{
    public class NavController:Controller
    {
        private IProductRepository productRepository;
        public NavController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public PartialViewResult Menu(string category=null)
        {
            ViewBag.selectedCategory = category;
            IEnumerable<string> categories = productRepository.Products.Select(x=>x.Category).Distinct().OrderBy(x=>x);
            
            return PartialView("MenuHorizontal", categories);
        }
    }
}