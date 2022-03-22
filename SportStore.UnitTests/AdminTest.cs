using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SportStore.UnitTests
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            //arrange -create mock
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            });
            //arrrange -create controller
            AdminController target = new AdminController(mock.Object);
            //action
            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();
            //assert
            Assert.AreEqual(result.Length,3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }
        [TestMethod]
        public void Can_Edit_Product()
        {
            //arrange -create mock
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product{ ProductID=1,Name="P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            });
            //arrange create controller
            AdminController target = new AdminController(mock.Object);
            //act
            Product p1 = (Product)target.Edit(1).ViewData.Model;
            Product p2 = (Product)target.Edit(2).ViewData.Model;
            Product p3 = (Product)target.Edit(3).ViewData.Model;
            //assert
            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }
        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            //arrange -create mock
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product{ ProductID=1,Name="P1"},
                new Product{ ProductID=2,Name="P2"},
                new Product{ ProductID=3,Name="P3"},
                new Product{ ProductID=10,Name="P10"},
                new Product{ ProductID=6,Name="P6"}
            });
            //arrange create controller
            AdminController target = new AdminController(mock.Object);
            //act
            Product result = target.Edit(4).ViewData.Model as Product;
            //asssert
            Assert.IsNull(result);
        }
        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            //arrange -create mock
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //arrange -create controller
            AdminController target = new AdminController(mock.Object);
            //arrange -create a product
            Product product = new Product()
            {
                Name="Test",
                Category="Test",
                Descrpition="asdasd",
                Price=3100,
                ProductID=3
            };
            //act -try to save product
            ActionResult result = target.Edit(product);
            //assert -check that the repository was called
            mock.Verify(x=>x.SaveProduct(product));
            //assert -check the method result type
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public void Cannot_Save_Invalid_changes()
        {
            //arrange  -create mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //arrange -create controller
            AdminController target = new AdminController(mock.Object);
            //arrange -create a product
            Product product = new Product()
            {
                Name = "Test",
                Category = "Test",
                Descrpition = "asdasd",
                Price = 3100,
                ProductID = 3
            };
            //arrange add anerror to the model state --o bölgeyi atlar
            target.ModelState.AddModelError("error", "error");
            //act -try to save product
            ActionResult result = target.Edit(product);
            //assert -check that the repository was not called
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()),Times.Never());
            //assert check the method result type
            Assert.IsInstanceOfType(result, typeof(ViewResult));

        }
        [TestMethod]
        public void Can_Delete_Valid_Products()
        {
            //arange -create a Product
            Product product = new Product {ProductID=2,Name="Test" };
            //arrange -create mock
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product{ProductID=1,Name="P1"},
                product,
                new Product{ProductID=3,Name="P3"}
            });
            //arrange -create a controller
            AdminController target = new AdminController(mock.Object);
            //Act --delete product
            target.Delete(product.ProductID);
            //assert
            mock.Verify(x => x.DeleteProduct(product.ProductID));
        }


    }
}
