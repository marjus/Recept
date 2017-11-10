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
using Rcpt.ViewModels;
using Rcpt.Util;
using System.Text.RegularExpressions;
using rcpt.Util;

namespace Rcpt.Controllers
{
    public class RecipesController : Controller
    {
        private RecipeContext db = new RecipeContext();

        public ActionResult AjaxTest()
        { return View(); }

        public async Task<ActionResult> UploadImage(HttpPostedFileBase file, int Id)
        {
            if (file != null)
            {
                var recipe = await db.Recipes.SingleOrDefaultAsync(r => r.ID == Id);

                if(recipe == null)
                    return HttpNotFound();

                BlobHandler bh = new BlobHandler();

                var filetype = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                    var filename = DateTime.Now.Ticks + filetype;
                    bh.UploadImage(file, filename);

                    var subrec = recipe.SubRecipes.FirstOrDefault();
                    subrec.Image = filename;
            }

            return RedirectToAction("Index");
        }

        // GET: Recipes
        public async Task<ActionResult> Index(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return View(await db.Recipes
                    .OrderBy(r => r.Title)
                    .ToListAsync());
            }
            else
            {
                return View(
                    await db.Recipes
                        .OrderBy(r=> r.Title)
                        .Where(r => r.Title.Contains(filter) || r.Description.Contains(filter)).ToListAsync());
            }
        }

        // GET: Recipes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = await db.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        public async Task<ActionResult> Create()
        {
            var model = new Rcpt.ViewModels.EditRecipeVM();
            model.AllRecipeCategories = await db.RecipeCategories.ToListAsync();
            model.Recipe = new Recipe();

            model.Recipe.SubRecipes.Add(new SubRecipe { Category = db.RecipeCategories.Single(c => c.Category == "Huvudrätt") });
            return View(model);
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( EditRecipeVM newRecipe)
        {
            if (newRecipe.Recipe == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                var recipe = db.Recipes.Add(new Recipe());

                recipe.Title = newRecipe.Recipe.Title;
                recipe.Description = newRecipe.Recipe.Description;
                recipe.CreatedDate = DateTime.Now;
                recipe.CreatedByUserName = System.Environment.UserName;

                int i = 0;
                foreach (var subrec in newRecipe.Recipe.SubRecipes)
                {
                    var subrecipe = recipe.SubRecipes.SingleOrDefault(sr => sr.ID == subrec.ID);

                    if (!string.IsNullOrEmpty(subrec.Process) || !string.IsNullOrEmpty(subrec.IngredientsText))
                    {
                        if (subrecipe == null)
                        {
                            subrecipe = new SubRecipe { Category = subrec.Category, Description = subrec.Description, Title = subrec.Title, Process = subrec.Process, IngredientsText = subrec.IngredientsText };
                            recipe.SubRecipes.Add(subrecipe);
                        }

                        subrecipe.Title = i == 0 ? newRecipe.Recipe.Title : subrec.Title;
                        subrecipe.Description = i == 0 ? newRecipe.Recipe.Description : subrec.Description;
                        subrecipe.Process = subrec.Process;

                        subrecipe.IngredientsText = subrec.IngredientsText;
                        subrecipe.Category = await db.RecipeCategories.SingleOrDefaultAsync(rcat => rcat.ID == subrec.Category.ID);

                        subrecipe.Ingredients.Clear();

                        var ings = Util.InputParser.ParseIngredientList(subrec.IngredientsText);

                        if (ings != null)
                        {
                            foreach (var ing in ings)
                            {
                                var ingredient = await GetIngredient(ing);

                                subrecipe.Ingredients.Add(new RecipeIngredient { Amount = ing.amount, Ingredient = ingredient, Recipe = recipe, Unit = ing.unit });
                            }
                        }
                    }
                    else if (subrecipe != null)
                        recipe.SubRecipes.Remove(subrecipe);

                    i++;
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            newRecipe.AllRecipeCategories = await db.RecipeCategories.ToListAsync();
            return View(newRecipe);
        }

        private async Task<Ingredient> GetIngredient(InputParser.IngredientResult ing)
        {
            var ingredient = await db.Ingredients.FirstOrDefaultAsync(i => i.Name.Equals(ing.ingredient));
            if (ingredient == null)
                ingredient = db.Ingredients.Add(new Ingredient { Name = ing.ingredient });
            return ingredient;
        }


        // GET: Recipes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var vm = new EditRecipeVM();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = await db.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            vm.Recipe = recipe;
            vm.AllRecipeCategories = await db.RecipeCategories.ToListAsync();

            var units = await db.Units.Select(u => u.Name).ToListAsync();

            vm.KnownUnits = string.Join(",", units);

            return View(vm);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(/*[Bind(Include = "ID,Title,Description,SubRecipes")]*/ EditRecipeVM editedRecipe)
        {
            if (editedRecipe.Recipe == null || editedRecipe.Recipe.ID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                var recipe = await db.Recipes.Include(r => r.SubRecipes).SingleOrDefaultAsync(r => r.ID == editedRecipe.Recipe.ID);

                if (recipe == null || recipe.SubRecipes == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                recipe.Title = editedRecipe.Recipe.Title;
                recipe.Description = editedRecipe.Recipe.Description;

                int i = 0;
                foreach (var subrec in editedRecipe.Recipe.SubRecipes)
                {
                    var subrecipe = recipe.SubRecipes.SingleOrDefault(sr => sr.ID == subrec.ID);
                    
                    if (!string.IsNullOrEmpty(subrec.Process) || !string.IsNullOrEmpty(subrec.IngredientsText))
                    {
                        if (subrecipe == null)
                        {
                            subrecipe = new SubRecipe { Category = subrec.Category, Description = subrec.Description, Title = subrec.Title, Process = subrec.Process, IngredientsText = subrec.IngredientsText };
                            recipe.SubRecipes.Add(subrecipe);
                        }

                        subrecipe.Title = i==0 ? editedRecipe.Recipe.Title : subrec.Title;
                        subrecipe.Description = i == 0 ? editedRecipe.Recipe.Description : subrec.Description;
                        subrecipe.Process = subrec.Process;

                        subrecipe.IngredientsText = subrec.IngredientsText;
                        subrecipe.Category = await db.RecipeCategories.SingleOrDefaultAsync(rcat => rcat.ID == subrec.Category.ID);

                        subrecipe.Ingredients.Clear();

                        var ings = Util.InputParser.ParseIngredientList(subrec.IngredientsText);

                        if (ings != null)
                        {
                            foreach (var ing in ings)
                            {
                                var ingredient = await GetIngredient(ing);

                                subrecipe.Ingredients.Add(new RecipeIngredient { Amount = ing.amount, Ingredient = ingredient, Recipe = recipe, Unit = ing.unit });
                            }
                        }
                    }
                    else if (subrecipe != null)
                        recipe.SubRecipes.Remove(subrecipe);

                    i++;
                }

                db.Entry(recipe).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(editedRecipe);
        }

        // GET: Recipes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = await db.Recipes.FindAsync(id);

            if(!User.Identity.IsAuthenticated || recipe.CreatedByUserName != User.Identity.Name)
            {
                throw new UnauthorizedAccessException();
            }

            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Recipe recipe = await db.Recipes.FindAsync(id);

            if (!User.Identity.IsAuthenticated || recipe.CreatedByUserName != User.Identity.Name)
            {
                throw new UnauthorizedAccessException();
            }

            db.Recipes.Remove(recipe);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
