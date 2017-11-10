using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rcpt.Models;

namespace Rcpt.ViewModels
{
    public class EditRecipeVM
    {
        public Recipe Recipe { get; set; }
        public IEnumerable<RecipeCategory> AllRecipeCategories { get; set; }

        public string KnownUnits;
    }
}