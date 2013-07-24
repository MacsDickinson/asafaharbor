using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using HtmlAgilityPack;
using asafaweb.api.Enums;
using asafaweb.api.Models;

namespace asafaweb.api.Logic
{
    public class StatusLogic
    {
        #region Properties
        public bool FailOnWarning { get; set; }
        public bool FailOnNotTested { get; set; }
        public bool FailOnFailure { get; set; }
        public List<string> IgnoredTests { get; set; }
        #endregion

        public StatusLogic()
        {
            FailOnFailure = true;
            FailOnWarning = false;
            FailOnNotTested = false;
            IgnoredTests = new List<string>();
        }

        public Dictionary<string, ASafaResult> AnalyseResults(ApiScanResult results)
        {
            Dictionary<string, ASafaResult> failedResults = new Dictionary<string, ASafaResult>();
            foreach (Scan scan in results.Scans.Where(x => !IsTestIgnored(x.ScanType)))
            {
                if (FailOnWarning)
                {
                    if (scan.ScanStatus == ASafaResult.Warning)
                    {
                        failedResults.Add(scan.ScanType, scan.ScanStatus);
                    }
                }
                if (FailOnNotTested)
                {
                    if (scan.ScanStatus == ASafaResult.NotTested)
                    {
                        failedResults.Add(scan.ScanType, scan.ScanStatus);
                    }
                }
                if (FailOnFailure)
                {
                    if (scan.ScanStatus == ASafaResult.Fail)
                    {
                        failedResults.Add(scan.ScanType, scan.ScanStatus);
                    }
                }
            }
            return failedResults;
        }

        public Dictionary<string, ASafaResult> AnalyseResults(Dictionary<string, ASafaResult> results)
        {
            Dictionary<string, ASafaResult> failedResults = new Dictionary<string, ASafaResult>();
            foreach (KeyValuePair<string, ASafaResult> asafaResult in results.Where(asafaResult => !IsTestIgnored(asafaResult.Key)))
            {
                if (FailOnWarning)
                {
                    if (asafaResult.Value == ASafaResult.Warning)
                    {
                        failedResults.Add(asafaResult.Key, asafaResult.Value);
                    }
                }
                if (FailOnNotTested)
                {
                    if (asafaResult.Value == ASafaResult.NotTested)
                    {
                        failedResults.Add(asafaResult.Key, asafaResult.Value);
                    }
                }
                if (FailOnFailure)
                {
                    if (asafaResult.Value == ASafaResult.Fail)
                    {
                        failedResults.Add(asafaResult.Key, asafaResult.Value);
                    }
                }
            }
            return failedResults;
        }

        public bool IsTestIgnored(string name)
        {
            return IgnoredTests.Contains(name);
        }


        #region Static Methods

        public static ASafaResult GetStatus(string status)
        {
            switch (status)
            {
                case "Pass":
                    return ASafaResult.Pass;
                case "Fail":
                    return ASafaResult.Fail;
                case "Warning":
                    return ASafaResult.Warning;
                default:
                    return ASafaResult.NotTested;
            }
        }

        public static Dictionary<string, ASafaResult> GetTestResults(string url)
        {
            HtmlGetLogic htmlLogic = new HtmlGetLogic();
            try
            {
                HtmlDocument response = htmlLogic.LoadHtmlResponse(string.Format(@"https://asafaweb.com/Scan?Url={0}", HttpUtility.UrlEncode(url)));
                HtmlNodeCollection nodesMatchingXPath = response.DocumentNode.SelectNodes("//div[@id='StatusSummary']/span");
                return nodesMatchingXPath != null
                    ? nodesMatchingXPath.ToDictionary(element => element.Attributes["id"].Value.Replace("Summary", ""), element => GetStatus(element.Attributes["class"].Value))
                    : new Dictionary<string, ASafaResult>();
            }
            catch (WebException)
            {
                return new Dictionary<string, ASafaResult>();
            }
        }

        #endregion
    }
}
