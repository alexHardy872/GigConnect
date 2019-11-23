using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GigConnect.Models;
using GigConnect.Private;
using GigConnect.Models.ViewModels;

namespace GigConnect.Services
{
    public static class DistanceMatrix
    {

     
            public static double metersToMiles = 1609.34;


            public static async Task<TravelDistanceViewModel> GetTravelInfo(int bandId, int venueId)
            {
                HttpClient client = new HttpClient();
                ApplicationDbContext context = new ApplicationDbContext();

                string requestUrl = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=";
                string destinationsString = "&destinations=";
                string authenticationString = "&key=" + Keys.googleMapsApiKey;

            
                Venue venue = context.Venues.Include("Location").Where(p => p.VenueId == venueId).SingleOrDefault();
                Band band = context.Bands.Include("Location").Where(p => p.BandId == bandId).SingleOrDefault();

                Location GigLocation = venue.Location;
                Location BandLocation = band.Location;
                


                double[] venueLatLng = new double[2];
              
                venueLatLng[0] = double.Parse(venue.Location.lat);
                venueLatLng[1] = double.Parse(venue.Location.lng);

                double[] bandLatLng = new double[2];

                bandLatLng[0] = double.Parse(band.Location.lat);
                bandLatLng[1] = double.Parse(band.Location.lng);

       

                var response = await client.GetStringAsync(requestUrl + venueLatLng[0] + "," + venueLatLng[1] + destinationsString + bandLatLng[0] + "," + bandLatLng[1] + authenticationString);


                JObject distanceInfo = JObject.Parse(response);

                TravelDistanceViewModel travelInfo = new TravelDistanceViewModel();
                double tempDistance = (double)distanceInfo["rows"][0]["elements"][0]["distance"]["value"];
                travelInfo.travelDistance = Convert.ToInt32(tempDistance / metersToMiles);
                //}
                //else //minutes
                //{
                double tempMinutes = (int)distanceInfo["rows"][0]["elements"][0]["duration"]["value"]; // throws error?

                //lesson.travelDuration = Convert.ToInt32(Math.Floor(tempMinutes / 60));
                travelInfo.travelDuration = Convert.ToInt32(Math.Floor((tempMinutes / 60)));
                //}


                return travelInfo;
            }

   










        }
}