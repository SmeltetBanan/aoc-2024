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

    private record LevelEvaluation(int FromIdx, int ToIdx, Direction Direction, int Diff);

    private static int CheckReports(List<List<int>> reports)
    {
        var safeReportCount = 0;

        foreach (var report in reports)
        {
            var levelEvaluation = GetLevelEvaluations(report);

            var levelsWithUnchangedDirectionCount = levelEvaluation.Count(e =>
                e.Direction == Direction.Unchanged
            );
            var levelsWithIncreasingDirectionCount = levelEvaluation.Count(e =>
                e.Direction == Direction.Increasing
            );
            var levelsWithDecreasingDirectionCount = levelEvaluation.Count(e =>
                e.Direction == Direction.Decreasing
            );

            if (levelEvaluation.Count(eval => eval.Diff is < 1 or > 3) > 0) { }
            else if (levelsWithUnchangedDirectionCount > 0) { }
            else if (
                levelsWithIncreasingDirectionCount > 0
                && levelsWithDecreasingDirectionCount > 0
            ) { }
            else
            {
                safeReportCount++;
            }
        }

        return safeReportCount;
    }

    private static List<LevelEvaluation> GetLevelEvaluations(List<int> levels)
    {
        List<LevelEvaluation> levelEvaluations = [];

        for (var idx = 0; idx < levels.Count; idx++)
        {
            var nextIdx = idx + 1;
            if (nextIdx >= levels.Count)
                continue;

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

        var safeReportCount = CheckReports(reports);

        return safeReportCount;
    }
}
