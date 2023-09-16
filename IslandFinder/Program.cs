using System;
using System.Collections.Generic;
using System.Numerics;

namespace IslandFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] map =
            {
                { 0, 0, 1 },
                { 0, 1, 1 },
                { 0, 0, 0 },
                { 1, 1, 0 },
                { 1, 0, 1 },
            };

            Console.WriteLine("Остров: ");

            DrawMap(map);

            Console.WriteLine();

            Console.WriteLine("Островов на этой карте: " + GetIslandCount(map));
        }

        static int GetIslandCount(int[,] map)
        {
            int islandCount = 0;

            List<Vector2> pickedStorage = new List<Vector2>();

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j <= map.Rank; j++)
                {
                    if (pickedStorage.Contains(new Vector2(i, j)) == false && map[i, j] != 0)
                    {
                        islandCount++;

                        ScanIslandPoint(map, new Vector2(i, j), pickedStorage);
                    }
                }
            }

            return islandCount;
        }

        static void ScanIslandPoint(int[,] map, Vector2 coordinate, List<Vector2> pickedStorage)
        {
            List<Vector2> islandPoints = new List<Vector2>();

            islandPoints.Add(coordinate);
            pickedStorage.Add(coordinate);

            bool islandEnd = false;
            while (islandEnd == false)
            {
                int oldIslandCount = islandPoints.Count;

                for (int k = 0; k < oldIslandCount; k++)
                {
                    Vector2[] aroundIslandPoints =
                        GetAroundIslandPoints(map, islandPoints[k], pickedStorage);

                    foreach (var point in aroundIslandPoints)
                        islandPoints.Add(point);
                }

                islandEnd = true;
                foreach (var point in islandPoints)
                {
                    if (GetAroundIslandPoints(map, point, pickedStorage).Length > 0)
                    {
                        islandEnd = false;
                        break;
                    }
                }
            }
        }

        static Vector2[] GetAroundIslandPoints(int[,] map, Vector2 point, List<Vector2> pickedStorage)
        {
            List<Vector2> islandPoints = new List<Vector2>();

            Vector2 direction = Vector2.Zero;

            for (int i = 0; i < 8; i++)
            {
                direction = PickDirection(i);

                Vector2 scanningPoint = point + direction;

                if (IsPointInMapBounds(map, scanningPoint) && pickedStorage.Contains(scanningPoint) == false)
                {
                    if (map[(int)scanningPoint.X, (int)scanningPoint.Y] == 1)
                    {
                        islandPoints.Add(scanningPoint);
                        pickedStorage.Add(scanningPoint);
                    }
                }
            }

            return islandPoints.ToArray();
        }

        static bool IsPointInMapBounds(int[,] map, Vector2 point)
        {
            if (point.X < 0 || point.X >= map.GetLength(0))
                return false;
            if (point.Y < 0 || point.Y > map.Rank)
                return false;

            return true;
        }

        static void DrawMap(int[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j <= map.Rank; j++)
                    Console.Write(map[i, j]);

                Console.WriteLine();
            }
        }

        static Vector2 PickDirection(int index)
        {
            Vector2 direction;
            
            switch (index)
            {
                case 0:
                    direction = Vector2.UnitY;
                    break;
                case 1:
                    direction = Vector2.UnitX;
                    break;
                case 2:
                    direction = -Vector2.UnitY;
                    break;
                case 3:
                    direction = -Vector2.UnitX;
                    break;
                case 4:
                    direction = Vector2.UnitY + Vector2.UnitX;
                    break;
                case 5:
                    direction = Vector2.UnitY - Vector2.UnitX;
                    break;
                case 6:
                    direction = -Vector2.UnitY + Vector2.UnitX;
                    break;
                case 7:
                    direction = -Vector2.UnitY - Vector2.UnitX;
                    break;
                default:
                    direction = Vector2.Zero;
                    break;
            }

            return direction;
        }
    }
}