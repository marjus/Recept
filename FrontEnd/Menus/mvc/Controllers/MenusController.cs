using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Rcpt.DAL;
using Rcpt.Models;
using System.Globalization;

namespace Rcpt.Controllers
{
    public class MenusController : Controller
    {
        private RecipeContext db = new RecipeContext();

        // GET: Menus
        public ActionResult Index()
        {
            var menu = Menu.GetMenu(this.HttpContext);
            return View(menu);
        }
        [HttpPost]
        public ActionResult AddToMenu(int recipeId)
        {
            var recipe = db.Recipes.SingleOrDefault(r => r.ID == recipeId);
            var menu = Menu.GetMenu(this.HttpContext);
            if (string.IsNullOrEmpty(menu.UserName) && !string.IsNullOrEmpty(User.Identity.Name))
                menu.UserName = User.Identity.Name;
            menu.AddToMenu(recipe);
            return MenuRecipeCount();
        }
        [HttpPost]
        public ActionResult RemoveFromMenu(int recipeId)
        {
            var menu = Menu.GetMenu(this.HttpContext);
            var recipe = db.Recipes.SingleOrDefault(r => r.ID == recipeId);

            var message = "";
            if (recipe == null)
            {
               message = "Receptet hittades inte";
            }
            else
            {
                message = "Recept " + recipe.Title + " togs bort";
            }
            menu.RemoveFromMenu(recipeId);
            return Json(message);
        }

        [HttpPost]
        public ActionResult SetServings(ServingsInfo serving)
        {
            var menu = Menu.GetMenu(this.HttpContext);
            var recipe = db.Recipes.SingleOrDefault(r => r.ID == serving.recipeId);

            var message = "";
            if (recipe == null)
            {
                message = "Receptet hittades inte";
                return Json(message);
            }

            menu.SetServings(recipe, serving.servings);

            return Json(message);
        }

        public class ServingsInfo
        {
            public int recipeId { get; set; }
            public int servings { get; set; }
        }

        [ChildActionOnly]
        public ActionResult MenuSummary()
        {
            var menu = Menu.GetMenu(this.HttpContext);
            
            return PartialView("_MenuSummary", menu);
        }

        [ChildActionOnly]
        public ActionResult MenuRecipeCount()
        {
            var menu = Menu.GetMenu(this.HttpContext);

            if(menu != null)
            {
                if(menu.MenuRecipes != null)
                {
                    ViewBag.MenuRecipeCount = menu.MenuRecipes.Count();
                }
            }
                

            return PartialView("_MenuRecipeCount");
        }

        [HttpPost]
        public ActionResult EmptyMenu()
        {
            var menu = Menu.GetMenu(this.HttpContext);
            menu.EmptyMenu();
            return RedirectToAction("Index", "Recipes");
        }

        public ActionResult Details()
        {
            var menu = Menu.GetMenu(this.HttpContext);
            
            return View(menu);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
