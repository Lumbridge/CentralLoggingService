using CLS.Core.Data;
using CLS.Core.StaticData;
using CLS.Infrastructure.Classes;
using CLS.Infrastructure.Data;
using CLS.Infrastructure.Helpers;
using CLS.Infrastructure.Interfaces;
using CLS.Sender.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;

namespace CLS.Sender.Classes
{
    public class LogSender
    {
        private static OAuthResponse _OAuthResponse { get; set; }
        private static OAuthResponse OAuthResponse
        {
            get
            {
                if (_OAuthResponse == null || _OAuthResponse != null && DateTime.Now.AddHours(1) >= _OAuthResponse.expires)
                {
                    _OAuthResponse = GetAccessToken().Result;
                }
                return _OAuthResponse;
            }
        }
        private static PublishingSystem _publishingSystem { get; set; }

        // ============
        // Constructors
        // ============

        /// <summary>
        /// LogSender constructor.
        /// </summary>
        /// <param name="environmentType">The environment type of this publishing system. (DEV, SIT, UAT or LIVE).</param>
        /// <param name="systemType">The type of this publishing system. (REST, SOAP, Website, WindowsService, ConsoleApplication, Other)</param>
        /// <param name="publishingSystemName">The name of the publishing system.</param>
        public LogSender(StaticData.EnvironmentType environmentType, StaticData.SystemType systemType, string publishingSystemName = "")
        {
            _publishingSystem = string.IsNullOrEmpty(publishingSystemName) 
                ? GetPublishingSystem(Assembly.GetCallingAssembly().GetName().Name, environmentType, systemType).Result 
                : GetPublishingSystem(publishingSystemName, environmentType, systemType).Result;
        }

        // =======================
        // Main exposed Log Method
        // =======================

        /// <summary>
        /// Sends a log message to a chosen database or web service endpoint (specified in App_Data\CLSSettings.config).
        /// </summary>
        /// <param name="severity">The severity of the log message. (Debug, Info, Warn, Error or Fatal).</param>
        /// <param name="exception">Optional: An exception object.</param>
        /// <param name="info">Optional: Any information to store for this record.</param>
        /// <returns>Text result, empty string if failure.</returns>
        public string Log(StaticData.SeverityType severity, Exception exception = null, string info = null)
        {
            var useWebservice = bool.Parse(ConfigurationManager.AppSettings["UseWebservice"]);
            switch (severity)
            {
                case StaticData.SeverityType.Debug:
                {
                    return useWebservice 
                        ? LogToWebService("D", exception, info).Result
                        : LogToDatabase("D", exception, info);
                }
                case StaticData.SeverityType.Info:
                {
                    return useWebservice
                        ? LogToWebService("I", exception, info).Result
                        : LogToDatabase("I", exception, info);
                    }
                case StaticData.SeverityType.Warn:
                {
                    return useWebservice
                        ? LogToWebService("W", exception, info).Result
                        : LogToDatabase("W", exception, info);
                    }
                case StaticData.SeverityType.Error:
                {
                    return useWebservice
                        ? LogToWebService("E", exception, info).Result
                        : LogToDatabase("E", exception, info);
                    }
                case StaticData.SeverityType.Fatal:
                {
                    return useWebservice
                        ? LogToWebService("F", exception, info).Result
                        : LogToDatabase("F", exception, info);
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(severity), severity, "Invalid severity type supplied.");
            }
        }

        // =============================================
        // Methods for logging directly to a web service
        // =============================================

        /// <summary>
        /// Logs a message via the web service specified in App_Data\CLSSettings.config.
        /// </summary>
        /// <param name="severityCode">The severity code of the log message. (Debug = D, Info = I, Warn = W, Error = E or Fatal = F).</param>
        /// <param name="exception">Optional: An exception object.</param>
        /// <param name="info">Optional: Any information to store for this record.</param>
        /// <returns>Text result, empty string if failure.</returns>
        private static async Task<string> LogToWebService(string severityCode, Exception exception = null, string info = null)
        {
            var logObj = new Log
            {
                Exception = exception?.GetExceptionMessages(),
                StackTrace = exception?.StackTrace,
                Message = info,
                PublishingSystemId = _publishingSystem.Id,
                Timestamp = DateTime.Now,
                SeverityId = StaticData.Severities.First(x => x.Code == severityCode).Id
            };

            return await Post(OAuthResponse.access_token, logObj);
        }
        
        /// <summary>
        /// Attempts to gain authorisation from the CLS Web Service using the login credentials specified in AppData\CLSSettings.config.
        /// </summary>
        /// <returns>An OAuthResponse object.</returns>
        private static async Task<OAuthResponse> GetAccessToken()
        {
            // Posting
            using (var client = new HttpClient())
            {
                // Setting Base address
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebserviceEndpoint"]);

                // Setting content type
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Initialisation
                var allInputParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", ConfigurationManager.AppSettings["WebserviceUsername"]),
                    new KeyValuePair<string, string>("password", ConfigurationManager.AppSettings["WebservicePassword"])
                };

                // URL Request parameters
                HttpContent requestParams = new FormUrlEncodedContent(allInputParams);

                // HTTP POST
                var response = await client.PostAsync("Token", requestParams).ConfigureAwait(false);

                // Verification
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<OAuthResponse>(response.Content.ReadAsStringAsync().Result);
                }

                // Invalid login credentials check
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var result = JsonConvert.DeserializeObject<OAuthResponse>(response.Content.ReadAsStringAsync().Result);
                    throw new Exception("Error: " + result.error + ", Description: " + result.error_description);
                }

                return null;
            }
        }

        /// <summary>
        /// Logs a message via the web service specified in App_Data\CLSSettings.config via HTTP POST.
        /// </summary>
        /// <param name="authorizeToken">The OAuth 2.0 token received from the CLS Web Service.</param>
        /// <param name="message">The Log Message to send to the Web Service.</param>
        /// <returns>Text result, empty string if failure.</returns>
        private static async Task<string> Post(string authorizeToken, Log message)
        {
            // HTTP GET
            using (var client = new HttpClient())
            {
                // Initialisation
                var authorization = authorizeToken;

                // Setting Authorization
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                // Setting Base address
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebserviceEndpoint"]);

                // Setting content type
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                var response = await client.PostAsJsonAsync("CLS", message).ConfigureAwait(false);

                // Verification
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the publishing system info from the CLS Database, if it doesn't exist then it will be created.
        /// </summary>
        /// <param name="publishingSystemName">The name of the publishing system.</param>
        /// <param name="environmentType">The environment type of this publishing system. (DEV, SIT, UAT or LIVE).</param>
        /// <param name="systemType">The type of this publishing system. (REST, SOAP, Website, WindowsService, ConsoleApplication, Other)</param>
        /// <returns>The publishing system object from the CLS Web Service/Database.</returns>
        private static async Task<PublishingSystem> GetPublishingSystem(
            string publishingSystemName,
            StaticData.EnvironmentType environmentType,
            StaticData.SystemType systemType)
        {
            var useWebservice = bool.Parse(ConfigurationManager.AppSettings["UseWebservice"]);

            if (useWebservice)
            {
                // HTTP GET
                using (var client = new HttpClient())
                {
                    // Setting Authorization
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OAuthResponse.access_token);

                    // Setting Base address
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebserviceEndpoint"]);

                    // Setting content type
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Initialization

                    var requestString = $"CLS/PublishingSystem" +
                                        $"?publishingSystemName={publishingSystemName}" +
                                        $"&environmentType={environmentType}" +
                                        $"&systemType={systemType}";

                    // HTTP GET
                    var response = await client.GetAsync(requestString).ConfigureAwait(false);

                    // Verification
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResult = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result).ToString();
                        var model = JsonConvert.DeserializeObject<PublishingSystem>(jsonResult, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                        return model;
                    }
                }

                return null;
            }
            else
            {
                var uow = new UnitOfWork(new DBEntities());

                // get static data objects from database
                var environmentTypeObj =
                    uow.Repository<EnvironmentType>().First(x => x.Name == environmentType.ToString());
                var systemTypeObj = uow.Repository<PublishingSystemType>().First(x => x.Name == systemType.ToString());

                // check if publishing system exists in database
                var pSystem = uow.Repository<PublishingSystem>().FirstOrDefault(x =>
                    x.Name == publishingSystemName && x.EnvironmentTypeId == environmentTypeObj.Id &&
                    x.PublishingSystemTypeId == systemTypeObj.Id);

                // if the publishing system doesn't exist in the database then create it
                if (pSystem == null)
                {
                    pSystem = uow.Repository<PublishingSystem>().Put(new PublishingSystem
                    {
                        EnvironmentType = environmentTypeObj,
                        PublishingSystemType = systemTypeObj,
                        Name = publishingSystemName
                    });
                    TryCommit(uow);
                }

                // return the publishing system
                return pSystem;
            }
        }

        // ==========================================
        // Methods for logging directly to a database
        // ==========================================

        /// <summary>
        /// Logs a message via the web service specified in App_Data\Connections.config.
        /// </summary>
        /// <param name="severityCode">The severity code of the log message. (Debug = D, Info = I, Warn = W, Error = E or Fatal = F).</param>
        /// <param name="exception">Optional: An exception object.</param>
        /// <param name="info">Optional: Any information to store for this record.</param>
        /// <returns>Text result, empty string if failure.</returns>
        private string LogToDatabase(string severityCode, Exception exception = null, string info = null)
        {
            var uow = new UnitOfWork(new DBEntities());

            var model = new Log
            {
                Exception = exception?.GetExceptionMessages(),
                StackTrace = exception?.StackTrace,
                Message = info,
                PublishingSystem = _publishingSystem,
                Timestamp = DateTime.Now,
                SeverityId = StaticData.Severities.First(x => x.Code == severityCode).Id
            };

            uow.Repository<Log>().Put(model);

            return TryCommit(uow) ? "Successfully logged message." : string.Empty;
        }
        
        /// <summary>
        /// Commit the unit of work changes to the database.
        /// </summary>
        /// <param name="uow">The unit of work context.</param>
        /// <returns>True if success, false if failure.</returns>
        private static bool TryCommit(IUnitOfWork uow)
        {
            try
            {
                uow.Commit();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
