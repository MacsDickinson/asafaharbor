using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using HtmlAgilityPack;
using asafaweb.console.Enums;
using asafaweb.console.Models;

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

        public Dictionary<string, AsafaResult> AnalyseResults(ApiScanResult results)
        {
            Dictionary<string, AsafaResult> failedResults = new Dictionary<string, AsafaResult>();
            foreach (Scan scan in results.Scans.Where(x => !IsTestIgnored(x.ScanType)))
            {
                if (FailOnWarning)
                {
                    if (scan.ScanStatus == AsafaResult.Warning)
                    {
                        failedResults.Add(scan.ScanType, scan.ScanStatus);
                    }
                }
                if (FailOnNotTested)
                {
                    if (scan.ScanStatus == AsafaResult.NotTested)
                    {
                        failedResults.Add(scan.ScanType, scan.ScanStatus);
                    }
                }
                if (FailOnFailure)
                {
                    if (scan.ScanStatus == AsafaResult.Fail)
                    {
                        failedResults.Add(scan.ScanType, scan.ScanStatus);
                    }
                }
            }
            return failedResults;
        }

        public Dictionary<string, AsafaResult> AnalyseResults(Dictionary<string, AsafaResult> results)
        {
            Dictionary<string, AsafaResult> failedResults = new Dictionary<string, AsafaResult>();
            foreach (KeyValuePair<string, AsafaResult> asafaResult in results.Where(asafaResult => !IsTestIgnored(asafaResult.Key)))
            {
                if (FailOnWarning)
                {
                    if (asafaResult.Value == AsafaResult.Warning)
                    {
                        failedResults.Add(asafaResult.Key, asafaResult.Value);
                    }
                }
                if (FailOnNotTested)
                {
                    if (asafaResult.Value == AsafaResult.NotTested)
                    {
                        failedResults.Add(asafaResult.Key, asafaResult.Value);
                    }
                }
                if (FailOnFailure)
                {
                    if (asafaResult.Value == AsafaResult.Fail)
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

        public static AsafaResult GetStatus(string status)
        {
            switch (status)
            {
                case "Pass":
                    return AsafaResult.Pass;
                case "Fail":
                    return AsafaResult.Fail;
                case "Warning":
                    return AsafaResult.Warning;
                default:
                    return AsafaResult.NotTested;
            }
        }

        public static Dictionary<string, AsafaResult> GetTestResults(string url)
        {
            HtmlGetLogic htmlLogic = new HtmlGetLogic();
            try
            {
                HtmlDocument response = htmlLogic.LoadHtmlResponse(string.Format(@"https://asafaweb.com/Scan?Url={0}", HttpUtility.UrlEncode(url)));
                HtmlNodeCollection nodesMatchingXPath = response.DocumentNode.SelectNodes("//div[@id='StatusSummary']/span");
                return nodesMatchingXPath != null
                    ? nodesMatchingXPath.ToDictionary(element => element.Attributes["id"].Value.Replace("Summary", ""), element => GetStatus(element.Attributes["class"].Value))
                    : new Dictionary<string, AsafaResult>();
            }
            catch (WebException)
            {
                return new Dictionary<string, AsafaResult>();
            }
        }

        #endregion
    }
}
