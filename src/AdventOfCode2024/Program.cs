// See https://aka.ms/new-console-template for more information

using AdventOfCode2024;
using AdventOfCode2024.Day1;

Console.WriteLine("Choose a day, or type 'exit' to exit.:");
var consoleInput = Console.ReadLine();

if (Enum.TryParse(consoleInput, true,  out AdventChoice choice))
{
    
    switch (choice)
    {
        case AdventChoice.Day1:
            Console.WriteLine("Part 1: " 1+ await HistorianHysteria.Part1());
            Console.WriteLine("Part 2: " + await HistorianHysteria.Part2());
            break;
        case AdventChoice.Exit:
            
            break;

        default:
            throw new ArgumentOutOfRangeException();
    }
    
}