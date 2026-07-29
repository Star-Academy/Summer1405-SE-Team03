using System;
using System.Linq;

namespace SearchHistoryApp
{
    public static class AppStrings
    {
        public const string CommandSearch = "SEARCH";
        public const string CommandBack = "BACK";
        public const string CommandForward = "FORWARD";
        public const string CommandCurrent = "CURRENT";
        public const string CommandStats = "STATS";
        public const string CommandUnique = "UNIQUE";
        public const string CommandExit = "EXIT";

        public const string PrefixCurrent = "current: ";
        public const string MsgBackEmpty = "back is empty.";
        public const string MsgForwardEmpty = "forward is empty.";
        public const string MsgCurrentEmpty = "current is empty.";
        public const string MsgStatEmpty = "Stat is empty";
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var historyManager = new SearchHistoryManager();

            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                var words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var command = words[0].ToUpper();

                switch (command)
                {
                    case AppStrings.CommandSearch:
                        if (words.Length > 1)
                        {
                            var query = words[1];
                            var current = historyManager.Search(query);
                            Console.WriteLine(AppStrings.PrefixCurrent + current);
                        }
                        break;

                    case AppStrings.CommandBack:
                        var backResult = historyManager.GoBack();
                        if (backResult != null)
                            Console.WriteLine(AppStrings.PrefixCurrent + backResult);
                        else
                            Console.WriteLine(AppStrings.MsgBackEmpty);
                        break;

                    case AppStrings.CommandForward:
                        var forwardResult = historyManager.GoForward();
                        if (forwardResult != null)
                            Console.WriteLine(AppStrings.PrefixCurrent + forwardResult);
                        else
                            Console.WriteLine(AppStrings.MsgForwardEmpty);
                        break;

                    case AppStrings.CommandCurrent:
                        var currentResult = historyManager.GetCurrent();
                        if (currentResult != null)
                            Console.WriteLine(AppStrings.PrefixCurrent + currentResult);
                        else
                            Console.WriteLine(AppStrings.MsgCurrentEmpty);
                        break;

                    case AppStrings.CommandStats:
                        var top3Words = historyManager.GetTopStats(3);
                        
                        if (!top3Words.Any())
                        {
                            Console.WriteLine(AppStrings.MsgStatEmpty);
                        }
                        else
                        {
                            foreach (var stat in top3Words)
                                Console.WriteLine($"{stat.Key}: {stat.Value}");
                        }
                        break;

                    case AppStrings.CommandUnique:
                        Console.WriteLine(historyManager.GetUniqueCount());
                        break;

                    case AppStrings.CommandExit:
                        return;
                }
            }
        }
    }
}