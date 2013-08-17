namespace Asafaharbor.Web.ViewModels
{
    public class InternalServerErrorViewModel
    {
        public string ErrorId { get; set; }
        public string Summary { get; set; }
        public string[] Exceptions { get; set; }
        public string StackTrace { get; set; }
    }
}