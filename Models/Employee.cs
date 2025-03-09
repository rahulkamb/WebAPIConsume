using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace APIConsume.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string EmailId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
