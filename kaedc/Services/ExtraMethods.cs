using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using kaedc.Models;

namespace kaedc.Services
{
    public class ExtraMethods
    {
        public static string GenerateRandomNumber()
        {
            Random rnd = new Random();
            string r = "";
            int i;
            for (i = 0; i < 12; i++)
            {
                r += rnd.Next(0, 9).ToString();
            }

            var ref_id = Convert.ToInt64(r);
            return r;
        }

        public  static int GenerateId()
        {
            Kaedc db = new Kaedc();
            var lastId = db.Transaction.LastOrDefault().Id + 1;
            return lastId;
        }

        public static decimal BrinqProfit(decimal amount, int service)
        {
            //this can be changed based on commisions made available to Brinq Africa by her Superior Vendor
            decimal profit = 0.00M;
            if (service == 1)
            {
                profit = (amount - 100) * 0.015M;
                return profit;
            }
            else if(service == 2)
            {
                profit = (amount) * 0.02M;
                return profit;
            }

            return 0.00M;
        }        

        public static string GenerateHash(string combinedString, byte[] key)
        {
            HMACSHA1 myhmacsha1 = new HMACSHA1(key);
            byte[] byteArray = Encoding.ASCII.GetBytes(combinedString);
            MemoryStream stream = new MemoryStream(byteArray);
            return myhmacsha1.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "No network adapters with an IPv4 address in the system!";
        }

        public static void DebitUser(string brinqaccountNumber, double Amount)
        {
            Transaction transaction = new Transaction();
            var db = new Kaedc();
            var user = db.Kaedcuser.Where(k => k.BrinqaccountNumber == brinqaccountNumber).FirstOrDefault();

            //update user's balance
            var amount = Convert.ToInt64(Amount);
            user.MainBalance = user.MainBalance - amount;

            //update DB
            transaction.Id = GenerateId();
            transaction.ServiceId = 3;
            transaction.Amount = Amount;
            transaction.PayersName = user.UserName;
            transaction.PaymentMethodId = 1;
            transaction.transactionsStatus = "completed";
            transaction.Datetime = DateTime.Now;
            transaction.KaedcUserNavigation = user;
            transaction.Service = db.Service.Where(s => s.Id == 4).FirstOrDefault();

            db.Add(transaction);
            db.SaveChanges();
        }

        public static void CreditUser(string brinqaccountNumber, double Amount)
        {
            Transaction transaction = new Transaction();
            var db = new Kaedc();


            var user = db.Kaedcuser.Where(k => k.BrinqaccountNumber == brinqaccountNumber).FirstOrDefault();

            //deduct 50naira as stamp duty fee
            var amount = Convert.ToInt64(Amount) - 50;
            
            //update user's balance
            user.MainBalance = user.MainBalance + amount;

            //update DB
            transaction.Id = GenerateId();
            transaction.ServiceId = 3;
            transaction.Amount = Amount;
            transaction.PayersName = user.UserName;
            transaction.PaymentMethodId = 1;
            transaction.transactionsStatus = "completed";
            transaction.Datetime = DateTime.Now;
            transaction.KaedcUserNavigation = user;
            transaction.Service = db.Service.Where(s => s.Id == 3).FirstOrDefault();

            db.Add(transaction);
            db.SaveChanges();
        }

        public static void AddProfit(Kaedcuser user, Transaction transaction)
        {
            Transaction profitTransacton = new Transaction();
            Kaedc db = new Kaedc();

            var amount = transaction.Amount;
            const double commission = 0.005;
            double profit = 0.0;
            if (transaction.ServiceId == 1)
            {
                profit = transaction.Amount * commission;
            }
            user.MainBalance += Convert.ToDecimal(profit);

            
            profitTransacton.Id = GenerateId();
            profitTransacton.ServiceId = 6;
            profitTransacton.AgentProfit = Convert.ToDecimal(profit);
            profitTransacton.Amount = profit;
            profitTransacton.PayersName = user.UserName;
            profitTransacton.PaymentMethodId = 1;
            profitTransacton.transactionsStatus = "completed";
            profitTransacton.Datetime = DateTime.Now;
            profitTransacton.KaedcUserNavigation = user;
            //profitTransacton.Service = db.Service.Where(s => s.Id == 6).FirstOrDefault();

            db.Add(profitTransacton);
            
        }
    }
}
