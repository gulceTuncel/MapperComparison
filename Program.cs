using System;
using System.Diagnostics;
using AutoMapper;
using Mapster;
using MapperComparison.Models;

class Program
{
    static void Main(string[] args)
    {
        double totalAutoMapperTime = 0;
        double totalMapsterTime = 0;

        for (int i = 10; i <= 10000000; i *= 10)
        {
            Console.WriteLine($"\n# of mappings: {i}");

            var source = new Source
            {
                Id = 1,
                Name = "Test",
                Date = DateTime.Now,
                Description = "This is a test description",
                Amount = 12345.67m,
                IsActive = true,
                CreatedDate = DateTime.Now.AddDays(-10),
                ModifiedDate = DateTime.Now.AddDays(-5)
            };

            // AutoMapper Configuration
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Source, Destination>());
            var mapper = config.CreateMapper();

            // Mapster Configuration
            TypeAdapterConfig<Source, Destination>.NewConfig();

            // AutoMapper Performance Test
            var sw1 = new Stopwatch();
            sw1.Start();
            for (int j = 0; j < i; j++)
            {
                var dest = mapper.Map<Destination>(source);
            }
            sw1.Stop();
            Console.WriteLine($"AutoMapper: {sw1.ElapsedMilliseconds} ms");
            totalAutoMapperTime += sw1.ElapsedMilliseconds;

            // Mapster Performance Test
            var sw2 = new Stopwatch();
            sw2.Start();
            for (int j = 0; j < i; j++)
            {
                var dest = source.Adapt<Destination>();
            }
            sw2.Stop();
            Console.WriteLine($"Mapster: {sw2.ElapsedMilliseconds} ms");
            totalMapsterTime += sw2.ElapsedMilliseconds;

            double percentageDifference = ((double)Math.Abs(sw1.ElapsedMilliseconds - sw2.ElapsedMilliseconds) / Math.Max(sw1.ElapsedMilliseconds, sw2.ElapsedMilliseconds)) * 100;
            Console.WriteLine($"Percentage difference: {percentageDifference}%");

            Console.WriteLine(sw2.ElapsedMilliseconds > sw1.ElapsedMilliseconds ? "AutoMapper is faster" : "Mapster is faster");
        }

        Console.WriteLine(new String('*', 50));
        Console.WriteLine("\nTotal Performance:");
        Console.WriteLine($"AutoMapper Total Time: {totalAutoMapperTime} ms");
        Console.WriteLine($"Mapster Total Time: {totalMapsterTime} ms");
        Console.WriteLine(new String('*', 50));
        Console.WriteLine(totalMapsterTime > totalAutoMapperTime ? "\n\nOverall: AutoMapper is faster" : "\n\nOverall: Mapster is faster");
    }
}