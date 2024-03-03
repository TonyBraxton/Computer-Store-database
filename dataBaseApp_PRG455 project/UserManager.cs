using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataBaseApp_PRG455_project
{
    internal class UserManager
    {
        public static receiptInfo infoSaved { get; set; }
        public static Dictionary<string, receiptInfo> userDictionary = new Dictionary<string, receiptInfo>();
        private static string fileName = @"SavedOrders.txt";

        public static void SaveReceipt()
        {
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                foreach (var pair in userDictionary)
                {
                    writer.Write("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", pair.Value.CustomerFirstName, pair.Value.CustomerLastName, pair.Key, pair.Value.OrderTotal, pair.Value.OrderNumber, pair.Value.OrderDate);

                }

            }

        }
    }
}
