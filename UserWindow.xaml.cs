using BingMapsRESTToolkit;
using BingMapsRESTToolkit.Extensions;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Configuration;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        #region Private Properties
        public ApplicationIdCredentialsProvider Provider { get; set; }

        private readonly string BingMapsKey = "qXt59AKl8rwqrRMCRqOt~pPlEyB0rYIlIHFkAr4YeIA~AqrPcWkWVdN1c2yCpjsn64d5MpBwQxaz63p6-3fgnmNtGIXV4a25cIIBl5KLVr6e";

        private string SessionKey;

        private readonly Regex CoordinateRx = new Regex(@"^[\s\r\n\t]*(-?[0-9]{0,2}(\.[0-9]*)?)[\s\t]*,[\s\t]*(-?[0-9]{0,3}(\.[0-9]*)?)[\s\r\n\t]*$");

        #endregion
        public UserWindow()
        {
            InitializeComponent();

            string s = "";
            BingMap.CredentialsProvider.GetCredentials((c) =>
            {
                SessionKey = c.ApplicationId;
                s = c.ApplicationId;
            });
            InputTbx.Text = "Seattle, WA\r\nRedmond, WA\r\nBellevue, WA\r\n47.532122, -122.042934\r\nEverett, WA\r\nTacoma, WA\r\nKirkland, WA\r\nSammamish, WA\r\nLynnwood, WA\r\nRenton, WA\r\nDuvall, WA\r\nMonroe, WA\r\nSummer, WA";
        }
        private async void CalculateRouteBtn_Clicked(object sender, RoutedEventArgs e)
        {
            
            BingMap.Children.Clear();
            OutputTbx.Text = string.Empty;
            LoadingBar.Visibility = Visibility.Visible;

            var waypoints = GetWaypoints();

            if (waypoints.Count < 2)
            {
                MessageBox.Show("Need a minimum of 2 waypoints to calculate a route.");
                return;
            }

            var travelMode = (TravelModeType)Enum.Parse(typeof(TravelModeType), (string)(TravelModeTypeCbx.SelectedItem as ComboBoxItem).Content);
            var tspOptimization = (TspOptimizationType)Enum.Parse(typeof(TspOptimizationType), (string)(TspOptimizationTypeCbx.SelectedItem as ComboBoxItem).Tag);
            try
            {
                //Calculate a route between the waypoints so we can draw the path on the map. 
                var routeRequest = new RouteRequest()
                {
                    Waypoints = waypoints,

                    //оптимизация ( перебор до 10 пунктов и генетический если больше)
                    WaypointOptimization = tspOptimization,

                    RouteOptions = new RouteOptions()
                    {
                        TravelMode = travelMode,
                        RouteAttributes = new List<RouteAttributeType>()
                        {
                            RouteAttributeType.RoutePath,
                            RouteAttributeType.ExcludeItinerary
                        }
                    },

                    //When straight line distances are used, the distance matrix API is not used, so a session key can be used.
                    BingMapsKey = (tspOptimization == TspOptimizationType.StraightLineDistance) ? SessionKey : BingMapsKey
                };

                if (routeRequest.RouteOptions.TravelMode != TravelModeType.Driving)
                {
                    routeRequest.RouteOptions.Optimize = RouteOptimizationType.Time;
                }
                else
                {
                    routeRequest.RouteOptions.Optimize = RouteOptimizationType.TimeWithTraffic;
                }

                var r = await routeRequest.Execute();

                RenderRouteResponse(routeRequest, r);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            LoadingBar.Visibility = Visibility.Collapsed;
        }
        //спаси и сохрани
        private void SaveBtn_Clicked(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files(*.*)|*.*";
            saveFileDialog.Title = "Save image";


            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)BingMap.ActualWidth, (int)BingMap.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(BingMap);
            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            

            if (saveFileDialog.ShowDialog() == true)
            {
                pngImage.Save(saveFileDialog.OpenFile());
            }
        }
        //закрывалась бы нормально, цены бы ей не было...
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
        #region Private Methods

        /// <summary>
        /// Renders a route response on the map.
        /// </summary>
        private void RenderRouteResponse(RouteRequest routeRequest, Response response)
        {
            //Render the route on the map.
            if (response != null && response.ResourceSets != null && response.ResourceSets.Length > 0 &&
               response.ResourceSets[0].Resources != null && response.ResourceSets[0].Resources.Length > 0
               && response.ResourceSets[0].Resources[0] is Route)
            {
                var route = response.ResourceSets[0].Resources[0] as Route;

                var timeSpan = new TimeSpan(0, 0, (int)Math.Round(route.TravelDurationTraffic));

                OutputTbx.Text = string.Format("Время путешествия коммивояжера составит: {3} дней {0} ч {1} мин {2} сек\r\n", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Days);

                var routeLine = route.RoutePath.Line.Coordinates;
                var routePath = new LocationCollection();

                for (int i = 0; i < routeLine.Length; i++)
                {
                    routePath.Add(new Microsoft.Maps.MapControl.WPF.Location(routeLine[i][0], routeLine[i][1]));
                }

                var routePolyline = new MapPolyline()
                {
                    Locations = routePath,
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 3
                };

                BingMap.Children.Add(routePolyline);

                var locs = new List<Microsoft.Maps.MapControl.WPF.Location>();

                // маркеры на карте
                for (var i = 0; i < routeRequest.Waypoints.Count; i++)
                {
                    var loc = new Microsoft.Maps.MapControl.WPF.Location(routeRequest.Waypoints[i].Coordinate.Latitude, routeRequest.Waypoints[i].Coordinate.Longitude);

                    //отрисовка пути для последней точки.
                    if (i < routeRequest.Waypoints.Count - 1)
                    {
                        BingMap.Children.Add(new Pushpin()
                        {
                            Location = loc,
                            Content = i
                        });
                    }

                    locs.Add(loc);
                }

                BingMap.SetView(locs, new Thickness(20), 0);
            }
            else if (response != null && response.ErrorDetails != null && response.ErrorDetails.Length > 0)
            {
                throw new Exception(String.Join("", response.ErrorDetails));
            }
        }

        // Получение точек
        private List<SimpleWaypoint> GetWaypoints()
        {
            var places = InputTbx.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var waypoints = new List<SimpleWaypoint>();

            foreach (var p in places)
            {
                if (!string.IsNullOrWhiteSpace(p))
                {
                    var m = CoordinateRx.Match(p);

                    if (m.Success)
                    {
                        waypoints.Add(new SimpleWaypoint(double.Parse(m.Groups[1].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture),
                            double.Parse(m.Groups[3].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        waypoints.Add(new SimpleWaypoint(p));
                    }
                }
            }

            return waypoints;
        }

        #endregion
    }
}
