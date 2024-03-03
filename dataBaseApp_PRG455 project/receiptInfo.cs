using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataBaseApp_PRG455_project
{
    internal class receiptInfo
    {
        public string CustomerFirstName { get; }
        public string CustomerLastName { get; }
        public string CustomerEmail { get; }
        public int OrderTotal { get; }
        public int OrderNumber { get; }
        public string OrderDate { get; }


        public receiptInfo(string customerFirstName, string customerLastName, string customerEmail, int orderTotal, int orderNumber, string orderDate)
        {
            CustomerFirstName = customerFirstName;
            CustomerLastName = customerLastName;
            CustomerEmail = customerEmail;
            OrderNumber = orderNumber;
            OrderDate = orderDate;
            OrderTotal = orderTotal;
        }
    }
}
