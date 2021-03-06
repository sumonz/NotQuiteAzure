﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Globalization;

namespace NotQuiteAzure
{
    public class DatabaseConnection
    {
        string connectionString = "Server=(local)\\SQLEXPRESS;Initial Catalog=ClaimsReporting;Integrated Security=SSPI";

        public Customer GetCustomer(string customerId)
        {
            Customer customer = new Customer();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(
                    "SELECT TOP 1 cust_ID, fname, lname, DOB, home_phone, work_phone, address, email FROM Customers " +
                    "WHERE cust_ID = " + customerId, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customer.id = reader.GetString(0);
                        customer.firstName = reader.GetString(1);
                        customer.lastName = reader.GetString(2);
                        customer.dateOfBirth = reader.GetDateTime(3);
                        customer.homePhone = reader.GetString(4);
                        customer.workPhone = reader.GetString(5);
                        customer.address = reader.GetString(6);
                        customer.email = reader.GetString(7);
                    }
                }
            }
            return customer;
        }

        public void CreateCall(string customerId, string customerPhone)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {     
                SqlCommand nonqueryCommand = connection.CreateCommand();
                connection.Open();

                nonqueryCommand.CommandText = "INSERT INTO Call (cust_ID, phone_number) VALUES (@customerId, @customerPhone)";

                nonqueryCommand.Parameters.Add("@customerId", customerId);
                nonqueryCommand.Parameters.Add("@customerPhone", customerPhone);
                nonqueryCommand.ExecuteNonQuery();
            }
        }

        public void CreateClaim(Claim claim)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand nonqueryCommand = connection.CreateCommand();
                connection.Open();

                nonqueryCommand.CommandText = "INSERT INTO Claims (claim_ID, policy_ID, cust_ID) VALUES (@claim_ID, @policy_ID, @cust_ID)";

                nonqueryCommand.Parameters.Add("@claim_ID", claim.id);
                nonqueryCommand.Parameters.Add("@policy_ID", claim.policyId);
                nonqueryCommand.Parameters.Add("@cust_ID", claim.customerId);
                nonqueryCommand.ExecuteNonQuery();
            }
        }
    }
}