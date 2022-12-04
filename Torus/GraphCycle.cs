using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Torus
{
    public class GraphCycle
    {
        public void CreateCycle(int n)
        {
            var nodeList = InitNodes(n);
            Shuffle(nodeList);
            Console.WriteLine($"\nAfter {StartCycle(nodeList, n)} times cycle repeated");
        }

        private int StartCycle(List<Node> nodeList, int n)
        {
            var paths = new HashSet<string>();
            var currentNum = 0;
            var cycleLength = 0;
            var r = 0;

            var tempNode = nodeList.Last();
            while (true)
            {
                var left = tempNode.Left == null ? int.MaxValue : nodeList.IndexOf(tempNode.Left);
                var right = tempNode.Right == null ? int.MaxValue : nodeList.IndexOf(tempNode.Right);
                var up = tempNode.Up == null ? int.MaxValue : nodeList.IndexOf(tempNode.Up);
                var down = tempNode.Down == null ? int.MaxValue : nodeList.IndexOf(tempNode.Down);

                var minIndex = Math.Min(Math.Min(left, right), Math.Min(up, down));
                var minNode = nodeList[minIndex];

                currentNum++;
                var path = new StringBuilder();
                for (int i = 0; i < nodeList.Count; i++)
                {
                    var node= nodeList[i];
                    var p = $"{node.Position.X}{node.Position.Y}";
                    p += i == nodeList.Count - 1 ? "" : "-";
                    path.Append(p);
                    Console.Write(p);
                }

                if (paths.Contains(path.ToString()))
                {
                    r++;
                    Console.Write(" REPEATED");
                    if (r == cycleLength) break;
                }
                else
                {
                    r = 0;
                    paths.Add(path.ToString());
                    cycleLength++;
                }

                Console.WriteLine();
                nodeList.RemoveAt(minIndex);
                nodeList.Add(minNode);
                tempNode = minNode;
            }
            return cycleLength;
        }

        private List<Node> InitNodes(int n)
        {
            var nodes = new Node[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    nodes[i, j] = new Node(new Position(i, j));


            var nodeList = new List<Node>();
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    var node = nodes[i, j];
                    if (j > 0) node.Up = nodes[i, j - 1];
                    if (i > 0) node.Left = nodes[i - 1, j];
                    if (j + 1 < n) node.Right = nodes[i, j + 1];
                    if (i + 1 < n) node.Down = nodes[i + 1, j];
                    nodeList.Add(node);
                }
            return nodeList;
        }

        private void Shuffle(List<Node> nodes)
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
    }
}
