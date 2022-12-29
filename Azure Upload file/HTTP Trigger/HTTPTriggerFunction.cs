using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Trigger
{
    internal class User
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public static class HTTPTriggerFunction
    {
        [FunctionName("HTTPTriggerFunction")]

        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            Console.Write("\n Enter username:  ");

            string username = Console.ReadLine();

            Console.Write("\n Enter password:  ");

            string password = Console.ReadLine();

            Console.Write("\n \n");


            using (var httpClient = new HttpClient())
            {

                User user = new User()
                {
                    username= username,
                    password = username
                };

                string apiuser_jsondata = JsonConvert.SerializeObject(user, Formatting.Indented);

                StringContent httpContent = new StringContent(apiuser_jsondata, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://localhost:7076/api/Authenticate/login", httpContent);

                string? responsedata = string.Empty;
                string? responseuser_ = string.Empty;


                string combinedString = string.Empty;

                if (response.IsSuccessStatusCode)
                {

                    var token = await response.Content.ReadAsStringAsync();

                    JObject jObject = JObject.Parse(token);


                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jObject["token"].ToString());

                    //Console.WriteLine(jObject["token"].ToString());



                    responsedata = await httpClient.GetStringAsync($"https://localhost:7076/api/Authenticate/getusers");

                    var UserAccounts = JsonConvert.DeserializeObject<ObservableCollection<User>>(responsedata);

                    var jsonoption = new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true };


                    if (!UserAccounts.Any(u => u.username == "Admin_1234" && u.password == "Admin_1234"))
                    {



                        string requestBody = await httpClient.GetStringAsync($"https://localhost:7076/UploadFile/GetFile");
                        var data = JsonConvert.DeserializeObject<List<dynamic>>(requestBody);

                        List<string> list = new List<string>();

                        string responseMessage = string.Empty;

                        string name = req.Query["idFile"];




                        if (string.IsNullOrWhiteSpace(req.Query["idFile"]))
                        {


                            foreach (var item in data)
                            {

                                string fileName = item.fileName;
                                string filePath = item.filePath;
                                string fileSize = item.fileSize;


                                if (fileName.Length > 5)
                                {
                                    //shorten string
                                    fileName = fileName.Substring(0, 5) + "... ." + fileName.Substring(fileName.LastIndexOf('.') + 1);
                                }
                                else
                                {
                                    fileName = fileName + "    ";
                                }


                                responseMessage = $"\n {fileName}\t\t {filePath}  \t  {fileSize}";
                                list.Add(responseMessage);
                            }



                            combinedString = " \n" + "\n ==================================================================================================================================================================================================== \n" + string.Join(" \n \n", list.ToArray());

                        }

                        else
                        {

                            foreach (var item in data)
                            {
                                string idFile = item.idFile.ToString();

                                string fileName = item.fileName;

                                if (idFile == name)
                                {

                                    name = item.fileName;

                                }


                            }

                            combinedString = name;
                        }
                    }


                }
                return new OkObjectResult(combinedString.ToString());
            }
        }
    }
}

