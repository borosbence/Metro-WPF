using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Metro.Messages;
using Metro.Model;
using Metro.Models;
using Metro.Repositories;
using System.Collections.Generic;

namespace Metro.ViewModels
{
    public class TerkepViewModel : ObservableObject
    {
        private readonly MetroRepository _repository;
        public List<Vonal> MetroVonalak { get; }
        public List<Allomas> Allomasok { get; }
        public IRelayCommand<string> ZoomCommand { get; }

        private double _zoomX;
        public double ZoomX
        {
            get { return _zoomX; }
            set { SetProperty(ref _zoomX, value); }
        }

        private double _zoomY;
        public double ZoomY
        {
            get { return _zoomY; }
            set { SetProperty(ref _zoomY, value); }
        }

        public TerkepViewModel(MetroRepository repository)
        {
            _repository = repository;
            MetroVonalak = _repository.MetroVonalak;
            Allomasok = _repository.Allomasok;
            ZoomCommand = new RelayCommand<string>(Zoom);
            ZoomX = 1; ZoomY = 1;
            induloAllomas = true;
        }

        private void Zoom(string zoom)
        {
            zoom = zoom.Replace('.', ',');
            double.TryParse(zoom, out double num);
            if (num == 1)
            {
                ZoomX = 1;
                ZoomY = 1;
            }
            else if(ZoomX + num > 0 && ZoomY + num > 0)
            {
                ZoomX += num;
                ZoomY += num;
            }
        }

        private bool induloAllomas;
        public void SendMessage(string allomasNev)
        {
            var uzenet = new AllomasMessage(induloAllomas, allomasNev);
            WeakReferenceMessenger.Default.Send(new AllomasValtozasMessage(uzenet));
            induloAllomas = !induloAllomas;
        }
    }
}
