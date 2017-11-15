using System;
using System.Collections.Generic;
using System.Linq;

namespace coffeepoint.app.Model
{
    public class MoneyBackEntry
    {
        public CashItem Item { get; set; }
        public int Amount { get; set; }
    }

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

        public MoneyBackEntry[] FlushUserAmount()
        {
            var result = new List<MoneyBackEntry>();
            foreach (var pair in tempMoney.Where(x => x.Value != 0).ToArray())
            {
                result.Add(new MoneyBackEntry
                {
                    Amount = tempMoney[pair.Key],
                    Item = pair.Key
                });
                tempMoney[pair.Key] = 0;
            }
            return result.ToArray();
        }

        public int SetAmount(CashItem cash, int amount)
        {
            return internalMoney[cash] = Math.Max(amount, 0);
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

        public void ResetValues()
        {
            internalMoney[CashItem.Rubles1] = 0;
            internalMoney[CashItem.Rubles2] = 0;
            internalMoney[CashItem.Rubles5] = 0;
            internalMoney[CashItem.Rubles10] = 0;
            internalMoney[CashItem.Rubles50] = 0;
            internalMoney[CashItem.Rubles100] = 0;
        }

        public bool IsEnoughMoney(CoffeeRecipe coffeeRecipe)
        {
            return GetCurrentUserAmount() >= coffeeRecipe.Cost;
        }

        public int GetCurrentUserAmount()
        {
            return tempMoney.Sum(x => x.Value * x.Key.Amount);
        }

        public Dictionary<CashItem, int> ComputeExchange(CoffeeRecipe coffee)
        {
            // non-generic algorithm, solution valid for 100-50-10-5-2-1 only
            var sortedNominals = CashItem.GetAll().OrderByDescending(x => x.Amount).ToArray();
            var availableCash = new int[6];
            var selectedCash = new int[6];

            {
                var i = 0;
                foreach (var orderedNominal in sortedNominals)
                {
                    availableCash[i++] = internalMoney[orderedNominal] + tempMoney[orderedNominal];
                }
            }

            var requiredCash = GetCurrentUserAmount() - coffee.Cost;
            var solved = false;
            for (var currentNominalIndex = 0; currentNominalIndex < sortedNominals.Length; ++currentNominalIndex)
            {
                var n = sortedNominals[currentNominalIndex];
                var requiredCount = requiredCash / n.Amount;
                var leftCost = requiredCash % n.Amount;
                if (leftCost == 0 && requiredCount <= availableCash[currentNominalIndex])
                {
                    selectedCash[currentNominalIndex] = requiredCount;
                    solved = true;
                    break;
                }

                var gotOfThisKind = requiredCount > availableCash[currentNominalIndex] ? availableCash[currentNominalIndex] : requiredCount;
                selectedCash[currentNominalIndex] = gotOfThisKind;
                requiredCash -= gotOfThisKind * n.Amount;
            }
            // handle 5-2 problem
            if (!solved)
            {
                var rubles5Index = 3;
                var rubles2Index = 4;
                var remaining2Roubles = availableCash[rubles2Index] - selectedCash[rubles2Index];
                if (requiredCash == 1 && selectedCash[rubles5Index] > 0 && remaining2Roubles > 2)
                {
                    selectedCash[rubles5Index] -= 1;
                    selectedCash[rubles2Index] += 3;
                    solved = true;
                }
            }
            if (solved)
            {
                var result = new Dictionary<CashItem, int>();
                var i = 0;
                foreach (var nominal in sortedNominals)
                {
                    result[nominal] = selectedCash[i++];
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        public void Transact(Dictionary<CashItem, int> exchange)
        {
            foreach (var item in exchange)
            {
                var cashToGetFromUser = tempMoney[item.Key] - item.Value;
                tempMoney[item.Key] -= cashToGetFromUser;
                internalMoney[item.Key] += cashToGetFromUser;
            }
        }
    }
}