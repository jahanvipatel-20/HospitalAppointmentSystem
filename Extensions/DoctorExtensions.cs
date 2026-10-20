using HospitalAppointmentSystem.Models;

namespace HospitalAppointmentSystem.Extensions
{
    // Defines extension methods for the Doctor class.
    public static class DoctorExtensions
    {
        // Adds a service charge of ₹100 to the consultation fee.
        public static decimal CalculateTotalCharge(this Doctor doctor)
        {
            // Returns the consultation fee plus the service charge.
            return doctor.ConsultationFee + 100;
        }
    }
}