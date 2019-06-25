using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using PublishKB_Controller;

// NOTE: Install the Newtonsoft.Json NuGet package.
using Newtonsoft.Json;
using Controllertable_Controller;
using Model_UpdateTable;

namespace Update_Controller
{
    public class Update
    {
        static string host = "https://westus.api.cognitive.microsoft.com";
        static string service = "/qnamaker/v4.0";
        static string method = "/knowledgebases/";

        // NOTE: Replace this with a valid subscription key.
        static string key="24a24cfe74e24d0db0dbe38a649a423b";

        public struct Response
        {
            public HttpResponseHeaders headers;
            public string response;

            public Response(HttpResponseHeaders headers,string response)
            {
                this.headers = headers;
                this.response = response;
            }
        }

        static string PrettyPrint(string s)

        {
            PublishKB.Publish();
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(s),Formatting.Indented);
        }

        async static Task<Response> Patch(string uri, string body)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = new HttpMethod("PATCH");
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(body,Encoding.UTF8,"application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key",key);

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                PublishKB.Publish();
                return new Response(response.Headers,responseBody);

            }
        }

        async static Task<Response> Get(string uri)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(uri);
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                return new Response(response.Headers, responseBody);
               
            }
        }

        async static Task<Response> PostUpdateKB(string kb,string new_kb)
        {
            string uri = host + service + method + kb;
            Console.WriteLine("Calling "+uri+".");
            PublishKB.Publish();
            return await Patch(uri,new_kb);

        }

        async static Task<Response> GetStatus(string operation)
        {
            string uri = host+service+operation;
            Console.WriteLine("Calling "+uri+".");
            PublishKB.Publish();
            return await Get(uri);
        }

        public async static void UpdateKB(string kb,string new_kb)
        {
            //if updatetable has't data then add it;
            UpdateTable updatetable = JsonConvert.DeserializeObject<UpdateTable>(new_kb);
            bool acomplish = await CotrollerTable.Query(updatetable.add.qnaList[0].questions[0],
                updatetable.add.qnaList[0].answer);
            if (acomplish)
            {
                CotrollerTable.Insert(updatetable.add.qnaList[0].questions[0],
                    updatetable.add.qnaList[0].answer,updatetable.update.name );
            }  

            var response = await PostUpdateKB(kb, new_kb);
            var operation = response.headers.GetValues("Location").First();
            Console.WriteLine(PrettyPrint(response.response));

            var done = false;
            while (true != done)
            {
                response = await GetStatus(operation);
                Console.WriteLine(PrettyPrint(response.response));

                var fields = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.response);

                String state = fields["operationState"];
                if (state.CompareTo("Running")==0||state.CompareTo("NotStarted")==0)
                {
                    PublishKB.Publish();
                    var wait = response.headers.GetValues("Retry-After").First();
                    Console.WriteLine("Waiting "+wait+" seconds...");
                    Thread.Sleep(Int32.Parse(wait) * 1000);
                    PublishKB.Publish();
                }
                else
                {
                    Console.WriteLine("Press any key to continue.");
                    done = true;
                }
            }
        }
    }
}
