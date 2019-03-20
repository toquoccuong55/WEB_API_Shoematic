using DatabaseService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseService.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository()
        {
        }

        public bool updateCustomer(Customer customer)
        {
            Customer existedCustomer = DbSet.SingleOrDefault(c => c.ID == customer.ID);
            if (existedCustomer != null)
            {
                existedCustomer.Name = customer.Name;
                existedCustomer.Email = customer.Email;
                existedCustomer.Address = customer.Address;
                shoematicContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}