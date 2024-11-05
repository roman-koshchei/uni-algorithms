using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs;

public class FanoEncoder
{
    private readonly Dictionary<char, string> encodingTable;
    private readonly Dictionary<string, char> decodingTable;

    public FanoEncoder(string sampleText)
    {
        encodingTable = [];
        decodingTable = [];
        BuildEncodingTable(sampleText);
    }

    private void BuildEncodingTable(string sampleText)
    {
        // Calculate frequency of each character
        var frequencies = sampleText
            .GroupBy(c => c)
            .ToDictionary(g => g.Key, g => g.Count())
            .OrderByDescending(kvp => kvp.Value)
            .ToList();

        GenerateFanoCodes(frequencies, "");
    }

    private void GenerateFanoCodes(List<KeyValuePair<char, int>> symbols, string prefix)
    {
        if (symbols.Count == 1)
        {
            encodingTable[symbols[0].Key] = prefix;
            decodingTable[prefix] = symbols[0].Key;
            return;
        }

        int total = symbols.Sum(kvp => kvp.Value);
        int sum = 0;
        int splitIndex = 0;

        for (int i = 0; i < symbols.Count; i++)
        {
            sum += symbols[i].Value;
            if (sum >= total / 2)
            {
                splitIndex = i + 1;
                break;
            }
        }

        // Recursively assign codes
        GenerateFanoCodes(symbols.Take(splitIndex).ToList(), prefix + "0");
        GenerateFanoCodes(symbols.Skip(splitIndex).ToList(), prefix + "1");
    }

    public string Encode(string text)
    {
        StringBuilder encodedText = new StringBuilder();
        foreach (char c in text)
        {
            if (encodingTable.TryGetValue(c, out string? value))
            {
                encodedText.Append(value);
            }
            else
            {
                throw new ArgumentException($"Character '{c}' not found in encoding table.");
            }
        }
        return encodedText.ToString();
    }

    public string Decode(string encodedText)
    {
        StringBuilder decodedText = new();
        string currentCode = "";

        foreach (char bit in encodedText)
        {
            currentCode += bit;
            if (decodingTable.TryGetValue(currentCode, out char value))
            {
                decodedText.Append(value);
                currentCode = "";
            }
        }

        return decodedText.ToString();
    }

    // Print the Fano codes
    public void PrintCodes()
    {
        Console.WriteLine("Character Codes:");
        foreach (var kvp in encodingTable)
        {
            Console.WriteLine($"Character: '{kvp.Key}' - Code: {kvp.Value}");
        }
    }
}