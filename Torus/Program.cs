using System;

namespace Torus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please input Torus size\nN = ");
            var torus = new Torus(int.Parse(Console.ReadLine()));
            torus.Build();
            torus.Topple();
            Console.ReadKey();
        }
    }

}
