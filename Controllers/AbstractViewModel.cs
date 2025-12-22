namespace Todo_App.Controllers
{
    public class AbstractViewModel
    {
        public bool success { get; set; }
        public int Id { get; set; }
        public List<string> lstErrors { get; set; } = new List<string>();
        public List<string> lstWarning { get; set; } = new List<string>();
    }
}
