namespace HospitalAppointmentSystem.Models
{
    // Represents a post received from the external API.
    public class Post
    {
        // Stores the post's unique ID.
        public int Id { get; set; }

        // Stores the title of the post.
        public string Title { get; set; } = "";
    }
}