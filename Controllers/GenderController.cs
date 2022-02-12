using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdminPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenderController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public GenderController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        ////https://localhost:44351/api/Gender
        [HttpGet]
        public async Task<IActionResult> GetAllGenders()
        {
            var genders = await _studentRepository.GetGenders();
            if (genders == null || !genders.Any())
                return NotFound();  
            return Ok(_mapper.Map<List<Gender>>(genders));
        }
    }
}
