﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Collections;
using SQL_Injection.Windowss;
using SQL_Injection_Phase1_440.Windowss;

namespace SQL_Injection
{
    public partial class Login : Form
    {
        //varibales that we will need to connect to the mysql database
        static string connectionString = "Server=127.0.0.1;Database=project_phase_1_db;Uid=root;Pwd=123;";
        MySqlConnection connection = new MySqlConnection(connectionString);
       
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Load(object sender, EventArgs e)
        {

        }
        //this is the enter button for the login page
        private void Button1_Click(object sender, EventArgs e)
        {
            //this is where we will take the UID from the user table and pass it to the ratePage for future use
            //find the UID that is in play by comparing both the username and password  in the query 

            //we create our variabels that we will be using
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            //adding the @ in the sql query code will stop from user accesing the actual sql code, so whatever they put
            //will have to be passed through another area. This will make it alot harder for user to try to do a sql injection attack
            string query = $"SELECT * FROM user WHERE username = @username AND password = @password";

            string getUID = $"SELECT UID FROM user WHERE username = @username AND password = @password";

            connection.Open();
            // Create the command that will run the command
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlCommand cmd = new MySqlCommand(getUID, connection);

            //the input for the login page will be passed through here which will make it more secure since user can no longer pass sql code directly
            //into sql
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            //command.ExecuteNonQuery();
            object result = cmd.ExecuteScalar();
            //we then call reader and have it ready to exectue
            MySqlDataReader reader = command.ExecuteReader();
            //we first check the input is null if it is then we send the user a message letting them know that they didnt put anything
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                connection.Close();
                MessageBox.Show("Please input Username and Password");
            }
            //if user did put something then we come here
            else
            {
                // Check if the login was successful
                if (reader.HasRows)
                {
                    //this is how we pass the id or user ID to ratePage which we will use to keep track of who rates, how many times they have rated, and if they have ratede 3 times in the same day
                    if (result != null)
                    {
                        // UID found, pass it to Rate_Page constructor
                        int uid = Convert.ToInt32(result);
                        Rate_Page ratePage = new Rate_Page();
                        ratePage.SetUid(uid);
                        Console.WriteLine("found the UID and PASSED to the RatePage");
                    }
                    else
                    {
                        // UID not found, display error message to user
                        MessageBox.Show("Invalid username or password.");
                    }


                    MessageBox.Show("Login successful!");
                    //this will open up the product page
                    ProductPage pdP = new ProductPage();
                    pdP.Show();
                    // Close the connection and the reader and the login page
                    reader.Close();
                    connection.Close();
                    this.Close();
                }
                //else if the login info was wrong we come here
                else
                {
                    //we let the user know that the login info was wrong and to try again
                    MessageBox.Show("Login failed. Please try again.");
                    //we then close the connection and reader to retry 
                    reader.Close();
                    connection.Close();
                }
            }

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            //opens up the sign up page 
            SignUp s = new SignUp();
            s.Show();
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            //is called when the user clicks the exit button
            Application.Exit();
        }
    }
}
