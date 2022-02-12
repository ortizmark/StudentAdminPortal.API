using Microsoft.EntityFrameworkCore;
using StudentAdminPortal.API.DataModels;
using System;
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

        public async Task<Student> GetStudent(Guid id)
        {
            return await _context.Student.Include(nameof(Gender)).Include(nameof(Address)).FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<List<Gender>> GetGenders()
        {
            return await _context.Gender.ToListAsync();  //Adds stuff in model but not in database
        }

        public async Task<bool> HasStudent(Guid id)
        {
            return await _context.Student.AnyAsync(x => x.Id == id);
        }

        public async Task<Student> UpdateStudent(Guid id, Student request)
        {
            var existingStudent = await GetStudent(id);
            if (existingStudent != null)
            {
                existingStudent.FirstName = request.FirstName;
                existingStudent.LastName = request.LastName;
                existingStudent.DateOfBirth = request.DateOfBirth;
                existingStudent.Email = request.Email;
                existingStudent.Mobile = request.Mobile;
                existingStudent.GenderId = request.GenderId;
                existingStudent.Address.PhysicalAddress = request.Address.PhysicalAddress;
                existingStudent.Address.PostalAddress = request.Address.PostalAddress;
                await _context.SaveChangesAsync();
                return existingStudent;
            }
            return null;
        }

        public async Task<Student> DeleteStudent(Guid id)
        {
            var student = await GetStudent(id);
            if (student != null)
            {
                _context.Student.Remove(student);
                await _context.SaveChangesAsync();
                return student;
            }
            return null; ;
        }

        public async Task<Student> AddStudent(Student request)
        {
            var student = await _context.Student.AddAsync(request);
            await _context.SaveChangesAsync();
            return student.Entity;
        }

        public async Task<bool> UpdateProfileImage(Guid id, string profileImageUrl)
        {
            var student = await GetStudent(id);
            if (student != null)
            {
                student.ProfileImageUrl = profileImageUrl;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
