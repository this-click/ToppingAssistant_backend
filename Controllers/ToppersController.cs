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
            var toppers = await _appDbContext.Toppers.OrderBy(topper => topper.Name).ToListAsync();
            var mappedToppers = _mapper.Map<IEnumerable<TopperDto>>(toppers);
            return Ok(mappedToppers);
        }

        /// <summary>
        /// Get recommended toppers for the week.
        /// 
        /// If a topper was never fed or if it was fed long ago, it's in the front of the list and will be recommended first.
        /// </summary>
        /// <returns>200 OK response if successful</returns>
        [HttpGet]
        [Route("Recommended")]
        public async Task<ActionResult<IEnumerable<TopperDto>>> GetRecommendedToppers()
        {
            var recommendedToppers = await _appDbContext.Toppers
                .OrderBy(topper => topper.FedDate)
                .ThenBy(topper => topper.Name)
                .Take(7).ToListAsync();
            var mappedToppers = _mapper.Map<IEnumerable<TopperDto>>(recommendedToppers);

            return Ok(mappedToppers);
        }

        /// <summary>
        /// Get weekly schedule of toppers for the week.
        /// 
        /// Toppers are filtered by ThisWeek: if true, they are returned.
        /// Later on, checks will be added so they are not expired.
        /// 
        /// </summary>
        /// <returns>200 OK response if successful</returns>
        [HttpGet]
        [Route("Weekly")]
        public async Task<ActionResult<IEnumerable<TopperDto>>> GetWeeklyToppers()
        {
            var weeklyToppers = await _appDbContext.Toppers
                .Where(topper => topper.ThisWeek == true)
                .OrderBy(topper => topper.Name)
                .Take(7)
                .ToListAsync();
            var mappedToppers = _mapper.Map<IEnumerable<TopperDto>>(weeklyToppers);

            return Ok(mappedToppers);
        }

        /// <summary>
        /// Receives a list of toppers, each to be updated with:
        ///     - PurchaseDate = today
        ///     - ThisWeek = true
        /// 
        /// For each topperDto in the list, if its counterpart exists in the database, we map the dto to the DB object and then update the DB object.
        /// </summary>
        /// <param name="buyToppersDtos">list of topper Dtos to be updated in DB</param>
        /// <returns>
        ///     - bad request, if toppersUpdateDtos list is null
        ///     - not found, if NONE of the IDs provided were mapped to a DB object. This is tracked by numUpdates.
        ///     - OK, if some or all toppers were updated
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
                buyTopperDto.ThisWeek = true;

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

        /// <summary>
        /// Feed a topper. When feeding:
        ///     - FedDate = today
        ///     - ThisWeek = false, it's no longer supposed to be fed this week.
        /// </summary>
        /// <param name="Id">ID of the topper</param>
        /// <param name="feedTopperDto">feed topper DTO</param>
        ///     - bad request, if feedTopperDto is null
        ///     - bad request, if ID mismatch
        ///     - not found, if topper not found in DB.
        ///     - UnprocessableEntity, if PurchaseDate > today
        ///     - UnprocessableEntity, if attempting to feed a topper that was not purchased first
        ///     - OK, if topper was successfully fed
        [HttpPatch]
        [Route("Feed/{Id}")]
        public IActionResult FeedTopper(Guid Id, [FromBody] FeedTopperDto feedTopperDto)
        {

            if (feedTopperDto == null)
                return BadRequest();
            if (Id != feedTopperDto.Id)
                return BadRequest();

            Topper? existingTopperDb = _appDbContext.Toppers.FirstOrDefault(topp => topp.Id == feedTopperDto.Id);

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            feedTopperDto.FedDate = today;
            feedTopperDto.ThisWeek = false;

            if (existingTopperDb == null)
                return NotFound();
            if (existingTopperDb.PurchaseDate != null && existingTopperDb.PurchaseDate > today)
                return UnprocessableEntity("Error: Purchase date is more recent than Fed date.");
            if (existingTopperDb.PurchaseDate == null)
                return UnprocessableEntity("Error: There is no Purchase date information in the system.");

            _mapper.Map(feedTopperDto, existingTopperDb);
            _appDbContext.Toppers.Update(existingTopperDb);
            _appDbContext.SaveChanges();

            return Ok($"Fed topper Ok: '{existingTopperDb.Id}'.");
        }
    }
}
