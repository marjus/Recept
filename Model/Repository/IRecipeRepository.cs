using System;
using System.Collections.Generic;
using System.Text;
using Recipes.Models;

namespace Repository
{
    public interface IRecipeRepository : IRepositoryBase<Recipe, string>
    {
    }
}
