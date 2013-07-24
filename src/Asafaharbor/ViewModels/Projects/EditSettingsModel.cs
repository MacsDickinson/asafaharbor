using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Asafaharbor.Web.Models.Enums;

namespace Asafaharbor.Web.ViewModels.Projects
{
    public class EditSettingsModel
    {
        public string ProjectId { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string UrlToScan { get; set; }
        [Required]
        public bool FailOnWarnings { get; set; }
        [Required]
        public bool FailOnNotTested { get; set; }
        public List<ASafaTest> IgnoredTests { get; set; }
        [Required]
        public bool EmailOnFailure { get; set; }
        public string FailureEmail { get; set; }
        [Required]
        public bool TweetOnFailure { get; set; }
        public string FailureTwitter { get; set; }
    }
}