using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coffeepoint.app.Model;
using Microsoft.AspNetCore.Mvc;

namespace coffeepoint.app.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private readonly ResourcesService resourcesService;
        private readonly CashService cashService;

        public AdminController(ResourcesService resourcesService, CashService cashService)
        {
            this.resourcesService = resourcesService;
            this.cashService = cashService;
        }

        [HttpPost("Resources")]
        public int SetResourceCount([FromBody] SetAmountDto dto)
        {
            var resource = MachineResource.Match(dto.Name);
            ;
            return resourcesService.SetAmount(resource, dto.Amount);
        }

        [HttpGet("Resources/{name}")]
        public ResourceEntryDto GetResource(string name)
        {
            var resource = MachineResource.Match(name);
            return new ResourceEntryDto()
            {
                Name = name,
                Amount = resourcesService.GetAmount(resource)
            };
        }

        [HttpGet("Resources")]
        public ResourceEntryDto[] GetAllResources()
        {
            return MachineResource.GetAll().Select(x => GetResource(x.Name)).ToArray();
        }

        [HttpPost("Cash")]
        public int SetCashCount([FromBody] SetAmountDto dto)
        {
            var cashItem = CashItem.Match(dto.Name);
            return cashService.SetAmount(cashItem, dto.Amount);
        }

        [HttpGet("Cash")]
        public CashEntryDto[] GetAllCash()
        {
            return CashItem.GetAll().Select(x => new CashEntryDto
            {
                Amount = cashService.GetAmount(x),
                Name = x.Name,
                Paper = x.Paper
            }).ToArray();
        }

        [HttpPost("LoadInitialValues")]
        public JsonResult LoadInitialValues()
        {
            cashService.LoadInitialValues();
            resourcesService.LoadInitialValues();
            return Json("Success");
        }

        [HttpPost("GetAllMoney")]
        public JsonResult GetAllMoney()
        {
            cashService.ResetValues();
            return Json("Success");
        }

        public class ResourceEntryDto
        {
            public string Name { get; set; }
            public int Amount { get; set; }
        }

        public class CashEntryDto
        {
            public string Name { get; set; }
            public int Amount { get; set; }
            public bool Paper { get; set; }
        }

        public class SetAmountDto
        {
            public string Name { get; set; }
            public int Amount { get; set; }
        }
    }
}
