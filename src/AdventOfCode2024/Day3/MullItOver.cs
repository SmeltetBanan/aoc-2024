using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day3;

public static partial class MullItOver
{
    [GeneratedRegex(
        @"((?'condition'(don't\(\))|(do\(\)))|((?'operation'mul)\((?'X'[0-9]{1,3}),(?'Y'[0-9]{1,3})\)))",
        RegexOptions.ExplicitCapture
    )]
    private static partial Regex MatchIfValidMultiplicationMemoryWithCondtions();

    private enum Operation
    {
        Multiplication,
    }

    private record Instruction(int X, int Y, Operation Operation, bool Ignore);

    private static async Task<List<Instruction>> GetCorruptedMemory()
    {
        List<Instruction> instructions = [];

        var streamReader = new StreamReader("./Day3/CorruptedMemory.txt");

        var finishedReading = false;

        var ignoreNextInstructions = false;

        while (!finishedReading)
        {
            var line = await streamReader.ReadLineAsync();

            if (line != null)
            {
                if (!MatchIfValidMultiplicationMemoryWithCondtions().IsMatch(line))
                    continue;

                var matchCollection = MatchIfValidMultiplicationMemoryWithCondtions().Matches(line);

                foreach (Match match in matchCollection)
                {
                    ignoreNextInstructions = match.Groups["condition"].Value switch
                    {
                        "don't()" => true,
                        "do()" => false,
                        _ => ignoreNextInstructions,
                    };

                    if (match.Groups["operation"].Value == "mul")
                    {
                        instructions.Add(
                            new Instruction(
                                int.Parse(match.Groups["X"].Value),
                                int.Parse(match.Groups["Y"].Value),
                                Operation.Multiplication,
                                ignoreNextInstructions
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

    private static int CalculateSumOfInstructions(
        List<Instruction> instructions,
        bool checkIgnore = false
    )
    {
        var instructionsToCheck = instructions;
        if (checkIgnore)
            instructionsToCheck = instructionsToCheck
                .Where(instruction => !instruction.Ignore)
                .ToList();

        return instructionsToCheck.Sum(instruction =>
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

    public static async Task<int> Part2()
    {
        var instructions = await GetCorruptedMemory();

        var sum = CalculateSumOfInstructions(instructions, true);

        return sum;
    }
}
