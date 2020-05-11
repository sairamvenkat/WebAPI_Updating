using ContactsApi.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ContactsApi.Repository
{
    public interface IContactsRepository
    {
        void Add(Contacts item);
        IEnumerable<Contacts> GetAll();
        //Contacts Find(string key);
        string Find(string key);
        void Remove(string Id);
        void Update(Contacts item);
    }
}