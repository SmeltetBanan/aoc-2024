namespace AdventOfCode2024.Day1;

public static class HistorianHysteria
{
    private record LocationIds(List<int> LeftIds, List<int> RightIds, int RowCount);

    private static async Task<LocationIds> GetLocationIds()
    {
        List<int> leftIds = [];
        List<int> rightIds = [];
        var rowCount = 0;

        var streamReader = new StreamReader("./Day1/LocationIds.txt");

        var finishedReading = false;

        while (!finishedReading)
        {
            var line = await streamReader.ReadLineAsync();

            if (line != null)
            {
                var splitLine = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                leftIds.Add(int.Parse(splitLine[0]));
                rightIds.Add(int.Parse(splitLine[1]));
                rowCount++;
            }
            else
            {
                finishedReading = true;
            }
        }

        return new LocationIds(leftIds, rightIds, rowCount);
    }

    private static List<int> GetDistances(LocationIds locationIds)
    {
        List<int> distances = [];

        for (var i = 0; i < locationIds.RowCount; i++)
        {
            var minLeftId = locationIds.LeftIds.Min();
            var minRightId = locationIds.RightIds.Min();

            var distance = Math.Abs(minLeftId - minRightId);
            distances.Add(distance);

            locationIds.LeftIds.Remove(minLeftId);
            locationIds.RightIds.Remove(minRightId);
        }

        return distances;
    }

    private static List<int> GetSimilarityScores(LocationIds locationIds)
    {
        List<int> similarityScores = [];

        foreach (var leftId in locationIds.LeftIds)
        {
            var occurrences = locationIds.RightIds.Count(rightId => rightId == leftId);
            var similarityScore = leftId * occurrences;

            similarityScores.Add(similarityScore);
        }

        return similarityScores;
    }

    public static async Task<int> Part1()
    {
        var locationIds = await GetLocationIds();

        var distances = GetDistances(locationIds);

        return distances.Sum();
    }

    public static async Task<int> Part2()
    {
        var locationIds = await GetLocationIds();

        var similarityScore = GetSimilarityScores(locationIds);

        return similarityScore.Sum();
    }
}
