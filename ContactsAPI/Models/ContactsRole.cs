using System;
namespace ContactsApi.Models
{
    public class ContactsRole
    {
        public int CONTACTID { get; set; }
        public string CONTACTROLE { get; set; }

    }
    public class AssignedContactRoles
    {
        public int CONTACTID { get; set; }
        public string FULLNAME { get; set; }
        public string CONTACTROLE { get; set; }

    }
}