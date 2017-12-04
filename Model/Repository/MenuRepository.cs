using Microsoft.Azure.Documents.Client;
using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;

namespace Repository
{
    public class MenuRepository : RepositoryBase<Menu, string>
    {
        public MenuRepository() : base("RecipesDB", "Menus")
        {
        }
    }
}
