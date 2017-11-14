using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coffeepoint.app.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace coffeepoint.app.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CoffeeController : Controller
    {
        [HttpGet]
        public CoffeeCard[] GetAll()
        {
            return CoffeeRecipe.GetAll().Select(x => new CoffeeCard
            {
                Name = x.Name,
                Cost = x.Cost
            }).ToArray();
        }

        public class CoffeeCard
        {
            public string Name { get; set; }
            public int Cost { get; set; }
        }
    }
}