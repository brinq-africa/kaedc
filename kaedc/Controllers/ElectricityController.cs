using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using kaedc.Models;
using kaedc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace kaedc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElectricityController : ControllerBase
    {
        private Kaedc _db;
        public static IConfiguration config;
        private UserManager<Kaedcuser> _userManager;
        static string baseUri_MeterInfo = "https://irecharge.com.ng/pwr_api_sandbox/v2/get_meter_info.php"; //TODO: Parameterize
        static string baseUri_VendPower = "https://irecharge.com.ng/pwr_api_sandbox/v2/vend_power.php"; //TODO: Parameterize
        static string pubkey = "c5165ff3eab458e89425bad8c4f0908c"; //TODO: Parameterize
        static string privKey = "66a7b282a044c656f037230200a3f53ea6e25227b8977c15e91822505bedc0319fbb69338f565c2e834382e62d2ebf8dddd35c5b14b44d07750b0597bdf106c5"; //TODO: Parameterize
        static string vendor_code = "1901E58329"; //TODO: Parameterize



        public ElectricityController(Kaedc db, IConfiguration _config, UserManager<Kaedcuser> userManager)
        {
            _db = db;
            config = _config;
            _userManager = userManager;
        }

        //static string vendor_code = config["Irecharge:VendorCode"];
        //static string pubkey = config["Irecharge:PublicKey"];
        //static string privKey = config["Irecharge:PrivateKey"];
        //static string baseUri_MeterInfoAEDC = config["Irecharge:MeterInfoURL"];
        //static string baseUri_VendPowerAEDC = config["Irecharge:VendPowerURL"];

        [HttpGet]
        [Route("alltransactions")]
        [Authorize]
        public IEnumerable<Transaction> Get()
        {
            return _db.Transaction.Where(t => t.ServiceId == 1);
        }


        // POST: api/Electricity        
        [HttpPost("FromBody")]
        [Authorize]
        [Route("GetMeterInfo")]        
        public async Task<IActionResult> GetMeterInfoAsync([FromBody] Transaction transaction)
        {            
            if (!ModelState.IsValid)
            {
                return BadRequest("Model State is not Valid");
            }
            if (transaction.Meternumber == null || transaction.RecipientPhoneNumber == null)
            {
                return BadRequest("Null Values");
            }

            //get user details
            var username = HttpContext.User.Identity.Name;
            var user = _userManager.FindByEmailAsync(username).Result;

            if (user.MainBalance <= Convert.ToDecimal(transaction.Amount))
            {
                return Ok("Insufficient Balance");
            }

            string meter_number = transaction.Meternumber;
            //string disco = transaction.Service.Name;
            string disco;
            if (transaction.ServiceId == 1)
            {
                disco = "Kaduna_Electricity_Disco";
            }
            else if (transaction.ServiceId == 2)
            {
                disco = "Kaduna_Electricity_Disco_Postpaid";
            }
            else
            {
                return BadRequest("Invalid Service");
            }

            //var service = await _db.Service.Where(s => s.Id == transaction.ServiceId).FirstAsync();


            string ref_id = ExtraMethods.GenerateRandomNumber();
            string combinedstring = vendor_code + "|" + ref_id + "|" + meter_number + "|" + disco + "|" + pubkey;
            byte[] key = Encoding.ASCII.GetBytes(privKey);
            string hash = ExtraMethods.GenerateHash(combinedstring, key);


            WebClient webClient = new WebClient();
            webClient.QueryString.Add("vendor_code", vendor_code);
            webClient.QueryString.Add("meter", meter_number);
            webClient.QueryString.Add("reference_id", ref_id);
            webClient.QueryString.Add("disco", disco);
            webClient.QueryString.Add("response_format", "json");
            webClient.QueryString.Add("hash", hash);

            try
            {
                string response = webClient.DownloadString(baseUri_MeterInfo);

                var jsonresult = JsonConvert.DeserializeObject<iRechargeMeterInfo>(response);

                transaction.PayerIp = ExtraMethods.GetLocalIPAddress();
                transaction.PaymentMethodId = 1;
                transaction.MeterName = jsonresult.customer.name;
                transaction.Statuscode = Convert.ToInt32(jsonresult.status);
                transaction.StatusMessage = jsonresult.message;
                transaction.ApiUniqueReference = ref_id;
                transaction.Service = _db.Service.Where(s => s.Id == transaction.ServiceId).FirstOrDefault();
                transaction.PhcnUnique = jsonresult.access_token;
                transaction.Hash = hash;
                transaction.transactionsStatus = "initiated";
                transaction.Datetime = DateTime.Now;
                transaction.KaedcUserNavigation = user;
                transaction.KaedcUser = user.Id;
                transaction.Amount = (Convert.ToDouble(transaction.Amount) + 100).ToString();
                transaction.PayersName = user.UserName;

                _db.Transaction.Add(transaction);
                await _db.SaveChangesAsync();

                return Ok(transaction);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                    {
                        return Ok("Unable to retrieve meter details");
                    }
                }
                throw;
            }
        }

        
        [HttpPost]
        [Authorize]
        [Route("vendpower/{id}")]
        public IActionResult VendMeter(int id, Transaction transaction)
        {
            //var transaction = db.Transaction.Where(t => t.Id == id).FirstOrDefault(); 

            if (transaction.Id != id)
            {
                return BadRequest("Invalid Transaction");
            }

            var transact = _db.Transaction.Where(t => t.Id == transaction.Id).FirstOrDefault();
            //get user details
            var username = HttpContext.User.Identity.Name;
            var user = _userManager.FindByEmailAsync(username).Result;

            if (user.MainBalance <= Convert.ToDecimal(transact.Amount))
            {
                return Ok("Insufficient Balance"); 
            }


            string disco;
            if (transaction.ServiceId == 1)
            {
                disco = "Kaduna_Electricity_Disco";
            }
            else if (transaction.ServiceId == 2)
            {
                disco = "Kaduna_Electricity_Disco_Postpaid";
            }
            else
            {
                return BadRequest("Invalid Service");
            }

            string meter_number = transact.Meternumber;
            string ref_id = transact.ApiUniqueReference;
            string access_token = transact.PhcnUnique;
            string amount = transact.Amount;
            string phone = transact.RecipientPhoneNumber;

            //string email = transaction.BrinqpayUserNavigation.Email;
            string email = user.Email;

            string combinedstring = vendor_code + "|" + ref_id + "|" + meter_number + "|" + disco + "|" + amount + "|" + access_token + "|" + pubkey;
            byte[] key = Encoding.ASCII.GetBytes(privKey);
            string hash = ExtraMethods.GenerateHash(combinedstring, key);

            try
            {
                WebClient webClient = new WebClient();
                webClient.QueryString.Add("vendor_code", vendor_code);
                webClient.QueryString.Add("meter", meter_number);
                webClient.QueryString.Add("reference_id", ref_id);
                webClient.QueryString.Add("response_format", "json");
                webClient.QueryString.Add("disco", disco);
                webClient.QueryString.Add("access_token", access_token);
                webClient.QueryString.Add("amount", amount);
                webClient.QueryString.Add("phone", phone);
                webClient.QueryString.Add("email", email);
                webClient.QueryString.Add("hash", hash);

                //var fullstring = webClient.QueryString.ToString();
                string response = webClient.DownloadString(baseUri_VendPower);

                

                var jsonresult = JsonConvert.DeserializeObject<iRechargeVendPower>(response);

                transact.PayerIp = ExtraMethods.GetLocalIPAddress();
                transact.PaymentMethodId = 1;
                transact.Statuscode = Convert.ToInt32(jsonresult.status);
                transact.StatusMessage = jsonresult.message;
                transact.ApiUniqueReference = ref_id;
                transact.GatewayresponseMessage = response;
                transact.Token = jsonresult.meter_token;
                transact.TopUpValue = jsonresult.units;

                if (jsonresult.status != "00")
                {
                    transact.transactionsStatus = "pending";
                }
                else
                {
                    transact.transactionsStatus = "completed";
                    user.MainBalance = user.MainBalance - Convert.ToDecimal(transact.Amount);
                    ExtraMethods.AddProfit(user, transact);
                    transact.AgentProfit = 0.005M * Convert.ToDecimal(transact.Amount);
                }

                //db.Entry(transaction).State = EntityState.Modified;
                _db.Update(transact);
                _db.SaveChanges();

                return Ok(transact);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                    {
                        return Ok("Transaction could not be completed");
                    }
                }

                throw;
            }
        }

        private class iRechargeMeterInfo
        {
            public string status { get; set; }
            public string message { get; set; }
            public string access_token { get; set; }
            public string minimum_purchase { get; set; }
            public string response_hash { get; set; }
            public Customer customer { get; set; }

            public class Customer
            {
                public string name { get; set; }
                public string address { get; set; }
                public string util { get; set; }
                public string minimumAmount { get; set; }
            }
        }

        private class iRechargeVendPower
        {
            public string status { get; set; }
            public string message { get; set; }
            public string wallet_balance { get; set; }
            public string reff { get; set; }
            public string amounts { get; set; }
            public string amount { get; set; }
            public string units { get; set; }
            public string meter_token { get; set; }
            public string address { get; set; }
            public string hash { get; set; }

        }
    }
}
