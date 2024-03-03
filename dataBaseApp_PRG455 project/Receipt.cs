using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dataBaseApp_PRG455_project
{
    public partial class Receipt : Form
    {
        public Receipt()
        {
                InitializeComponent();
        }

        private void Receipt_Load(object sender, EventArgs e)
        {
            receiptInfo receipt = UserManager.infoSaved;

            lblCustomerFirstName.Text = receipt.CustomerFirstName;
            lblCustomerLastName.Text = receipt.CustomerLastName;
            lblCustomerEmail.Text = receipt.CustomerEmail;
            lblOrderTotal.Text = receipt.OrderTotal.ToString();
            lblOrderNumber.Text = receipt.OrderNumber.ToString();
            lblOrderDate.Text = receipt.OrderDate.ToString();


        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
