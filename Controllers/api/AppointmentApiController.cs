using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using HospitalService.Models.ViewModels;
using HospitalService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalService.Controllers.api
{
    [Authorize]
    [ApiController]
    [Route("api/appointment")]
    public class AppointmentApiController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string loginUserId;
        private readonly string role;
        public AppointmentApiController(IAppointmentService appointmentService, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentService = appointmentService;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        [HttpPost("SaveCalendarData")]
        public IActionResult SaveCalendarData(AppointmentVM data)
        {
            ServiceResponse<int> commonResponse = new();
            try
            {
                commonResponse.status = _appointmentService.AddUpdate(data).Result;
                if (commonResponse.status == 1)
                {
                    commonResponse.message = Helper.Helper.appointmentUpdated;
                }
                if (commonResponse.status == 2)
                {
                    commonResponse.message = Helper.Helper.appointmentAdded;
                }
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.Helper.failure_code;
            }

            return Ok(commonResponse);
        }
        [HttpGet("GetCalendarData")]
        public IActionResult GetCalendarData(string doctorId)
        {
            ServiceResponse<List<AppointmentVM>> response = new();
            try {
                if(role == Helper.Helper.Doctor)
                {
                    response.data = _appointmentService.GetDoctorEventsById(loginUserId);
                    response.status = Helper.Helper.success_code;
                }
                else if(role == Helper.Helper.Patient)
                {
                    response.data = _appointmentService.GetPatientEventsById(loginUserId);
                    response.status = Helper.Helper.success_code;
                }
                else {
                    response.data = _appointmentService.GetDoctorEventsById(doctorId);
                    response.status = Helper.Helper.success_code;
                }
            } 
            catch(Exception e) {
                response.message = e.Message;
                response.status = Helper.Helper.failure_code;
                
                return BadRequest(response);
            }
            return Ok(response);
        }
    
        [HttpGet("GetCalendarDataById/{id}")]
        public IActionResult GetCalendarDataById(int id)
        {
            ServiceResponse<AppointmentVM> response = new();
            try {
                response.data = _appointmentService.GetAppointmentById(id);
                response.status = Helper.Helper.success_code;
            }
            catch(Exception e) {
                response.message = e.Message;
                response.status = Helper.Helper.failure_code;
                
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("DeleteAppointment/{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            ServiceResponse<int> response = new();
            try {
                response.status = await _appointmentService.DeleteAppointment(id);
                response.message = response.status != 0 ? Helper.Helper.appointmentDeleted : Helper.Helper.somethingWentWrong;
            }
            catch(Exception e) {
                response.status = Helper.Helper.failure_code;
                response.message = e.Message;
                
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("ConfirmEvent/{id}")]
        public async Task<IActionResult> ConfirmEvent(int id)
        {
            ServiceResponse<int> response = new();
            try {
                int result = await _appointmentService.ConfirmEvent(id);
                if(result > 0)
                {
                    response.status = result;
                    response.message = result != 0 ? Helper.Helper.appointmentUpdated : Helper.Helper.somethingWentWrong;
                }
            }
            catch(Exception e) {
                response.status = Helper.Helper.failure_code;
                response.message = e.Message;
                
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
