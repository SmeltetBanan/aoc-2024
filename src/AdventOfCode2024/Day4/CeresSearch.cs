using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Day4;

public static class CeresSearch
{
    private static async Task<char[][]> GetWordSearch()
    {
        var lineReaderAsyncEnumerable = FileReaderHelper.ReadFile("./Day4/WordSearch.txt");

        List<List<char>> charLines = [];

        await foreach (var line in lineReaderAsyncEnumerable)
            charLines.Add(line.ToList());

        return charLines.Select(cl => cl.ToArray()).ToArray();
    }

    private static int GetXmasCount(char[][] charLines)
    {
        var totalCount = 0;
        for (var x = 0; x < charLines.Length; x++)
        {
            for (var y = 0; y < charLines[x].Length; y++)
            {
                if (charLines[x][y] == 'X')
                {
                    var validWordCount = charLines.ValidWordCount(x, y, ['M', 'A', 'S']);
                    totalCount += validWordCount;
                }
            }
        }

        return totalCount;
    }

    private record CoordinateWithDirection(int X, int Y, int XDirection, int YDirection);

    private static List<CoordinateWithDirection> GetSurroundingCoordinates(
        this char[][] charLines,
        int x,
        int y
    )
    {
        var maxX = charLines.Length - 1;
        var maxY = charLines[x].Length - 1;

        var coordinates = new List<CoordinateWithDirection>()
        {
            new(x - 1, y, -1, 0),
            new(x + 1, y, +1, 0),
            new(x - 1, y - 1, -1, -1),
            new(x - 1, y + 1, -1, 1),
            new(x + 1, y - 1, 1, -1),
            new(x + 1, y + 1, 1, 1),
            new(x, y - 1, 0, -1),
            new(x, y + 1, 0, 1),
        };

        //Remove invalid coords
        coordinates = coordinates
            .Where(coordinate =>
                coordinate.X >= 0
                && coordinate.Y >= 0
                && coordinate.X <= maxX
                && coordinate.Y <= maxY
            )
            .ToList();

        return coordinates;
    }

    private record ValidMasCoordinates(
        (int X, int Y)[] LeftDiagonal,
        (int X, int Y)[] RightDiagonal
    );

    private static int ValidWordCount(this char[][] charLines, int x, int y, char[] charsLeftToFind)
    {
        var validSurroundingCoordinates = charLines.GetSurroundingCoordinates(x, y);

        var nextChar = charsLeftToFind[0];
        var wordCount = 0;

        foreach (var coordinateWithDirection in validSurroundingCoordinates)
        {
            if (nextChar != charLines[coordinateWithDirection.X][coordinateWithDirection.Y])
                continue;

            if (
                charLines.CheckForNextCharacterXmas2(
                    coordinateWithDirection.X + coordinateWithDirection.XDirection,
                    coordinateWithDirection.Y + coordinateWithDirection.YDirection,
                    coordinateWithDirection.XDirection,
                    coordinateWithDirection.YDirection,
                    charsLeftToFind[1..]
                )
            )
            {
                wordCount++;
            }
        }

        return wordCount;
    }

    private static bool CheckForNextCharacterXmas2(
        this char[][] charLines,
        int x,
        int y,
        int xDirection,
        int yDirection,
        char[] charsLeftToFind
    )
    {
        if (charsLeftToFind.Length == 0)
            return true;

        var maxX = charLines.Length - 1;
        var maxY = charLines[0].Length - 1;

        if (x < 0 || y < 0 || x > maxX || y > maxY)
        {
            return false;
        }

        var nextChar = charsLeftToFind[0];

        if (nextChar == charLines[x][y])
        {
            return charLines.CheckForNextCharacterXmas2(
                x + xDirection,
                y + yDirection,
                xDirection,
                yDirection,
                charsLeftToFind[1..]
            );
        }

        return false;
    }

    private static ValidMasCoordinates? GetMasCoordinates(this char[][] charLines, int x, int y)
    {
        var maxX = charLines.Length - 1;
        var maxY = charLines[x].Length - 1;

        (int X, int Y)[] leftDiagonal = [(x - 1, y - 1), (x + 1, y + 1)];
        (int X, int Y)[] rightDiagonal = [(x + 1, y - 1), (x - 1, y + 1)];

        if (
            rightDiagonal.Any(coordinate =>
                coordinate.X < 0 || coordinate.Y < 0 || coordinate.X > maxX || coordinate.Y > maxY
            )
            || leftDiagonal.Any(coordinate =>
                coordinate.X < 0 || coordinate.Y < 0 || coordinate.X > maxX || coordinate.Y > maxY
            )
        )
            return null;

        return new ValidMasCoordinates(leftDiagonal, rightDiagonal);
    }

    private static int GetMasCount(char[][] charLines)
    {
        var totalCount = 0;
        for (var x = 0; x < charLines.Length; x++)
        {
            for (var y = 0; y < charLines[x].Length; y++)
            {
                if (charLines[x][y] != 'A')
                    continue;

                if (charLines.CheckIfIsXmas(x, y))
                {
                    totalCount++;
                }
            }
        }

        return totalCount;
    }

    private static bool CheckIfIsXmas(this char[][] charLines, int x, int y)
    {
        var validMaxCoordinates = charLines.GetMasCoordinates(x, y);

        if (validMaxCoordinates is null)
            return false;

        List<char> leftDiagonalCharacters = [];
        List<char> rightDiagonalCharacters = [];

        leftDiagonalCharacters.AddRange(
            validMaxCoordinates.LeftDiagonal.Select(coordinate =>
                charLines[coordinate.X][coordinate.Y]
            )
        );

        rightDiagonalCharacters.AddRange(
            validMaxCoordinates.RightDiagonal.Select(coordinate =>
                charLines[coordinate.X][coordinate.Y]
            )
        );

        return leftDiagonalCharacters.Count(c => c == 'M') == 1
            && leftDiagonalCharacters.Count(c => c == 'S') == 1
            && rightDiagonalCharacters.Count(c => c == 'M') == 1
            && rightDiagonalCharacters.Count(c => c == 'S') == 1;
    }

    public static async Task<int> Part1()
    {
        var charLines = await GetWordSearch();

        var count = GetXmasCount(charLines);

        return count;
    }

    public static async Task<int> Part2()
    {
        var charLines = await GetWordSearch();

        var count = GetMasCount(charLines);

        return count;
    }
}
