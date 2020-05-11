using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ContactsApi.Models;
using ContactsAPI.Connections;

namespace ContactsApi.Repository
{
    public class ContactsRepository : IContactsRepository
    {
        static List<Contacts> ContactsList = new List<Contacts>();

        public void Add(Contacts item)
        {
            SqlConnectionStringBuilder conn = Connections.connectToDB();
            ContactsList.Add(item);
            using (var connection = new SqlConnection(conn.ConnectionString))
            {
                String query = "INSERT INTO dbo.Contacts (FirstName, LastName) VALUES ('" + item.FirstName.ToString() + "','" + item.LastName.ToString() +
                    "');";
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public SqlDataReader Find(string key)
        {
            SqlConnectionStringBuilder conn = Connections.connectToDB();
            SqlDataReader reader;
            using (var connection = new SqlConnection(conn.ConnectionString))
            {
                String query = "select * from dbo.Contacts where FirstName=@key";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@key", key);
                connection.Open();
                reader = command.ExecuteReader();
                //   SqlCommand cmd = new SqlCommand(query, connection);
                //   cmd.ExecuteNonQuery();
               
            }
            return reader;
           
            //return ContactsList
            //    .Where(e => e.FirstName.Equals(key))
            //    .SingleOrDefault();
        }

        public IEnumerable<Contacts> GetAll()
        {
            return ContactsList;
        }

        public void Remove(string Id)
        {
            var itemToRemove = ContactsList.SingleOrDefault(r => r.FirstName == Id);
            if (itemToRemove != null)
                ContactsList.Remove(itemToRemove);
        }

        public void Update(Contacts item)
        {
            var itemToUpdate = ContactsList.SingleOrDefault(r => r.MobilePhone == item.MobilePhone);
            if (itemToUpdate != null)
            {
                itemToUpdate.FirstName = item.FirstName;
                itemToUpdate.LastName = item.LastName;
                itemToUpdate.IsFamilyMember = item.IsFamilyMember;
                itemToUpdate.Company = item.Company;
                itemToUpdate.JobTitle = item.JobTitle;
                itemToUpdate.Email = item.Email;
                itemToUpdate.MobilePhone = item.MobilePhone;
                itemToUpdate.DateOfBirth = item.DateOfBirth;
                itemToUpdate.AnniversaryDate = item.AnniversaryDate;
            }
        }
    }
}