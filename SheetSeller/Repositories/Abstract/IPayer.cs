using SheetSeller.Models.DTO;

namespace SheetSeller.Repositories.Abstract
{
    public interface IPayer
    {
        Dictionary<string, string> CreatePayment(int amount, string order);
        Status AcceptPayment(string data, string signature);
    }
}
