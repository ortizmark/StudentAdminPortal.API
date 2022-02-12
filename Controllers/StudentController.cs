using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace StudentAdminPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly IImageRepository _imageRepository;

        public StudentController(IStudentRepository studentRepository, IMapper mapper, IImageRepository imageRepository )
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _imageRepository = imageRepository;
        }

        ////https://localhost:44351/api/Student
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentRepository.GetStudents();
            return Ok(_mapper.Map<List<Student>>(students));    
            //return Ok(students);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetStudent(Guid id)
        {
            var student = await _studentRepository.GetStudent(id);
            if (student == null)
                return NotFound();  
            else
                return Ok(_mapper.Map<Student>(student)); 
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateStudent(Guid id, [FromBody] UpdateStudentRequest request)
        {
            if (await _studentRepository.HasStudent(id))
            {
                var updatedStudent = await _studentRepository.UpdateStudent(id, _mapper.Map<DataModels.Student>(request));
                if (updatedStudent != null)
                {
                    return Ok(_mapper.Map<Student>(updatedStudent));      
                }
            } 
            return NotFound();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            if (await _studentRepository.HasStudent(id))
            {
                var student = await _studentRepository.DeleteStudent(id);   
                return Ok(_mapper.Map<Student>(student));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] AddStudentRequest request)
        {
            var student = await _studentRepository.AddStudent(_mapper.Map<DataModels.Student>(request));
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id } ,
                _mapper.Map<Student>(student));
        }

        [HttpPost("{id:Guid}")]
        public async Task<IActionResult> UploadImage(Guid id, IFormFile profileImage)
        {
            var validExtensions = new List<string>
            { ".jpeg", ".png", ".gif", ".jpg", "jpeg", "png", "gif", "jpg" };
            if (profileImage != null && profileImage.Length > 0)
            {
                var extention = Path.GetExtension(profileImage.FileName).ToLower(); 
                if (validExtensions.Contains(extention))
                {
                    if (await _studentRepository.HasStudent(id))
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
                        var fileImagePath = await _imageRepository.Upload(profileImage, fileName);
                        if (await _studentRepository.UpdateProfileImage(id, fileImagePath))
                        {
                            return Ok(fileImagePath);
                        }
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading image");
                    }
                }
            }
            return BadRequest("Image is invalid");
        }

    }
}
