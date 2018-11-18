using System;
using System.Diagnostics;
using System.Threading;

namespace Zadanie_3
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
            public double local_result = 0;

            public CountIntegral() { }

            public void Count(Object c)
            {
                Interval compartment = (Interval)c;

                for (double i = compartment.a; i < compartment.b; i += dx)
                {
                    if (i + dx >= compartment.b) local_result += Function(i) * (compartment.b - i);
                    else local_result += Function(i) * dx;
                }

                lock(this)
                {
                    result += local_result;
                }
            }
        }

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();

            //********************************************************// Zadanie 3 - Zapisywanie wyników dla
            // poszczególnych przedziałów i na końcu w sekcji krytycznej sumowanie ich do wspólnej dla wszystkich
            // obiektów zmiennej statycznej.

            CountIntegral count_integral1 = new CountIntegral();
            CountIntegral count_integral2 = new CountIntegral();
            CountIntegral count_integral3 = new CountIntegral();
            CountIntegral count_integral4 = new CountIntegral();

            Thread t1 = new Thread(new ParameterizedThreadStart(count_integral1.Count));
            Thread t2 = new Thread(new ParameterizedThreadStart(count_integral2.Count));
            Thread t3 = new Thread(new ParameterizedThreadStart(count_integral3.Count));
            Thread t4 = new Thread(new ParameterizedThreadStart(count_integral4.Count));

            sw.Start();
            t1.Start(new Interval(1, 10));
            t2.Start(new Interval(10, 22));
            t3.Start(new Interval(22, 33));
            t4.Start(new Interval(33, 40));
            sw.Stop();

            t1.Join(); t2.Join(); t3.Join(); t4.Join();

            Console.WriteLine("#3. Liczenie całki w 4 wątkach (z zamkiem dopiero przy sumowaniu wyników" +
                " dla poszczególnych przedziałów): ");
            Console.WriteLine("\tElapsed milliseconds: " + sw.ElapsedMilliseconds);
            Console.WriteLine("\tWynik: " + Math.Round(CountIntegral.result, 2));
            sw.Reset();


            Console.ReadKey();
        }
    }
}
