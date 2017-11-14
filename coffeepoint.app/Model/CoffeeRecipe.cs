using System;
using System.Collections.Generic;

namespace coffeepoint.app.Model
{
    public class CoffeeRecipe
    {
        public static readonly CoffeeRecipe Espresso = new CoffeeRecipe("Espresso", 60, MakeResourceMap(10, 30, 0, MachineResource.SmallCup));
        public static readonly CoffeeRecipe Capuccino = new CoffeeRecipe("Capuccino", 100, MakeResourceMap(10, 30, 70, MachineResource.MidCup));
        public static readonly CoffeeRecipe Americano = new CoffeeRecipe("Americano", 80, MakeResourceMap(10, 100, 0, MachineResource.MidCup));
        public static readonly CoffeeRecipe DoubleEspresso = new CoffeeRecipe("Double Espresso", 100, MakeResourceMap(20, 60, 0, MachineResource.MidCup));
        public static readonly CoffeeRecipe DoubleCapuccino = new CoffeeRecipe("Double Capuccino", 180, MakeResourceMap(20, 60, 140, MachineResource.BigCup));
        public static readonly CoffeeRecipe DoubleAmericano = new CoffeeRecipe("Double Americano", 150, MakeResourceMap(20, 200, 0, MachineResource.BigCup));

        private static readonly Dictionary<string, CoffeeRecipe> Recipes = new Dictionary<string, CoffeeRecipe>();

        public string Name { get; }
        public uint Cost { get; }
        public Dictionary<MachineResource, uint> RequiredResources { get; }

        public CoffeeRecipe(string name, uint cost, Dictionary<MachineResource, uint> requiredResources)
        {
            Name = name;
            Cost = cost;
            RequiredResources = requiredResources;
            Recipes[name] = this;
        }

        public static CoffeeRecipe Match(string name)
        {
            try
            {
                return Recipes[name];
            }
            catch (Exception ex)
            {
                throw new Exception("Unknown recipe " + name, ex);
            }
        }

        private static Dictionary<MachineResource, uint> MakeResourceMap(uint coffee, uint water, uint milk,
            MachineResource cup)
        {
            var result = new Dictionary<MachineResource, uint>
            {
                {cup, 1},
                {MachineResource.CoffeeGramms, coffee},
                {MachineResource.WaterGramms, water}
            };
            if (milk > 0)
            {
                result.Add(MachineResource.MilkGramms, milk);
            }
            return result;
        }
    }
}