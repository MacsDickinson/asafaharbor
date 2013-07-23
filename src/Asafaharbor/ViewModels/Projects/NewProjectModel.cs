using System.ComponentModel.DataAnnotations;

namespace Asafaharbor.Web.ViewModels.Projects
{
    public class NewProjectModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Url]
        public string Url { get; set; }
    }
}