using Microsoft.Maps.MapControl.WPF;
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

namespace Geocaching
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Contains the ID string needed to use the Bing map.
        // Instructions here: https://docs.microsoft.com/en-us/bingmaps/getting-started/bing-maps-dev-center-help/getting-a-bing-maps-key
        private readonly string applicationId = "AlkAgPVerBBqzh_R2hMjn3aFu4ymqxo2PObuhkEwN-hZDA5VcjGgsTa8aVK8gnSV";

        private MapLayer layer;
        private Person SelectedPerson;

        class PinHandler
        {
            private static void AddPin()
            {

            }
        }

        // Contains the location of the latest click on the map.
        // The Location object in turn contains information like longitude and latitude.
        private Location latestClickLocation;

        private Location gothenburg = new Location(57.719021, 11.991202);

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            if (applicationId == null)
            {
                MessageBox.Show("Please set the applicationId variable before running this program.");
                Environment.Exit(0);
            }

            CreateMap();

            using (var db = new AppDbContext())
            {
                var p = db.Persons.Include(x => x.Geocashes).Include(x => x.FoundGeocaches).ToList();

                // Load data from database and populate map here.
                p.ForEach(person =>
                {
                    AddPersonPin(person);

                    person.Geocashes.ToAsyncEnumerable().ForEachAsync(geo =>
                    {
                        string pinGeoInfo = $"Geo cache\n" +
                                            $"Added by {person.FirstName } {person.LastName}\n" +
                                            $"Longitude: {geo.Location.Longitude}\n" +
                                            $"Latitude: {geo.Location.Latitude}\n" +
                                            $"Message: {geo.Message}";

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

        private void UpdateMap()
        {
            // It is recommended (but optional) to use this method for setting the color and opacity of each pin after every user interaction that might change something.
            // This method should then be called once after every significant action, such as clicking on a pin, clicking on the map, or clicking a context menu option.
        }

        private void OnMapLeftClick()
        {
            // Handle map click here.

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

            UpdateMap();
        }
#warning MakeAsync
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
                MessageBox.Show("Select a person!");
                return;
            }

            var geocache = new Geocashe
            {
                Location = latestClickLocation,
                Content = dialog.GeocacheContents,
                Message = dialog.GeocacheMessage,
            };

            //var t = Task.Run(() => AddGeocasheAsync(geocache));

            using (var context = new AppDbContext())
            {
                geocache.Person = context.Persons.FirstOrDefault(x => x == SelectedPerson);
                context.Geocashes.Add(geocache);
                await context.SaveChangesAsync();
            }

            // Add geocache to map and database here.
            var pin = AddPin(latestClickLocation, geocache.Message, Colors.Gray, geocache);

            pin.MouseDown += (s, a) =>
            {
                SelectGeoPin(pin, geocache);

                // Prevent click from being triggered on map.
                a.Handled = true;
            };
        }

        private Pushpin AddPersonPin(Person person)
        {
            string pinInfo = $"{person.FirstName} {person.LastName}\n{person.StreetName} {person.StreetNumber}";

            var pin = AddPin(person.Location, pinInfo, Colors.Blue, person);

            //när vi klickar på en  person så ska pin ändra färg till halv grå?
            pin.MouseDown += (s, a) =>
            {
                SelectPersonPin(pin, person);

                // Prevent click from being triggered on map.
                a.Handled = true;
            };

            return pin;
        }

        private void SelectGeoPin(Pushpin pin, Geocashe geo)
        {
            // Handle click on geocache pin here.
            if (SelectedPerson == null)
            {
                MessageBox.Show("Select a person first");
            }
            else
            {
                var found = SelectedPerson.FoundGeocaches.SingleOrDefault(f => f.Geocashe == geo);

                if (found != null)
                {
                    SelectedPerson.FoundGeocaches.Remove(found);

                    pin.Background = new SolidColorBrush(Colors.Red);

                    //AddFoundGeoCacheAsync(found);
                }
                else
                {
                    var newFound = new FoundGeocache() { PersonID = SelectedPerson.ID, Person = SelectedPerson, Geocashe = geo, GeocasheID = geo.ID };

                    SelectedPerson.FoundGeocaches.Add(newFound);

                    pin.Background = new SolidColorBrush(Colors.Green);

                    //RemoveFoundGeoCacheAsync(newFound);
                }

                UpdateMap();
            }
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

            UpdateMap();
        }

#warning MakeAsync
        private void OnAddPersonClick(object sender, RoutedEventArgs args)
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
                StreetNumber = dialog.AddressStreetNumber
            };

            var pin = AddPersonPin(person);

            SelectPersonPin(pin, person);

            AddPersonAsync(person);
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
            await LoadDatabase.FromFlatFile(path);
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

        private static async void AddPersonAsync(Person person)
        {
            using (var context = new AppDbContext())
            {
                context.Persons.Add(person);

                await context.SaveChangesAsync();
            }
        }

        private static async void AddFoundGeoCacheAsync(FoundGeocache geo)
        {
            using (var context = new AppDbContext())
            {
                context.FoundGeocaches.Add(geo);

                await context.SaveChangesAsync();
            }
        }

        private static async void RemoveFoundGeoCacheAsync(FoundGeocache geo)
        {
            using (var context = new AppDbContext())
            {
                context.FoundGeocaches.Remove(geo);

                await context.SaveChangesAsync();
            }
        }

        //private static async Task AddGeocasheAsync(Geocashe geocasche)
        //{
        //    using (var context = new AppDbContext())
        //    {
        //        context.Geocashes.Add(geocasche);
        //        await context.SaveChangesAsync();
        //    }
        //}
    }
}
