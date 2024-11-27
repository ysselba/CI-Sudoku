using System;
using System.Collections;
using System.Linq.Expressions;

namespace sudoku
{
    internal class Program
    {
        static void Main(string[] args)
        { // TEST
            string[] tempInput = Console.ReadLine().Split(' ');
            List<int> input = new List<int>();
            foreach (string line in tempInput)
            {
                // Console.Write(line, int.Parse(line));
                input.Add(int.Parse(line));
            }
            
            //kan straks weg
            if (input.Count != 81) Console.WriteLine("Verkeerd aantal cijfers");
            else Console.WriteLine("parse gelukt");
            
            Hashtable ht = new Hashtable();
            for (int i = 1; i <= 9; i++)
            {
                List<int> blok = new List<int>();
                for (int n=0; n < 9; n++)
                {
                    blok.Add(input[n*(i)]);
                }
                ht.Add(i, blok);
            }
        }
    }
}