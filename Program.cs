namespace HelloWorld;
internal class Program
{
    private static void Main(string[] args)
    {
        var item = new List<string>();
        var dict = new Dictionary<string, int>();
        var currentIndex = -1;
        while (true)
        {
            var input = Console.ReadLine();
            var words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 1)
            {
                switch (words[0])
                {
                    case "BACK":
                    {
                        if (currentIndex > 0)
                        {
                            currentIndex--;
                            Console.WriteLine("current:" + item[currentIndex]);
                        }
                        else
                        {
                            Console.WriteLine("back is empty.");
                        }

                        break;
                    }
                    case "FORWARD":
                    {
                        if (currentIndex < item.Count - 1 && currentIndex >= 0)
                        {
                            currentIndex++;
                            Console.WriteLine("current:" + item[currentIndex]);
                        }
                        else
                        {
                            Console.WriteLine("forward is empty.");
                        }
                        break;
                    }
                    case "CURRENT":
                    {
                        if (currentIndex >= 0 && currentIndex < item.Count) Console.WriteLine("current:" + item[currentIndex]);
                        else
                        {
                            Console.WriteLine("current is empty.");
                        }
                        break;
                    }
                    case "STATS":
                    {
                        if (dict.Count == 0)
                        {
                            Console.WriteLine("Stat is empty");
                            break;
                        }
                        var top3 = dict.OrderByDescending(p => p.Value).Take(3);
                        foreach (var top in top3) Console.WriteLine(top.Key + " " + top.Value);
                        break;
                    }
                    case "UNIQUE":
                    {
                        Console.WriteLine(dict.Count);
                        break;
                    }
                    case "EXIT":
                    {
                        return;
                    }
                }
            }
            else
            {
                if (words[0] == "SEARCH")
                {
                    if (currentIndex < item.Count - 1 && currentIndex >= 0)
                        item.RemoveRange(currentIndex + 1, item.Count - (currentIndex + 1));

                    item.Add(words[1]);
                    currentIndex = item.Count - 1;
                    if (dict.ContainsKey(words[1]))
                        dict[words[1]]++;
                    else
                        dict.Add(words[1], 1);
                    Console.WriteLine("current:" + item[currentIndex]);
                }
            }
        }
    }
}