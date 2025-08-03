namespace Domain.Models.Objects
{
    public class SearchVehicle
    {
        public string vehicleID { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string variant { get; set; }
        public string location { get; set; }
        public int price { get; set; }
        public decimal? monthlyPrice { get; set; }
        public int mileage { get; set; }
        public string fuelType { get; set; }
        public string transmission { get; set; }
        public int doors { get; set; }
        public int engineSize { get; set; }
        public string bodyStyle { get; set; }
        public string colour { get; set; }
        public int year { get; set; }
        public string description { get; set; }
        public string manufactureColour { get; set; }
        public string vrm { get; set; }
        public string imageURL { get; set; }
        public string pageUrl { get; set; }
    }
}