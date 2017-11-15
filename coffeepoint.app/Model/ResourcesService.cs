using System;
using System.Collections.Generic;
using System.Linq;

namespace coffeepoint.app.Model
{
    public class ResourcesService
    {
        private readonly Dictionary<MachineResource, int> resources = new Dictionary<MachineResource, int>();
        private readonly object safe = new object();

        public ResourcesService()
        {
            foreach (var machineResource in MachineResource.GetAll())
            {
                resources.Add(machineResource, 0);
            }
        }

        public void Cook(CoffeeRecipe recipe)
        {
            lock (safe)
            {
                var firstNonFitResource = recipe.RequiredResources.FirstOrDefault(x => resources[x.Key] < x.Value);
                if (!Equals(firstNonFitResource, default(KeyValuePair<MachineResource, int>)))
                {
                    throw new Exception("Not enough " + firstNonFitResource.Key.Name);
                }

                foreach (var resource in recipe.RequiredResources)
                {
                    resources[resource.Key] -= resource.Value;
                }
            }
        }

        public int SetAmount(MachineResource resource, int amount)
        {
            lock (safe)
            {
                return resources[resource] = Math.Max(amount, 0);
            }
        }

        public int GetAmount(MachineResource resource)
        {
            lock (safe)
            {
                return resources[resource];
            }
        }

        public void LoadInitialValues()
        {
            lock (safe)
            {
                resources[MachineResource.BigCup] = 10;
                resources[MachineResource.MidCup] = 10;
                resources[MachineResource.SmallCup] = 10;
                resources[MachineResource.CoffeeGramms] = 80;
                resources[MachineResource.MilkGramms] = 700;
                resources[MachineResource.WaterGramms] = 700;
            }
        }

        public bool IsEnoughResources(CoffeeRecipe coffeeRecipe)
        {
            lock (safe)
            {
                return coffeeRecipe.RequiredResources.All(x => resources[x.Key] >= x.Value);
            }
        }
    }
}