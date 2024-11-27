using System;

public class Program
{
    public static void Main(string[] args)
    {
        string[] tempInput = Console.ReadLine().Split(' ');
        List<int> input = new List<int>();
        foreach (string line in tempInput)
        {
            // Console.Write(line, int.Parse(line));
            input.Add(int.Parse(line));
        }
    }

    public class Sudoku
    {
        // Klasse body
    }
}
