using System.Text.RegularExpressions;
using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Day5;

public partial class PrintQueue
{
    [GeneratedRegex(@"((?'left'[0-9]*)\|(?'right'[0-9]*))")]
    private static partial Regex MatchIfIsPageOrderingRule();

    [GeneratedRegex(@"([0-9]+,)+|([0-9]+)")]
    private static partial Regex MatchIfIsPageOrder();

    private record PageOrdering(List<(int Left, int Right)> Rules, List<List<int>> Pages);

    private static async Task<PageOrdering> GetPageOrderingRules()
    {
        List<(int, int)> pageOrderingRules = [];
        List<List<int>> pageOrder = [];

        var lineReaderAsyncEnumerable = FileReaderHelper.ReadFile("./Day5/PageOrderingRules.txt");

        await foreach (var line in lineReaderAsyncEnumerable)
        {
            if (MatchIfIsPageOrderingRule().IsMatch(line))
            {
                var match = MatchIfIsPageOrderingRule().Match(line);

                var left = match.Groups["left"].Value;
                var right = match.Groups["right"].Value;

                pageOrderingRules.Add((int.Parse(left), int.Parse(right)));
            }
            else if (MatchIfIsPageOrder().IsMatch(line))
            {
                pageOrder.Add(line.Split(',').Select(int.Parse).ToList());
            }
        }

        return new PageOrdering(pageOrderingRules, pageOrder);
    }

    private static List<List<int>> ValidatePageOrder(PageOrdering pageOrdering)
    {
        var validPageNumberLists = new List<List<int>>();

        foreach (var pageNumbers in pageOrdering.Pages)
        {
            if (ArePageNumbersValidForRuleSet(pageNumbers, pageOrdering.Rules))
            {
                validPageNumberLists.Add(pageNumbers);
            }
        }

        return validPageNumberLists;
    }

    private static bool ArePageNumbersValidForRuleSet(
        List<int> pageNumbers,
        List<(int Left, int Right)> rules
    )
    {
        for (var i = 0; i < pageNumbers.Count; i++)
        {
            var pageNumber = pageNumbers[i];

            var applicableLeftRules = rules.Where(rule => rule.Left == pageNumber);

            foreach (var applicableLeftRule in applicableLeftRules)
            {
                var idx = i - 1;
                while (idx >= 0)
                {
                    var pageNumberToEval = pageNumbers[idx];
                    if (pageNumberToEval == applicableLeftRule.Right)
                    {
                        return false;
                    }

                    idx--;
                }
            }

            var applicableRightRules = rules.Where(rule => rule.Right == pageNumber);

            foreach (var applicableRightRule in applicableRightRules)
            {
                var idx = i + 1;
                while (idx < pageNumbers.Count)
                {
                    var pageNumberToEval = pageNumbers[idx];
                    if (pageNumberToEval == applicableRightRule.Left)
                    {
                        return false;
                    }

                    idx++;
                }
            }
        }

        return true;
    }

    private static int SumMiddleOfValidPageNumbers(List<List<int>> listOfPageNumbers)
    {
        var sum = 0;

        foreach (var pageNumber in listOfPageNumbers)
        {
            sum += pageNumber[pageNumber.Count / 2];
        }

        return sum;
    }

    public static async Task<int> Part1()
    {
        var pageOrdering = await GetPageOrderingRules();

        var validatedPages = ValidatePageOrder(pageOrdering);

        var sum = SumMiddleOfValidPageNumbers(validatedPages);

        return sum;
    }
}
