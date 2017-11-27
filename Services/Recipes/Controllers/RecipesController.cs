using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Recipes.Models;
using Recipes.Context;

namespace Recipes.Controllers
{
    [Route("api/[controller]")]
    public class RecipesController : Controller
    {
        private readonly RecipeContext _db;

        public RecipesController(RecipeContext db)
        {
            _db = db;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Recipe> Get()
        {
            return _db.Recipes.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
