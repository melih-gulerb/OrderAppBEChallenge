using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Model
{
    #region Order Model
    public class Order : Base
    {
        [Key]
        public override Guid Id { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }
        
        public string CustomerId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public override string Addressline { get; set; }
        public override string City{get; set; }
        public override string Country { get; set; }
        public override int CityCode { get; set; }
        
        [ForeignKey("Key")]
        public Guid ProductId { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
    }
    //[Keyless]
    //[NotMapped]
    //public class Product
    //{
    //    public Guid ProductId;
    //    public string ImageUrl;
    //    public string Name;
    //}
    #endregion
}