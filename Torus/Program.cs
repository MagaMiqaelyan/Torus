using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Torus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please input size\nN = ");
            var n = int.Parse(Console.ReadLine());
            var graph = new GraphCycle();
            graph.CreateCycle(n);
            //var torus = new Torus(n);
            //torus.Build();
            //torus.Topple();
            Console.ReadKey();
        }


    }

}
