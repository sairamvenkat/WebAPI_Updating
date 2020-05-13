using System;
namespace ContactsApi.Models
{
    public class Contacts
    {
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public bool ISFAMILYMEMBER { get; set; }
        public string GENDER { get; set; }
        public string EMAIL { get; set; }
        public string MOBILEPHONE { get; set; }
        public DateTime DATEOFBIRTH { get; set; }
        public string CONTACTID { get; }
    }
}