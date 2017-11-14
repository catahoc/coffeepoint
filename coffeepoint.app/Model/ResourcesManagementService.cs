using System;
using System.Collections.Generic;
using System.Linq;

namespace coffeepoint.app.Model
{
    public class ResourcesManagementService
    {
        private readonly Dictionary<MachineResource, uint> resources = new Dictionary<MachineResource, uint>();

        public ResourcesManagementService()
        {
            foreach (var machineResource in MachineResource.GetAll())
            {
                resources.Add(machineResource, 0);
            }
        }

        public void Cook(CoffeeRecipe recipe)
        {
            var enough = recipe.RequiredResources.FirstOrDefault(x => resources[x.Key] < x.Value);
            if (!Equals(enough, default(KeyValuePair<MachineResource, uint>)))
            {
                throw new Exception("Not enough " + enough.Key.Name);
            }

            foreach (var resource in recipe.RequiredResources)
            {
                resources[resource.Key] -= resource.Value;
            }
        }

        public uint IncreaseAmount(MachineResource resource, uint amount)
        {
            var newAmount = Math.Min(resources[resource] + amount, resource.Limit);
            return resources[resource] = newAmount;
        }

        public uint DecreaseAmount(MachineResource resource, uint amount)
        {
            var newAmount = Math.Max(resources[resource] - amount, 0);
            return resources[resource] = newAmount;
        }

        public uint GetResourceAmount(MachineResource resource)
        {
            return resources[resource];
        }
    }
}