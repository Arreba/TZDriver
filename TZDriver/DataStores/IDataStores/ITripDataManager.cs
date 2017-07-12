using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TZDriver.Models.DataModels;

namespace TZDriver.DataStores.IDataStores
{
    public interface ITripDataManager
    {
        List<TripData> GetActiveTrips();
        List<TripData> GetClosedTrips();
        TripData GetTripById(int id);
        void SaveTrip(TripData model);
        void DeleteTrip(TripData model);
        void CleanUpTrips();
    }
}
