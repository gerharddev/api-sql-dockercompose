using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sample.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<PersonsController> _logger;
        public PersonsController(DataContext context, ILogger<PersonsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var persons = await _context.Persons.ToListAsync();
            return new JsonResult(persons);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(n => n.Id == id);

            if (person == null)
                return NotFound("Person not found");

            return new JsonResult(person);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PersonDto person)
        {
            try
            {
                var newPerson = new Person { Name = person.Name, Surname = person.Surname, Gender = person.Gender,Age=person.Age };
                _context.Persons.Add(newPerson);
                await _context.SaveChangesAsync();

                return new JsonResult(newPerson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Failed to add person");
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PersonDto person)
        {
            var existingPerson = await _context.Persons.FirstOrDefaultAsync(n => n.Id == id);

            if (existingPerson == null)
                return NotFound("Person not found");

            existingPerson.Name = person.Name;
            existingPerson.Surname = person.Surname;
            existingPerson.Gender = person.Gender;
            existingPerson.Age = person.Age;
            var success = (await _context.SaveChangesAsync()) > 0;

            return new JsonResult(success);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var person = await _context.Persons.FirstOrDefaultAsync(n => n.Id == id);
                _context.Remove(person);
                var success = (await _context.SaveChangesAsync()) > 0;

                return new JsonResult(success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Failed to update person");
            }

        }
    }
}
