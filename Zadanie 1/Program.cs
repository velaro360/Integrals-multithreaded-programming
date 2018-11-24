using System;
using System.Diagnostics;
using System.Threading;

namespace Zadanie_1
{
    class Program
    {
        public static double Function(double x)
        {
            return 3 * Math.Pow(x, 3) + Math.Cos(7 * x) - Math.Log(2 * x);
        }

        class Interval
        {
            public double a, b;

            public Interval(double a, double b)
            {
                this.a = a;
                this.b = b;
            }
        }

        class CountIntegral
        {
            private static readonly double dx = 0.00001;
            public static double result = 0;

            public CountIntegral() { }

            public void Count(Object o)
            {
                Interval interval = (Interval)o;

                for (double i = interval.a; i < interval.b; i += dx)
                {
                    if (i + dx >= interval.b) result += Function(i) * (interval.b - i);
                    else result += Function(i) * dx;
                }
                   
            }
        }

        static void Main(string[] args)
        {
            CountIntegral count_integral = new CountIntegral();

            Stopwatch sw = new Stopwatch();

            //********************************************************// Liczenie w 1 wątku

            Console.WriteLine("Liczenie całki normalnie - w jednym wątku: ");
            sw.Start();
            count_integral.Count(new Interval(1, 40));
            sw.Stop();
            Console.WriteLine("\tElapsed milliseconds: " + sw.ElapsedMilliseconds);
            Console.WriteLine("\tWynik: " + Math.Round(CountIntegral.result, 2));
            CountIntegral.result = 0;
            sw.Reset();

            //********************************************************// Zadanie 1 - Liczenie bez zamka w wielu wątkach

            Thread t1 = new Thread(new ParameterizedThreadStart(count_integral.Count));
            Thread t2 = new Thread(new ParameterizedThreadStart(count_integral.Count));
            Thread t3 = new Thread(new ParameterizedThreadStart(count_integral.Count));
            Thread t4 = new Thread(new ParameterizedThreadStart(count_integral.Count));

            sw.Start();
            t1.Start(new Interval(1, 10));
            t2.Start(new Interval(10, 22));
            t3.Start(new Interval(22, 33));
            t4.Start(new Interval(33, 40));

            t1.Join(); t2.Join(); t3.Join(); t4.Join();
            sw.Stop();

            Console.WriteLine("\n#1. Liczenie całki w 4 wątkach (bez zamka): ");
            Console.WriteLine("\tElapsed milliseconds: " + sw.ElapsedMilliseconds);
            Console.WriteLine("\tWynik: " + Math.Round(CountIntegral.result, 2));
            CountIntegral.result = 0;
            sw.Reset();


            Console.ReadKey();
        }
    }
}
