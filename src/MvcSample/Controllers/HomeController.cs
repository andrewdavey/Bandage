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
            model.Customers = Customer.LoadAll();

            // In addition to the regular Customer properties,
            // we want to have a "Url" property as well.
            model.Add(DynamicProperty.For<Customer>(
                "Url", 
                c => Url.Action("Details", new { c.Id }))
            );
            
            // BTW: Trying to do something like this using extension methods,
            // instead of Bandage, will not work. You cannot call an extension
            // method on a dynamic object.

            return View(model);
        }

        public ActionResult Details(int id)
        {
            return Content("Customer " + id);
        }
    }

}
