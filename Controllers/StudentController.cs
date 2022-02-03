using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentAdminPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        ////https://localhost:44351/api/Student
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentRepository.GetStudents();
            return Ok(_mapper.Map<List<Student>>(students));    
            //return Ok(students);
       }
    }
}
