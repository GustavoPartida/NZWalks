using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultiesRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultiesRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var walkDifficultiesDomain = await walkDifficultiesRepository.GetAllAsync();
            var walkDifficultiesDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficultiesDomain);
            return Ok(walkDifficultiesDTO);
        }


        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyById")]
        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            var walkDifficulty = await walkDifficultiesRepository.GetAsync(id);
            if (walkDifficulty == null)
            {
                return NotFound();
            }
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficutyAsync(AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if (!ValidateAddWalkDifficutyAsync(addWalkDifficultyRequest)) return BadRequest(ModelState);

            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
            };
            walkDifficultyDomain = await walkDifficultiesRepository.AddAsync(walkDifficultyDomain);
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            return CreatedAtAction(nameof(GetWalkDifficultyById), new { id = walkDifficultyDTO .Id}, walkDifficultyDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (!ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest)) return BadRequest(ModelState);

            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code,
            };
            walkDifficultyDomain = await walkDifficultiesRepository.UpdateAsync(id, walkDifficultyDomain);
            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            //GetRegion from database
            var walkDifficultyDomain = await walkDifficultiesRepository.DeleteAsync(id);
            //Id null NotFound
            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }
            //Convert reponsebackto DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            //return Ok response
            return Ok(walkDifficultyDTO);

        }

        #region Private methods

        private bool ValidateAddWalkDifficutyAsync(AddWalkDifficultyRequest addWalkDifficultyRequest )
        {
            if (addWalkDifficultyRequest == null) ModelState.AddModelError(nameof(addWalkDifficultyRequest), $"add Region Data is required");
            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code)) ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code), $"{nameof(addWalkDifficultyRequest.Code)} cannot be mull or empty or white space.");
            if (ModelState.ErrorCount > 0) return false;
            return true;
        }

        private bool ValidateUpdateWalkDifficultyAsync(UpdateWalkDifficultyRequest updateWalkDifficultyRequest )
        {
            if (updateWalkDifficultyRequest == null) ModelState.AddModelError(nameof(updateWalkDifficultyRequest), $"add Region Data is required");
            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code)) ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code), $"{nameof(updateWalkDifficultyRequest.Code)} cannot be mull or empty or white space.");
            if (ModelState.ErrorCount > 0) return false;
            return true;
        }

        #endregion
    }
}
