using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs;

public static class Lab4
{
    private class City(int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
    }

    private class Tour
    {
        public List<City> Cities { get; set; }
        public double Distance { get; set; }

        public Tour(List<City> cities)
        {
            Cities = new List<City>(cities);
            Distance = CalculateDistance();
        }

        public double CalculateDistance()
        {
            double totalDistance = 0;
            for (int i = 0; i < Cities.Count; i++)
            {
                var from = Cities[i];
                var to = Cities[(i + 1) % Cities.Count];
                totalDistance += Math.Sqrt(Math.Pow(from.X - to.X, 2) + Math.Pow(from.Y - to.Y, 2));
            }
            return totalDistance;
        }
    }

    private class GeneticAlgorithm
    {
        private List<Tour> Population;
        private int PopulationSize;
        private double MutationRate;
        private Random Random;

        public GeneticAlgorithm(List<City> cities, int populationSize, double mutationRate)
        {
            PopulationSize = populationSize;
            MutationRate = mutationRate;
            Random = new Random();
            Population = new List<Tour>();

            for (int i = 0; i < PopulationSize; i++)
            {
                Population.Add(CreateRandomTour(cities));
            }
        }

        private Tour CreateRandomTour(List<City> cities)
        {
            var shuffledCities = cities.OrderBy(c => Random.Next()).ToList();
            return new Tour(shuffledCities);
        }

        public Tour Evolve()
        {
            while (true)
            {
                Population = Population.OrderBy(t => t.Distance).ToList();

                var newPopulation = new List<Tour>();
                for (int i = 0; i < PopulationSize; i++)
                {
                    var parent1 = SelectParent();
                    var parent2 = SelectParent();
                    var child = Crossover(parent1, parent2);
                    Mutate(child);
                    newPopulation.Add(child);
                }

                Population = newPopulation;
                if (Population[0].Distance < 100) // Stop condition can be adjusted
                {
                    break;
                }
            }
            return Population[0];
        }

        private Tour SelectParent()
        {
            return Population[Random.Next(PopulationSize / 2)];
        }

        private Tour Crossover(Tour parent1, Tour parent2)
        {
            var start = Random.Next(parent1.Cities.Count);
            var end = Random.Next(start, parent1.Cities.Count);

            var childCities = new List<City>();
            childCities.AddRange(parent1.Cities.GetRange(start, end - start));
            childCities.AddRange(parent2.Cities.Where(c => !childCities.Contains(c)));

            return new Tour(childCities);
        }

        private void Mutate(Tour tour)
        {
            for (int i = 0; i < tour.Cities.Count; i++)
            {
                if (Random.NextDouble() < MutationRate)
                {
                    int j = Random.Next(tour.Cities.Count);
                    var temp = tour.Cities[i];
                    tour.Cities[i] = tour.Cities[j];
                    tour.Cities[j] = temp;
                }
            }
            tour.Distance = tour.CalculateDistance(); // Recalculate distance
        }
    }

    public static void Run()
    {
        int cityCount = 10;
        var cities = new List<City>();
        var random = new Random();
        for (int i = 0; i < cityCount; i++)
        {
            cities.Add(new City(random.Next(0, 100), random.Next(0, 100)));
        }

        var ga = new GeneticAlgorithm(cities, populationSize: 10, mutationRate: 0.5);
        var bestTour = ga.Evolve();

        // Output the results
        Console.WriteLine("Best tour found:");
        foreach (var city in bestTour.Cities)
        {
            Console.WriteLine($"City at ({city.X}, {city.Y})");
        }
        Console.WriteLine($"Total Distance: {bestTour.Distance}");
    }
}