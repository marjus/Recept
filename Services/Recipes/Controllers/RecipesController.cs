using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Recipes.Models;
using Repository;
using System;

namespace Recipes.Controllers
{
    [Route("api/[controller]")]
    public class RecipesController : Controller
    {
        private readonly IRecipeRepository _repo;

        public RecipesController(IRecipeRepository repo)
        {
            _repo = repo;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<Recipe>> Get()
        {
            var recipe = new Recipe();
            recipe.ID = Guid.NewGuid().ToString();
            recipe.Title = "My second recipe";
            recipe.CreatedDate = DateTime.Now;
            recipe.SubRecipes = new List<SubRecipe>();
            recipe.SubRecipes.Add(new SubRecipe { Title = "Sallad", IngredientsText="200 g icebergsallad, 2 tomater" });
            var addedRcp = _repo.AddDocumentIntoCollectionAsync(recipe);

            return await _repo.GetItemsFromCollectionAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<Recipe> Get(string id)
        {
            return await _repo.GetItemFromCollectionAsync(id);
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]Recipe recipe)
        {
            await _repo.AddDocumentIntoCollectionAsync(recipe);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody]Recipe recipe)
        {
            await _repo.UpdateDocumentFromCollection(id, recipe);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _repo.DeleteDocumentFromCollectionAsync(id);
        }
    }
}
