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
    public class ImageTest
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            //arrange -create a Product with image data
            Product prod = new Product()
            {
                ProductID=2,
                Name="Test",
                ImageData=new byte[] { },
                ImageMimeType="image/png"
            };
            //arrange -create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product{ ProductID=1,Name="P1"},
                prod,
                new Product{ ProductID=3,Name="P3"}
            });
            //arrange -create a controller
            ProductController target = new ProductController(mock.Object);
            //act
            ActionResult result = target.GetImage(2);
            //assert 
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.ImageMimeType, ((FileResult)result).ContentType);
        }
        [TestMethod]
        public void Cannot_Retrieve_Image_Data_for_Invalid_ID()
        {
            //arrange -create a mock repisitory
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product{ ProductID=1,Name="P1"},
                new Product{ ProductID=2,Name="P2"}
            });
            //arrange -create the controller
            ProductController target = new ProductController(mock.Object);
            //act
            ActionResult result = target.GetImage(100);
            //assert
            Assert.IsNull(result);
        }
    }
}
