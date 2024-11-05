namespace Labs;

internal static class Lab1
{
    public static Task Run()
    {
        MixedItem[] data = [
            new(5, 4, false), new(7, 5, true), new(4, 3, true), new(9, 7, false), new(8, 6, false)
        ];
        var weights = data.Select(x => x.Weight).ToArray();
        var prices = data.Select(x => (double)x.Price).ToArray();

        Console.WriteLine("Lab 1");

        var price = Knapsack(weights, prices, 16, data.Length);
        Console.WriteLine($"Knapsack: ${price}");

        var maxPrice = UnboundedKnapsackSolve(weights, prices, 16, data.Length);
        Console.WriteLine($"Unbounded Knapsack: ${maxPrice}");

        var fractionalPrice = FractionalKnapsackSolve(data, 16);
        Console.WriteLine($"Fractional Knapsack: ${fractionalPrice}");

        var mixedPrice = MixedKnapsackSolve(data, 16);
        Console.WriteLine($"Mixed Knapsack: ${mixedPrice}");

        return Task.CompletedTask;
    }

    private static double UnboundedKnapsackSolve(int[] weights, double[] prices, int capacity, int count)
    {
        double[] pricesForWeights = new double[capacity + 1];

        for (int i = 0; i <= capacity; i++)
        {
            for (int j = 0; j < count; j++)
            {
                if (weights[j] <= i)
                {
                    pricesForWeights[i] = Math.Max(pricesForWeights[i], pricesForWeights[i - weights[j]] + prices[j]);
                }
            }
        }
        return pricesForWeights[capacity];
    }

    private static double FractionalKnapsackSolve(
        IEnumerable<IItem> items, int maxWeight
    )
    {
        List<(double Price, int Weight)> fractionalItems = new(items.Count());

        foreach (var item in items)
        {
            var piecesCount = item.Weight;
            var price = 1.0 * item.Price / item.Weight;
            var weight = 1;

            for (var i = 0; i < piecesCount; i += 1)
            {
                fractionalItems.Add(new(price, weight));
            }
        }

        var weights = fractionalItems.Select(x => x.Weight).ToArray();
        var prices = fractionalItems.Select(x => x.Price).ToArray();
        return Knapsack(weights, prices, maxWeight, fractionalItems.Count);
    }

    private static double MixedKnapsackSolve(
        IEnumerable<MixedItem> items, int maxWeight
    )
    {
        List<(double Price, int Weight)> newItems = new(items.Count());

        foreach (var item in items)
        {
            if (item.IsFractional)
            {
                var piecesCount = item.Weight;
                var price = 1.0 * item.Price / item.Weight;
                var weight = 1;

                for (var i = 0; i < piecesCount; i += 1)
                {
                    newItems.Add(new(price, weight));
                }
            }
            else
            {
                newItems.Add(new(item.Price, item.Weight));
            }
        }

        var weights = newItems.Select(x => x.Weight).ToArray();
        var prices = newItems.Select(x => x.Price).ToArray();
        return Knapsack(weights, prices, maxWeight, newItems.Count);
    }

    private static double Knapsack(int[] weights, double[] values, int capacity, int itemCount)
    {
        double[,] maxPricesForWeight = new double[itemCount + 1, capacity + 1];

        for (int i = 0; i <= itemCount; i++)
        {
            for (int w = 0; w <= capacity; w++)
            {
                if (i == 0 || w == 0)
                {
                    maxPricesForWeight[i, w] = 0;
                }
                else if (weights[i - 1] <= w)
                {
                    var notTake = maxPricesForWeight[i - 1, w];
                    var take = maxPricesForWeight[i - 1, w - weights[i - 1]] + values[i - 1];
                    maxPricesForWeight[i, w] = Math.Max(notTake, take);
                }
                else
                {
                    maxPricesForWeight[i, w] = maxPricesForWeight[i - 1, w];
                }
            }
        }

        return maxPricesForWeight[itemCount, capacity];
    }

    private class MixedItem(int price, int weight, bool isFractional) : IItem
    {
        public int Price { get; set; } = price;
        public int Weight { get; set; } = weight;
        public bool IsFractional { get; set; } = isFractional;
    }

    private interface IItem
    {
        public int Price { get; set; }
        public int Weight { get; set; }
    }

    private class GenericItem(double price, double weight)
    {
        public double Price { get; set; } = price;
        public double Weight { get; set; } = weight;
    }
}