namespace LL.Core.Models.ViewModels;

public class DateTimeViewModel
{
    public int Minute { get; set; }
    public int Day { get; set; }
    public int Month { get; set; }
    
    public string AsString { get; set; }
    public DateTimeViewModel(DateTime dateTime)
    {
        Minute = dateTime.Minute;
        Day = dateTime.Day;
        Month = dateTime.Month;
        
        AsString = dateTime.ToString("dd/MM/yyyy");
    }
}