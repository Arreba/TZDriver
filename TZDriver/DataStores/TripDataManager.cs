using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TZDriver.DataStores.IDataStores;
using TZDriver.Models.DataModels;

namespace TZDriver.DataStores
{
    public class TripDataManager : ITripDataManager
    {
        public List<TripData> GetActiveTrips()
        {
            using (var db = new DataContext())
            {
                return ( from t in db.TripDatas
                         where t.IsTripComplete.Equals(false)
                         orderby t.TripStartTime ascending
                         select t ).ToList();
            }
        }

        public List<TripData> GetClosedTrips()
        {
            using (var db = new DataContext())
            {
                return ( from t in db.TripDatas.Include(t => t.Points)
                         where t.IsTripComplete.Equals(true) && t.IsTripStarted.Equals(true)
                         orderby t.TripEndTime ascending
                         select t ).Take(15).ToList();
            }
        }

        public TripData GetTripById(int id)
        {
            using (var db = new DataContext())
            {
                return (from t in db.TripDatas.Include(t => t.Points)
                        where t.TripDataId.Equals(id)
                        select t).FirstOrDefault();
            }
        }

        public void SaveTrip(TripData model)
        {
            using (var db = new DataContext())
            {
                if (model.TripDataId > 0)
                {
                    db.Attach(model);
                    db.Update(model);
                }
                else
                {
                    db.Add(model);
                }

                db.SaveChanges();
            }
        }

        public void DeleteTrip(TripData model)
        {
            using (var db = new DataContext())
            {
                db.Remove(model);
                db.SaveChanges();
            }
        }

        public void CleanUpTrips()
        {
            using (var db = new DataContext())
            {
                var trips = from t in db.TripDatas.Include(t => t.Points)
                            where t.IsTripComplete == false && t.TripStartTime <= DateTime.Now.AddDays(-10)
                            select t;
                if(trips != null)
                {
                    db.TripDatas.RemoveRange(trips);
                    db.SaveChanges();
                }
            }
        }
    }
}
