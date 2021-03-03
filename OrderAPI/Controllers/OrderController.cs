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

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase //Controller of methods for order entity
    {
        private readonly DataContext _context;
        private IRepoOrder _repoOrder;// Use customer repository interface as _repoOrder

        public OrderController(DataContext context, IRepoOrder repoOrder)
        {
            _context = context;
            _repoOrder = repoOrder;// call IRepoCustomer as _repoCustomer
        }
        
        //GET: api/order
        [HttpGet]
        public IEnumerable<Order> GetAllOrders() //Invoke GetAllRecords() for _repoOrder
        {
            return _repoOrder.GetAllRecords();
        }
        
        //GET: api/order/customerId/id
        [HttpGet]
        [Route("customer/{id}")]
        public IEnumerable<Order> GetOrderbyCustomer(int id)//Invoke orders from customer by giving its Id
        {
            //Make a list that created order where all have same customerId's 
            var list = _context.Orders.Where(o => o.CustomerId == id).AsNoTracking();
            return list; 
        }
        
        //GET: api/order/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id)//Get an order by given Id
        {
            const string jsonResponse = "There is no such an order for given Order Id";
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  //Return Status400
            }

            var order = await _repoOrder.GetFromId(id);

            if (order == null)
            {
                return NotFound(jsonResponse);  //Return Status404
            }
            return Ok(order);
        }
        
        //PUT: api/orders/id
        [Authorize] //First authorize, if a updating action will happened 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder([FromRoute] Guid id, [FromBody] Order order)//Change order's specs
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  //If order model is invalid, return Status400
            }

            const string jsonResponse = "There is no such an Id found in order table in database..";
            if (!_context.Orders.Any(o => o.Id == id)) return BadRequest(jsonResponse); // If there is no id that equals as given input id
            try
            {
                await _repoOrder.Update(id, order); // try update order that given id as input from order table 
            }
            catch
            {
                const string jsonResponse2 = "Something went wrong while routing database or table..";
                return BadRequest(jsonResponse2);
            }
            return Ok(ModelState);
        }
        
        
        //PUT: api/order/id
        [Authorize] //If status of order object will be change, first authorize
        [HttpPut("change/{id}")]
        public async Task<IActionResult> PutChangeStatus([FromRoute] Guid id, [FromBody] Order order)//Change order's status
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);  //If order model is invalid, return Status400
                }

                var orderStatus = await _context.Orders.FindAsync(id);  //Find order by its id and assign as orderStatus 
                orderStatus.Status = order.Status;
                
                const string jsonResponse = "There is no such an Id found in order table in database..";
                if (!_context.Orders.Any(o => o.Id == id)) return BadRequest(jsonResponse); // If there is no id that equals as given input id
                {
                    try
                    {
                        await _repoOrder.Update(id, order); // Update order
                    }
                    catch
                    {
                        const string jsonResponse2 = "Something went wrong while routing database or table..";
                        return BadRequest(jsonResponse2);
                    }
                }

                return Ok(ModelState);
            }
        
        
        //POST: api/order
        [Authorize] //If customer object will deleted, first authorize
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] Order order)//Create an order
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  //If order model is invalid, return Status400
            }

            try
            {
                await _repoOrder.Create(order); //Create an order to order table
            }
            catch
            {
                const string jsonResponse2 = "Something went wrong while routing database or table..";
                return BadRequest(jsonResponse2);
            }
            return CreatedAtAction("GetOrder", new {id = order.Id}, order); //return created order by invoking GetOrder() method.
        }
        
        
        //DELETE: api/order/id
        [Authorize] //If order object will deleted, first authorize
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id) //Delete order by giving its Id 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  //If order model is invalid, return Status400
            }

            var order = await _context.Orders.FindAsync(id);        //Find order by its id and assign as order
            const string jsonResponse = "There is no such an Order that have given Id"; 
            if (order == null) return BadRequest(jsonResponse);     //If there is no such an 
            _context.Entry(order).State = EntityState.Detached;     // Seperate order entity with database. 
          
            try
            {
                await _repoOrder.Delete(id);    //Delete order by giving its Id
                return Ok(order);   //Return deleted order object 
            }
            
            catch 
            {
                const string jsonResponse2 = "Something went wrong while routing database or table..";
                return BadRequest(jsonResponse2);
            }
        }
    }
}