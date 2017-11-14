using System;
using System.Collections.Generic;
using System.Linq;

namespace coffeepoint.app.Model
{
    public class CashItem
    {
        private static readonly Dictionary<string, CashItem> CashItems = new Dictionary<string, CashItem>();

        public static readonly CashItem Rubles1 = new CashItem("1 ruble", 1, false);
        public static readonly CashItem Rubles2 = new CashItem("2 rubles", 2, false);
        public static readonly CashItem Rubles5 = new CashItem("5 rubles", 5, false);
        public static readonly CashItem Rubles10 = new CashItem("10 rubles", 10, false);
        public static readonly CashItem Rubles50 = new CashItem("50 rubles", 50, true);
        public static readonly CashItem Rubles100 = new CashItem("100 rubles", 100, true);

        private CashItem(string name, int amount, bool paper)
        {
            Name = name;
            Amount = amount;
            Paper = paper;
            CashItems[name] = this;
        }

        public static CashItem[] GetAll()
        {
            return CashItems.Values.ToArray();
        }

        public static CashItem Match(string name)
        {
            try
            {
                return CashItems[name];
            }
            catch (Exception ex)
            {
                throw new Exception("Unknown cash" + name, ex);
            }
        }

        public string Name { get; set; }
        public int Amount { get; set; }
        public bool Paper { get; }
    }
}