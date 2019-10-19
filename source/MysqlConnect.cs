using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;


namespace CurrencyDisp
{
    class MysqlConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        public MysqlConnect()
        {
            Initialize();
        }
        private void Initialize()
        {
            server = "localhost";
            database = "currency";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public string Choose_TypeR(int cur_type)
        {
            string type;
            switch (cur_type)
            {
                case 1:
                    {
                        type = "JPY";
                        break;
                    }
                case 2:
                    {
                        type = "RUB";
                        break;
                    }
                case 3:
                    {
                        type = "GBP";
                        break;
                    }
                case 4:
                    {
                        type = "USD";
                        break;
                    }
                case 5:
                    {
                        type = "BYN";
                        break;
                    }
                case 6:
                    {
                        type = "EUR";
                        break;
                    }
                case 7:
                    {
                        type = "PLN";
                        break;
                    }
                default:
                    type = "USD";
                    break;
            }
            return type;
        }
        public int Choose_Type(string cur_type)
        {
            int type;
            switch (cur_type)
            {
                case "JPY":
                    {
                        type = 1;
                        break;
                    }
                case "RUB":
                    {
                        type = 2;
                        break;
                    }
                case "GBP":
                    {
                        type = 3;
                        break;
                    }
                case "USD":
                    {
                        type = 4;
                        break;
                    }
                case "BYN":
                    {
                        type = 5;
                        break;
                    }
                case "EUR":
                    {
                        type = 6;
                        break;
                    }
                case "PLN":
                    {
                        type = 7;
                        break;
                    }
                default:
                    type = 4;
                    break;
            }
            return type;
        }
        public void Insert(int type, string ratio, string date)
        {
            string query = "INSERT INTO ratio (type_id, ratio, r_date) VALUES ("+ type +",'"+ ratio + "','" + date +"')";
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        } 
       public List<string>[] Exists(int type, string date)
        {
            string query = "select * from ratio where type_id = " + type + " and r_date = '" + date + "'";

            //Create a list to store the result
            List<string>[] list = new List<string>[4];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["id"] + "");
                    list[1].Add(dataReader["type_id"] + "");
                    list[2].Add(dataReader["ratio"] + "");
                    list[3].Add(dataReader["r_date"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

    }
}
