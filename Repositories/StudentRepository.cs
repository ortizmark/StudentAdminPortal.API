using Microsoft.EntityFrameworkCore;
using StudentAdminPortal.API.DataModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdminPortal.API.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private StudentAdminContext _context;
        public StudentRepository(StudentAdminContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetStudents()
        {
            return await _context.Student.Include(nameof(Gender)).Include(nameof(Address)).ToListAsync();  //Adds stuff in model but not in database
            //return _context.Student.ToList();
        }

     }
}
