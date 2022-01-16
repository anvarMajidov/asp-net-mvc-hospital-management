using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalService.Data;
using HospitalService.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using HospitalService.Helper;
using Microsoft.EntityFrameworkCore;
using System;

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
    }
}