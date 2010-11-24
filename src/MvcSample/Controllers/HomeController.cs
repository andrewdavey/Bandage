using System.Linq;
using System.Web.Mvc;
using Bandage;
using MvcSample.Models;

namespace MvcSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            dynamic model = new DynamicViewModel();
            model.Products = Product.LoadAll();
            
            // In addition to the regular Customer properties,
            // we want to have a "Url" property as well.
            model.Add(DynamicProperty.For<Product>(
                "Url", 
                p => Url.RouteUrl("Product", new { p.Id, slug = Util.GetSlug(p.Name) }))
            );
            
            // BTW: Trying to do something like this using extension methods,
            // instead of Bandage, will not work. You cannot call an extension
            // method on a dynamic object.

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var product = Product.LoadAll().First(p => p.Id == id);
            return Content("Info about " + product.Name);
        }
    }

}
