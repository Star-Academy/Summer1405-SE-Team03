using MyConsoleApp;
using SearchHistoryApp;


var historyManager = new SearchHistoryManager();

while (true)
{
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) continue;

    var words = input.Split([' '], StringSplitOptions.RemoveEmptyEntries);
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