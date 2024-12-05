namespace AdventOfCode2024.Helpers;

public static class FileReaderHelper
{
    public static async IAsyncEnumerable<string> ReadFile(string relativeFilePath)
    {
        var streamReader = new StreamReader(relativeFilePath);

        var finishedReading = false;

        while (!finishedReading)
        {
            var line = await streamReader.ReadLineAsync();

            if (line != null)
            {
                yield return line;
            }
            else
            {
                finishedReading = true;
            }
        }
    }
}
