﻿namespace EasyHash.Benchmark
{
    using System;
    using System.Diagnostics;
    using FizzWare.NBuilder;
    using static System.Console;
    using static Helpers.ConsoleHelper;

    class Program
    {
        private const int Times = 1000000;

        static void Main(string[] args)
        {
            try
            {
                WriteLine($"Benchmark started", ConsoleColor.Green);
                WriteLine($"Each hashing implementation will be executed {Times} times", ConsoleColor.Green);

                long manualMs = Benchmark(New<HashedManually>());
                WriteLine($"1) Hashing with regular implementation: {manualMs}");

                long easyHashMs = Benchmark(New<HashedWithEasyHash>());
                WriteLine($"2) Hashing with EasyHash: {easyHashMs}");

                long reflectionMs = Benchmark(New<HashedWithReflection>());
                WriteLine($"3) Hashing with cached reflection: {reflectionMs}");

                WriteLine($"Benchmark Finished", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message, ConsoleColor.Red);
            }

            WriteLine("Press enter to exit", ConsoleColor.Yellow);
            ReadKey();
        }

        private static T New<T>() => Builder<T>.CreateNew().Build();

        private static long Benchmark(object target, int times = Times)
        {
            var sw = new Stopwatch();
            target.GetHashCode();
            sw.Start();

            for (int i = 0; i < times; i++)
            {
                target.GetHashCode();
            }

            return sw.ElapsedMilliseconds;
        }
    }
}