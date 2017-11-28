using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Recipes.Models;
using Repository;

namespace Recipes.Controllers
{
    [Route("api/[controller]")]
    public class RecipesController : Controller
    {
        private readonly RecipeRepository _db;

        public RecipesController(RecipeRepository db)
        {
            _db = db;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<Recipe>> Get()
        {
            
            return await _db.GetItemsFromCollectionAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<Recipe> Get(string id)
        {
            return await _db.GetItemFromCollectionAsync(id);
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]Recipe recipe)
        {
            await _db.AddDocumentIntoCollectionAsync(recipe);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _db.DeleteDocumentFromCollectionAsync(id);
        }
    }
}
