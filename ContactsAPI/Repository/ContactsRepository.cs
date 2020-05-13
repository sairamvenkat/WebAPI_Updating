using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ContactsApi.Models;
using ContactsAPI.Connections;

namespace ContactsApi.Repository
{
    public class ContactsRepository : IContactsRepository
    {
        static List<Contacts> ContactsList = new List<Contacts>();

        public string Add(Contacts item)
        {
            SqlConnectionStringBuilder conn = Connections.connectToDB();
            ContactsList.Add(item);
            using (var connection = new SqlConnection(conn.ConnectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "returnContactID";
                int isFamilymember = item.ISFAMILYMEMBER == true ? 1 : 0;
                command.Parameters.Add("@fn", SqlDbType.NVarChar).Value = item.FIRSTNAME.ToString();
                command.Parameters.Add("@ln", SqlDbType.NVarChar).Value = item.LASTNAME.ToString();
                command.Parameters.Add("@isfamilymember", SqlDbType.Bit).Value = isFamilymember;
                command.Parameters.Add("@gender", SqlDbType.NVarChar).Value = item.GENDER.ToString();
                command.Parameters.Add("@email", SqlDbType.NVarChar).Value = item.EMAIL.ToString();
                command.Parameters.Add("@mp", SqlDbType.NVarChar).Value = item.MOBILEPHONE.ToString();
                command.Parameters.Add("@Identity", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Connection = connection;
                command.ExecuteNonQuery();
                string contactId = command.Parameters["@Identity"].Value.ToString();
                connection.Close();
                return contactId;
            }
        }

        public string Find(string key)
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
                while (reader.Read())
                {
                    if (reader["FirstName"].ToString() == key)
                    {
                        return key;
                    }
                    else
                    {
                        key = string.Empty;
                    }
                }
            }
            return key;

            //return ContactsList
            //    .Where(e => e.FirstName.Equals(key))
            //    .SingleOrDefault();
        }

        public IEnumerable<Contacts> GetAll()
        {
            return ContactsList;
        }

        public void Remove(string key)
        {

            SqlConnectionStringBuilder conn = Connections.connectToDB();
            SqlDataReader reader;
            using (var connection = new SqlConnection(conn.ConnectionString))
            {
                connection.Open();
                string del = "DELETE FROM dbo.Contacts WHERE FirstName=@key;";
                SqlCommand command = new SqlCommand(del, connection);
                command.Parameters.AddWithValue("@key", key);
                command.ExecuteReader();
                connection.Close();

            }

            //var itemToRemove = ContactsList.SingleOrDefault(r => r.FirstName == Id);
            //if (itemToRemove != null)
            //    ContactsList.Remove(itemToRemove);
        }

        public async Task UpdateAsync(Contacts item)
        {


            SqlConnectionStringBuilder conn = Connections.connectToDB();
            using (var connection = new SqlConnection(conn.ConnectionString))
            {
                string key = item.FIRSTNAME;
                connection.Open();
                string update = "UPDATE dbo.Contacts  SET firstname =@key WHERE contactid=@contactid;";
                SqlCommand command = new SqlCommand(update, connection);
                command.Parameters.AddWithValue("@key", key);
                command.Parameters.AddWithValue("@contactid", item.CONTACTID);
                command.ExecuteReader();
                connection.Close();

            }
            //var itemToUpdate = ContactsList.SingleOrDefault(r => r.FirstName == item.FirstName);
            //if (itemToUpdate != null)
            //{
            //    itemToUpdate.FirstName = item.FirstName;
            //    itemToUpdate.LastName = item.LastName;
            //    itemToUpdate.IsFamilyMember = item.IsFamilyMember;
            //    itemToUpdate.Company = item.Company;
            //    itemToUpdate.JobTitle = item.JobTitle;
            //    itemToUpdate.Email = item.Email;
            //    itemToUpdate.MobilePhone = item.MobilePhone;
            //    itemToUpdate.DateOfBirth = item.DateOfBirth;
            //    itemToUpdate.AnniversaryDate = item.AnniversaryDate;
            //}

        }
    }
}