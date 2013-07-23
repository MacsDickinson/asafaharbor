using System.Collections.Generic;
using Asafaharbor.Web.Models.Enums;

namespace Asafaharbor.Web.Models
{
    public class ProjectSettings
    {
        public bool FailOnWarnings { get; set; }
        public bool FailOnNotTested { get; set; }
        public List<ASafaTest> IgnoredTests { get; set; }
        public bool EmailOnFailure { get; set; }
        public string FailureEmail { get; set; }
        public bool TweetOnFailure { get; set; }
        public string FailureTwitter { get; set; }
    }
}