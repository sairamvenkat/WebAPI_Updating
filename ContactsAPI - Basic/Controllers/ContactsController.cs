﻿using ContactsApi.Models;
using ContactsApi.Repository;
using Microsoft.AspNetCore.Mvc;
using ContactsAPI.Connections;

using System.Collections.Generic;
using System.Data.SqlClient;

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
            ContactsRepo.Add(item);
            return CreatedAtRoute("GetContacts", new { Controller = "Contacts", id = item.MobilePhone }, item);
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
            ContactsRepo.Update(item);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            ContactsRepo.Remove(id);
        }
    }
}