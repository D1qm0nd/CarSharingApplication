using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSharingApplication
{
    public abstract class ACoordinate
    {
        System.Nullable<double> _Lat;
        System.Nullable<double> _Lng;
        public virtual System.Nullable<double> Lat { get { return _Lat; } set { _Lat = value; } }
        public virtual System.Nullable<double> Lng { get { return _Lng; } set { _Lng = value; } }
    }
    //heirs
    //VehiclesINFO
    //Overrided Lat, Lng

}
