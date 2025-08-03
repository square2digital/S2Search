namespace Domain.Models
{
    public class SearchInsightTile
    {
        public string Title { get; set; }
        public int Count { get; set; }
        public string PreviousPeriod { get; set; }
        public double PreviousPeriodPercentageChange { get; set; }
        public bool IsIncreaseFromPreviousPeriod { get; set; }
        public bool HasPreviousPeriod => !string.IsNullOrEmpty(PreviousPeriod);
    }
}
