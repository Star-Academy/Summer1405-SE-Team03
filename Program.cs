using System;
using System.Text.Json;
using System.Linq;

namespace HelloWorld
{
    

    class Program
    {
        static void Main(string[] args)
        {
            List<string> item = new List<string>();
            Dictionary<string, int> dict = new Dictionary<string, int>();
            int currentIndex=-1;
            while (true)
            {
              var input = Console.ReadLine();
              string[] words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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

                        break;
                    }
                    case "FORWARD":
                    {
                        if (currentIndex < item.Count - 1 && currentIndex >= 0)
                        {
                            currentIndex++;
                            Console.WriteLine("current:" + item[currentIndex]);
                        }

                        break;
                    }
                    case "CURRENT":
                    {
                        if (currentIndex >= 0)
                        {
                            Console.WriteLine("current:" + item[currentIndex]);
                        }

                        break;
                    }
                    case "STATS":
                    {
                        var top3 = dict.OrderByDescending(p => p.Value).Take(3);
                        foreach (var top in top3)
                        {
                            Console.WriteLine(top.Key + " "  + top.Value);
                        }

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
                    {
                        item.RemoveRange(currentIndex + 1, item.Count - (currentIndex + 1));
                    }
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
}