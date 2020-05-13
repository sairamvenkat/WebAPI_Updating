using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using ContactsApi.Models;
using ContactsAPI.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace ContactsApi.Repository
{
    public class ContactsRoleRepository : IContactsRoleRepository
    {
        static List<ContactsRole> ContactsRoleList = new List<ContactsRole>();

        public bool GetRoledetails(int id)
        {
            List<string> role = new List<string>() ;
            bool isAdmin = false;
            SqlDataReader reader; ;
            SqlConnectionStringBuilder conn = Connections.connectToDB();
            using (var connection = new SqlConnection(conn.ConnectionString))
            {
                string getRole = "select * from ContactRole where ContactId=@ContactId";
                connection.Open();
                SqlCommand cmd = new SqlCommand(getRole, connection);
                cmd.Parameters.AddWithValue("@contactId", id);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    role.Add(Convert.ToString(reader["ContactRole"]));
                }
                if (role != null && role.Contains("Administrator"))
                {
                    isAdmin = true;
                }
                connection.Close();
            }

            return isAdmin;
        }

        public void Add(ContactsRole item)
        {
            SqlConnectionStringBuilder conn = Connections.connectToDB();
            //bool isAdmin = GetRoledetails(item.CONTACTID);
            //if (isAdmin)
            //{
                using (var connection = new SqlConnection(conn.ConnectionString))
                {
                    //Search if a contact with provided ID exists in db or not if exists update Contact Role else terminate with status code 404.
                    string search = "select * from contacts where ContactId=@ContactId";
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(search, connection);
                    cmd.Parameters.AddWithValue("@contactId", item.CONTACTID);
                    // cmd.ExecuteReader();
                    while (cmd.ExecuteReader().Read())
                    {
                        String query = "INSERT INTO dbo.ContactRole (ContactId, ContactRole) VALUES ('" + Convert.ToInt32(item.CONTACTID)
                            + "','" + item.CONTACTROLE.ToString()
                            + "');";
                        SqlCommand cmd1 = new SqlCommand(query, connection);
                        cmd1.ExecuteNonQuery();
                        connection.Close();
                    }
                    if (!cmd.ExecuteReader().Read())
                    {
                        //return HttpStatusCode.NotFound;
                    }
                }
         //   }
        }

        public void Update(ContactsRole item)
        {
            SqlConnectionStringBuilder conn = Connections.connectToDB();
            bool isAdmin = GetRoledetails(item.CONTACTID);
            if (isAdmin)
            {
                using (var connection = new SqlConnection(conn.ConnectionString))
                {
                    string search = "select * from contacts where ContactId=@ContactId";
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(search, connection);
                    cmd.Parameters.AddWithValue("@contactId", item.CONTACTID);
                    if (cmd.ExecuteReader().Read())
                    {
                        int contactId = item.CONTACTID;
                        string contactRole = item.CONTACTROLE;
                        string update = "UPDATE dbo.ContactRole SET ContactRole =@contactRole WHERE Contactid=@contactId';";
                        SqlCommand command = new SqlCommand(update, connection);
                        command.Parameters.AddWithValue("@contactId", item.CONTACTID);
                        command.Parameters.AddWithValue("@contactRole", item.CONTACTROLE);
                        command.ExecuteReader();
                        connection.Close();
                    }
                }
            }
        }
        //Returns roles of specified contacts.
        public AssignedContactRoles Find(int item)
        {
            SqlConnectionStringBuilder conn = Connections.connectToDB();
            using (var connection = new SqlConnection(conn.ConnectionString))
            {
                connection.Open();
                string find = "select c.ContactId,c.FirstName+c.LastName as FullName,cr.contactRole from Contacts c inner join ContactRole cr on c.@ContactId = cr.@ContactId";
                //string update = "select * from  dbo.ContactRole WHERE Contactid=@contactId';";
                SqlCommand command = new SqlCommand(find, connection);
                command.Parameters.AddWithValue("@contactId", item);
                command.ExecuteReader();
                connection.Close();

                //Construct ur AssignedContactRoles Object 
                return new AssignedContactRoles();
            }
        }

        public void Remove(int item)
        {
            SqlConnectionStringBuilder conn = Connections.connectToDB();
            using (var connection = new SqlConnection(conn.ConnectionString))
            {
                int contactId = item;
                connection.Open();
                string del = "Delete from  dbo.ContactRole WHERE Contactid=@contactId;";
                SqlCommand command = new SqlCommand(del, connection);
                command.Parameters.AddWithValue("@contactId", item);
                command.ExecuteReader();
                connection.Close();
            }
        }
    }
}