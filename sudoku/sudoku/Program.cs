using System;
using System.Collections;
using System.Linq.Expressions;

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
        Hashtable ht = new Hashtable();
        for (int i = 0; i < 9; i++)
        {
            List<int> blok = new List<int>();

            for (int n=0; n < 9; n++)
            {
                blok.Add(input[n + (i*9)]);
                Console.WriteLine($"{n}, {i}: {input[n + (i * 9)]}");
            }

            ht.Add(i, blok);
        }
        for(int i = 0; i < 9; i++)
        {
            Console.Write($"{i}: {ht[i]}");
        }
    }

    public class Sudoku
    {
        // Klasse body
    }
}
