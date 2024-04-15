using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Metro.Messages;
using Metro.Models;
using Metro.Repositories;
using Metro.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Metro.ViewModels
{
    public class UtvonalViewModel : ObservableObject
    {
        private readonly MetroRepository _repository;

        public List<Allomas> Allomasok { get; }

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
        public IRelayCommand TervezesCommand { get; }
        public IRelayCommand ResetCommand { get; }

        public UtvonalViewModel(MetroRepository repository)
        {
            _repository = repository;
            Allomasok = new List<Allomas>(repository.Allomasok);
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
            if (string.IsNullOrEmpty(Indulas) || string.IsNullOrEmpty(Erkezes))
            {
                UtvonalTerv.Add("Kérem válassza ki az indulási és érkezési állomást!");
                return;
            }
            UtvonalTerv.Add($"Indulás innen: {Indulas}.");
            UtvonalTerv.Add($"Érkezés ide: {Erkezes}.");
            UtvonalTerv.Add("------------------------------");

            var utvonalak = UtvonalService.Tervezes(_repository.MetroVonalak, Indulas, Erkezes);
            foreach (string sor in utvonalak)
            {
                UtvonalTerv.Add(sor);
            }
        }
    }
}
