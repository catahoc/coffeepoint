using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coffeepoint.app.Model;
using Microsoft.AspNetCore.Mvc;

namespace coffeepoint.app.Controllers
{
    [Route("api/[controller]")]
    public class ResourcesController : Controller
    {
        private readonly ResourcesManagementService resourcesManagementService;
        public ResourcesController(ResourcesManagementService resourcesManagementService)
        {
            this.resourcesManagementService = resourcesManagementService;
        }

        [HttpPost("[action]")]
        public uint IncrementResourceCount(string name, uint count)
        {
            var resource = MachineResource.Match(name);
            var amountUnfit = resourcesManagementService.FullfillAndReturnRemaining(resource, count);
            return amountUnfit;
        }

        [HttpGet("{name}")]
        public uint GetResourceCount(string name)
        {
            var resource = MachineResource.Match(name);
            return resourcesManagementService.GetResourceAmount(resource);
        }

        [HttpGet]
        public ResourceEntry[] GetAllResources()
        {
            return MachineResource.GetAll().Select(x => new ResourceEntry
            {
                Name = x.Name,
                Amount = GetResourceCount(x.Name)
            }).ToArray();
        }

        public class ResourceEntry
        {
            public string Name { get; set; }
            public uint Amount { get; set; }
            public uint Limit { get; set; }
        }
    }
}
