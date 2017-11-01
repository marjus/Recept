using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Rcpt.DAL;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recept.Models
{
    public class ShoppingList
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual List<ShoppingListItem> ShoppingListItems { get; set; }
        public bool IsOpen { get; set; }
        public int? MenuId { get; set; }
        public virtual Menu Menu { get; set; }

        public string UserName { get; set; }

        [NotMapped]
        public string Title { get; set; }

        public ShoppingList()
        {
            this.ShoppingListItems = new List<ShoppingListItem>();
        }

        public async void AddMenu(int menuId)
        {
            if (menuId == 0)
                throw new ArgumentException("Menu id missing");

            using (RecipeContext db = new RecipeContext())
            {
                var menu = await db.Menus.SingleOrDefaultAsync(m => m.Id == MenuId);
                if (menu.MenuRecipes != null)
                {
                    foreach(var recipe in menu.MenuRecipes)
                    {
                        
                    }
                }
            }
        }
    }
}