using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportStore.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private EFDBContext context = new EFDBContext();
        public IEnumerable<Product> Products => context.Products.ToList();

        public Product DeleteProduct(int productID)
        {
            Product dbEntity = context.Products.Find(productID);
            if (dbEntity!=null)
            {
                context.Products.Remove(dbEntity);
                context.SaveChanges();
            }
            return dbEntity;
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0)
                context.Products.Add(product);
            else
            {
                Product dbEntry = context.Products.Where(x => x.ProductID == product.ProductID).FirstOrDefault();
                if (dbEntry!=null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Descrpition = product.Descrpition;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                    dbEntry.ImageMimeType = product.ImageMimeType;
                    dbEntry.ImageData = product.ImageData;
                }
            }
            context.SaveChanges();
        }
        
    }
}
