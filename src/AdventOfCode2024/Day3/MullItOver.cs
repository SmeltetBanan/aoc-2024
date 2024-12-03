using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day3;

public static partial class MullItOver
{
    [GeneratedRegex(@"(mul\(([0-9]{0,3}),([0-9]{0,3})\))", RegexOptions.None)]
    private static partial Regex MatchIfValidMultiplicationMemory();

    private enum Operation
    {
        Multiplication,
    }

    private record Instruction(int X, int Y, Operation Operation);

    private static async Task<List<Instruction>> GetCorruptedMemory()
    {
        List<Instruction> instructions = [];

        var streamReader = new StreamReader("./Day3/CorruptedMemory.txt");

        var finishedReading = false;

        while (!finishedReading)
        {
            var line = await streamReader.ReadLineAsync();

            if (line != null)
            {
                if (MatchIfValidMultiplicationMemory().IsMatch(line))
                {
                    var matchCollection = MatchIfValidMultiplicationMemory().Matches(line);

                    foreach (Match match in matchCollection)
                    {
                        instructions.Add(
                            new Instruction(
                                int.Parse(match.Groups[2].Value),
                                int.Parse(match.Groups[3].Value),
                                Operation.Multiplication
                            )
                        );
                    }
                }
            }
            else
            {
                finishedReading = true;
            }
        }

        return instructions;
    }

    private static int CalculateSumOfInstructions(List<Instruction> instructions)
    {
        return instructions.Sum(instruction =>
            instruction.Operation switch
            {
                Operation.Multiplication => instruction.X * instruction.Y,
                _ => throw new ArgumentOutOfRangeException(),
            }
        );
    }

    public static async Task<int> Part1()
    {
        var instructions = await GetCorruptedMemory();

        var sum = CalculateSumOfInstructions(instructions);

        return sum;
    }
}
