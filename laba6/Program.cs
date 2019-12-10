using System;

namespace laba6
{

    class Program
    {
        delegate string Cost(string name, int basecost);

        private static string Day(string name, int basecost)
        {
            return "Стоимость билета на кино '" + name + "' в данный момент равняется " + basecost * 1.1 + " рублей.";
        }

        private static string Evening(string name, int basecost)
        {
            return "Стоимость билета на кино '" + name + "' в данный момент равняется " + basecost * 1.5 + " рублей.";
        }

        private static string Night(string name, int basecost)
        {
            return "Стоимость билета на кино '" + name + "' в данный момент равняется " + basecost * 0.8 + " рублей.";
        }

        private static string Morning(string name, int basecost)
        {
            return "Стоимость билета на кино '" + name + "' в данный момент равняется " + basecost * 1.0 + " рублей.";
        }

        private static void ShowMassage(Cost CostParam, string NameParam, int BasecostParam)
        {
            Console.WriteLine(CostParam(NameParam, BasecostParam));
        }

        static void Main(string[] args)
        {
            Console.Title = "Терентьев Владислав ИУ5-33";
            Cost cst;
            if (DateTime.Now.Hour < 6) { cst = Night; }
            else
            {
                if (DateTime.Now.Hour < 10) { cst = Morning; }
                else
                {
                    if (DateTime.Now.Hour < 16) { cst = Day; }
                    else { cst = Evening; }
                }
            }
            ShowMassage(cst, "Матрица 4", 200);
            ShowMassage((name, basecost) => "Стоимость детского билета на кино '" + name + "' равняется " + basecost * 0.5 + " рублей.", "Матрица 4", 200);
            Console.ReadKey();
        }
    }
}