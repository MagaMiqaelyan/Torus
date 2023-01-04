using System;
using System.Collections.Generic;

namespace Torus
{
    public class SandPile
    {
        private int[,] grid;
        private Queue<Position> overweight;
        private int n;
        public int _nodeCount;

        public SandPile(int n)
        {
            this.n = n;
            this.grid = new int[n, n];
            overweight = new Queue<Position>();
        }

        public int[,] GetGrid => grid;

        public SandPile(int n, int[,] grid) : this(n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    this.grid[i, j] = grid[i, j];
                    if (this.grid[i, j] > 3)
                        overweight.Enqueue(new Position(i, j));
                }
            }
        }

        public void Topple()
        {
            while (overweight.Count > 0)
            {
                var current = overweight.Dequeue();
                var height = grid[current.X, current.Y];
                if (height > 3)
                {
                    Console.WriteLine("Topple Node {0} {1}", current.X, current.Y);
                    _nodeCount++;
                    Topple(current.X, current.Y, height);
                }
            }
            Console.WriteLine("TOOPLE NODES COUNT {0}", _nodeCount);
        }

        public void Topple(int i, int j, int height)
        {
            grid[i, j] = height % 4;
            var fillCount = height / 4;
            if (i - 1 >= 0)
            {
                grid[i - 1, j] += fillCount;
                if (grid[i - 1, j] > 3)
                    overweight.Enqueue(new Position(i - 1, j));
            }
            if (i + 1 < n)
            {
                grid[i + 1, j] += fillCount;
                if (grid[i + 1, j] > 3)
                    overweight.Enqueue(new Position(i + 1, j));
            }
            if (j - 1 >= 0)
            {
                grid[i, j - 1] += fillCount;
                if (grid[i, j - 1] > 3)
                    overweight.Enqueue(new Position(i, j - 1));
            }
            if (j + 1 < n)
            {
                grid[i, j + 1] += fillCount;
                if (grid[i, j + 1] > 3)
                    overweight.Enqueue(new Position(i, j + 1));
            }

            if (i - 1 == 0)
            {
                grid[i - 1, j] += fillCount;
                if (grid[i - 1, j] > 3)
                    overweight.Enqueue(new Position(i - 1, j));
            }
            if (j - 1 == 0)
            {
                grid[i, j - 1] += fillCount;
                if (grid[i, j - 1] > 3)
                    overweight.Enqueue(new Position(i, j - 1));
            }
        }

        public void RandomFill(double grains)
        {
            var rnd = new Random();
            for (int i = 0; i < grains; i++)
            {
                var k = rnd.Next(0, n);
                var m = rnd.Next(0, n);
                grid[k, m]++;
                if (grid[k, m] > 3)
                    overweight.Enqueue(new Position(k, m));

            }
        }

        public double Density()
        {
            var sum = 0.0;
            foreach (var item in grid)
            {
                sum += item;
            }
            return sum / (n * n);
        }

    }

}
