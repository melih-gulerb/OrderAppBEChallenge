using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Model;
using Entities.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace CustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IRepoCustomer _repoCustomer;   // Use customer repository interface as _repoCustomer

        public CustomerController(DataContext context, IRepoCustomer repoCustomer)
        {
            _context = context;
            _repoCustomer = repoCustomer;   // call IRepoCustomer as _repoCustomer
        }
        
        // GET: api/customer
        [HttpGet]
        public IEnumerable<Customer> GetAllCustomers()  //Invoke GetAllRecords() for _repoCustomer
        {
            return _repoCustomer.GetAllRecords();
        }

        //GET: api/customer/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] Guid id)   //Get a customer by given Id
        {
            const string jsonresponse = "You should give an accurate model..";
            const string jsonresponse3 = "There is no such a customer found for given Id..";
            if (!ModelState.IsValid)
            {
                return BadRequest(jsonresponse);
            }
            var customer = await _repoCustomer.GetFromId(id);
            if (customer == null)
            {
                return BadRequest(jsonresponse3);
            }
            else
            {
                return Ok(customer);
            }
        }
        [Authorize] //First authorize, if a posting action will happened
        //POST: api/customer
        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromBody] Customer customer)
        {
            const string jsonresponse = "You should give an accurate model..";
            const string jsonresponse2 = "Something went wrong while accessing database or table..";
            if (!ModelState.IsValid)
                return BadRequest(jsonresponse);  //If given model of customer object is not proper (invalid), return Status400 

            try
            {
                await _repoCustomer.Create(customer);   // If model is valid, try to create customer to database 
                
            }
            catch
            {
                return BadRequest(jsonresponse2);  // Return if database or any other problems occur
            }

            return CreatedAtAction("GetCustomer", new {id = customer.Id}, customer);    // If everything is perfect,
                                                                                        // return a new customer information
                                                                                        // by invoking GetCustomer() meth. 
        }
        [Authorize] //If customer object will change, first authorize
        // PUT: api/Customer/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomers([FromRoute] Guid id, [FromBody] Customer customer)
        {
            const string jsonresponse = "You should give an accurate customer model..";
            const string jsonresponse2 = "Something went wrong while accessing database or table..";
            const string jsonresponse3 = "There is no such a customer found for given Id..";
            if (!ModelState.IsValid)
            {
                return BadRequest(jsonresponse);  //If model has invalid property, return BadRequest
            }

            if (id != customer.Id) //If there is no id by given input, return BadRequest
            {
                return BadRequest(jsonresponse3);    
            }

            try
            {
                await _repoCustomer.Update(id, customer);  // Update customer by given id and table 
            }
            catch 
            {
                return BadRequest(jsonresponse2);  // If an error occured by database or its table, catch it
            }

            return CreatedAtAction("GetCustomer", new {id = customer.Id}, customer);    //Return updated customer by using its Id
        }
        
        [Authorize] //If customer object will deleted, first authorize
        //DELETE: api/customer/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)    //Delete customer by given its Id
        {
            const string jsonresponse = "Accurate id please..";
            const string jsonresponse3 = "There is no such a customer found for given Id..";
            if (!ModelState.IsValid)
                return BadRequest(jsonresponse);

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound(jsonresponse3);
            }
            else
            {
                _context.Entry(customer).State = EntityState.Detached;
            }
            try
            {
                await _repoCustomer.Delete(id); //Delete given Id from database
            }
            catch
            {
                return BadRequest(ModelState);
            }
            return Ok(customer);
        }
    }
}