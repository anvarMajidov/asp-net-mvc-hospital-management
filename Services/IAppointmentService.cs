using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalService.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HospitalService.Services
{
    public interface IAppointmentService
    {
        Task<List<DoctorVM>> GetDoctorList();
        Task<List<PatientVM>> GetPatientList();
    }
}