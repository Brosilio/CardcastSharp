using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardcastSharp;

namespace CardcastTester
{
    /// <summary>
    /// Some example usage of this library
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter deck playcode: ");

            Deck d = Cardcast.GetDeck(Console.ReadLine(), OnGetDeckError).Result;
            if(d != null)
            {
                foreach (Card c in d.Responses)
                {
                    Console.WriteLine(c.Text[0]);
                }
            }

            Console.Write("destroy enter key to exit");
            Console.ReadLine();
        }

        private static void OnGetDeckError(Exception obj)
        {
            Console.WriteLine("fuck. " + obj.Message);
        }
    }
}
