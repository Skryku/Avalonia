using HotelReservation.Exceptions;
using HotelReservation.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HotelReservation.ViewModels;
using HotelReservation.Views;
using HotelReservation.Commands;
using HotelReservation.Stores;
using HotelReservation.Services;
using HotelReservation.DbContexts;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using HotelReservation.Services.ReservationCreators;
using HotelReservation.Services.ReservationConflictValidators;
using HotelReservation.Services.ReservationProviders;

namespace HotelReservation
{
    /// <summary>
    /// Interaction logic for App.axaml
    /// </summary>
    public partial class App : Application
    {
        private const string CONNECTION_STRING = "Data Source=hotelreservation.db";

        private readonly Hotel _hotel;
        private readonly HotelStore _hotelStore;
        private readonly NavigationStore _navigationsStore;
        private readonly HotelReservationDbContextFactory _hotelReservationDbContextFactory;
        private NavigationStore _navigationStore;

        public NavigationStore NavigationsStore => _navigationsStore;
        public App()
        {
            _hotelReservationDbContextFactory = new HotelReservationDbContextFactory(CONNECTION_STRING);
            IReservationProvider reservationProvider = null DatabaseReservationProvider(_hotelReservationDbContextFactory);
            IReservationCreator reservationCreator = null DatabaseReservationCreator(_hotelReservationDbContextFactory);
            IReservationConflictValidator reservationConflictValidator = null DatabaseReservationConflictValidator(_hotelReservationDbContextFactory);

            ReservationBook reservationBook = new ReservationBook(reservationProvider, reservationCreator, reservationConflictValidator);

            _hotel = new Hotel("SingletonSean Suites", reservationBook);
            _hotelStore = new HotelStore(_hotel);
            _navigationsStore = new NavigationStore();
        }

        public override void OnStartup(StartupEventArgs e, MainWindow MainWindow)
        {
            using (HotelReservationDbContext dbContext = _hotelReservationDbContextFactory.CreateDbContext())
            {
                dbContext.Database.Migrate();
            }

            _navigationsStore.CurrentViewModel = CreateMakeReservationViewModel;

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);

        }

        private MakeReservationViewModel CreateMakeReservationViewModel()
        {
            return new MakeReservationViewModel(_hotelStore, new NavigationService(_navigationsStore, CreateMakeReservationViewModel));
        }

        private ReservationListingViewModel CreateReservationViewModel()
        {
            return ReservationListingViewModel.LoadViewModel(_hotelStore, CreateMakeReservationViewModel(), new NavigationService(_navigationsStore));
        }
    }
}