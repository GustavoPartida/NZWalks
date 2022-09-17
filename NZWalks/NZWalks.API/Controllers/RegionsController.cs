﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {

            var regions = await regionRepository.GetAllAsync();

            //return DTO regions
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name, 
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,

            //    };
            //    regionsDTO.Add(regionDTO);
            //});
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
           var region =  await regionRepository.GetAsync(id);
            if (region == null)
            {
            return NotFound();
            }            
           var regionDTO =  mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO); 
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(AddRegionRequest addRegionRequest)
        {
            //Validate the request
            if (!ValidateAddRegionAsync(addRegionRequest)) return BadRequest(ModelState);  

            //Request(DTO) to Domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population,
            };
            //Pass details to Repository
            region = await regionRepository.AddAsync(region);

            //Convert back to DTO

            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id },regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //GetRegion from database
            var region = await regionRepository.DeleteAsync(id);
            //Id null NotFound
            if(region == null)
            {
                return NotFound();
            }
            //Convert reponsebackto DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };
            //return Ok response
            return Ok(regionDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] UpdateRegionRequest  updateRegionRequest)
        {
            //Validate the request
            if (!ValidateUpdateRegionAsync(updateRegionRequest)) return BadRequest(ModelState);

            //convert DTO to Domain model
            var region = new Models.Domain.Region
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population,
            };
            //Update region using repository
            region = await regionRepository.UpdateAsync(id, region);

            //If Null then NotFound
            if (region == null)
            {
                return NotFound();
            }
            //Convert Domain back to DTO 
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };
            //Retund OK response
            return Ok(regionDTO);
        }

        #region Private methods

        private bool ValidateAddRegionAsync(AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest == null) ModelState.AddModelError(nameof(addRegionRequest), $"add Region Data is required");
            if (string.IsNullOrWhiteSpace(addRegionRequest.Code)) ModelState.AddModelError(nameof(addRegionRequest.Code), $"{nameof(addRegionRequest.Code)} cannot be mull or empty or white space.");
            if (string.IsNullOrWhiteSpace(addRegionRequest.Name)) ModelState.AddModelError(nameof(addRegionRequest.Name), $"{nameof(addRegionRequest.Name)} cannot be mull or empty or white space.");
            if (addRegionRequest.Area <= 0) ModelState.AddModelError(nameof(addRegionRequest.Area), $"{nameof(addRegionRequest.Area)} cannot be less than or equal to zero");
            if (addRegionRequest.Lat <= 0) ModelState.AddModelError(nameof(addRegionRequest.Lat), $"{nameof(addRegionRequest.Lat)} cannot be less than or equal to zero"); 
            if (addRegionRequest.Long <= 0) ModelState.AddModelError(nameof(addRegionRequest.Long), $"{nameof(addRegionRequest.Long)} cannot be less than or equal to zero"); 
            if (addRegionRequest.Population < 0) ModelState.AddModelError(nameof(addRegionRequest.Population), $"{nameof(addRegionRequest.Population)} cannot be less than zero");
            if (ModelState.ErrorCount > 0) return false;
            return true;
        }

        private bool ValidateUpdateRegionAsync(UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null) ModelState.AddModelError(nameof(updateRegionRequest), $"add Region Data is required");
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code)) ModelState.AddModelError(nameof(updateRegionRequest.Code), $"{nameof(updateRegionRequest.Code)} cannot be mull or empty or white space.");
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name)) ModelState.AddModelError(nameof(updateRegionRequest.Name), $"{nameof(updateRegionRequest.Name)} cannot be mull or empty or white space.");
            if (updateRegionRequest.Area <= 0) ModelState.AddModelError(nameof(updateRegionRequest.Area), $"{nameof(updateRegionRequest.Area)} cannot be less than or equal to zero");
            if (updateRegionRequest.Lat <= 0) ModelState.AddModelError(nameof(updateRegionRequest.Lat), $"{nameof(updateRegionRequest.Lat)} cannot be less than or equal to zero");
            if (updateRegionRequest.Long <= 0) ModelState.AddModelError(nameof(updateRegionRequest.Long), $"{nameof(updateRegionRequest.Long)} cannot be less than or equal to zero");
            if (updateRegionRequest.Population < 0) ModelState.AddModelError(nameof(updateRegionRequest.Population), $"{nameof(updateRegionRequest.Population)} cannot be less than zero");
            if (ModelState.ErrorCount > 0) return false;
            return true;
        }

        #endregion

    }
}

