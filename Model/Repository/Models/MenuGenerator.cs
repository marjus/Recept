using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Recept.Models
{
    public struct PreferredMenuDescriptor 
    {
        public bool Random { get; set; }
        public int FishDishCount { get; set; }
        public int ChickenDishCount { get; set; }
        public int PorkDishCount { get; set; }
        public int BeefDishCount { get; set; }
        public int VegDishCount { get; set; }
    }

    public class MenuGenerator
    {
        public static Menu GenerateMenuAsync(PreferredMenuDescriptor pref, int? wantedRecipes = 5)
        {
            if (wantedRecipes < 1 || wantedRecipes > 7)
                return null;

            Menu m = new Menu() { CreatedDate = DateTime.Now, Name = wantedRecipes.Value + " recept (autoskapad)" };
            
            if (pref.Random)
            {
                pref.FishDishCount = 1;
                pref.ChickenDishCount = 1;
                pref.BeefDishCount = 1;
            }
            
            int takeCount = 3;

            if (wantedRecipes < 4)
            {
                takeCount = wantedRecipes.Value;
            }
            else
            {
                takeCount = wantedRecipes.Value - 2;
            }

        //    using(var db = new DAL.RecipeContext())
        //    { 
        //        var rcps = db.Recipes.OrderBy(r => Guid.NewGuid()).Take(takeCount);

        //        foreach (var rec in rcps)
        //        {
        ////            m.Recipes.Add(rec);
        //        }

        //        db.SaveChangesAsync();

        //        //for (int i = 0; i < wantedRecipes-takeCount; i++ )
        //        //{
        //        //    // find well suited recipes that minimize ingredient waiste
        //        //}
        //    }
            return m;
        }
    }
}