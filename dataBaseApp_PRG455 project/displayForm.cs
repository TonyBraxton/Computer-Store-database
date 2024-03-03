using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dataBaseApp_PRG455_project
{
    public partial class displayForm : Form
    {
        private OleDbConnection connection;
        DataTable vt = new DataTable();
        string conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database11.accdb";
        
        int productID;
        string qtyStock;
        string RAM;
        string storage;
        string color;
        int price;
        public displayForm(int productID, string qtyStock, string RAM, string storage, int price, byte[] imageData)
        {
            InitializeComponent();
            connection = new OleDbConnection(conStr);
            this.productID = productID;
            this.qtyStock = qtyStock;
            this.RAM = RAM;
            this.storage = storage;
            this.price = price;
            lblQty.Text = qtyStock;
            lblRAM.Text = RAM;
            lblStorage.Text = storage;
            lblPrice.Text = price.ToString();

            if (imageData != null && imageData.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    pictureBox1.Image = Image.FromStream(ms);
                }
            }
        }

        private void displayForm_Load(object sender, EventArgs e)
        {
            
        }

        private void btContinue_Click(object sender, EventArgs e)
        {
            TransactionForm F3 = new TransactionForm(productID, price);
            F3.Show();
            this.Hide();

        }

        private void btReturn_Click(object sender, EventArgs e)
        {
            Form1 F1 = new Form1();
            F1.Show();
            this.Hide();
        }
    }
}
