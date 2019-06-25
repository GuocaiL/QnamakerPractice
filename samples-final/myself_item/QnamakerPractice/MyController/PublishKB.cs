using System;
using System.Threading.Tasks;
using System.Net.Http;

// NOTE: Install the Newtonsoft.Json NuGet package.
using Newtonsoft.Json;

namespace PublishKB_Controller
{
    public class PublishKB
    {
        static string host = "https://westus.api.cognitive.microsoft.com";
        static string service = "/qnamaker/v4.0";
        static string method = "/knowledgebases/";

        // NOTE: Replace this with a valid subscription key.
        static string key = "24a24cfe74e24d0db0dbe38a649a423b";

        // NOTE: Replace this with a valid knowledge base ID.
        static string kb = "0586beac-69d9-4dc9-8f7f-756707fa1bee";

        static string PrettyPrint(string s)
        {
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(s), Formatting.Indented);
        }

        async static Task<string> Post(string uri)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return "{'result' : 'Success.'}";
                }
                else
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
   
        public async static void Publish()
        {
            var uri = host + service + method + kb;
            Console.WriteLine("Calling " + uri + ".");
            var response = await Post(uri);
            Console.WriteLine(PrettyPrint(response));
            Console.WriteLine("PUBLISHKB");
        }
    }
}
