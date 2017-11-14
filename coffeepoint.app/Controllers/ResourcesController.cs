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
        public uint IncrementResourceCount([FromBody] ResourceIncrementDto dto)
        {
            var resource = MachineResource.Match(dto.Name);
            ;
            return resourcesManagementService.IncreaseAmount(resource, dto.Delta);
        }

        [HttpPost("[action]")]
        public uint DecrementResourceCount([FromBody] ResourceIncrementDto dto)
        {
            var resource = MachineResource.Match(dto.Name);
            ;
            return resourcesManagementService.DecreaseAmount(resource, dto.Delta);
        }

        [HttpGet("{name}")]
        public ResourceEntryDto GetResourceCount(string name)
        {
            var resource = MachineResource.Match(name);
            return new ResourceEntryDto()
            {
                Name = name,
                Limit = resource.Limit,
                Amount = resourcesManagementService.GetResourceAmount(resource)
            };
        }

        [HttpGet]
        public ResourceEntryDto[] GetAllResources()
        {
            return MachineResource.GetAll().Select(x => GetResourceCount(x.Name)).ToArray();
        }

        public class ResourceEntryDto
        {
            public string Name { get; set; }
            public uint Amount { get; set; }
            public uint Limit { get; set; }
        }

        public class ResourceIncrementDto
        {
            public string Name { get; set; }
            public uint Delta { get; set; }
        }
    }
}
