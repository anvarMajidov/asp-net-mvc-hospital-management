using System.Threading.Tasks;
using HospitalService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalService.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public IActionResult Index()
        {
            ViewBag.Duration = Helper.Helper.GetTimeDropDown();
            ViewBag.DropDown = new SelectList(_appointmentService.GetDoctorList(), "Id", "Name");
            ViewBag.PatientList = new SelectList(_appointmentService.GetPatientList(), "Id", "Name");
            return View();
        }
    }
}
