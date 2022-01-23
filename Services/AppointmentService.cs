using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalService.Data;
using HospitalService.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using HospitalService.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using HospitalService.Models;

namespace HospitalService.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;
        public AppointmentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<DoctorVM> GetDoctorList()
        {
            List<DoctorVM> doctors = (
                from user in _db.Users
                join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                join roles in _db.Roles.Where(r => r.Name == Helper.Helper.Doctor) on 
                userRoles.RoleId equals roles.Id
                
                select new DoctorVM {
                    Id = user.Id,
                    Name = user.Name
                }).ToList();

            return doctors;
        }

        public List<PatientVM> GetPatientList()
        {
            List<PatientVM> patients = (
                from user in _db.Users
                join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                join roles in _db.Roles.Where(r => r.Name == Helper.Helper.Patient) on 
                userRoles.RoleId equals roles.Id
                
                select new PatientVM {
                    Id = user.Id,
                    Name = user.Name
                }).ToList();

            return patients;
        }

        public async Task<int> AddUpdate(AppointmentVM model)
        {
            ServiceResponse<int> response = new();

            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));

            if(model != null && model.Id > 0)
            {
                //update
                return 1;
            }
            else 
            {
                Appointment appointment = new Appointment()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = model.Duration,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    AdminId = model.AdminId,
                    IsDoctorApproved = model.IsDoctorApproved
                };
                await _db.Appointments.AddAsync(appointment);
                await _db.SaveChangesAsync();
                return 2;
            }
        }

        public List<AppointmentVM> GetDoctorEventsById(string doctorId)
        {
            return _db.Appointments.Where(a => a.DoctorId == doctorId).Select(a => new AppointmentVM
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                StartDate = a.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = a.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Duration = a.Duration,
                IsDoctorApproved = a.IsDoctorApproved
            }).ToList();
        }

        public List<AppointmentVM> GetPatientEventsById(string patientId)
        {
            return _db.Appointments.Where(a => a.PatientId == patientId).ToList().Select(a => new AppointmentVM
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                StartDate = a.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = a.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Duration = a.Duration,
                IsDoctorApproved = a.IsDoctorApproved
            }).ToList();
        }
    }
}