var maxPrice = MaxPriceForWeight([new(5, 4), new(7, 5), new(4, 3), new(9, 7), new(8, 6)], 16);
Console.WriteLine(maxPrice);

int MaxPriceForWeight(IEnumerable<Item> items, int maxWeight)
{
    var maxPrice = 0;
    foreach (var item in items)
    {
        var newMaxWeight = maxWeight - item.Weight;
        if (newMaxWeight < 0) continue;

        var newCurrentPrice = item.Price + MaxPriceForWeight(items.Where(x => x != item), newMaxWeight);
        if (newCurrentPrice > maxPrice)
        {
            maxPrice = newCurrentPrice;
        }
    }
    return maxPrice;
}

record Item(int Price, int Weight);