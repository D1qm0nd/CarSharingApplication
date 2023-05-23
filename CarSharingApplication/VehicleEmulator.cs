using CarSharingApplication.SQL.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarSharingApplication
{
    public static class VehicleEmulator
    {
        private static string conectionstring = App.GetConnectionString("CARHANDLERConnection");
        private static bool isWorking = false; 

        public static async void AsyncStartEmulate()
        {
            isWorking = true;
            await Task.Factory.StartNew(() => Emulate()).ConfigureAwait(false);
        }

        public static void StopEmulate() 
        {
            isWorking = false;
        }

        private static void Emulate()
        {
            using (var context = new CarSharingDataBaseClassesDataContext(conectionstring))
            {
                while (true)
                {
                    var randomizer = new Random();
                    var vehicles = context.ExecuteQuery<VehiclesINFO>("SELECT * FROM VehiclesINFO WHERE AccessStatus = 'занят'");
                    if (vehicles != null) 
                    { 
                        foreach (var vehicle in vehicles) 
                        {
                            vehicle.Lat += randomizer.Next(0,1) > 0 ? randomizer.NextDouble() * 0.000001 : randomizer.NextDouble() * -0.000001;
                            vehicle.Lng += randomizer.Next(0, 1) > 0 ? randomizer.NextDouble() * 0.000001 : randomizer.NextDouble() * -0.000001;
                            var command =
                                "BEGIN TRANSACTION COORDCHANGE " +
                                "BEGIN TRY " +
                                "INSERT VehicleCoordinates VALUES " +
                                $"({vehicle.ID_Vehicle}, {vehicle.Lng.ToString().Replace(',', '.')}, {vehicle.Lat.ToString().Replace(',', '.')}, '{DateTime.Now}') " +
                                "COMMIT TRANSACTION COORDCHANGE "+
                                "END TRY " +
                                "BEGIN CATCH " +
                                "ROLLBACK TRANSACTION COORDCHANGE " +
                                "END CATCH";
                            context.ExecuteCommand(
                                command
                            );
                        }
                    }
                    Thread.Sleep(15000);
                }
            }
        }
    }
}
