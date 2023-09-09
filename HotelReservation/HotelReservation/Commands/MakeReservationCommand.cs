using HotelReservation.Exceptions;
using HotelReservation.Models;
using HotelReservation.ViewModels;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xceed.Wpf.Toolkit;
using HotelReservation.Services;
using HotelReservation.Stores;

namespace HotelReservation.Commands
{
    public class MakeReservationCommand : AsyncCommandeBase
    {
        private readonly MakeReservationViewModel _makeReservationViewModel;
        private readonly HotelStore _hotelStore;
        private readonly NavigationService _reservationViewNavigationService;
        private MakeReservationViewModel makeReservationViewModel;
        private Hotel? hotel;
        private NavigationService reservationViewNavigationService;

        public MakeReservationCommand(MakeReservationViewModel makeReservationViewModel,
            HotelStore hotelStore,
            NavigationService reservationViewNavigationService)
        {
            _makeReservationViewModel = makeReservationViewModel;
            _hotelStore= hotelStore;
            _reservationViewNavigationService = reservationViewNavigationService;

            _makeReservationViewModel.PropertyChanged += OnVIewModelPropertyChanged;
        }

        public MakeReservationCommand(MakeReservationViewModel makeReservationViewModel, Hotel? hotel, NavigationService reservationViewNavigationService)
        {
            this.makeReservationViewModel=makeReservationViewModel;
            this.hotel=hotel;
            this.reservationViewNavigationService=reservationViewNavigationService;
        }

        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_makeReservationViewModel.Username) &&
                _makeReservationViewModel.FloorNumber > 0 &&
                base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Reservation reservation = new Reservation(
                new RoomID(_makeReservationViewModel.FloorNumber, _makeReservationViewModel.RoomNumber),
                _makeReservationViewModel.Username,
                _makeReservationViewModel.StartDate,
                _makeReservationViewModel.EndDate);

            try
            {
                await _hotelStore.MakeReservation(reservation);

                MessageBox.Show("Successfully reserved room.", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);

               // _reservationViewNavigationService.Navigate();
            }
            catch (ReservationConflictException) 
            {
                MessageBox.Show("This room is already taken.", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception)
            {
                MessageBox.Show("Failed to make reservation.", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(MakeReservationViewModel.Username) ||
                e.PropertyName == nameof(MakeReservationViewModel.FloorNumber)) 
            { 
                OnCanExecuteChanged();
            }
        }
    }
}
