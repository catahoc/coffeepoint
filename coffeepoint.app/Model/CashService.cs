using System.Collections.Generic;
using System.Linq;

namespace coffeepoint.app.Model
{
    public class CashService
    {
        private readonly Dictionary<CashItem, int> internalMoney = new Dictionary<CashItem, int>();
        private readonly Dictionary<CashItem, int> tempMoney = new Dictionary<CashItem, int>();

        public CashService()
        {
            foreach (var cashItem in CashItem.GetAll())
            {
                internalMoney.Add(cashItem, 0);
                tempMoney.Add(cashItem, 0);
            }
        }

        public int IncreaseUserAmount(CashItem cash)
        {
            return tempMoney[cash] += 1;
        }

        public CashItem[] FlushUserAmount()
        {
            var result = tempMoney.SelectMany(x => Enumerable.Repeat(x.Key, (int) x.Value)).ToArray();
            foreach (var pair in tempMoney)
            {
                tempMoney[pair.Key] = 0;
            }
            return result;
        }

        public int SetAmount(CashItem cash, int amount)
        {
            return internalMoney[cash] = amount;
        }

        public int GetAmount(CashItem cashItem)
        {
            return internalMoney[cashItem];
        }

        public void LoadInitialValues()
        {
            internalMoney[CashItem.Rubles1] = 10;
            internalMoney[CashItem.Rubles2] = 10;
            internalMoney[CashItem.Rubles5] = 10;
            internalMoney[CashItem.Rubles10] = 10;
            internalMoney[CashItem.Rubles50] = 10;
            internalMoney[CashItem.Rubles100] = 10;
        }
    }
}