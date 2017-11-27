using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Web;
using Recipes.Models;

namespace Recipes.Context
{
    public class RecipeContext : DbContext
    {
         public RecipeContext(DbContextOptions<RecipeContext> options)
            : base(options)
        {
        }
        
        public DbSet<Recipe> Recipes { get; set; }
    //    public DbSet<Ingredient> Ingredients { get; set; }

    //    public DbSet<Product> Products { get; set; }
    //    public DbSet<ShoppingList> ShoppingLists { get; set; }

    //    public DbSet<ShoppingListItem> ShoppingListItems { get; set; }

    ////    public DbSet<Menu> Menus { get; set; }

    //    public DbSet<MenuRecipe> MenuRecipes { get; set; }

    //    public DbSet<RecipeCategory> RecipeCategories { get; set; }

    //    public DbSet<Unit> Units { get; set; }

       

     //   public static DbSet<IngredientMapper> IngredientMappers;

    }
}