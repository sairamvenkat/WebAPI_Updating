using ContactsApi.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ContactsApi.Repository
{
    public interface IContactsRepository
    {
        string Add(Contacts item);
        IEnumerable<Contacts> GetAll();
        //Contacts Find(string key);
        string Find(string key);
        void Remove(string Id);
        Task UpdateAsync(Contacts item);
    }
}