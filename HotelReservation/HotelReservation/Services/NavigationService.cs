using HotelReservation.Commands;
using HotelReservation.Stores;
using HotelReservation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservation.Services
{
    public class NavigationService
    {
        private readonly NavigateCommand? _navigationStore;
        private readonly Func<ViewModelBase> createViewModel;

        public NavigationService(NavigateCommand navigationStore, Func<ViewModelBase> createViewModel)
        {
            _navigationStore = navigationStore;
            this.createViewModel=createViewModel;
        }

        public NavigationService(NavigationStore navigationsStore, Func<MakeReservationViewModel> createMakeReservationViewModel)
        {
            NavigationsStore=navigationsStore;
            CreateMakeReservationViewModel=createMakeReservationViewModel;
        }

        public NavigationStore NavigationsStore { get; }
        public Func<MakeReservationViewModel> CreateMakeReservationViewModel { get; }

        public void Navigate() 
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
