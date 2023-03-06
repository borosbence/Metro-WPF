using System;

namespace Metro.Models
{
    public class Allomas
    {
        public string AllomasNev { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Allomas(string nev, string x, string y)
        {
            AllomasNev = nev;
            X = Convert.ToInt32(x);
            Y = Convert.ToInt32(y);
        }
    }
}
