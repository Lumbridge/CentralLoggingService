using CLS.Core.Data;
using CLS.Core.StaticData;
using CLS.Infrastructure.Classes;
using CLS.Infrastructure.Helpers;
using CLS.Infrastructure.Interfaces;
using CLS.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CLS.Sender.Models;
using Newtonsoft.Json;

namespace CLS.Sender.Classes
{
    public class LogSender
    {
        private string _accessToken { get; set; }
        private DateTime _expiryDate { get; set; }
        private string _publishingSystemName { get; set; }
        private StaticData.EnvironmentType _environmentType { get; set; }
        private StaticData.SystemType _systemType { get; set; }
        private PublishingSystem _publishingSystem { get; set; }

        public LogSender(string publishingSystemName, StaticData.EnvironmentType environmentType, StaticData.SystemType systemType)
        {
            _publishingSystemName = publishingSystemName;
            _environmentType = environmentType;
            _systemType = systemType;
            _publishingSystem = GetPublishingSystem(publishingSystemName, environmentType, systemType).Result;
        }

        public LogSender(StaticData.EnvironmentType environmentType, StaticData.SystemType systemType)
        {
            _publishingSystemName = Assembly.GetCallingAssembly().FullName;
            _environmentType = environmentType;
            _systemType = systemType;
            _accessToken = GetAccessToken().Result.access_token;
            _publishingSystem = GetPublishingSystem(_publishingSystemName, environmentType, systemType).Result;
        }

        public static async Task<OAuthResponse> GetAccessToken()
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
            }

            return null;
        }
        
        public static async Task<string> Get(string authorizeToken)
        {
            // Initialization
            var responseObj = string.Empty;

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
                var response = await client.GetAsync("CLS").ConfigureAwait(false);

                // Verification
                if (response.IsSuccessStatusCode)
                {
                    // Reading Response
                }
            }

            return responseObj;
        }
        
        public static async Task<string> Post(string authorizeToken, Log message)
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

                // Generate request payload
                var logMessageJson = JsonConvert.SerializeObject(message);
                HttpContent requestContent = new StringContent(logMessageJson, Encoding.UTF8, "application/json");

                // HTTP GET
                var response = await client.PostAsync("CLS", requestContent).ConfigureAwait(false);

                // Verification
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }

            return string.Empty;
        }
        
        public string Log(StaticData.SeverityType severity, Exception exception = null, string info = null)
        {
            var useWebservice = bool.Parse(ConfigurationManager.AppSettings["UseWebservice"]);
            switch (severity)
            {
                case StaticData.SeverityType.Debug:
                {
                    return useWebservice 
                        ? LogToWebService("D", exception, info)
                        : LogToDatabase("D", exception, info);
                }
                case StaticData.SeverityType.Info:
                {
                    return useWebservice
                        ? LogToWebService("I", exception, info)
                        : LogToDatabase("I", exception, info);
                    }
                case StaticData.SeverityType.Warn:
                {
                    return useWebservice
                        ? LogToWebService("W", exception, info)
                        : LogToDatabase("W", exception, info);
                    }
                case StaticData.SeverityType.Error:
                {
                    return useWebservice
                        ? LogToWebService("E", exception, info)
                        : LogToDatabase("E", exception, info);
                    }
                case StaticData.SeverityType.Fatal:
                {
                    return useWebservice
                        ? LogToWebService("F", exception, info)
                        : LogToDatabase("F", exception, info);
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(severity), severity, "Invalid severity type supplied.");
            }
        }

        public string LogToWebService(string severityCode, Exception exception = null, string info = null)
        {
            var oAuthResponse = GetAccessToken().Result;

            var logObj = new Log
            {
                Exception = exception?.GetExceptionMessages(),
                StackTrace = exception?.StackTrace,
                Message = info,
                PublishingSystem = _publishingSystem,
                Timestamp = DateTime.Now,
                Severity = StaticData.Severities.First(x => x.Code == severityCode)
            };

            return Post(oAuthResponse.access_token, logObj).Result;
        }

        public string LogToDatabase(string severityCode, Exception exception = null, string info = null)
        {
            var uow = new UnitOfWork(new DBEntities());

            //_publishingSystem = uow.Repository<PublishingSystem>()
            //                       .FirstOrDefault(x => x.Name == _publishingSystem.Name
            //                                            && x.EnvironmentType.Code == _publishingSystem.EnvironmentType.Code) ?? _publishingSystem;

            var model = new Log
            {
                Exception = exception?.GetExceptionMessages(),
                StackTrace = exception?.StackTrace,
                Message = info,
                PublishingSystem = _publishingSystem,
                Timestamp = DateTime.Now,
                Severity = StaticData.Severities.First(x => x.Code == severityCode)
            };

            uow.Repository<Log>().Put(model);

            return TryCommit(uow) ? JsonConvert.SerializeObject(model) : string.Empty;
        }

        public static async Task<PublishingSystem> GetPublishingSystem(
            string publishingSystemName, 
            StaticData.EnvironmentType environmentType, 
            StaticData.SystemType systemType)
        {
            var useWebservice = bool.Parse(ConfigurationManager.AppSettings["UseWebservice"]);

            if (useWebservice)
            {
                var oAuthResponse = GetAccessToken().Result;

                // HTTP GET
                using (var client = new HttpClient())
                {
                    // Setting Authorization
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oAuthResponse.access_token);

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
                        return JsonConvert.DeserializeObject<PublishingSystem>(response.Content.ReadAsStringAsync().Result);
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
                        EnvironmentType = environmentTypeObj, PublishingSystemType = systemTypeObj,
                        Name = publishingSystemName
                    });
                    TryCommit(uow);
                }

                // return the publishing system
                return pSystem;
            }
        }

        private static bool TryCommit(IUnitOfWork uow)
        {
            try
            {
                uow.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
