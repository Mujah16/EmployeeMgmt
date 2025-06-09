using api.Models;
using api.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync() =>
            await _context.Employees.ToListAsync();

        public async Task<Employee> GetByIdAsync(int id) =>
            await _context.Employees.FindAsync(id);

        public async Task AddAsync(Employee employee) =>
            await _context.Employees.AddAsync(employee);

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
