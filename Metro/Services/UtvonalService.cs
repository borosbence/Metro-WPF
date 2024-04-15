using Metro.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metro.Services
{
    public static class UtvonalService
    {
        public static bool VonalonLetezik(Vonal vonal, string? allomas)
        {
            return vonal.Allomasok.Any(x => x.Value.AllomasNev == allomas);
        }

        public static int MegalloSzam(Vonal vonal, string? indulo, string? erkezes)
        {
            var induloAllomas = vonal.Allomasok.FirstOrDefault(x => x.Value.AllomasNev == indulo);
            var celAllomas = vonal.Allomasok.FirstOrDefault(x => x.Value.AllomasNev == erkezes);
            return Math.Abs(induloAllomas.Key - celAllomas.Key);
        }

        public static List<string> Tervezes(List<Vonal> vonalak, string? indulas, string? erkezes)
        {
            List<string> UtvonalTerv = new List<string>();

            Vonal? induloVonal = null, vegVonal = null;
            foreach (var vonal in vonalak)
            {
                bool indulasLetezik = VonalonLetezik(vonal, indulas);
                bool vegLetezik = VonalonLetezik(vonal, erkezes);
                // Vonalak eltárolása, ahol ott van az egyik megálló
                if (indulasLetezik)
                {
                    induloVonal = vonal;
                }
                if (vegLetezik)
                {
                    vegVonal = vonal;
                }
                // Ha meg van mindkét megálló valamelyik két vonalon
                if (induloVonal != null && vegVonal != null)
                {
                    break;
                }
            }

            if (induloVonal != null && vegVonal != null)
            {
                // Ha egy vonalon van a két megálló
                if (induloVonal == vegVonal)
                {
                    string vonalNev = induloVonal.VonalNev;
                    UtvonalTerv.Add($"Induljon el átszállás nélkül az {vonalNev} vonalon");
                    int megalloSzam = MegalloSzam(induloVonal, indulas, erkezes);
                    UtvonalTerv.Add($"szálljon le a {megalloSzam}. megállón.");
                }
                else
                {
                    // Megkeresi az induló vonalon szerepel-e közös átszálló állomás a végvonalon
                    foreach (var allomas in induloVonal.Allomasok)
                    {
                        string allomasNev = allomas.Value.AllomasNev;
                        bool vanAtszallas = VonalonLetezik(vegVonal, allomasNev);
                        // Ha van közvetlen átszállás
                        if (vanAtszallas)
                        {
                            string indulovonal = induloVonal.VonalNev;
                            string vegvonal = vegVonal.VonalNev;
                            UtvonalTerv.Add($"Induljon el az {indulovonal} vonalon");
                            int megalloSzam = MegalloSzam(induloVonal, indulas, allomasNev);
                            UtvonalTerv.Add($"szálljon át a(z) {megalloSzam}. megállón, {allomasNev} állomáson");
                            UtvonalTerv.Add($"az {vegvonal} vonalra");
                            megalloSzam = MegalloSzam(vegVonal, allomasNev, erkezes);
                            UtvonalTerv.Add($"szálljon le a {megalloSzam}. megállón.");
                            return UtvonalTerv;
                        }
                        // Ha nincs közvetlen átszállás
                        else
                        {
                            List<Vonal> kulsoVonalak = new();
                            kulsoVonalak.AddRange(vonalak);
                            kulsoVonalak.Remove(induloVonal);
                            kulsoVonalak.Remove(vegVonal);
                            foreach (var vonal in kulsoVonalak)
                            {
                                foreach (var kozosAllomas in vonal.Allomasok)
                                {
                                    string jelenlegiAllomas = kozosAllomas.Value.AllomasNev;
                                    bool induloVonalKozos = VonalonLetezik(vonal, allomasNev);
                                    bool vegVonalKozos = VonalonLetezik(vegVonal, jelenlegiAllomas);
                                    // Ha csak egyszer kell átszállni
                                    if (induloVonalKozos && vegVonalKozos)
                                    {
                                        UtvonalTerv.Add($"Induljon el az {induloVonal.VonalNev} vonalon");
                                        int megalloSzam = MegalloSzam(induloVonal, indulas, allomasNev);
                                        UtvonalTerv.Add($"szálljon át a(z) {megalloSzam}. megállón, {allomasNev} állomáson");
                                        // UtvonalTerv.Add($"szálljon át a(z) megállón, {allomasNev} állomáson");
                                        UtvonalTerv.Add($"az {vonal.VonalNev} vonalra");
                                        //megalloSzam = MegalloSzam(vonal, allomasNev, erkezes);
                                        // UtvonalTerv.Add($"szálljon át a(z) {megalloSzam}. megállón, {jelenlegiAllomas} állomáson");
                                        UtvonalTerv.Add($"szálljon át a(z) megállón, {jelenlegiAllomas} állomáson");
                                        UtvonalTerv.Add($"az {vegVonal.VonalNev} vonalra");
                                        return UtvonalTerv;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return UtvonalTerv;
        }
    }
}
