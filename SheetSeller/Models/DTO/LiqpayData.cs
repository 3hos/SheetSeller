namespace SheetSeller.Models.DTO
{
    public class LiqpayData
    {
        public string version { get; set; } = "3";
        public string public_key { get; set; }
        public string private_key { get; set; }
        public string amount { get; set; }
        public string currency { get; set; } = "USD";
        public string description { get; set; } = "Test Payment";
        public string order_id { get; set; }
        public string action { get; set; } = "pay";
        public string result_url { get; set; } = "https://sheetseller20230720213642.azurewebsites.net/PaymentAcception/Accept";
    }
}
