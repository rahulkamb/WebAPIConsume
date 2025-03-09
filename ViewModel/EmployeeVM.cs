using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using APIConsume.Models;

namespace APIConsume.ViewModel
{
    public class EmployeeVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }


        public List<Employee> empItems { get; set; } = new List<Employee>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPreiousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
