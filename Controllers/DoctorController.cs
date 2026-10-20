using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using HospitalAppointmentSystem.Models;
using HospitalAppointmentSystem.Extensions;

namespace HospitalAppointmentSystem.Controllers
{
    // Handles doctor details and appointment requests.
    public class DoctorController : Controller
    {
        // Stores the memory cache service.
        private readonly IMemoryCache _cache;

        // Creates the controller and receives the cache service.
        public DoctorController(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Displays the list of doctors.
        public IActionResult Index(string? specialization)
        {
            // Stores the selected specialization in session if provided.
            if (!string.IsNullOrWhiteSpace(specialization))
            {
                HttpContext.Session.SetString("Specialization", specialization);
            }

            // Retrieves the selected specialization from session.
            string selected = HttpContext.Session.GetString("Specialization") ?? "";

            // Checks whether the doctor list is already cached.
            if (!_cache.TryGetValue("DoctorList", out List<Doctor>? doctors))
            {
                // Creates sample doctor records if the cache is empty.
                doctors = new List<Doctor>
                {
                    new Doctor { DoctorId = 1, DoctorName = "Dr. Amit Patel",
                        Specialization = "Cardiology", Experience = 10,
                        ConsultationFee = 500 },

                    new Doctor { DoctorId = 2, DoctorName = "Dr. Neha Shah",
                        Specialization = "Neurology", Experience = 7,
                        ConsultationFee = 600 },

                    new Doctor { DoctorId = 3, DoctorName = "Dr. Raj Mehta",
                        Specialization = "Cardiology", Experience = 5,
                        ConsultationFee = 400 }
                };

                // Stores the doctor list in memory cache for 3 minutes.
                _cache.Set("DoctorList", doctors,
                    TimeSpan.FromMinutes(3));
            }

            // Filters doctors when a specialization is selected.
            var result = string.IsNullOrEmpty(selected)
                ? doctors!
                : doctors!.Where(d => d.Specialization == selected).ToList();

            // Sends the doctor list to the Index view.
            return View(result);
        }

        // Enables response caching for the doctor details page.
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public IActionResult Details(int id)
        {
            // Retrieves the cached doctor list.
            if (!_cache.TryGetValue("DoctorList", out List<Doctor>? doctors))
            {
                // Loads the list again if the cache has expired.
                Index(null);
                _cache.TryGetValue("DoctorList", out doctors);
            }

            // Finds the doctor whose ID matches the QueryString value.
            var doctor = doctors?.FirstOrDefault(d => d.DoctorId == id);

            // Displays an error if the doctor does not exist.
            if (doctor == null)
                return NotFound();

            // Sends doctor information to the Details view.
            return View(doctor);
        }

        // Displays the appointment booking form.
        [HttpGet]
        public IActionResult Book(int id)
        {
            // Finds the selected doctor.
            if (!_cache.TryGetValue("DoctorList", out List<Doctor>? doctors))
            {
                // Loads the doctor list if it is not cached.
                Index(null);
                _cache.TryGetValue("DoctorList", out doctors);
            }

            // Checks whether the requested doctor exists.
            var doctor = doctors?.FirstOrDefault(d => d.DoctorId == id);

            // Returns a 404 response if the doctor is not found.
            if (doctor == null)
                return NotFound();

            // Displays the booking form with doctor information.
            return View(doctor);
        }

        // Processes the submitted appointment form.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Book(int DoctorId, string PatientName)
        {
            // Checks whether the patient name is provided.
            if (string.IsNullOrWhiteSpace(PatientName))
            {
                // Displays a validation error for an empty name.
                ModelState.AddModelError("PatientName",
                    "Patient name is required.");

                // Reloads the selected doctor.
                if (!_cache.TryGetValue("DoctorList", out List<Doctor>? doctors))
                {
                    Index(null);
                    _cache.TryGetValue("DoctorList", out doctors);
                }

                var doctor = doctors?.FirstOrDefault(d => d.DoctorId == DoctorId);

                // Returns the form if the doctor exists.
                if (doctor == null)
                    return NotFound();

                return View(doctor);
            }

            // Stores the patient name in a browser cookie for one day.
            Response.Cookies.Append("PatientName", PatientName,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(1),
                    HttpOnly = true,
                    IsEssential = true
                });

            // Stores the patient's name for the confirmation page.
            ViewBag.PatientName = PatientName;

            // Sends the selected doctor ID to the confirmation page.
            ViewBag.DoctorId = DoctorId;

            // Displays the appointment confirmation page.
            return View("Confirmation");
        }
    }
}