using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
// NOTE: Install the Newtonsoft.Json NuGet package.
using Newtonsoft.Json;

namespace Mycontroller_GetFileId
{
    public class GetFileId
    {
        private string file_path = null;
        private string filejsonpart1 = @"{""file_id"":"; 
        private string filejsonpart2="\"}";
        private string url = @"https://api.telegram.org/bot649665785:AAF3lm9sagdFTtiF4t5p0uBhwr1PhabLSCs/getFile";
        
        public string PostUrl(string response)
        {
            string result = "";
            //string s = @"{""file_id"":" + "\"" + response + "\"}";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            req.Method = "POST";

            req.Timeout = 80000;//设置请求超时时间，单位为毫秒

            req.ContentType = "application/json";

            byte[] data = Encoding.UTF8.GetBytes(@"{""file_id"":" + "\"" +response+ "\"}");

            req.ContentLength = data.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);

                reqStream.Close();
            }
            HttpWebResponse resp = null ;
            try
            { resp = (HttpWebResponse)req.GetResponse(); } catch (Exception e)
            { return e.ToString(); }
            

            Stream stream = resp.GetResponseStream();

            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

    }

}
