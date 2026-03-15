using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos;
using backend.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

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

        /// <summary>
        /// Get weekly schedule of toppers for the week.
        /// 
        /// Toppers are filtered by PurchaseDate, which means they have been bought at some point and are available to feed.
        /// Later on, checks will be added so they are not expired.
        /// 
        /// The main sort is by priority, the lower the value the stronger the recommendation to feed soon.
        /// The second sort criteria is PurchaseDate. The idea is that fresh foods should be fed before frozen foods, which have an older PurchaseDate value.
        /// 
        /// More or less, should be the same list as recommended toppers, except that PurchaseDate is not null.
        /// 
        /// </summary>
        /// <returns>200 OK response if successful</returns>
        [HttpGet]
        [Route("Weekly")]
        public async Task<ActionResult<IEnumerable<TopperDto>>> GetWeeklyToppers()
        {
            var weeklyToppers = await _appDbContext.Toppers
                .Where(topper => topper.PurchaseDate != null)
                .OrderBy(topper => topper.Priority)
                .ThenByDescending(topper => topper.PurchaseDate)
                .Take(7)
                .ToListAsync();
            var mappedToppers = _mapper.Map<IEnumerable<TopperDto>>(weeklyToppers);

            return Ok(mappedToppers);
        }

        //Create - TODO: del this, not needed in the appv1
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateTopper([FromBody] TopperDto newTopperDto)
        {
            Topper newTopper = _mapper.Map<Topper>(newTopperDto);
            await _appDbContext.AddAsync(newTopper);
            await _appDbContext.SaveChangesAsync();

            return Ok("New topper created Ok.");
        }


        /// <summary>
        /// Receives a list of toppers, each to be updated with purchase day - today. 
        /// For each topperDto in the list, if its counterpart exists in the database, we map the dto to the DB object and then update the DB object.
        /// </summary>
        /// <param name="buyToppersDtos">list of topper Dtos to be updated in DB</param>
        /// <returns>
        ///     bad request, if toppersUpdateDtos list is null
        ///     not found, if NONE of the IDs provided were mapped to a DB object. This is tracked by numUpdates.
        ///     OK, if some or all toppers were updated
        /// </returns>
        [HttpPatch]
        [Route("Buy")]
        public IActionResult BuyToppers([FromBody] BuyToppersDto[] buyToppersDtos)
        {

            if (buyToppersDtos == null)
            {
                return BadRequest();
            }

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            int numUpdates = 0;

            foreach (BuyToppersDto buyTopperDto in buyToppersDtos)
            {
                buyTopperDto.PurchaseDate = today;

                Topper? existingTopperDb = _appDbContext.Toppers.FirstOrDefault(topp => topp.Id == buyTopperDto.Id);
                if (existingTopperDb == null)
                    continue;
                _mapper.Map(buyTopperDto, existingTopperDb);
                _appDbContext.Toppers.Update(existingTopperDb);
                numUpdates++;
            }

            _appDbContext.SaveChanges();

            if (numUpdates > 0)
                return Ok($"Toppers bought Ok.");
            else
                return NotFound();
        }
    }
}
