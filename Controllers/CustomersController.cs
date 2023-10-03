using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using SimpleCustomerApi.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace SimpleCustomerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ApplicationDbContext context, ILogger<CustomersController> logger)
        {
           _context = context;
           _logger = logger;
        }

        private static List<Customer> customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "John" },
            new Customer { Id = 2, Name = "Jane" }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            return _context.Customers.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Customer> Get(int id)
        {
            try
            {
                var customer = _context.Customers.Find(id);
                if (customer == null)
                    return NotFound(new ErrorResponse { StatusCode = 404, Message = "Customer not found." });

                return Ok(customer);
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { StatusCode = 500, Message = ex.Message, Details = ex.StackTrace });
            }
        }

        [HttpPost]
        public ActionResult Post(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

            _logger.LogInformation("New customer added with ID: {CustomerId}", customer.Id);
            return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, Customer updatedCustomer)
        {
            if (id != updatedCustomer.Id)
            {
                return BadRequest();
            }

            _context.Entry(updatedCustomer).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var customer = _context.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
