using System.Collections.Generic;
using NUnit.Framework;
using asafaweb.console.Enums;
using asafaweb.console.Exceptions;
using asafaweb.console.Logic;
using asafaweb.console.Models;

namespace asafaweb.tests
{
    [TestFixture]
    public class ApiLogicTestFixture
    {
        private const string KEY = ""; // Set as your key to make tests pass
        private const string NAME = ""; // Set as your username to make tests pass

        private const string JSON_RESULT = "{'ScanUri':'http://notasafaweb.apphb.com/','Requests':[{'RequestUri':'http://notasafaweb.apphb.com/','ResponseUri':'http://notasafaweb.apphb.com/','DurationMs':48,'ResponseBytes':3028,'HttpStatusCode':200,'HttpStatusDescription':'OK','IsResponseGzipped':true,'PageTitle':'Home Page','RequestType':'GetScanUrl','Exception':null,'HttpMethod':'GET'},{'RequestUri':'http://notasafaweb.apphb.com/trace.axd','ResponseUri':'http://notasafaweb.apphb.com/trace.axd','DurationMs':42,'ResponseBytes':4478,'HttpStatusCode':200,'HttpStatusDescription':'OK','IsResponseGzipped':true,'PageTitle':null,'RequestType':'GetTracePath','Exception':null,'HttpMethod':'GET'},{'RequestUri':'http://notasafaweb.apphb.com/?foo=<script>','ResponseUri':'http://notasafaweb.apphb.com/?foo=<script>','DurationMs':12,'ResponseBytes':3045,'HttpStatusCode':200,'HttpStatusDescription':'OK','IsResponseGzipped':true,'PageTitle':'Home Page','RequestType':'GetPathWithXssPayload','Exception':null,'HttpMethod':'GET'},{'RequestUri':'http://notasafaweb.apphb.com/','ResponseUri':'http://notasafaweb.apphb.com/','DurationMs':297,'ResponseBytes':7648,'HttpStatusCode':500,'HttpStatusDescription':'Internal server error','IsResponseGzipped':false,'PageTitle':'The state information is invalid for this page and might be corrupted.','RequestType':'PostInvalidViewState','Exception':null,'HttpMethod':'POST'},{'RequestUri':'http://notasafaweb.apphb.com/elmah.axd','ResponseUri':'http://notasafaweb.apphb.com/elmah.axd','DurationMs':51,'ResponseBytes':3264,'HttpStatusCode':200,'HttpStatusDescription':'OK','IsResponseGzipped':true,'PageTitle':'Error log for /LM/W3SVC/1243/ROOT on IP-0A7ADD4A (Page #1)','RequestType':'GetElmah','Exception':null,'HttpMethod':'GET'}],'Scans':[{'ScanStatus':'Fail','ScanOutcome':'NotSet','Request':{'RequestUri':'http://notasafaweb.apphb.com/trace.axd','ResponseUri':'http://notasafaweb.apphb.com/trace.axd','DurationMs':42,'ResponseBytes':4478,'HttpStatusCode':200,'HttpStatusDescription':'OK','IsResponseGzipped':true,'PageTitle':null,'RequestType':'GetTracePath','Exception':null,'HttpMethod':'GET'},'ScanType':'Tracing'},{'ScanStatus':'Fail','ScanOutcome':'NotSet','Request':{'RequestUri':'http://notasafaweb.apphb.com/','ResponseUri':'http://notasafaweb.apphb.com/','DurationMs':297,'ResponseBytes':7648,'HttpStatusCode':500,'HttpStatusDescription':'Internal server error','IsResponseGzipped':false,'PageTitle':'The state information is invalid for this page and might be corrupted.','RequestType':'PostInvalidViewState','Exception':null,'HttpMethod':'POST'},'ScanType':'CustomErrors'},{'ScanStatus':'Fail','ScanOutcome':'NotSet','Request':{'RequestUri':'http://notasafaweb.apphb.com/','ResponseUri':'http://notasafaweb.apphb.com/','DurationMs':297,'ResponseBytes':7648,'HttpStatusCode':500,'HttpStatusDescription':'Internal server error','IsResponseGzipped':false,'PageTitle':'The state information is invalid for this page and might be corrupted.','RequestType':'PostInvalidViewState','Exception':null,'HttpMethod':'POST'},'ScanType':'StackTrace'},{'ScanStatus':'Fail','ScanOutcome':'NotSet','Request':{'RequestUri':'http://notasafaweb.apphb.com/?foo=<script>','ResponseUri':'http://notasafaweb.apphb.com/?foo=<script>','DurationMs':12,'ResponseBytes':3045,'HttpStatusCode':200,'HttpStatusDescription':'OK','IsResponseGzipped':true,'PageTitle':'Home Page','RequestType':'GetPathWithXssPayload','Exception':null,'HttpMethod':'GET'},'ScanType':'RequestValidation'},{'ScanStatus':'Pass','ScanOutcome':'NotSet','Request':{'RequestUri':'http://notasafaweb.apphb.com/','ResponseUri':'http://notasafaweb.apphb.com/','DurationMs':48,'ResponseBytes':3028,'HttpStatusCode':200,'HttpStatusDescription':'OK','IsResponseGzipped':true,'PageTitle':'Home Page','RequestType':'GetScanUrl','Exception':null,'HttpMethod':'GET'},'ScanType':'HttpToHttps'},{'ScanStatus':'Pass','ScanOutcome':'HashDosDotNet45Present','Request':{'RequestUri':'http://notasafaweb.apphb.com/trace.axd','ResponseUri':'http://notasafaweb.apphb.com/trace.axd','DurationMs':42,'ResponseBytes':4478,'HttpStatusCode':200,'HttpStatusDescription':'OK','IsResponseGzipped':true,'PageTitle':null,'RequestType':'GetTracePath','Exception':null,'HttpMethod':'GET'},'ScanType':'HashDosPatch'},{'ScanStatus':'Fail','ScanOutcome':'NotSet','Request':{'RequestUri':'http://notasafaweb.apphb.com/elmah.axd','ResponseUri':'http://notasafaweb.apphb.com/elmah.axd','DurationMs':51,'ResponseBytes':3264,'HttpStatusCode':200,'HttpStatusDescription':'OK','IsResponseGzipped':true,'PageTitle':'Error log for /LM/W3SVC/1243/ROOT on IP-0A7ADD4A (Page #1)','RequestType':'GetElmah','Exception':null,'HttpMethod':'GET'},'ScanType':'ElmahLog'},{'ScanStatus':'Warning','ScanOutcome':'NotSet','Request':{'RequestUri':'http://notasafaweb.apphb.com/','ResponseUri':'http://notasafaweb.apphb.com/','DurationMs':48,'ResponseBytes':3028,'HttpStatusCode':200,'HttpStatusDescription':'OK','IsResponseGzipped':true,'PageTitle':'Home Page','RequestType':'GetScanUrl','Exception':null,'HttpMethod':'GET'},'ScanType':'ExcessiveHeaders','ServerHeader':'nginx','XPoweredBy':[],'XAspNetVersion':null,'XAspNetMvcVersion':null},{'ScanStatus':'Warning','ScanOutcome':'NotSet','Request':{'RequestUri':'http://notasafaweb.apphb.com/','ResponseUri':'http://notasafaweb.apphb.com/','DurationMs':48,'ResponseBytes':3028,'HttpStatusCode':200,'HttpStatusDescription':'OK','IsResponseGzipped':true,'PageTitle':'Home Page','RequestType':'GetScanUrl','Exception':null,'HttpMethod':'GET'},'ScanType':'HttpOnlyCookies','NonHttpOnlyCookies':{'InsecureCookie1':'I can be read by the client as I havent been flagged as HttpOnly.','InsecureCookie2':'So can I!'}},{'ScanStatus':'Pass','ScanOutcome':'SecureCookiesNoHttpsResponses','Request':{'RequestUri':'http://notasafaweb.apphb.com/','ResponseUri':'http://notasafaweb.apphb.com/','DurationMs':48,'ResponseBytes':3028,'HttpStatusCode':200,'HttpStatusDescription':'OK','IsResponseGzipped':true,'PageTitle':'Home Page','RequestType':'GetScanUrl','Exception':null,'HttpMethod':'GET'},'ScanType':'SecureCookies','NonSecureCookies':{'ASP.NET_SessionId':'2lvyl0q0oyprfxkoaaarvujb','InsecureCookie1':'I can be read by the client as I havent been flagged as HttpOnly.','InsecureCookie2':'So can I!'}},{'ScanStatus':'Warning','ScanOutcome':'NotSet','Request':{'RequestUri':'http://notasafaweb.apphb.com/','ResponseUri':'http://notasafaweb.apphb.com/','DurationMs':48,'ResponseBytes':3028,'HttpStatusCode':200,'HttpStatusDescription':'OK','IsResponseGzipped':true,'PageTitle':'Home Page','RequestType':'GetScanUrl','Exception':null,'HttpMethod':'GET'},'ScanType':'Clickjacking','XFrameOptions':null}],'SiteTitle':'Home Page','IsWebFormsApp':true,'IsAspNetSite':true,'ServerHeader':'nginx','XAspNetVersion':null,'XAspNetMvcVersion':null,'XPoweredBy':[],'AspNetVersion':'4.0.30319.18034','OverallScanStatus':'Fail'}";

        [Test]
        [Ignore("Ignoring this as API Key and Name arenot specified")]
        public void ApiLogic_Scan_NotAsafaWeb_ResponseValid()
        {
            // Arrange
            ApiLogic logic = new ApiLogic(NAME, KEY, "notasafaweb.apphb.com");
            // Act
            var result = logic.Scan();
            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(ApiScanResult)));
        }

        [Test]
        [ExpectedException(typeof(ApiException), ExpectedMessage = "Unauthorized: Ensure you have specified your correct API username and key.")]
        public void ApiLogic_Scan_NoKeyOrName_ResponseValid()
        {
            // Arrange
            ApiLogic logic = new ApiLogic(string.Empty, string.Empty, "notasafaweb.apphb.com");
            // Act
            logic.Scan();
        }

        [Test]
        public void ApiLogic_ApiScanResult_JSONString_ResponseValid()
        {
            // Arrange
            // Act
            var result = ApiLogic.ApiScanResult(JSON_RESULT);
            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(ApiScanResult)));
        }

        //[Test]
        //public void StatusLogic_AnalyseApiResults_CorrectResultsReturned()
        //{
        //    // Arrange
        //    ApiScanResult results = ApiLogic.ApiScanResult(JSON_RESULT);
        //    StatusLogic statusLogic = new StatusLogic();
        //    // Act
        //    Dictionary<string, AsafaResult> analysedResults = statusLogic.AnalyseResults(results);
        //    // Assert
        //    Assert.That(analysedResults.Count,Is.EqualTo(5));
        //}
    }
}
