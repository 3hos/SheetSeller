using SheetSeller.Models.DTO;
using SheetSeller.Repositories.Abstract;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace SheetSeller.Repositories.Implement
{
    public class Payer : IPayer
    {
        private readonly string publicKey = "sandbox_i62122614525";
        private readonly string privateKey = Environment.GetEnvironmentVariable("LiqPayPrivateKey", EnvironmentVariableTarget.Machine);

        public Dictionary<string, string> CreatePayment(int amount,string order)
        {
            var param = new LiqpayData() 
            { 
                public_key = publicKey,
                private_key = privateKey,
                amount = amount.ToString(),
                order_id = order 
            };
            var json_string = JsonConvert.SerializeObject(param);
            var data = Convert.ToBase64String(Encoding.UTF8.GetBytes(json_string));

            var sign_string = $"{privateKey}{data}{privateKey}";
            var sha1 = SHA1.Create();
            var signature = Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(sign_string)));

            Dictionary<string, string> res = new()
            {
                { "data", data },
                { "signature", signature }
            };
            return res;
        }
        public Status AcceptPayment(string data, string signature)
        {
            var sha1 = SHA1.Create();
            var new_signature= Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes($"{privateKey}{data}{privateKey}")));
            if(signature != new_signature)
            {
                return new Status() { StatusCode = 0, Message = "Wrong signature" };
            }
            var param = Encoding.UTF8.GetString(Convert.FromBase64String(data));
            return new Status() { StatusCode = 1,Message=param };
        }
    }
}
