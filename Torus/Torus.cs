using System;
using System.Collections.Generic;

namespace Torus
{
    public class Torus
    {
        private Node[,] _nodes;
        private List<Position> _edges;
        private Queue<Position> _overweight;
        private int _toppledNodesCount;
        private int _n;

        public Torus(int n)
        {
            _nodes = new Node[n, n];
            _edges = new List<Position>();
            _overweight = new Queue<Position>();
            _n = n;
        }

        public void Build()
        {
            Initialize();
            for (int i = 0; i < _n; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    var node = _nodes[i, j];
                    if (!node.IsEdge)
                    {
                        var left = j == 0 ? _n - 1 : j - 1;
                        node.Left = _nodes[i, left];

                        var down = i == _n - 1 ? 0 : i + 1;
                        node.Down = _nodes[down, j];

                        var up = i == 0 ? _n - 1 : i - 1;
                        if (!_nodes[up, j].IsEdge)
                            node.Up = _nodes[up, j];

                        var right = j == _n - 1 ? 0 : j + 1;
                        if (!_nodes[i, right].IsEdge)
                            node.Right = _nodes[i, right];
                    }
                    else
                    {
                        var left = j == 0 ? _n - 1 : j - 1;
                        if (j - 1 >= 0 && _nodes[i, j - 1].IsEdge)
                            node.Left = _nodes[i, left];

                        var down = i == _n - 1 ? 0 : i + 1;
                        if (i + 1 < _n && _nodes[i + 1, j].IsEdge)
                            node.Down = _nodes[down, j];


                        node.Up = _nodes[i == 0 ? _n - 1 : i - 1, j];
                        //if (node.Down == null || !node.Down.IsEdge || !node.Up.IsEdge)
                        node.Right = _nodes[i, j == _n - 1 ? 0 : j + 1];
                    }
                    node.Weight = 2;
                }
            }

            for (int i = 0; i < _edges.Count; i += 2)
            {
                var e = _edges[i];
                _nodes[e.X, e.Y].Weight = 4;
                _overweight.Enqueue(e);
            }
            if (_n % 2 != 0)
            {
                _nodes[_n / 2, _n / 2].Weight = 4;
                _overweight.Enqueue(new Position(_n / 2, _n / 2));
            }
            Print();
        }

        public void Topple()
        {
            while (_overweight.Count > 0)
            {
                var current = _overweight.Dequeue();
                var node = _nodes[current.X, current.Y];
                if (node.Weight > 3)
                {
                    _toppledNodesCount++;
                    Console.WriteLine("{2}: Topple Node {0} {1}", current.X, current.Y, _toppledNodesCount);
                    Topple(current.X, current.Y, node.Weight);
                }
            }
            Console.WriteLine("TOOPLE NODES COUNT {0}", _toppledNodesCount);
        }

        private void Topple(int i, int j, int height)
        {
            var node = _nodes[i, j];
            node.Weight = height % 4;
            var fillCount = height / 4;

            if (node.Left != null)
            {
                node.Left.Weight += fillCount;
                if (node.Left.Weight > 3)
                    _overweight.Enqueue(node.Left.Position);
            }

            if (node.Right != null)
            {
                node.Right.Weight += fillCount;
                if (node.Right.Weight > 3)
                    _overweight.Enqueue(node.Right.Position);
            }

            if (node.Up != null)
            {
                node.Up.Weight += fillCount;
                if (node.Up.Weight > 3)
                    _overweight.Enqueue(node.Up.Position);
            }

            if (node.Down != null)
            {
                node.Down.Weight += fillCount;
                if (node.Down.Weight > 3)
                    _overweight.Enqueue(node.Down.Position);
            }
        }

        private void Initialize()
        {
            for (int i = 0; i < _n; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    _nodes[i, j] = new Node(new Position(i, j));
                    //if (j == (N - 1) || i == 0)
                    //  if (i == (N - 1) || j == 0)
                    //if (i == j || (i - 1) == j)
                    //{
                    //    Nodes[i, j].Weight = 1;
                    //    Nodes[i, j].IsEdge = true;
                    //    edges.Add(Nodes[i, j].Index);
                    //}
                }
            }
            CutTorus();
            Print();
        }

        private void CutTorus()
        {
            var directions = new[] { -1, 1 };
            int m = 0, k = 0;
            var rnd = new Random();

            var i = rnd.Next(0, _n);
            var j = rnd.Next(0, _n);
            var upDown = directions[rnd.Next(0, 2)];
            var rightLeft = directions[rnd.Next(0, 2)];
            _nodes[i, j].Weight = 1;
            _nodes[i, j].IsEdge = true;
            _edges.Add(_nodes[i, j].Position);
            Console.WriteLine("Cut path \n{0} {1}", i, j);

            while (m < (_n - 1) && k < (_n - 1))
            {
                var direction = rnd.Next(0, 2);
                if (direction == 1)
                {
                    i += upDown;
                    m++;
                }
                else
                {
                    j += rightLeft;
                    k++;
                }

                if (i >= _n)
                {
                    i = 0;
                    m--;
                }
                else if (i < 0)
                {
                    i = _n - 1;
                    m--;
                }

                if (j >= _n)
                {
                    j = 0;
                    k--;
                }
                else if (j < 0)
                {
                    j = _n - 1;
                    k--;
                }

                if (_nodes[i, j].Weight != 1)
                {
                    _nodes[i, j].Weight = 1;
                    _nodes[i, j].IsEdge = true;
                    _edges.Add(_nodes[i, j].Position);
                }
                Console.WriteLine("{0} {1}", i, j);
            }

            while (m < (_n - 1))
            {
                i += upDown;
                m++;

                if (i >= _n)
                {
                    i = 0;
                    m--;
                }
                else if (i < 0)
                {
                    i = _n - 1;
                    m--;
                }

                if (_nodes[i, j].Weight != 1)
                {
                    _edges.Add(_nodes[i, j].Position);
                    _nodes[i, j].Weight = 1;
                    _nodes[i, j].IsEdge = true;
                }
                Console.WriteLine("{0} {1}", i, j);
            }

            while (k < (_n - 1))
            {
                j += rightLeft;
                k++;
                if (j >= _n)
                {
                    j = 0;
                    k--;
                }
                else if (j < 0)
                {
                    j = _n - 1;
                    k--;
                }
                if (_nodes[i, j].Weight != 1)
                {
                    _nodes[i, j].Weight = 1;
                    _nodes[i, j].IsEdge = true;
                    _edges.Add(_nodes[i, j].Position);
                }
                Console.WriteLine("{0} {1}", i, j);
            }
        }

        private void Print()
        {
            Console.WriteLine("//////////////////////////////////");
            for (int i = 0; i < _n; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    Console.Write(_nodes[i, j].Weight + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
