﻿using System;
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

        public static void DebitUser(string brinqaccountNumber, string Amount)
        {
            var db = new Kaedc();
            var user = db.Kaedcuser.Where(k => k.BrinqaccountNumber == brinqaccountNumber).FirstOrDefault();

            var amount = Convert.ToInt64(Amount);
            user.MainBalance = user.MainBalance - amount;
        }

        public static void CreditUser(string brinqaccountNumber, string Amount)
        {
            var db = new Kaedc();
            var user = db.Kaedcuser.Where(k => k.BrinqaccountNumber == brinqaccountNumber).FirstOrDefault();

            var amount = Convert.ToInt64(Amount) - 50;
            
            user.MainBalance = user.MainBalance + amount;
        }

        public static void AddProfit(Kaedcuser user, Transaction transaction)
        {
            Transaction profitTransacton = new Transaction();
            Kaedc db = new Kaedc();

            var amount = transaction.Amount;
            const decimal commission = 0.005M;
            decimal profit = 0.0M;
            if (transaction.ServiceId == 1)
            {
                profit = (Convert.ToDecimal(transaction.Amount) * commission);
            }
            user.MainBalance += profit;

            
            profitTransacton.Id = GenerateId();
            profitTransacton.ServiceId = 6;
            profitTransacton.AgentProfit = profit;
            profitTransacton.Amount = profit.ToString();
            profitTransacton.PayersName = user.UserName;
            profitTransacton.PaymentMethodId = 1;
            profitTransacton.transactionsStatus = "completed";
            profitTransacton.Datetime = DateTime.Now;
            profitTransacton.KaedcUserNavigation = user;

            db.Add(profitTransacton);
            
        }
    }
}