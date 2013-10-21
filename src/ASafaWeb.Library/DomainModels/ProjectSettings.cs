using System.Collections.Generic;
using ASafaWeb.Library.DomainModels.Enums;

namespace ASafaWeb.Library.DomainModels
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