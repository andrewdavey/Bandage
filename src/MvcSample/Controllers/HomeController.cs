using System.Linq;
using System.Web.Mvc;
using Bandage;
using MvcSample.Models;

namespace MvcSample.Controllers
{
    public class HomeController : SampleControllerBase
    {
        public ActionResult Index()
        {
            ViewModel.Message = "Hello Bandage!";
            ViewModel.Products = Product.LoadAll();
            
            // In addition to the regular Product properties,
            // we want to have a "Url" property as well.
            // This property is defined by a simple delegate.
            ViewModel.Add(DynamicProperty.For<Product>(
                "Url", 
                p => Url.RouteUrl("Product", new { p.Id, slug = Util.GetSlug(p.Name) })
            ));
            
            // BTW: Trying to do something like this using extension methods,
            // instead of Bandage, will not work. You cannot call an extension
            // method on a dynamic object.

            return View();
        }

        public ActionResult Details(int id)
        {
            var product = Product.LoadAll().First(p => p.Id == id);
            return Content("Info about " + product.Name);
        }
    }

}
