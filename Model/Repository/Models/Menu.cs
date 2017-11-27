
using Recipes.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace Recipes.Models
{
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string MenuUserKey { get; set; }

        public string Name { get; set; }

        [Display(Name = "Skapad")]
        public DateTime CreatedDate { get; set; }

        public bool IsOpen { get; set; }

        public string UserName { get; set; }

        public virtual ICollection<MenuRecipe> MenuRecipes { get; set; }

        private RecipeContext db = new RecipeContext(null);

        public const string MenuKey = "_menu_session_id_";



        public Menu()
        {
            this.MenuRecipes = new List<MenuRecipe>();
        }

        // public string GetMenuKey(HttpContextBase context)
        // {
        //     if (context.Session[MenuKey] == null)
        //     {
        //         if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
        //         {
        //             context.Session[MenuKey] =
        //                 context.User.Identity.Name;
        //         }
        //         else
        //         {
        //             // Generate a new random GUID using System.Guid class
        //             Guid tempCartId = Guid.NewGuid();
        //             // Send tempCartId back to client as a cookie
        //             context.Session[MenuKey] = tempCartId.ToString();
        //         }
        //     }
        //     return context.Session[MenuKey].ToString();
        // }

        //public static Menu GetMenu(HttpContextBase context)
        //{
        //    var menu = new Menu();

        //    menu.MenuUserKey = menu.GetMenuKey(context);

        //    menu = menu.GetMenuWithValues();

        //    if (menu.CreatedDate < DateTime.Parse("2015-01-01"))
        //        menu.CreatedDate = DateTime.Now;

        //    if (string.IsNullOrEmpty(menu.Name))
        //    {
        //        var dfi = DateTimeFormatInfo.CurrentInfo;

        //        menu.Name = "Meny v." + dfi.Calendar.GetWeekOfYear(menu.CreatedDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        //    }

        //    return menu;
        //}

        //// Helper method to simplify shopping cart calls
        //public static Menu GetMenu(Controller controller)
        //{
        //    return GetMenu(controller.HttpContext);
        //}

        //public Menu GetMenuWithValues()
        //{

        //    var menu = db.Menus.Where(m => m.MenuUserKey == this.MenuUserKey && m.IsOpen).OrderByDescending(m => m.CreatedDate).FirstOrDefault();

        //    if (menu == null)
        //    {
        //        return this;
        //    }
        //    else
        //    {
        //        return menu;
        //    }

        //}

        //public void AddToMenu(Recipe recipe)
        //{

        //    var menu = db.Menus.SingleOrDefault(m => m.MenuUserKey == this.MenuUserKey && m.IsOpen);

        //    if (menu == null)
        //    {
        //        this.CreatedDate = DateTime.Now;
        //        this.IsOpen = true;
        //        db.Menus.Add(this);
        //        db.SaveChanges();
        //    }

        //    var menuRecipe = db.MenuRecipes.SingleOrDefault(
        //        m => m.MenuId == Id
        //        && m.RecipeID == Recipes.ID
        //        );

        //    if (menuRecipe == null)
        //    {

        //        menuRecipe = db.MenuRecipes.Add(new MenuRecipe
        //        {
        //            MenuId = Id,
        //            RecipeID = Recipes.ID,
        //            Servings = 4,
        //            DateCreated = DateTime.Now
        //        });
        //    }

        //    db.SaveChanges();

        //}

        //public async void RemoveFromMenu(int id)
        //{

        //    var menuRecipe = await db.MenuRecipes.SingleOrDefaultAsync(
        //    r => r.MenuId == Id
        //    && r.RecipeID == id);

        //    if (menuRecipe != null)
        //    {
        //        db.MenuRecipes.Remove(menuRecipe);
        //        db.SaveChanges();
        //    }

        //}

        //public async void SetServings(Recipe recipe, int servings)
        //{
            
        //    var rcp = await db.MenuRecipes.SingleOrDefaultAsync(mr => mr.MenuId == Id && mr.RecipeID == Recipes.ID);

        //    if (rcp != null)
        //    {
        //        rcp.Servings = servings;
        //        await db.SaveChangesAsync();
        //    }

        //}

        //public void EmptyMenu()
        //{

        //    var recipes = db.MenuRecipes.Where(r => r.MenuId == Id).ToArray();
        //    db.MenuRecipes.RemoveRange(recipes);
        //    db.SaveChanges();

        //}

        //public async Task<List<MenuRecipe>> GetRecipes()
        //{

        //    return await db.MenuRecipes.Where(r => r.MenuId == Id).Include("Recipe").ToListAsync();

        //}

    }
}