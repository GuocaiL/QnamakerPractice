using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Model_AnswerType;
using Newtonsoft.Json;

namespace GetAnswer_Controller
{
    class GetAnswers
    {
        // NOTE: Replace this with a valid host name.
        static string host = "https://qnaserviceone.azurewebsites.net";

        // NOTE: Replace this with a valid endpoint key.
        // This is not your subscription key.
        // To get your endpoint keys, call the GET /endpointkeys method.
        static string endpoint_key = "38c1961e-d58d-4d16-9f7c-825923124808";

        // NOTE: Replace this with a valid knowledge base ID.
        // Make sure you have published the knowledge base with the
        // POST /knowledgebases/{knowledge base ID} method.
        static string kb = "0586beac-69d9-4dc9-8f7f-756707fa1bee";

        static string service = "/qnamaker";
        static string method = "/knowledgebases/" + kb + "/generateAnswer/";


        async static Task<string> Post(string uri,string body)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                request.Headers.Add("Authorization","EndpointKey " + endpoint_key);

                var response = await client.SendAsync(request);
                return await response.Content.ReadAsStringAsync();
                //return strResponse;
            }
        }

        //The method be uesed to get anwser request and return result;
        public  static async Task<string> GetAnswerAsync(string question)
        {
            
                var uri = host + service + method;         
                Console.WriteLine("Calling " + uri + ".");
                var response = await Post(uri, question);              
                string str = response.ToString();
                Rootobject  answer = JsonConvert.DeserializeObject<Rootobject>(str);                
                return answer.answers[0].answer ;            
        }
    }
}
