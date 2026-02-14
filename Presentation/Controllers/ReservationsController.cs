using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.DTOs.ResrvationDTOs;
using Application.Helpers;
using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Attributes;
using Presentation.ViewModels.ReservationsViewModels;

namespace Presentation.Controllers
{
    [Authorize] // Requires JWT
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController(ReservationManagmentService reservationService) : ControllerBase
    {
        // Extracts the User ID from the JWT Token
        private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet]
        //[HasPermission(Feature.GetAllReservation)]
        public async Task<IActionResult> GetAll()
        {
            // 1. Get DTOs from Service
            var dtos = await reservationService.GetAll();
            
            // 2. Map DTOs to ViewModels for the client
            var viewModels = dtos.Map<GetAllReservationsDTO, ReservationDisplayViewModel>();
            
            return Ok(viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationViewModel vm)
        {
            // 1. Map VM to DTO (Application Layer expects DTO)
            var dto = vm.MapOne<AddResrvationDTO>();
            
            // 2. Set the UserId from the JWT (Security Best Practice)
            dto.UserId = CurrentUserId;

            // 3. Call Service
            var result = await reservationService.AddReservation(dto, CurrentUserId);
            
            return result ? StatusCode(201, "Reservation Created") : BadRequest("Failed to create reservation.");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await reservationService.DeleteReservation(id);
            return result ? NoContent() : NotFound();
        }

        [HttpPut("{id:guid}")] // URL: api/reservations/{id}
        public async Task<IActionResult> Update(
            [FromRoute] Guid id, 
            [FromBody] UpdateReservationViewModel vm)
        {
            // 1. Map the View Model to the DTO
            var dto = vm.MapOne<UpdateReservationDTO>();
            
            // 2. Assign the ID from the route to the DTO
            dto.Id = id;

            // 3. Call the service
            // We pass CurrentUserId for the 'UpdatedBy' audit field
            var result = await reservationService.UpdateReservation(dto, CurrentUserId);

            if (!result)
            {
                return NotFound(new { Message = $"Reservation {id} not found or update failed." });
            }

            return Ok(new { Message = "Reservation updated successfully." });
        }
    }
}