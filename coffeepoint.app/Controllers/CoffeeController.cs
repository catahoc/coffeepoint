using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coffeepoint.app.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace coffeepoint.app.Controllers
{
    [SingleAccess]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CoffeeController : Controller
    {
        private readonly ResourcesService resourcesService;
        private readonly CashService cashService;

        public CoffeeController(ResourcesService resourcesService, CashService cashService)
        {
            this.resourcesService = resourcesService;
            this.cashService = cashService;
        }

        [HttpGet]
        public CoffeeScreen GetAll()
        {
            return new CoffeeScreen
            {
                Coffees = CoffeeRecipe.GetAll().Select(x => new CoffeeCard
                {
                    Name = x.Name,
                    Cost = x.Cost,
                    IsAvailable = resourcesService.IsEnoughResources(x),
                    IsEnoughMoney = cashService.IsEnoughMoney(x),
                    IsHaveExchange = cashService.ComputeExchange(x) != null
                }).ToArray(),
                CashItems = CashItem.GetAll().Select(x => new CashCard
                {
                    Name = x.Name
                }).ToArray(),
                CurrentAmount = cashService.GetCurrentUserAmount()
            };
        }

        [HttpPost("PutCash")]
        public JsonResult PutCash([FromBody]CashCard cash)
        {
            var cashItem = CashItem.Match(cash.Name);
            cashService.IncreaseUserAmount(cashItem);
            return Json("Success");
        }

        [HttpPost("GetMoneyBack")]
        public MoneyBackEntry[] GetMoneyBack()
        {
            return cashService.FlushUserAmount().Select(x => new MoneyBackEntry
            {
                Name = x.Item.Name,
                Count = x.Amount
            }).ToArray();
        }

        [HttpPost("Order")]
        public JsonResult PostOrder([FromBody]Order order)
        {
            var coffee = CoffeeRecipe.Match(order.Name);
            var exchange = cashService.ComputeExchange(coffee);
            var isValid = resourcesService.IsEnoughResources(coffee) && cashService.IsEnoughMoney(coffee) && exchange != null;
            if (!isValid)
            {
                throw new Exception("Cannot create coffee");
            }
            resourcesService.Cook(coffee);
            cashService.Transact(exchange);
            resourcesService.Cook(coffee);

            return Json("Success");
        }

        public class Order
        {
            public string Name { get; set; }
        }


        public class MoneyBackEntry
        {
            public string Name { get; set; }
            public int Count { get; set; }
        }

        public class CoffeeScreen
        {
            public CoffeeCard[] Coffees { get; set; }
            public CashCard[] CashItems { get; set; }
            public int CurrentAmount { get; set; }
        }

        public class CashCard
        {
            public string Name { get; set; }
        }

        public class CoffeeCard
        {
            public string Name { get; set; }
            public int Cost { get; set; }
            public bool IsAvailable { get; set; }
            public bool IsEnoughMoney { get; set; }
            public bool IsHaveExchange { get; set; }
        }
    }
}