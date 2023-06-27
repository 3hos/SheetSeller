using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SheetSeller.Models.DTO;
using SheetSeller.Repositories.Abstract;
using SheetSeller.Repositories.Implement;

namespace SheetSeller.Controllers
{
    public class PaymentAcception : Controller
    {
        private readonly IPayer payer;
        private readonly ISheetService sheetService;
        public PaymentAcception(IPayer payer, ISheetService sheetService)
        {
            this.payer = payer;
            this.sheetService = sheetService;
        }

        public async Task<IActionResult> AcceptAsync(string data, string signature)
        {
            var res=payer.AcceptPayment(data, signature);
            if (res.StatusCode==1)
            {
                var param = JsonConvert.DeserializeObject<LiqpayData>(res.Message);
                var username = param.order_id[..param.order_id.IndexOf("[")];
                var sheetID = param.order_id.Substring(param.order_id.IndexOf("[") + 1, param.order_id.IndexOf("]") - param.order_id.IndexOf("[")-1);
                var status = await sheetService.Own(Convert.ToInt32(sheetID), username);
                if (status.StatusCode == 1)
                {
                    return RedirectToAction("Sheet","Sheet", new { ID = sheetID });
                }
            }
            return RedirectToAction("Sheet","Create");
        }
    }
}
