using api.DTOs;
using api.Models;
using api.Repositories;

namespace api.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeService(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Employee>> GetEmployees() => await _repo.GetAllAsync();

        public async Task<Employee> GetEmployee(int id) => await _repo.GetByIdAsync(id);

        public async Task AddEmployee(EmployeeDto dto)
        {
            var emp = new Employee
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Department = dto.Department,
                JoinDate = DateTime.UtcNow
            };
            await _repo.AddAsync(emp);
            await _repo.SaveAsync();
        }
    }
}
