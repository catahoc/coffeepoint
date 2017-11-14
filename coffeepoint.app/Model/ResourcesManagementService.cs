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

        public uint FullfillAndReturnRemaining(MachineResource resource, uint amount)
        {
            var newAmount = resources[resource] + amount;
            if (newAmount > resource.Limit)
            {
                var returns = newAmount - resource.Limit;
                resources[resource] = resource.Limit;
                return returns;
            }
            else
            {
                resources[resource] = newAmount;
                return 0;
            }
        }

        public uint GetResourceAmount(MachineResource resource)
        {
            return resources[resource];
        }
    }
}