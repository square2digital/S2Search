using S2Search.Backend.Domain.AzureFunctions.FeedServices.AzureSearch.Index;
using S2Search.Backend.Domain.AzureSearch.Indexes;
using System;

namespace S2Search.Backend.Domain.AzureFunctions.FeedServices.AzureSearch.Index
{
    public partial class DMS14
    {
        public string Vehicle_ID { get; set; }

        public string FullRegistration { get; set; }

        public string Colour { get; set; }

        public string FuelType { get; set; }

        public int Year { get; set; }

        public int Mileage { get; set; }

        public string Bodytype { get; set; }

        public int Doors { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Variant { get; set; }

        public int EngineSize { get; set; }

        public int Price { get; set; }

        public int MonthlyPrice { get; set; }

        public string Transmission { get; set; }

        public string PictureRefs { get; set; }

        public string FourWheelDrive { get; set; }

        public string New { get; set; }
        public string Used { get; set; }
        public string Site { get; set; }
        public string PageUrl { get; set; }

        public VehicleIndex ConvertToVehicleIndex()
        {
            var VehicleIndex = new VehicleIndex();

            try
            {
                VehicleIndex.BodyStyle = this.Bodytype;
                VehicleIndex.Colour = this.Colour;
                VehicleIndex.Doors = this.Doors;
                VehicleIndex.EngineSize = Convert.ToInt32(RoundUpToNearestHundred(this.EngineSize));
                VehicleIndex.FuelType = this.FuelType;
                VehicleIndex.ImageURL = HandlePictureRefs(this.PictureRefs);
                VehicleIndex.Location = this.Site;
                VehicleIndex.Mileage = this.Mileage;
                VehicleIndex.Make = this.Make;
                VehicleIndex.Model = this.Model;
                VehicleIndex.Price = this.Price;
                VehicleIndex.MonthlyPrice = this.MonthlyPrice;
                VehicleIndex.Transmission = this.Transmission;
                VehicleIndex.Variant = this.Variant;
                VehicleIndex.VehicleID = this.Vehicle_ID;
                VehicleIndex.VRM = this.FullRegistration;
                VehicleIndex.Year = this.Year;
                VehicleIndex.ModelYear = this.Year.ToString();
                VehicleIndex.PageUrl = this.PageUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception trying to convert DMS14 => VehicleIndex - {ex.Message}");
            }

            return VehicleIndex;
        }

        /// <summary>
        /// We need to fallback to the default image URL if the refs param is not a URL
        /// </summary>
        /// <param name="pictureRefs"></param>
        /// <returns></returns>
        private string HandlePictureRefs(string pictureRefs)
        {
            string url = Environment.GetEnvironmentVariable("DefaultImageURL");

            if (Uri.IsWellFormedUriString(pictureRefs, UriKind.Absolute))
            {
                url = pictureRefs;
            }

            return url;
        }

        /// <summary>
        ///  this will round up to the nearest 100
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private decimal RoundUpToNearestHundred(int value)
        {
            return Math.Round(Convert.ToDecimal(value) / 100M, 0) * 100;
        }
    }
}
