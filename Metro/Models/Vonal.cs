using Metro.Models;
using System.Collections.Generic;

namespace Metro.Model
{
    public class Vonal
    {
        public string VonalNev { get; set; }
        // public List<Allomas> Allomasok = new List<Allomas>();
        public Dictionary<int, Allomas> Allomasok;
        public Vonal(string nev)
        {
            VonalNev = nev;
            Allomasok = new Dictionary<int, Allomas>();
        }
    }
}
