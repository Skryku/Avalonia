using HotelReservation.Commands;
using HotelReservation.Models;
using HotelReservation.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelReservation.ViewModels
{
    public class ReservationListingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ReservationViewModel> _reservations;
        private NavigationStore navigationsStore;
        private Func<MakeReservationViewModel> createMakeReservationViewModel;

        public IEnumerable<ReservationViewModel> Reservations => _reservations;

        public MakeReservationViewModel MakeReservationViewModel { get; }

        private string _errorMassage;
        public string ErrorMessage
        {
            get
            {
                return _errorMassage;
            }
            set
            {
                _errorMassage = value;
                OnPropertyChanged(nameof(ErrorMessage));

                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set 
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        public ICommand LoadReservationsCommand { get; }
        public ICommand MakeReservationCommand { get; }

        public ReservationListingViewModel(HotelStore hotelStore,MakeReservationViewModel makeReservationViewModel, Services.NavigationService MakeReservationNavigationService)
        {
            _hotelStore = hotelStore;
            _reservations = new ObservableCollection<ReservationViewModel>();
            MakeReservationViewModel = makeReservationViewModel;

            LoadReservationsCommand = new LoadReservationsCommand(this, _hotelStore);
            MakeReservationCommand = new NavigateCommand(MakeReservationNavigationService);

            _hotelStore.ReservationMade += OnReservationMade;

        }

        public virtual void Dispose()
        {
            _hotelStore.ReservationMade -= OnReservationMade;
            base.Dispose();
        }

        private void OnReservationMade(Reservation reservation)
        {
            ReservationViewModel reservationViewModel = new ReservationViewModel(reservation);
            _reservations.Add(reservationViewModel);
        }

        public static ReservationListingViewModel LoadViewModel(HotelStore hotelStore, 
            MakeReservationViewModel makeReservationViewModel, 
            Services.NavigationService MakeReservationNavigationService)
        {
            ReservationListingViewModel viewModel = new ReservationListingViewModel(hotelStore, MakeReservationNavigationService);

            viewModel.LoadReservationsCommand.Execute(null);

            return viewModel;
        }

        public void UpdateReservations(IEnumerable<Reservation> reservations)
        {
            _reservations.Clear();

            foreach (Reservation reservation in reservations)
            {
                ReservationViewModel reservationViewModel = new ReservationViewModel(reservation);
                _reservations.Add(reservationViewModel);
            }
        }
    }
}
