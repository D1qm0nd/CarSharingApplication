using CarSharingApplication.SQL.Linq;
using System.Collections.Generic;

namespace CarSharingApplication.Windows.VehicleSelector
{
    /// <summary>
    /// Singleton VehiclesData
    /// </summary>
    public class VehiclesData
    {
        public List<VehiclesINFO> vehiclesInfoList { get; set; }
        public VehiclesINFO selectedVehicle { get; set; }
        public List<string> vehClasses { get; set; }
        public List<string> vehBrands { get; set; }
        public List<string> vehCategories { get; set; }

        private static VehiclesData _Instance;

        public static VehiclesData GetInstance
        {
            get
            {
                if (_Instance == null)
                    return _Instance = new VehiclesData();
                else return _Instance;
            }
        }
    }
}
