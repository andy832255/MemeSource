namespace MemeSource.Model.Models
{
    public class SystemProperty
    {
        public int ID { get; set; }
        public string? SP_Name { get; set; }
        public string? Parameter1 { get; set; }
        public string? Parameter2 { get; set; }
        public string? Parameter3 { get; set; }
        public string? Parameter4 { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
