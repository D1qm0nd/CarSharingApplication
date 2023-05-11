using CarSharingApplication.SQL.Linq;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace CarSharingApplication.Windows.XAMLModels
{
    /// <summary>
    /// Логика взаимодействия для Map.xaml
    /// </summary>
    public partial class Map : UserControl
    {
        //public int MapHeight { get; set; } = 0;
        //public int MapWidth { get; set; } = 0;

        public new double Height { get; set; } = 100;
        public new double Width { get; set; } = 100;

        public double InitializeLat { get; set; } = 0;
        public double InitializeLng { get; set; } = 0;
#nullable enable
        public static VehiclesINFO? SelectedVehicle { get; set; }

        public Map()
        {
            InitializeComponent();
        }

#nullable disable
        private void LoadMap(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache; //выбор подгрузки карты – онлайн или из ресурсов
            MapController.MapProvider = GoogleMapProvider.Instance; //какой провайдер карт используется (в нашем случае гугл) 
            MapController.MinZoom = 13; //минимальный зум
            MapController.MaxZoom = 18; //максимальный зум
            MapController.Zoom = 13; // какой используется зум при открытии
            if (SelectedVehicle != null)
                MoveCursorToVehicleOnMap(SelectedVehicle);
            else MapController.Position = new PointLatLng(InitializeLat,InitializeLng);
            MapController.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter; // как приближает (просто в центр карты или по положению мыши)
            MapController.CanDragMap = true; // перетаскивание карты мышью
            MapController.DragButton = MouseButton.Left; // какой кнопкой осуществляется перетаскивание
            MapController.ShowCenter = false; //показывать или скрывать красный крестик в центре
            MapController.ShowTileGridLines = false; //показывать или скрывать тайтлы
        }

        public void MoveCursorToVehicleOnMap(VehiclesINFO obj)
        {
            MapController.Position = new PointLatLng((double)obj.Lat!, (double)obj.Lng!);
        }

        /// <summary>
        /// Установить маркеры на карте
        /// Возможно только после загрузки карты
        /// </summary>
        /// <param name="markers"></param>
        public void SetMarkers(List<GMapMarker> markers)
        {
            if (markers != null)
            {
                MapController.Markers.Clear();
                markers.ForEach(marker => MapController.Markers.Add(marker));
            }
        }



    }
}
