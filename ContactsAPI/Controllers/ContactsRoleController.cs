using ContactsApi.Models;
using ContactsApi.Repository;
using Microsoft.AspNetCore.Mvc;
using ContactsAPI.Connections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ContactsApi.Controllers
{
    [Route("api/[controller]")]
    public class ContactsRoleController : Controller
    {
        public IContactsRoleRepository ContactsRoleRepo { get; set; }
        public ContactsRoleController(IContactsRoleRepository _repo)
        {
            ContactsRoleRepo = _repo;
        }

        //Name="GetContacts" in the following verb is used to redirect to controller/GetContacts path and fetch data as per the passed id
        //Get user roles
        [HttpGet("{id}", Name = "GetContactsRole")]
        public IActionResult GetById(int id)
        {
            var item = ContactsRoleRepo.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        //When User Gets Reggistered Assign basic role
        [HttpPost]
        public IActionResult Create([FromBody] ContactsRole item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            ContactsRoleRepo.Add(item);
            return CreatedAtRoute("GetContactsRole", new { Controller = "ContactsRoleController", id = item.CONTACTID }, item);
        }

        //When User Gets unreggistered delete respective roles 
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            ContactsRoleRepo.Remove(id);
        }

        [HttpPut("{id}")]
        public void Update(ContactsRole item)
        {
            ContactsRoleRepository conRep = new ContactsRoleRepository();
            Update(item);
        }
    }
}