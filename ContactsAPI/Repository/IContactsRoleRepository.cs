using ContactsApi.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ContactsApi.Repository
{
    public interface IContactsRoleRepository
    {
        void Add(ContactsRole item);
        void Update(ContactsRole item);
        AssignedContactRoles Find(int key);
        void Remove(int key);

    }
}