using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Metro.Messages;
using Metro.Models;
using Metro.Repositories;
using System.Collections.ObjectModel;

namespace Metro.ViewModels
{
    public class UtvonalViewModel : ObservableRecipient
    {
        private readonly MetroRepository repository;
        public ObservableCollection<Allomas> Allomasok { get; }
        private string? _indulas;
        public string? Indulas
        {
            get { return _indulas; }
            set { SetProperty(ref _indulas, value); }
        }
        private string? _erkezes;
        public string? Erkezes
        {
            get { return _erkezes; }
            set { SetProperty(ref _erkezes, value); }
        }
        public ObservableCollection<string> UtvonalTerv { get; }
        public RelayCommand TervezesCommand { get; }
        public RelayCommand ResetCommand { get; }

        public UtvonalViewModel()
        {
            repository = new MetroRepository();
            Allomasok = new ObservableCollection<Allomas>(repository.Allomasok);
            UtvonalTerv = new ObservableCollection<string>();
            TervezesCommand = new RelayCommand(Tervezes);
            ResetCommand = new RelayCommand(Reset);
            RegisterMessages();
        }

        private void RegisterMessages()
        {
            WeakReferenceMessenger.Default.Register<AllomasValtozasMessage>(this, (r, m) =>
            {
                if (m.Value.Indulo)
                {
                    Indulas = m.Value.AllomasNev;
                }
                else
                {
                    Erkezes = m.Value.AllomasNev;
                }
            });
        }

        private void Reset()
        {
            UtvonalTerv.Clear();
            Indulas = null;
            Erkezes = null;
        }

        private void Tervezes()
        {
            UtvonalTerv.Clear();
            UtvonalTerv.Add("Indulás innen: " + Indulas);
            UtvonalTerv.Add("Érkezés ide: " + Erkezes);
            UtvonalTerv.Add("---");

            bool indulasLetezik, vegLetezik;
            bool direktVonal = false;
            foreach (var induloVonal in repository.MetroVonalak)
            {
                string metroVonal = induloVonal.VonalNev;
                indulasLetezik = repository.VonalonLetezik(induloVonal, Indulas);
                vegLetezik = repository.VonalonLetezik(induloVonal, Erkezes);
                if (indulasLetezik == true && vegLetezik == true)
                {
                    UtvonalTerv.Add("Induljon el átszállás nélkül az " + metroVonal + " vonalon");
                    direktVonal = true;
                }

                if (direktVonal == false)
                {
                    // Megkeresi a végállomást a vonalak közül
                    foreach (var vegVonal in repository.MetroVonalak)
                    {
                        vegLetezik = repository.VonalonLetezik(vegVonal, Erkezes);
                        // Ha megtaláta az egyik vonalon
                        if (vegLetezik)
                        {
                            // Megkeresi az induló vonalon szerepel e közös átszálló állomás a végvonalon
                            foreach (var allomas in vegVonal.Allomasok)
                            {
                                string allomasNev = allomas.Value.AllomasNev;
                                bool vanAtszallas = repository.VonalonLetezik(induloVonal, allomasNev);
                                if (indulasLetezik && vegLetezik && vanAtszallas)
                                {
                                    string vegvonal = vegVonal.VonalNev;
                                    UtvonalTerv.Add("Induljon el az " + metroVonal + " vonalon");
                                    UtvonalTerv.Add("szálljon át a(z) " + allomasNev + " állomáson");
                                    UtvonalTerv.Add("az " + vegvonal + " vonalra");
                                }
                            }

                        }
                    }
                }
            }
        }
    }
}
