using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;

namespace dataBaseApp_PRG455_project
{
    public partial class TransactionForm : Form
    {
        OleDbConnection connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database11.accdb");
        OleDbCommand commnd = new OleDbCommand();
        string conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database11.accdb";
        int productID;
        int price;
        int randomOrderID;
        int randomCustomerID;
        public TransactionForm(int productID, int price)
        {
            InitializeComponent();
            this.productID = productID;
            this.price = price;
            connection = new OleDbConnection(conStr);
        }

        private void TransactionForm_Load(object sender, EventArgs e)
        {
            Random random = new Random();

            do
            {
                randomOrderID = random.Next();
                randomCustomerID = random.Next();

            }
            while (OrderIDExistsInDatabase(randomCustomerID) && CustomerIDExistsInDatabase(randomCustomerID));

        }
        private bool OrderIDExistsInDatabase(int orderID)
        {
            bool exists = false;

            try
            {
                string query = "SELECT orderID FROM OrdersData WHERE orderID = @OrderID";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", orderID);
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    exists = count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking order ID existence: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return exists;
        }
        private void SaveOrderDetails(int orderID, int customerID, int productID, int total, string quantity)
        {

            try
            {
                string query = "INSERT INTO OrdersData (orderID, customerID, productID, total, orderDate, quantity)" +
                               "VALUES (@OrderID, @CustomerID, @ProductID, @Total, @OrderDate, @Quantity)";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", orderID);
                    command.Parameters.AddWithValue("@CustomerID", customerID);
                    command.Parameters.AddWithValue("@ProductID", productID);
                    command.Parameters.AddWithValue("@Total", total);
                    command.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                    command.Parameters.AddWithValue("@Quantity", quantity);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving order details: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private bool CustomerIDExistsInDatabase(int customerID)
        {
            bool exists = false;

            try
            {
                string query = "SELECT customerID FROM CustomerData WHERE customerID = @CustomerID";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerID);
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    exists = count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking customer ID existence: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return exists;
        }
        private void SaveCustomerDetails(int customerID, string firstName, string lastName, string phoneNumber, string cardNumber, string email)
        {
            OleDbConnection connection = new OleDbConnection(conStr);

            try
            {
                string query = "INSERT INTO CustomerData (customerID, firstName, lastName, phoneNumber, cardNumber, email) " +
                               "VALUES (@CustomerID, @FirstName, @LastName, @PhoneNumber, @CardNumber, @Email); ";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerID);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@cardNumber", cardNumber);
                    command.Parameters.AddWithValue("@Email", email);
                    OleDbDataAdapter adapter = new OleDbDataAdapter();
                    connection.Open();
                    adapter.InsertCommand = command;
                    adapter.InsertCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving customer details: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    private void UpdateQuantityInStock(int productID, string quantity)
        {
            OleDbConnection connection = new OleDbConnection(conStr);

            try
            {
                string query = "UPDATE productsData1 SET quantityInStock = quantityInStock - @quantity WHERE productID = "+productID+"";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@ProductID", productID);
                    OleDbDataAdapter adapter = new OleDbDataAdapter();
                    connection.Open();
                    adapter.UpdateCommand = command;
                    adapter.UpdateCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating stock quantity: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btPurchase_Click(object sender, EventArgs e)
        {
            string firstName = tbFirstName.Text;
            string lastName = tbLastName.Text;
            string phoneNumber = tbPhoneNumber.Text;
            string cardNumber = tbCardNumber.Text;
            string email = tbEmail.Text;
            string quantity = tbQty.Text;
            int total = price*Convert.ToInt32(quantity);

            SaveCustomerDetails(randomCustomerID, firstName, lastName, phoneNumber, cardNumber, email);
            SaveOrderDetails(randomOrderID, randomCustomerID, productID, total, quantity);
            UpdateQuantityInStock(productID, quantity);
            //Show Form3 (Receipt Display Form)
            receiptInfo receipt = new receiptInfo(firstName, lastName, email, total, randomOrderID, DateTime.Now.ToString());
            UserManager.userDictionary[email] = receipt;
            UserManager.infoSaved = receipt;
            UserManager.SaveReceipt();
            Receipt F4 = new Receipt();
            F4.Show();
            this.Hide();

        }
    }
}
