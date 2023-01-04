using System;
using System.Threading.Tasks;

namespace Torus
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.BufferHeight = Int16.MaxValue - 1;
            while (true)
            {
                Console.Write("Please input size\nN = ");
                var n = int.Parse(Console.ReadLine());
                Console.Write("Please input period times\nT = ");
                var t = int.Parse(Console.ReadLine());
                if (n == 0) break;
                var graph = new GraphCycle();
                await graph.CreateCycle(n, t);
                //var torus = new Torus(n);
                //torus.Build();
                //torus.Topple();
            }
            Console.ReadKey();
        }


    }

}
