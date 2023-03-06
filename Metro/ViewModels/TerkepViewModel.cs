using CommunityToolkit.Mvvm.Messaging;
using Metro.Messages;
using Metro.Model;
using Metro.Models;
using Metro.Repositories;
using System.Collections.Generic;

namespace Metro.ViewModels
{
    public class TerkepViewModel
    {
        private readonly MetroRepository _repository;
        public List<Vonal> MetroVonalak { get; }
        public List<Allomas> Allomasok { get; }
        private int allomasSzam;

        public TerkepViewModel(MetroRepository repository)
        {
            _repository = repository;
            MetroVonalak = _repository.MetroVonalak;
            Allomasok = _repository.Allomasok;
        }

        public void SendMessage(string allomasNev)
        {
            var uzenet = new AllomasMessage(true, allomasNev);
            allomasSzam++;
            if (allomasSzam == 2)
            {
                uzenet.Indulo = false;
                allomasSzam = 0;
            }
            WeakReferenceMessenger.Default.Send(new AllomasValtozasMessage(uzenet));
        }
    }
}
