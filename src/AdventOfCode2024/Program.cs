// See https://aka.ms/new-console-template for more information

using AdventOfCode2024;
using AdventOfCode2024.Day1;
using AdventOfCode2024.Day2;
using AdventOfCode2024.Day3;
using AdventOfCode2024.Day4;

Console.WriteLine("Choose a day, or type 'exit' to exit.:");
var consoleInput = Console.ReadLine();

if (Enum.TryParse(consoleInput, true, out AdventChoice choice))
{
    switch (choice)
    {
        case AdventChoice.Day1:
            Console.WriteLine("Part 1: " + await HistorianHysteria.Part1());
            Console.WriteLine("Part 2: " + await HistorianHysteria.Part2());
            break;
        case AdventChoice.Day2:
            Console.WriteLine("Part 1: " + await RedNosedReports.Part1());
            Console.WriteLine("Part 2: " + await RedNosedReports.Part2());
            break;
        case AdventChoice.Day3:
            Console.WriteLine("Part 1: " + await MullItOver.Part1());
            Console.WriteLine("Part 2: " + await MullItOver.Part2());
            break;
        case AdventChoice.Day4:
            // Console.WriteLine("Part 1: " + await CeresSearch.Part1());
            Console.WriteLine("Part 2: " + await CeresSearch.Part2());
            break;
        case AdventChoice.Exit:
            break;

        case AdventChoice.Day5:
        case AdventChoice.Day6:
        case AdventChoice.Day7:
        case AdventChoice.Day8:
        case AdventChoice.Day9:
        case AdventChoice.Day10:
        case AdventChoice.Day11:
        case AdventChoice.Day12:
        case AdventChoice.Day13:
        case AdventChoice.Day14:
        case AdventChoice.Day15:
        case AdventChoice.Day16:
        case AdventChoice.Day17:
        case AdventChoice.Day18:
        case AdventChoice.Day19:
        case AdventChoice.Day20:
        case AdventChoice.Day21:
        case AdventChoice.Day22:
        case AdventChoice.Day23:
        case AdventChoice.Day24:
        case AdventChoice.Day25:
        default:
            throw new ArgumentOutOfRangeException();
    }
}
