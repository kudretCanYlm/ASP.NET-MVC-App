using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Web.Mvc;
using Moq;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.Domain.Concrete;
using System.Web.Routing;
using System.Configuration;
using SportStore.WebUI.Infrastructure.Abstrack;
using SportStore.WebUI.Infrastructure.Concreate;

namespace SportStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver :DefaultControllerFactory// IDependencyResolver
    {
        private IKernel Kernel;
        public NinjectDependencyResolver()
        {
            Kernel = new StandardKernel();
            AddBindings();
        }
        //public object GetService(Type serviceType)
        //{
        //    return Kernel.TryGet(serviceType);
        //}

        //public IEnumerable<object> GetServices(Type serviceType)
        //{
        //    return Kernel.GetAll(serviceType);
        //}
        
        

        private void  AddBindings()
        {
            //bunları denemek için sanal olarak oluşturduk şimdi yeni bir gerçek oluşturma
            ////bindings here
            ////Iproduct repostoriyi taklit eder
            //Mock<IProductRepository> mock = new Mock<IProductRepository>();
            ////sahte bir kurulum oluşturur
            //mock.Setup(m => m.Products).Returns(new List<Product> 
            //{
            //  new Product { Name = "Football", Price = 25 },
            //  new Product { Name = "Surf board", Price = 179 },
            //  new Product { Name = "Running shoes", Price = 95 }

            //});
            ////devamlı mock.objecte bağlar
            ////hizmetlerin sabit bir değere bağlanması gerektiğini gösterir
            //Kernel.Bind<IProductRepository>().ToConstant(mock.Object);
           
            Kernel.Bind<IProductRepository>().To<EFProductRepository>();
            
            EmailSettings emailSettings = new EmailSettings
            {
                writeAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };
            Kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("emailSettings", emailSettings);
            Kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();

        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)Kernel.Get(controllerType);
        }

    }
}