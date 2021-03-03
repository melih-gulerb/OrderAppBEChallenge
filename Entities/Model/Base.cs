using System;

namespace Entities.Model
{
    //All records in tables from database, should have Id, CreatedAt and UpdatedAt variables based on Base class
    public abstract class Base 
    {
        public abstract Guid Id { get; set; }   //Identificate all records

        protected Base()
        {
            this.Id = Id; CreatedAt = DateTime.Now; UpdatedAt = DateTime.Now;   //Save times where the creation or update of records happened
        }
        public abstract DateTime CreatedAt { get; set; }
        public abstract DateTime UpdatedAt { get; set; }
        
        
        //I should made these variables as "Address" type
        //But because i use SQL Server, i couldn't find a way 
        //replace variable to columns as "class type"
        public abstract string Addressline { get; set; }     
        public abstract string City { get; set; }
        public abstract string Country { get; set; }
        public abstract int CityCode { get; set; }
    }
}