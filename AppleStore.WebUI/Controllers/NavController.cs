using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppleStore.Domain.Abstract;


namespace AppleStore.WebUI.Controllers
{
    public class NavController : Controller
    {

        private IAppleRepository repository;

        public NavController(IAppleRepository repos)
        {
            repository = repos;
        }

        public PartialViewResult Menu(string selectCat = null)
        {
            ViewBag.SelectedCategory = selectCat;
            IEnumerable<string> categories = repository.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(p => p);
            
            return PartialView("FlexMenu", categories);
        }
    }
}