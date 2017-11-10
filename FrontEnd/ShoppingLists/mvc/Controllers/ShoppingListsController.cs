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

namespace Rcpt.Controllers
{
    public class ShoppingListsController : Controller
    {
        private RecipeContext db = new RecipeContext();


        // GET: ShoppingLists
        public async Task<ActionResult> Index()
        {
            var vm = new ViewModels.ShoppingListVM();
            vm.CustomList = new ShoppingList();

            var customList = await db.ShoppingLists.Include(l => l.ShoppingListItems).Where(s => s.IsOpen && s.UserName == System.Environment.UserName && !s.MenuId.HasValue).OrderByDescending(s => s.CreatedDate).FirstOrDefaultAsync();
            if (customList != null)
            {
                customList.Title = customList.CreatedDate.ToString("yyyy-MM-dd");

                vm.CustomList = customList;
            }

            var shoppingLists = await db.ShoppingLists.Where(s=> s.IsOpen && s.UserName==System.Environment.UserName && s.MenuId.HasValue).OrderByDescending(s=> s.CreatedDate).ToListAsync();

            if (shoppingLists != null)
            {
                foreach (var shoppingList in shoppingLists)
                {
                    shoppingList.ShoppingListItems = await db.ShoppingListItems.Include(si => si.FromRecipe).Where(si => si.ShoppingListId == shoppingList.Id).OrderBy(si => si.Product.Name).ToListAsync();
                }
            }
            vm.MenuLists = shoppingLists;
            
            return View(vm);
        }

        // GET: ShoppingLists/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingList shoppingList = await db.ShoppingLists.FindAsync(id);
            if (shoppingList == null)
            {
                return HttpNotFound();
            }
            return View(shoppingList);
        }

        // GET: ShoppingLists/Create
        public ActionResult Create()
        {
            ViewBag.MenuId = new SelectList(db.Menus, "Id", "MenuUserKey");
            return View();
        }

        // POST: ShoppingLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CreatedDate,IsOpen,MenuId")] ShoppingList shoppingList)
        {
            if (ModelState.IsValid)
            {
                db.ShoppingLists.Add(shoppingList);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MenuId = new SelectList(db.Menus, "Id", "MenuUserKey", shoppingList.MenuId);
            return View(shoppingList);
        }

        // GET: ShoppingLists/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingList shoppingList = await db.ShoppingLists.FindAsync(id);
            if (shoppingList == null)
            {
                return HttpNotFound();
            }
            ViewBag.MenuId = new SelectList(db.Menus, "Id", "MenuUserKey", shoppingList.MenuId);
            return View(shoppingList);
        }

        // POST: ShoppingLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CreatedDate,IsOpen,MenuId")] ShoppingList shoppingList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shoppingList).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MenuId = new SelectList(db.Menus, "Id", "MenuUserKey", shoppingList.MenuId);
            return View(shoppingList);
        }

        public async Task<ActionResult> Close(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ShoppingList shoppingList = await db.ShoppingLists.FindAsync(id);
            if (shoppingList == null)
            {
                return HttpNotFound();
            }

            shoppingList.IsOpen = false;

            return RedirectToAction("Index");
        }

        // GET: ShoppingLists/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingList shoppingList = await db.ShoppingLists.FindAsync(id);
            if (shoppingList == null)
            {
                return HttpNotFound();
            }
            return View(shoppingList);
        }

        // POST: ShoppingLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ShoppingList shoppingList = await db.ShoppingLists.FindAsync(id);
            db.ShoppingLists.Remove(shoppingList);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> CreateFromMenu(int menuId)
        {
            var menu =await db.Menus.SingleOrDefaultAsync(m => m.Id == menuId);

            if (menu == null)
                throw new ArgumentException("Fel meny referens, prova igen");

            if (menu.MenuRecipes == null || menu.MenuRecipes.Count == 0)
                throw new ArgumentException("Fel: Menyn har inga recept");
            
            var shoppingList = db.ShoppingLists.Add(new ShoppingList { CreatedDate = DateTime.Now, IsOpen = true, MenuId = menuId, UserName=System.Environment.UserName });
            
            foreach (var menurecipe in menu.MenuRecipes)
            {
                if(menurecipe.Recipe != null && menurecipe.Recipe.SubRecipes != null )
                {
                    foreach (var subrec in menurecipe.Recipe.SubRecipes)
                    {
                        if (subrec.Ingredients != null)
                        {
                            foreach (var ing in subrec.Ingredients)
                            {
                                shoppingList.ShoppingListItems.Add(new ShoppingListItem { Amount = ing.Amount, FromRecipe = menurecipe.Recipe, Unit = ing.Unit, Product = ing.Ingredient });
                            }
                        }
                    }
                }
            }

            await db.SaveChangesAsync();

            return RedirectToAction("Index", "ShoppingLists");
        }
/*
        public async Task AddToLatestOpenList(string prodName)
        {
            var list = await db.ShoppingLists.Where(s => s.IsOpen && s.UserName == System.Environment.UserName).OrderByDescending(s => s.CreatedDate).FirstOrDefaultAsync();

            if (list == null)
                list = db.ShoppingLists.Add(new ShoppingList { CreatedDate = DateTime.Now, IsOpen = true, UserName = System.Environment.UserName });

            var parsedInput = Util.InputParser.ParseIngredient(prodName);

            if (list.ShoppingListItems == null)
                list.ShoppingListItems = new List<ShoppingListItem>();

            var product = await db.Products.SingleOrDefaultAsync(p => p.Name.ToLower().Equals(parsedInput.ingredient.ToLower()));

            if (product == null)
                product = db.Products.Add(new Product { Name = parsedInput.ingredient });

            var sli = new ShoppingListItem { Unit = parsedInput.unit, Product = product };

            if (parsedInput.amount > 0)
                sli.Amount = parsedInput.amount;

            list.ShoppingListItems.Add(sli);

            await db.SaveChangesAsync();
        }
*/
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
