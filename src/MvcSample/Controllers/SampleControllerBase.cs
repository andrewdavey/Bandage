using System.Web.Mvc;
using Bandage;

namespace MvcSample.Controllers
{
    public abstract class SampleControllerBase : Controller
    {
        // This code is optional when using Bandage.
        // It means you can keep using the handy ViewModel property
        // in your actions, but also gain the dynamic property awesomesauce.
        public new dynamic ViewModel
        {
            get { return viewModel ?? (viewModel = new DynamicViewModel(ViewData)); }
        }
        DynamicViewModel viewModel;

        // The alternative is to create an instance of DynamicViewModel
        // in your action method and pass it to the view as the "Model".

        // Or you can do "ViewData = myDynamicViewModel" before returning the View()
    }
}