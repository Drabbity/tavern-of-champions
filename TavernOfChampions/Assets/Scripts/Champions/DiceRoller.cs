using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TavernOfChampions.Champion
{
    public static class DiceRoller
    {
        public static List<int> RollDice(string formula)
        {
            if (formula == "")
                return new List<int>() { 0 };

            if (!Regex.IsMatch(formula, @"\dd(\d|[0-9!])"))
                throw new ArgumentException($"Invalid formula; { formula }");

            var hasRerolls = formula.EndsWith("!");
            if (hasRerolls)
                formula = formula.Remove(formula.Length - 1);

            var numbers = formula.Split('d');

            return RollDice(int.Parse(numbers[1]), int.Parse(numbers[0]), hasRerolls);
        }

        public static List<int> RollDice(int dice, int rolls = 1, bool hasRerolls = false)
        {
            List<int> rolledNumbers = new List<int>();

            for (var rollCounter = 0; rollCounter < rolls; rollCounter++)
            {
                var rolledNumber = UnityEngine.Random.Range(1, dice + 1);

                rolledNumbers.Add(rolledNumber);
                if (hasRerolls && rolledNumber == dice)
                    rolledNumbers.AddRange(RollDice(dice, hasRerolls: hasRerolls));
            }

            return rolledNumbers;
        }
    }
}
