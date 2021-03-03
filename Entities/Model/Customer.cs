using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Model
{
    #region CustomerModel 
    public class Customer : Base 
    {
        [Key]
        public override Guid Id { get; set; }   //Defined specified Id for each of all different customers
        public string Name  { get; set; }
        public string Email { get; set; }
        public override DateTime CreatedAt { get; set; } //When customer is created, save the time
        public override DateTime UpdatedAt { get; set; } //When customer is updated, save the time
        
        public override string  Addressline { get; set; }
        public override string City{get; set; }
        public override string Country { get; set; }
        public override int CityCode { get; set; }
    }

    //public class Address
    //{
    //    public string Addressline { get; set; }
    //    public string City{get; set; }
    //    public string Country { get; set; }
    //   public int CityCode { get; set; }
    //}
    
    #endregion
}