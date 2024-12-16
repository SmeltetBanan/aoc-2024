using System.Text.RegularExpressions;
using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Day7;

public static partial class BridgeRepair
{
    [GeneratedRegex("(?'testValue'[0-9]+):")]
    private static partial Regex MatchIfIsTestValue();

    [GeneratedRegex("(?![0-9]+:)(?'number'[0-9]+)")]
    private static partial Regex MatchIfIsNumber();

    private record CalibrationEquation(long TestValue, List<long> Numbers);

    private static async Task<List<CalibrationEquation>> GetCalibrationEquations()
    {
        List<CalibrationEquation> calibrationEquations = [];

        var lineReaderAsyncEnumerable = FileReaderHelper.ReadFile(
            "./Day7/CalibrationEquations.txt"
        );

        await foreach (var line in lineReaderAsyncEnumerable)
        {
            var testValue = MatchIfIsTestValue().Match(line).Groups["testValue"].Value;

            var numbers = MatchIfIsNumber()
                .Matches(line)
                .Select(match => long.Parse(match.Groups["number"].Value))
                .ToList();

            calibrationEquations.Add(new CalibrationEquation(long.Parse(testValue), numbers));
        }

        return calibrationEquations;
    }

    private static List<List<char>> GetAllPossibleOperatorCombinations(int length)
    {
        var mulOperators = new string('*', length);
        var addOperators = new string('+', length);

        var operators = Enumerable.Range(0, length / 2).Select(_ => "*+").ToList();

        var operatorsStr = string.Join(string.Empty, operators);

        var permutations = GfG.Permute(operatorsStr).Distinct().ToList();
        permutations.Add(mulOperators);
        permutations.Add(addOperators);

        return permutations.Select(permutation => permutation.ToList()).Distinct().ToList();
    }

    private static bool IsEquationValid(CalibrationEquation equation)
    {
        var possiblePermutations = GetAllPossibleOperatorCombinations(equation.Numbers.Count);

        foreach (var possiblePermutation in possiblePermutations)
        {
            var calculatedNumber = 0L;

            for (var i = 0; i < equation.Numbers.Count; i++)
            {
                if (i == 0)
                {
                    calculatedNumber = equation.Numbers[i];
                }
                else if (possiblePermutation[i - 1] == '+')
                {
                    calculatedNumber += equation.Numbers[i];
                }
                else
                {
                    calculatedNumber *= equation.Numbers[i];
                }
            }

            if (calculatedNumber == equation.TestValue)
            {
                return true;
            }
        }

        return false;
    }

    private static long GetSumOfValidEquations(List<CalibrationEquation> calibrationEquations)
    {
        var validEquations = new List<CalibrationEquation>();

        foreach (var calibrationEquation in calibrationEquations)
            if (IsEquationValid(calibrationEquation))
                validEquations.Add(calibrationEquation);

        return validEquations.Sum(equation => equation.TestValue);
    }

    public static async Task<long> Part1()
    {
        var calibrationEquations = await GetCalibrationEquations();

        var sumOfValidEquations = GetSumOfValidEquations(calibrationEquations);

        return sumOfValidEquations;
    }
}

//https://www.geeksforgeeks.org/write-a-c-program-to-print-all-permutations-of-a-given-string/
class GfG
{
    // Function to swap characters in a string
    static string Swap(string s, int i, int j)
    {
        char[] charArray = s.ToCharArray();
        char temp = charArray[i];
        charArray[i] = charArray[j];
        charArray[j] = temp;
        return new string(charArray);
    }

    // Function to print permutations of the string
    // This function takes two parameters:
    // 1. String
    // 2. Starting index of the string.
    static void PermuteRec(string s, int idx, List<string> permutations)
    {
        // Base case
        if (idx == s.Length - 1)
        {
            permutations.Add(s);
            return;
        }

        for (int i = idx; i < s.Length; i++)
        {
            // Swapping
            s = Swap(s, idx, i);

            // First idx+1 characters fixed
            PermuteRec(s, idx + 1, permutations);

            // Backtrack
            s = Swap(s, idx, i);
        }

        permutations.Add(s);
    }

    // Wrapper function
    public static List<string> Permute(string s)
    {
        List<string> permutations = [];
        PermuteRec(s, 0, permutations);

        return permutations;
    }
}
