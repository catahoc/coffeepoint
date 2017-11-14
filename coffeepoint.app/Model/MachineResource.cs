using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace coffeepoint.app.Model
{
    public class MachineResource
    {
        private static readonly Dictionary<string, MachineResource> Resources = new Dictionary<string, MachineResource>();

        public static readonly MachineResource SmallCup = new MachineResource("Small Cup");
        public static readonly MachineResource MidCup = new MachineResource("Mid Cup");
        public static readonly MachineResource BigCup = new MachineResource("Big Cup");
        public static readonly MachineResource CoffeeGramms = new MachineResource("Coffee Gramms");
        public static readonly MachineResource WaterGramms = new MachineResource("Water Gramms");
        public static readonly MachineResource MilkGramms = new MachineResource("Milk Gramms");

        private MachineResource(string name)
        {
            Name = name;
            Resources[name] = this;
        }

        public static MachineResource[] GetAll()
        {
            return Resources.Values.ToArray();
        }

        public static MachineResource Match(string name)
        {
            try
            {
                return Resources[name];
            }
            catch (Exception ex)
            {
                throw new Exception("Unknown ingredient" + name, ex);
            }
        }

        public string Name { get; set; }
    }
}