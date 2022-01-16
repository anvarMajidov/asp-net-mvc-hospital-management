using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalService.Data;
using HospitalService.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using HospitalService.Helper;
using Microsoft.EntityFrameworkCore;

namespace HospitalService.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;
        public AppointmentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<DoctorVM>> GetDoctorList()
        {
            List<DoctorVM> doctors = await (
                from user in _db.Users
                join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                join roles in _db.Roles.Where(r => r.Name == Helper.Helper.Doctor) on 
                userRoles.UserId equals roles.Id
                
                select new DoctorVM {
                    Id = user.Id,
                    Name = user.Name
                }).ToListAsync();

            return doctors;
        }

        public async Task<List<PatientVM>> GetPatientList()
        {
            List<PatientVM> patients = await (
                from user in _db.Users
                join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                join roles in _db.Roles.Where(r => r.Name == Helper.Helper.Patient) on 
                userRoles.UserId equals roles.Id
                
                select new PatientVM {
                    Id = user.Id,
                    Name = user.Name
                }).ToListAsync();

            return patients;
        }
    }
}