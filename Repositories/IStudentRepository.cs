using StudentAdminPortal.API.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentAdminPortal.API.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetStudents();
        Task<Student> GetStudent(Guid id);
        Task<List<Gender>> GetGenders();
        Task<bool> HasStudent(Guid id);
        Task<Student> UpdateStudent(Guid id, Student request);
        Task<Student> DeleteStudent(Guid id);
        Task<Student> AddStudent(Student request);
        Task<bool> UpdateProfileImage(Guid id, string profileImageUrl);
    }
}
