﻿using Microsoft.Maps.MapControl.WPF;
using System;
using System.Device;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Geocaching.Models;
using Microsoft.EntityFrameworkCore;
using Geocaching.Database;
using System.Threading;
using System.Globalization;
using System.Configuration;

namespace Geocaching
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string applicationId = ConfigurationManager.ConnectionStrings["BingMaps"].ConnectionString; 
            
        private MapLayer layer;
        private Person SelectedPerson;

        // Contains the location of the latest click on the map.
        // The Location object in turn contains information like longitude and latitude.
        private Location latestClickLocation;
        private Location gothenburg = new Location(57.719021, 11.991202);

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private async void Start()
        {
            if (applicationId == null)
            {
                MessageBox.Show("Please set the applicationId variable before running this program.");
                Environment.Exit(0);
            }

            CreateMap();

            await LoadMapDataFromDatabase();
        }

        private void CreateMap()
        {
            map.CredentialsProvider = new ApplicationIdCredentialsProvider(applicationId);
            map.Center = gothenburg;
            map.ZoomLevel = 12;
            layer = new MapLayer();
            map.Children.Add(layer);

            MouseDown += (sender, e) =>
            {
                var point = e.GetPosition(this);
                latestClickLocation = map.ViewportPointToLocation(point);

                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    OnMapLeftClick();
                }
            };

            map.ContextMenu = new ContextMenu();

            var addPersonMenuItem = new MenuItem { Header = "Add Person" };
            map.ContextMenu.Items.Add(addPersonMenuItem);
            addPersonMenuItem.Click += OnAddPersonClick;

            var addGeocacheMenuItem = new MenuItem { Header = "Add Geocache" };
            map.ContextMenu.Items.Add(addGeocacheMenuItem);
            addGeocacheMenuItem.Click += OnAddGeocacheClick;
        }

        private void OnMapLeftClick()
        {
            SelectedPerson = null;

            foreach (UIElement element in layer.Children)
            {
                if (element is Pushpin)
                {
                    element.Opacity = 1.0;

                    var pin = (Pushpin)element;

                    if (pin.Tag is Geocashe)
                    {
                        pin.Background = new SolidColorBrush(Colors.Gray);
                    }
                }
            }

        }

        private async void SelectGeoPin(Pushpin pin, Geocashe geo)
        {
            if (SelectedPerson == null)
                return;
            if (SelectedPerson.Geocashes.Contains(geo))
                return;

            if (SelectedPerson.FoundGeocaches.Any(x => x.GeocasheID == geo.ID))
            {
                FoundGeocache found = SelectedPerson.FoundGeocaches.FirstOrDefault(f => f.Geocashe.ID == geo.ID);
                SelectedPerson.FoundGeocaches.Remove(found);
                pin.Background = new SolidColorBrush(Colors.Red);
                var crud = new Crud<FoundGeocache>();
                await crud.DeleteAsync(found);
            }

            else
            {
                pin.Background = new SolidColorBrush(Colors.Green);
                SelectedPerson.FoundGeocaches.Add(new FoundGeocache { Geocashe = geo, GeocasheID = geo.ID, Person = SelectedPerson, PersonID = SelectedPerson.ID });
                var crud = new Crud<FoundGeocache>();
                await crud.CreateAsync(new FoundGeocache { GeocasheID = geo.ID, PersonID = SelectedPerson.ID });
            }
        }

        private async void OnAddGeocacheClick(object sender, RoutedEventArgs args)
        {
            var dialog = new GeocacheDialog();
            dialog.Owner = this;
            dialog.ShowDialog();
            if (dialog.DialogResult == false)
            {
                return;
            }

            if (SelectedPerson == null)
            {
                return;
            }

            var geocache = new Geocashe
            {
                Location = latestClickLocation,
                Content = dialog.GeocacheContents,
                Message = dialog.GeocacheMessage,
                PersonID = SelectedPerson.ID
            };

            var x = new Crud<Geocashe>();
            await x.CreateAsync(geocache);
            SelectedPerson.Geocashes.Add(geocache);


            var pin = AddPin(latestClickLocation, PinGeoInfo(SelectedPerson, geocache), Colors.Black, geocache);

            pin.MouseDown += (s, a) =>
            {
                SelectGeoPin(pin, geocache);

                a.Handled = true;
            };

            await LoadMapDataFromDatabase();
        }

        private async void OnAddPersonClick(object sender, RoutedEventArgs args)
        {
            var dialog = new PersonDialog();
            dialog.Owner = this;
            dialog.ShowDialog();
            if (dialog.DialogResult == false)
            {
                return;
            }

            var person = new Person
            {
                FirstName = dialog.PersonFirstName,
                LastName = dialog.PersonLastName,
                Location = new Location(latestClickLocation.Latitude, latestClickLocation.Longitude),
                Country = dialog.AddressCountry,
                City = dialog.AddressCity,
                StreetName = dialog.AddressStreetName,
                StreetNumber = dialog.AddressStreetNumber,
                Geocashes = new List<Geocashe>(),
                FoundGeocaches = new List<FoundGeocache>()
            };

            var pin = AddPersonPin(person);

            SelectPersonPin(pin, person);
            var crud = new Crud<Person>();
            await crud.CreateAsync(person);
        }

        private async void OnLoadFromFileClick(object sender, RoutedEventArgs args)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";
            bool? result = dialog.ShowDialog();
            if (result != true)
            {
                return;
            }

            string path = dialog.FileName;
            try
            {
                await LoadDatabase.FromFlatFile(path);
                await LoadMapDataFromDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load file. \n\n" + ex.ToString());
            }
        }

        private async void OnSaveToFileClick(object sender, RoutedEventArgs args)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";
            dialog.FileName = "Geocaches";
            bool? result = dialog.ShowDialog();
            if (result != true)
            {
                return;
            }

            string path = dialog.FileName;
            await SaveDatabase.ToFlatFile(path);
        }

        private void SelectPersonPin(Pushpin pin, Person person)
        {
            // Handle click on person pin here.
            SelectedPerson = person;

            pin.Opacity = 1.0;

            foreach (UIElement element in layer.Children)
            {
                if (element is Pushpin)
                {
                    var otherPin = (Pushpin)element;

                    if (otherPin.Tag is Person)
                    {
                        if (pin != otherPin)
                        {
                            otherPin.Opacity = 0.5;
                        }

                    }

                    else if (otherPin.Tag is Geocashe)
                    {
                        var geo = (Geocashe)((Pushpin)otherPin).Tag;

                        if (geo.Person == person)
                        {
                            otherPin.Background = new SolidColorBrush(Colors.Black);
                        }
                        else if (person.FoundGeocaches != null && person.FoundGeocaches.Any(g => g.Geocashe == geo))
                        {
                            otherPin.Background = new SolidColorBrush(Colors.Green);
                        }
                        else
                        {
                            otherPin.Background = new SolidColorBrush(Colors.Red);
                        }
                    }
                }
            }
        }

        private async Task LoadMapDataFromDatabase()
        {
            layer.Children.Clear();
            var crud = new Crud<Person>();
            var persons = await crud.GetListAsync(true);

            // Load data from database and populate map here.
            persons.ForEach(person =>
            {
                AddPersonPin(person);

                person.Geocashes.ToAsyncEnumerable().ForEachAsync(geo =>
                {

                    string pinGeoInfo = PinGeoInfo(person, geo);

                    var pinGeo = AddPin(geo.Location, pinGeoInfo, Colors.Gray, geo);

                    pinGeo.MouseDown += (s, a) =>
                    {
                        SelectGeoPin(pinGeo, geo);

                        // Prevent click from being triggered on map.
                        a.Handled = true;
                    };
                });
            });

        }

        private static string PinGeoInfo(Person person, Geocashe geocashe)
        {
            return $"Geo cache\n" +
                   $"Added by {person.FirstName } {person.LastName}\n" +
                   $"Longitude: {geocashe.Location.Longitude}\n" +
                   $"Latitude: {geocashe.Location.Latitude}\n" +
                   $"Message: {geocashe.Message}\n" +
                   $"Content: {geocashe.Content}";
        }

        private Pushpin AddPersonPin(Person person)
        {
            string pinInfo = $"{person.FirstName} {person.LastName}\n{person.StreetName} {person.StreetNumber}";

            var pin = AddPin(person.Location, pinInfo, Colors.Blue, person);

            pin.MouseDown += (s, a) =>
            {
                SelectPersonPin(pin, person);
                // Prevent click from being triggered on map.
                a.Handled = true;
            };

            return pin;
        }

        private Pushpin AddPin(Location location, string tooltip, Color color, object tag)
        {
            var pin = new Pushpin();
            pin.Tag = tag;
            pin.Cursor = Cursors.Hand;
            pin.Background = new SolidColorBrush(color);
            ToolTipService.SetToolTip(pin, tooltip);
            ToolTipService.SetInitialShowDelay(pin, 0);
            layer.AddChild(pin, new Location(location.Latitude, location.Longitude));

            return pin;
        }
    }

}
