﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walksDomain = await walkRepository.GetAllAsync();
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walkDomin =  await walkRepository.GetAsync(id);
            if (walkDomin == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomin);
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };
            walkDomain = await walkRepository.AddAsync(walkDomain);
            if (walkDomain == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute]Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };
            walkDomain = await walkRepository.UpdateAsync(id,walkDomain);
            if (walkDomain == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //GetRegion from database
            var walkDomain = await walkRepository.DeleteAsync(id);
            //Id null NotFound
            if (walkDomain == null)
            {
                return NotFound();
            }
            //Convert reponsebackto DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            //return Ok response
            return Ok(walkDTO);

        }
    }
}
