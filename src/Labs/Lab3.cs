using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs;

using System;
using System.Collections.Generic;

internal static class Lab3
{
    public class Node
    {
        public char Character;
        public int Frequency;
        public Node? Left;
        public Node? Right;

        public Node(char character, int frequency)
        {
            Character = character;
            Frequency = frequency;
            Left = null;
            Right = null;
        }
    }

    public static void Run()
    {
        // Define a sample text chunk to build the encoding and decoding tables
        string sampleText =
@"Once upon a time, in a quiet village surrounded by towering mountains, there lived an old storyteller. Every evening, villagers would gather around a crackling fire to listen to his tales. He spoke of distant lands, brave heroes, and mystical creatures that roamed in ancient forests. The storyteller's words painted vivid pictures in the minds of his listeners, transporting them to places they'd only dreamed of. Among the children, there was one boy who was especially captivated by these stories. He dreamed of embarking on his own adventure, of crossing rivers and climbing mountains, just like the heroes he heard about. And so, each night, he listened closely, holding onto every word, until he drifted off to sleep with dreams of faraway lands.";

        // Create an instance of the FanoEncoder
        FanoEncoder encoder = new(sampleText);

        // Define a test word to encode and decode
        string testWord = "fanoencoding";

        // Encode the test word
        string encodedWord = encoder.Encode(testWord);

        // Decode the encoded word
        string decodedWord = encoder.Decode(encodedWord);

        // Display results
        Console.WriteLine($"Original Word: {testWord}");
        Console.WriteLine($"Encoded Word: {encodedWord}");
        Console.WriteLine($"Decoded Word: {decodedWord}");

        // Verify if decoded word matches the original word
        if (decodedWord == testWord)
        {
            Console.WriteLine("Test Passed: Decoded word matches the original word.");
        }
        else
        {
            Console.WriteLine("Test Failed: Decoded word does not match the original word.");
        }

        // Print encoding table
        Console.WriteLine("\nEncoding Table:");
        encoder.PrintCodes();
    }

    private static void PrintCodes(Node root, string code, Dictionary<char, string> huffmanCodes)
    {
        if (root == null) return;

        if (root.Left == null && root.Right == null)
        {
            huffmanCodes[root.Character] = code;
            return;
        }

        PrintCodes(root.Left, code + "0", huffmanCodes);
        PrintCodes(root.Right, code + "1", huffmanCodes);
    }

    // Method to build the Huffman tree
    public static Node BuildHuffmanTree(string input)
    {
        Dictionary<char, int> frequencyMap = new Dictionary<char, int>();

        // Calculate frequencies of each character
        foreach (char c in input)
        {
            if (frequencyMap.ContainsKey(c))
                frequencyMap[c]++;
            else
                frequencyMap[c] = 1;
        }

        // Create a priority queue
        PriorityQueue<Node, int> priorityQueue = new PriorityQueue<Node, int>();

        foreach (var pair in frequencyMap)
        {
            priorityQueue.Enqueue(new Node(pair.Key, pair.Value), pair.Value);
        }

        // Build the Huffman tree
        while (priorityQueue.Count > 1)
        {
            Node left = priorityQueue.Dequeue();
            Node right = priorityQueue.Dequeue();
            Node parent = new Node('\0', left.Frequency + right.Frequency) { Left = left, Right = right };
            priorityQueue.Enqueue(parent, parent.Frequency);
        }

        return priorityQueue.Dequeue();
    }

    public static string Encode(string input)
    {
        Node root = BuildHuffmanTree(input);
        Dictionary<char, string> huffmanCodes = new();

        PrintCodes(root, "", huffmanCodes);

        // Generate encoded string
        string encodedString = "";
        foreach (char c in input)
        {
            encodedString += huffmanCodes[c];
        }

        return encodedString;
    }
}