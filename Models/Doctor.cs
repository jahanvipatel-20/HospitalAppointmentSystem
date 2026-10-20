namespace HospitalAppointmentSystem.Models
{
    // Defines the Doctor model and its properties.
    public class Doctor
    {
        // Stores the unique ID of the doctor.
        public int DoctorId { get; set; }

        // Stores the doctor's name.
        public string DoctorName { get; set; } = "";

        // Stores the doctor's medical specialization.
        public string Specialization { get; set; } = "";

        // Stores the doctor's years of experience.
        public int Experience { get; set; }

        // Stores the basic consultation fee.
        public decimal ConsultationFee { get; set; }
    }
}