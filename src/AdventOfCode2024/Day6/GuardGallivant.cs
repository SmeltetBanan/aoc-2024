using System.Runtime.Intrinsics.X86;
using AdventOfCode2024.Day5;
using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Day6;

public class GuardGallivant
{
    private static async Task<char[][]> GetMap()
    {
        List<char[]> mapMatrix = [];

        var lineReaderAsyncEnumerable = FileReaderHelper.ReadFile("./Day6/Map.txt");

        await foreach (var line in lineReaderAsyncEnumerable)
        {
            mapMatrix.Add(line.ToArray());
        }

        return mapMatrix.ToArray();
    }

    private enum Direction
    {
        Up,
        Down,
        Right,
        Left,
    }

    private record struct Guard(int X, int Y, Direction Direction);

    private static Guard FindStartPosition(char[][] mapMatrix)
    {
        for (var x = 0; x < mapMatrix.Length; x++)
        {
            for (var y = 0; y < mapMatrix[x].Length; y++)
            {
                var character = mapMatrix[x][y];

                switch (character)
                {
                    case '^':
                        return new Guard(x, y, Direction.Up);
                    case 'v':
                        return new Guard(x, y, Direction.Down);
                    case '<':
                        return new Guard(x, y, Direction.Left);
                    case '>':
                        return new Guard(x, y, Direction.Right);
                }
            }
        }

        return new Guard(0, 0, Direction.Up);
    }

    private static List<(int X, int Y)> GetAllObstructions(char[][] mapMatrix)
    {
        List<(int X, int Y)> obstructionCoords = [];

        for (var x = 0; x < mapMatrix.Length; x++)
        {
            for (var y = 0; y < mapMatrix[x].Length; y++)
            {
                var character = mapMatrix[x][y];

                if (character == '#')
                    obstructionCoords.Add((x, y));
            }
        }

        return obstructionCoords;
    }

    private static bool AreCoordinatesOutOfBounds(int x, int y, int maxX, int maxY) =>
        x > maxX || y > maxY || x < 0 || y < 0;

    private static (int X, int Y) GetNextCoordinates(int x, int y, Direction direction) =>
        direction switch
        {
            Direction.Up => (x - 1, y),
            Direction.Right => (x, y + 1),
            Direction.Down => (x + 1, y),
            Direction.Left => (x, y - 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
        };

    private static List<(int X, int Y)> MoveGuard(char[][] mapMatrix)
    {
        var guard = FindStartPosition(mapMatrix);
        List<(int X, int Y)> visitedCoords = [(guard.X, guard.Y)];

        var maxX = mapMatrix.Length - 1;
        var maxY = mapMatrix[guard.X].Length - 1;

        var outOfBounds = false;

        while (!outOfBounds)
        {
            var (nextX, nextY) = GetNextCoordinates(guard.X, guard.Y, guard.Direction);

            outOfBounds = AreCoordinatesOutOfBounds(nextX, nextY, maxX, maxY);

            if (outOfBounds)
                continue;

            if (mapMatrix[nextX][nextY] == '#')
            {
                guard.Direction = guard.Direction switch
                {
                    Direction.Up => Direction.Right,
                    Direction.Right => Direction.Down,
                    Direction.Down => Direction.Left,
                    Direction.Left => Direction.Up,
                    _ => guard.Direction,
                };
            }
            else
            {
                guard.X = nextX;
                guard.Y = nextY;
                visitedCoords.Add((nextX, nextY));
            }
        }

        return visitedCoords.Distinct().ToList();
    }

    private static async Task<int> GetPotentialObstructionCount(char[][] mapMatrix)
    {
        var obstructionPoint = 0;

        var visitedCoords = MoveGuard(mapMatrix);

        foreach (var visitedCoordinate in visitedCoords)
        {
            if (await CheckIfObstructionBecomesLoop(visitedCoordinate.X, visitedCoordinate.Y))
            {
                obstructionPoint++;
            }
        }

        return obstructionPoint;
    }

    private static async Task<bool> CheckIfObstructionBecomesLoop(int x, int y)
    {
        var localMapMatrix = await GetMap();

        localMapMatrix[x][y] = '#';

        return CheckNewMap(localMapMatrix, x, y);
    }

    private static bool CheckNewMap(char[][] mapMatrix, int x, int y)
    {
        var guard = FindStartPosition(mapMatrix);

        if (x == guard.X && y == guard.Y)
            return false;

        var maxX = mapMatrix.Length - 1;
        var maxY = mapMatrix[guard.X].Length - 1;

        var outOfBounds = false;

        var loop = 0;

        List<(int X, int Y)> visitedCoordinates = [(guard.X, guard.Y)];

        while (loop < 20)
        {
            var (nextX, nextY) = GetNextCoordinates(guard.X, guard.Y, guard.Direction);

            outOfBounds = AreCoordinatesOutOfBounds(nextX, nextY, maxX, maxY);

            if (outOfBounds)
                break;

            if (mapMatrix[nextX][nextY] == '#')
            {
                guard.Direction = guard.Direction switch
                {
                    Direction.Up => Direction.Right,
                    Direction.Right => Direction.Down,
                    Direction.Down => Direction.Left,
                    Direction.Left => Direction.Up,
                    _ => guard.Direction,
                };

                // if (nextX == x && nextY == y)
                // {
                //     loop++;
                // }
            }
            else
            {
                if (
                    visitedCoordinates.Count(coordinate =>
                        coordinate.X == nextX && coordinate.Y == nextY
                    ) > 2
                )
                {
                    loop++;
                }

                visitedCoordinates.Add((nextX, nextY));
                guard.X = nextX;
                guard.Y = nextY;
            }
        }

        return !outOfBounds;
    }

    public static async Task<int> Part1()
    {
        var mapMatrix = await GetMap();

        var visitedPoints = MoveGuard(mapMatrix);

        return visitedPoints.Count;
    }

    public static async Task<int> Part2()
    {
        var mapMatrix = await GetMap();

        var moveCount = await GetPotentialObstructionCount(mapMatrix);

        return moveCount;
    }

    static char[][] CopyArrayBuiltIn(char[][] source)
    {
        var len = source.Length;
        var dest = new char[len][];

        for (var x = 0; x < len; x++)
        {
            var inner = source[x];
            var ilen = inner.Length;
            var newer = new char[ilen];
            Array.Copy(inner, newer, ilen);
            dest[x] = newer;
        }

        return dest;
    }
}
