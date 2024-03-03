using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dataBaseApp_PRG455_project
{
    public partial class Form1 : Form
    {
        private OleDbConnection connection;
        DataTable vt = new DataTable();
        string conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database11.accdb";
        public Form1()
        {
            InitializeComponent();
            connection = new OleDbConnection(conStr);
        }
        public void VTInitialize(string select)
        {
            string sqlStr = select;
            OleDbDataAdapter adapt = new OleDbDataAdapter(sqlStr, conStr);
            adapt.Fill(vt);
            adapt.Dispose();
            dataGridView1.DataSource = vt;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.VTInitialize("SELECT CustomerData.customerID FROM CustomerData INNER JOIN OrdersData ON CustomerData.customerID=OrdersData.customerID WHERE OrdersData.quantity > '1'");

        }
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int productID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["productID"].Value);
                string qtyStock = dataGridView1.Rows[e.RowIndex].Cells["quantityInStock"].Value.ToString();
                string RAM = dataGridView1.Rows[e.RowIndex].Cells["RAM"].Value.ToString();
                string storage = dataGridView1.Rows[e.RowIndex].Cells["Storage"].Value.ToString();
                int price = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["price"].Value);
                byte[] imageData = GetProductImageFromDatabase(productID);

                displayForm F2 = new displayForm(productID, qtyStock, RAM, storage, price, imageData);
                F2.Show();
                this.Hide();

            }
        }
        private void btSearch_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM productsData1 WHERE " +
                "model LIKE '%' + @SearchTerm + '%'" +
                "OR type LIKE '%' + @SearchTerm + '%' " +
                "OR RAM LIKE '%' + @SearchTerm + '%'";
            string searchTerm = tbSearch.Text.Trim();
            OleDbDataAdapter adapt = new OleDbDataAdapter(query, conStr);
            adapt.SelectCommand.Parameters.AddWithValue("@SearchTerm", searchTerm);
            vt.Clear();
            adapt.Fill(vt);
            dataGridView1.DataSource = vt;
        }
        private byte[] GetProductImageFromDatabase(int productID)
        {
            byte[] imageData = null;

            try
            {
                string query = "SELECT productImage FROM productsData WHERE productID = @ProductID";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productID);
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            imageData = (byte[])reader["productImage"];
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading product image: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return imageData;
        }
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;

            MessageBox.Show("Error occurred while displaying data: " + e.Exception.Message);
        }
    }
}
