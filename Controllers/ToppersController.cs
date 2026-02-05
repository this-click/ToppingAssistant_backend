using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos;
using backend.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToppersController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public ToppersController(AppDbContext context, IMapper mapper)
        {
            _appDbContext = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all saved toppers.
        /// </summary>
        /// <returns>200 OK response if successful</returns>
        [HttpGet]
        [Route("All")]
        public async Task<ActionResult<IEnumerable<TopperDto>>> GetAllToppers()
        {
            var toppers = await _appDbContext.Toppers.OrderBy(topper => topper.Priority).ToListAsync();
            var mappedToppers = _mapper.Map<IEnumerable<TopperDto>>(toppers);
            return Ok(mappedToppers);
        }

        /// <summary>
        /// Get recommended toppers for the week.
        /// </summary>
        /// <returns>200 OK response if successful</returns>
        [HttpGet]
        [Route("Recommended")]
        public async Task<ActionResult<IEnumerable<TopperDto>>> GetRecommendedToppers()
        {
            var recommendedToppers = await _appDbContext.Toppers.OrderBy(topper => topper.Priority).Take(7).ToListAsync();
            var mappedToppers = _mapper.Map<IEnumerable<TopperDto>>(recommendedToppers);

            return Ok(mappedToppers);
        }

        //Create - del this
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateTopper([FromBody] TopperDto newTopperDto)
        {
            Topper newTopper = _mapper.Map<Topper>(newTopperDto);
            await _appDbContext.AddAsync(newTopper);
            await _appDbContext.SaveChangesAsync();

            return Ok("New topper created Ok");
        }


        [HttpPut]
        [Route("Update")]
        public IActionResult UpdateTopper([FromBody] TopperUpdateDto topperUpdateDto)
        {

            if (topperUpdateDto == null)
            {
                return BadRequest();
            }

            Topper? existingTopper = _appDbContext.Toppers.FirstOrDefault(x => x.Id == topperUpdateDto.Id);

            if (existingTopper == null)
            {
                return NotFound();
            }

            _mapper.Map(topperUpdateDto, existingTopper);
            _appDbContext.Toppers.Update(existingTopper);
            _appDbContext.SaveChanges();

            return Ok($"Topper updated Ok");
        }
    }
}
