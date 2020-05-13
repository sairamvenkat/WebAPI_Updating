using ContactsApi.Models;
using ContactsApi.Repository;
using Microsoft.AspNetCore.Mvc;
using ContactsAPI.Connections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Net;
using Nancy.Json;
using System.Text;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ContactsApi.Controllers
{
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        public IContactsRepository ContactsRepo { get; set; }
        public ContactsController(IContactsRepository _repo)
        {
            ContactsRepo = _repo;
        }

        [HttpGet]
        public IEnumerable<Contacts> GetAll()
        {
            return ContactsRepo.GetAll();
        }

        //Name="GetContacts" in the following verb is used to redirect to controller/GetContacts path and fetch data as per the passed id
        [HttpGet("{id}", Name = "GetContacts")]
        public IActionResult GetById(string id)
        {
            var item = ContactsRepo.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Contacts item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            string contactId = ContactsRepo.Add(item);
            if (!string.IsNullOrEmpty(contactId))
            {
                ContactsRole cr = new ContactsRole();
                cr.CONTACTID = int.Parse(contactId);
                cr.CONTACTROLE = "Administrator";
                string contactroleURI =  "https://onlineshoppingcontactsapi.azurewebsites.net/api/ContactsRole";//"http://localhost:5000/api/ContactsRole" ;
                var response1 = string.Empty;
                HttpContent c = new StringContent(JsonConvert.SerializeObject(cr), Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(contactroleURI),
                        Content = c
                    };

                    HttpResponseMessage result = client.SendAsync(request).Result;
                    if (result.IsSuccessStatusCode)
                    {
                        string responsereceived = result.StatusCode.ToString();
                    }
                }
            }

            CreatedAtRoute("GetContacts", new { Controller = "Contacts", id = item.CONTACTID }, item);
            return CreatedAtRoute("GetContacts", new { Controller = "Contacts", id = item.CONTACTID }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Contacts item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            var contactObj = ContactsRepo.Find(id);
            if (contactObj == null)
            {
                return NotFound();
            }
            ContactsRepo.UpdateAsync(item);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            ContactsRepo.Remove(id);
        }
    }
}