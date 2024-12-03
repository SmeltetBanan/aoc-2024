namespace AdventOfCode2024.Day2;

public static class RedNosedReports
{
    private static async Task<List<List<int>>> GetReports()
    {
        List<List<int>> reports = [];

        var streamReader = new StreamReader("./Day2/Reports.txt");

        var finishedReading = false;

        while (!finishedReading)
        {
            var line = await streamReader.ReadLineAsync();

            if (line != null)
            {
                var splitLine = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var report = splitLine.Select(int.Parse).ToList();

                reports.Add(report);
            }
            else
            {
                finishedReading = true;
            }
        }

        return reports;
    }

    private enum Direction
    {
        Increasing,
        Decreasing,
        Unchanged,
    }

    private static bool IsDiffInvalid(int diff) => diff is < 1 or > 3;

    private static bool CheckDiff(
        this List<int> levels,
        int fromIdx,
        int toIdx,
        Direction? wantedDirection = null
    )
    {
        if (fromIdx < 0)
            return false;

        if (toIdx > levels.Count - 1)
            return false;

        var validDirection = wantedDirection switch
        {
            Direction.Increasing => levels[fromIdx] < levels[toIdx],
            Direction.Decreasing => levels[fromIdx] > levels[toIdx],
            _ => true,
        };

        return !IsDiffInvalid(Math.Abs(levels[fromIdx] - levels[toIdx])) && validDirection;
    }

    private record LevelEvaluation(int FromIdx, int ToIdx, Direction Direction, int Diff);

    private static int CheckReports(List<List<int>> reports, bool problemDampener = false)
    {
        var safeReportCount = 0;

        var problemDampenedReports = new List<int>();

        for (var i = 0; i < reports.Count; i++)
        {
            var report = reports[i];

            var levelEvaluation = GetLevelEvaluations(report);

            var unchangedCount = levelEvaluation.Count(e => e.Direction == Direction.Unchanged);
            var increasingCount = levelEvaluation.Count(e => e.Direction == Direction.Increasing);
            var decreasingCount = levelEvaluation.Count(e => e.Direction == Direction.Decreasing);

            var errDiffCount = levelEvaluation.Count(eval => IsDiffInvalid(eval.Diff));

            if (unchangedCount > 0)
            {
                if (!problemDampener || problemDampenedReports.Contains(i))
                    continue;

                var unchangedLevel = levelEvaluation.First(eval =>
                    eval.Direction == Direction.Unchanged
                );
                report.RemoveAt(unchangedLevel.ToIdx);
                problemDampenedReports.Add(i);
                i--;
            }
            else if (increasingCount == decreasingCount) { }
            else if (
                increasingCount > 0
                && increasingCount > decreasingCount
                && decreasingCount > 0
            )
            {
                if (!problemDampener || problemDampenedReports.Contains(i))
                    continue;

                var decreasingLevel = levelEvaluation.First(eval =>
                    eval.Direction == Direction.Decreasing
                );

                report.RemoveInvalidDiffIndex(decreasingLevel, Direction.Increasing);

                problemDampenedReports.Add(i);
                i--;
            }
            else if (
                decreasingCount > 0
                && decreasingCount > increasingCount
                && increasingCount > 0
            )
            {
                if (!problemDampener || problemDampenedReports.Contains(i))
                    continue;

                var increasingLevel = levelEvaluation.First(eval =>
                    eval.Direction == Direction.Increasing
                );

                report.RemoveInvalidDiffIndex(increasingLevel, Direction.Decreasing);

                problemDampenedReports.Add(i);
                i--;
            }
            else if (errDiffCount > 0)
            {
                if (!problemDampener || problemDampenedReports.Contains(i))
                    continue;

                var invalidDiffEval = levelEvaluation.First(eval => IsDiffInvalid(eval.Diff));

                var wantedDirection =
                    increasingCount > 0 ? Direction.Increasing : Direction.Decreasing;

                report.RemoveInvalidDiffIndex(invalidDiffEval, wantedDirection);

                problemDampenedReports.Add(i);
                i--;
            }
            else
            {
                safeReportCount++;
            }
        }

        return safeReportCount;
    }

    private static void RemoveInvalidDiffIndex(
        this List<int> levels,
        LevelEvaluation levelEvaluation,
        Direction? wantedDirection = null
    )
    {
        if (levels.CheckDiff(levelEvaluation.FromIdx, levelEvaluation.ToIdx + 1, wantedDirection))
        {
            levels.RemoveAt(levelEvaluation.ToIdx);
        }
        else if (
            levels.CheckDiff(levelEvaluation.FromIdx - 1, levelEvaluation.ToIdx, wantedDirection)
        )
        {
            levels.RemoveAt(levelEvaluation.FromIdx);
        }
        else if (
            levels.CheckDiff(levelEvaluation.ToIdx, levelEvaluation.ToIdx + 1, wantedDirection)
        )
        {
            levels.RemoveAt(levelEvaluation.FromIdx);
        }
        else if (
            levels.CheckDiff(levelEvaluation.FromIdx - 1, levelEvaluation.FromIdx, wantedDirection)
        )
        {
            levels.RemoveAt(levelEvaluation.ToIdx);
        }
        else
        {
            levels.RemoveAt(levelEvaluation.ToIdx);
        }
    }

    private static List<LevelEvaluation> GetLevelEvaluations(List<int> levels)
    {
        List<LevelEvaluation> levelEvaluations = [];

        for (var idx = 0; idx < levels.Count; idx++)
        {
            var nextIdx = idx + 1;
            if (nextIdx >= levels.Count)
                break;

            var currentLevel = levels[idx];
            var nextLevel = levels[nextIdx];
            var diff = Math.Abs(currentLevel - nextLevel);

            var nextDirection = Direction.Unchanged;

            if (currentLevel < nextLevel)
                nextDirection = Direction.Increasing;
            else if (currentLevel > nextLevel)
                nextDirection = Direction.Decreasing;

            levelEvaluations.Add(new LevelEvaluation(idx, nextIdx, nextDirection, diff));
        }

        return levelEvaluations;
    }

    public static async Task<int> Part1()
    {
        var reports = await GetReports();

        var safeReportCount = CheckReports(reports);

        return safeReportCount;
    }

    public static async Task<int> Part2()
    {
        var reports = await GetReports();

        var safeReportCount = CheckReports(reports, true);

        return safeReportCount;
    }
}
