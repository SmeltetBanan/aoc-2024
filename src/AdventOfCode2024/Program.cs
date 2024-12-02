// See https://aka.ms/new-console-template for more information

using AdventOfCode2024;
using AdventOfCode2024.Day1;

Console.WriteLine("Choose a day, or type 'exit' to exit.:");
var consoleInput = Console.ReadLine();

if (Enum.TryParse(consoleInput, true,  out AdventChoice choice))
{
    var result = string.Empty;
    
    switch (choice)
    {
        case AdventChoice.Day1:
            result = (await HistorianHysteria.Part1()).ToString();
            break;
        case AdventChoice.Exit:
            
            break;

        default:
            throw new ArgumentOutOfRangeException();
    }
    
    Console.WriteLine(result);
}