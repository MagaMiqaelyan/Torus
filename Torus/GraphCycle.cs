using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Torus
{
    public class GraphCycle
    {
        private int n;
        private int digitsCount;

        private List<Node> nodes;
        private Queue<Position> route;

        private int origRow;
        private int origCol;

        public async Task CreateCycle(int n, int t)
        {
            this.n = n;
            this.digitsCount = (int)Math.Floor(Math.Log10(n) + 1) * 2;
            InitNodes();
            Shuffle();
            //Console.WriteLine($"Array \n{NodesToString()}");

            for (int i = 0; i < t; i++)
            {
                await Cycle();
            }

            Console.WriteLine();
        }

        private async Task Cycle()
        {
            var cycleCount = StartCycle();
            Console.WriteLine($"\nAfter {cycleCount} times cycle repeated");

            origRow = Console.CursorTop;
            origCol = Console.CursorLeft;

            await ShowContureOnMatrix();

            Console.ForegroundColor = ConsoleColor.White;
            try
            {
                Console.SetCursorPosition(0, origRow + n);
            }
            catch (Exception)
            {
            }
        }

        private int StartCycle()
        {
            route = new Queue<Position>();
            var paths = new HashSet<string>();
            var visitied = new HashSet<Position>();
            var path = new StringBuilder();

            var cycleLength = 1;
            var index = 0;

            nodes.ForEach(x => x.Index = index++);

            var startNode = nodes.Last();
            var tempNode = startNode;
            tempNode.Index = index++;

            route.Enqueue(startNode.Position);
            visitied.Add(startNode.Position);
            path.Append($"{tempNode.Position.X}-{tempNode.Position.Y}");

            while (true)
            {
                if (visitied.Count == nodes.Count)
                {
                    var endNode = GetMinNeighbourNode(tempNode);
                    var isCycle = startNode == endNode;
                    if (isCycle)
                    {
                        if (paths.Contains(path.ToString()))
                        {
                            nodes.Remove(endNode);
                            nodes.Add(endNode);
                            break;
                        }

                        paths = new HashSet<string>
                        {
                            path.ToString()
                        };
                        visitied = new HashSet<Position>();
                        route = new Queue<Position>();
                        path = new StringBuilder();

                        index = 0;
                        cycleLength = 1;
                        tempNode = startNode;

                        visitied.Add(startNode.Position);
                        route.Enqueue(startNode.Position);
                        path.Append($"{tempNode.Position.X}-{tempNode.Position.Y}");

                        nodes.ForEach(x => x.Index = index++);
                        tempNode.Index = index++;
                    }
                }

                cycleLength++;
                var minNode = GetMinNeighbourNode(tempNode);
                minNode.Index = index++;

                path.Append($" {minNode.Position.X}-{minNode.Position.Y}");

                route.Enqueue(minNode.Position);
                visitied.Add(minNode.Position);

                nodes.Remove(minNode);
                nodes.Add(minNode);

                tempNode = minNode;
            }
            return cycleLength;
        }


        private void InitNodes()
        {
            var nodeArray = new Node[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    nodeArray[i, j] = new Node(new Position(i, j));

            nodes = new List<Node>();
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    var node = nodeArray[i, j];
                    if (j > 0) node.Left = nodeArray[i, j - 1];
                    if (j + 1 < n) node.Right = nodeArray[i, j + 1];

                    if (i > 0) node.Up = nodeArray[i - 1, j];
                    if (i + 1 < n) node.Down = nodeArray[i + 1, j];
                    nodes.Add(node);
                }
        }

        private void Shuffle()
        {
            var rnd = new Random();
            for (int i = nodes.Count - 1; i > 0; i--)
            {
                var j = rnd.Next(0, i + 1);
                var temp = nodes[i];
                nodes[i] = nodes[j];
                nodes[j] = temp;
            }
        }

        private async Task ShowContureOnMatrix()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    WriteAt(i, j);
                Console.WriteLine();
            }

            await Task.Delay(100);

            Console.ForegroundColor = ConsoleColor.Red;

            while (route.Count > 0)
            {
                var position = route.Dequeue();
                WriteAt(position.X, position.Y, ConsoleColor.Red);
                await Task.Delay(800);
            }
        }

        private void WriteAt(int x, int y, ConsoleColor color = ConsoleColor.White)
        {
            try
            {
                Console.SetCursorPosition(origCol + y * (digitsCount + 2), origRow + x);
                Console.Write($"{x}.{y}", color);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        private Node GetMinNeighbourNode(Node parent)
        {
            var left = parent.Left == null ? int.MaxValue : parent.Left.Index;
            var right = parent.Right == null ? int.MaxValue : parent.Right.Index;
            var up = parent.Up == null ? int.MaxValue : parent.Up.Index;
            var down = parent.Down == null ? int.MaxValue : parent.Down.Index;

            var min = Math.Min(Math.Min(left, right), Math.Min(up, down));

            if (min == down) return parent.Down;
            if (min == right) return parent.Right;
            if (min == left) return parent.Left;
            return parent.Up;
        }

    }
}
