using System;
using System.Collections.Generic;
using System.Text;

namespace GraphCycle
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please input size\nN = ");
            var n = int.Parse(Console.ReadLine());
            Create(n);
            Console.ReadKey();

        }

        private static void Create(int n)
        {
            var matrix = new double[n, n];
            var directions = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>( -1, 0 ),
                new KeyValuePair<int, int>(1, 0),
                new KeyValuePair<int, int>(0, -1 ),
                new KeyValuePair<int, int>( 0, 1 )
            };
            var temp = Math.Pow(n, 2) + 1;
            int i = 0, j = 0;
            var path = new StringBuilder();
            var paths = new HashSet<string>();
            var cycleLength = 0;
            var currentN = 0;
            while (true)
            {
                if (matrix[i, j] == 0)
                    matrix[i, j] = temp++;
                var minNeighbour = double.MaxValue;
                var minX = 0;
                var minY = 0;
                var rnd = new Random();
                while (true)
                {
                    var index = rnd.Next(0, 4);
                    var k = i + directions[index].Key;
                    var m = j + directions[index].Value;
                    if (k >= 0 && k < n && m >= 0 && m < n)
                    {
                        if (minNeighbour > matrix[k, m])
                        {
                            minNeighbour = matrix[k, m];
                            minX = k;
                            minY = m;
                            break;
                        }
                    }

                }
                i = minX;
                j = minY;

                if (i < 0) i = n - 1;
                if (j < 0) j = n - 1;

                if (i >= n) i = 0;
                if (j >= n) j = 0;

                currentN++;
                path.Append($"{i}{j}");

                if (currentN == Math.Pow(n, 2))
                {
                    var current = path.ToString();
                    if (paths.Contains(current)) break;
                    paths.Add(current);
                    path = new StringBuilder();
                    currentN = 0;
                    cycleLength++;
                }
            }

            Console.WriteLine($"After {cycleLength} times cycle repeated");
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
