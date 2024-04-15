using ClosedXML.Excel;
using Metro.Model;
using Metro.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metro.Repositories
{
    public class MetroRepository
    {
        private readonly string _filePath;
        public List<Vonal> MetroVonalak { get; }
        public List<Allomas> Allomasok { get; }

        public MetroRepository(string filepath = "Data/metro.xlsx")
        {
            MetroVonalak = new List<Vonal>();
            Allomasok = new List<Allomas>();
            _filePath = filepath;
            ReadAll();
        }

        private void ReadAll()
        {
            using (var workbook = new XLWorkbook(_filePath))
            {
                // Állomások beolvasása
                var munkalap = workbook.Worksheet(1);
                var sorokSzama = munkalap.RowsUsed().Count();
                for (int sor = 2; sor <= sorokSzama; sor++)
                {
                    string nev = munkalap.Cell(sor, 1).GetValue<string>();
                    string x = munkalap.Cell(sor, 2).GetValue<string>();
                    string y = munkalap.Cell(sor, 3).GetValue<string>();
                    Allomasok.Add(new Allomas(nev, x, y));
                }

                // Vonalak beolvasása
                munkalap = workbook.Worksheet(2);
                sorokSzama = munkalap.RowsUsed().Count();
                int oszlopokSzama = munkalap.ColumnsUsed().Count();
                for (int sor = 2; sor <= sorokSzama; sor++)
                {
                    string nev = munkalap.Cell(sor, 1).GetValue<string>();
                    var vonal = new Vonal(nev);
                    int megalloSzam = 1;
                    for (int oszlop = 2; oszlop <= oszlopokSzama; oszlop++)
                    {
                        string allomasNev = munkalap.Cell(sor, oszlop).GetValue<string>();
                        // Állomás kikeresése
                        var allomas = Allomasok.SingleOrDefault(x => x.AllomasNev == allomasNev);
                        if (allomas != null)
                        {
                            vonal.Allomasok.Add(megalloSzam, allomas);
                            megalloSzam++;
                        }
                    }
                    MetroVonalak.Add(vonal);
                }
            }
        }
    }
}
