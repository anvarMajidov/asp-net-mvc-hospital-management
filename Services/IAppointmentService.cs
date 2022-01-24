using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalService.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HospitalService.Services
{
    public interface IAppointmentService
    {
        List<DoctorVM> GetDoctorList();
        List<PatientVM> GetPatientList();
        Task<int> AddUpdate(AppointmentVM model);
        List<AppointmentVM> GetDoctorEventsById(string doctorId);
        List<AppointmentVM> GetPatientEventsById(string patientId);
        AppointmentVM GetAppointmentById(int id);
        Task<int> DeleteAppointment(int id);
        Task<int> ConfirmEvent(int id);
    }
}