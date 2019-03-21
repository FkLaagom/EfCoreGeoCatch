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
                // Load data from database and populate map here.
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
            UpdateMap();
        }
#warning MakeAsync
        private void OnAddGeocacheClick(object sender, RoutedEventArgs args)
        {
            var dialog = new GeocacheDialog();
            dialog.Owner = this;
            dialog.ShowDialog();
            if (dialog.DialogResult == false)
            {
                return;
            }

            var geocache = new Geocashe
            {
                Latitude = latestClickLocation.Latitude,
                Longitude = latestClickLocation.Longitude,
                Content = dialog.GeocacheContents,
                Message = dialog.GeocacheMessage,
            };

            //var t = Task.Run(() => AddGeocasheAsync(geocache));

            using (var context = new AppDbContext())
            {
                geocache.Person = context.Persons.FirstOrDefault(x => x == SelectedPerson);
                context.Geocashes.Add(geocache);
                context.SaveChanges();
            }

            // Add geocache to map and database here.
            var pin = AddPin(latestClickLocation, geocache.Message, Colors.Gray);

            pin.MouseDown += (s, a) =>
            {
                // Handle click on geocache pin here.
                MessageBox.Show("You clicked a geocache");
                UpdateMap();

                // Prevent click from being triggered on map.
                a.Handled = true;
            };
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
                Latitude = latestClickLocation.Latitude,
                Longitude = latestClickLocation.Longitude,
                Country = dialog.AddressCountry,
                City = dialog.AddressCity,
                StreetName = dialog.AddressStreetName,
                StreetNumber = dialog.AddressStreetNumber
            };

            string pinInfo = person.FirstName + " " + person.LastName + "\n " + person.StreetName + " " + person.StreetNumber;
            var pin = AddPin(latestClickLocation, pinInfo, Colors.Blue);

            pin.MouseDown += (s, a) =>
            {
                SelectedPerson = person;
                // Handle click on person pin here.
                MessageBox.Show(person.FirstName + " Selected!");
                UpdateMap();

                // Prevent click from being triggered on map.
                a.Handled = true;
            };

            AddPersonAsync(person);
        }

        private Pushpin AddPin(Location location, string tooltip, Color color)
        {
            var pin = new Pushpin();
            pin.Cursor = Cursors.Hand;
            pin.Background = new SolidColorBrush(color);
            ToolTipService.SetToolTip(pin, tooltip);
            ToolTipService.SetInitialShowDelay(pin, 0);
            layer.AddChild(pin, new Location(location.Latitude, location.Longitude));
            return pin;
        }

        private void OnLoadFromFileClick(object sender, RoutedEventArgs args)
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
            LoadDatabase.FromFlatFile(path);
        }

        private void OnSaveToFileClick(object sender, RoutedEventArgs args)
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
            SaveDatabase.ToFlatFile(path);
        }

        private static async void AddPersonAsync(Person person)
        {
            using (var context = new AppDbContext())
            {
                context.Persons.Add(person);
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
