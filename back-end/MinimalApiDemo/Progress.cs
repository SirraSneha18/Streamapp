
namespace Cat.Turbo.FluidsAndFilters.Data;

public class Progress
{
    public string Prefix { get; set; }
    public decimal Value { get; set; }
    public string Message { get; set; }

    public Progress()
    {

    }
    public Progress(string prefix, decimal value, string message)
    {
        Prefix = prefix;
        Value = value;
        Message = message;
    }
}