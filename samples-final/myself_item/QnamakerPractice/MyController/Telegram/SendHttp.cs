using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Model_BotStateSeting_Kick;
using Newtonsoft.Json;

namespace Mycontroller_Telegram_SendHttp
{
    public class SendHttp
    {
        private string file_path = null;
        private string url = @"https://api.telegram.org/bot649665785:AAF3lm9sagdFTtiF4t5p0uBhwr1PhabLSCs/";

        public string getFile(string response)
        {
            string result = "";
            //string s = @"{""file_id"":" + "\"" + response + "\"}"

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url+"getFile");

            req.Method = "POST";

            req.Timeout = 800000;//设置请求超时时间，单位为毫秒

            req.ContentType = "application/json";

            byte[] data = Encoding.UTF8.GetBytes(@"{""file_id"":" + "\"" + response + "\"}");

            req.ContentLength = data.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);

                reqStream.Close();
            }
            HttpWebResponse resp = null;
            try
            { resp = (HttpWebResponse)req.GetResponse(); }
            catch (Exception e)
            { return e.ToString(); }


            Stream stream = resp.GetResponseStream();

            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
        /// <summary>
        /// /////////////////////////////////////////////////
        /// </summary>
        /// <param name="chat_id"></param>
        /// <param name="user_Id"></param>
        /// <returns></returns>
        /// 
        /*
        public static async Task<ResponseData> SetQuizGroup(SetQuizRequest request)
        {
            if (request == null) return null;
            try
            {
                string url = "https://kyc.jarvisplus.io/qna/setQnaGroup";
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync(url, request).ConfigureAwait(false);
                    string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        ResponseData resdata = null;
                        try
                        {
                            resdata = (ResponseData)JsonConvert.DeserializeObject(result, typeof(ResponseData));
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.ToString());
                        }
                        return resdata;

                    }

                    else
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }
        */
        public async Task<string> kickChatMember(int chat_id, int user_Id)
        {            
            //string s = @"{""file_id"":" + "\"" + response + "\"}"
            //string s = @"{""chat_id"":" + "\"" + chat_id + "\"," + @"""user_id"":" + "\"" + user_Id + "\"}";
            Kick kick = new Kick();
            kick.chat_id = chat_id;
            kick.user_id = user_Id;
           // HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url + "kickChatMember");           
            try
            {
                //string url = "https://kyc.jarvisplus.io/qna/setQnaGroup";
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync(url + "kickChatMember", kick).ConfigureAwait(false);
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        return true.ToString();

                    }

                    else
                        return false.ToString();
                }
            }
            catch(Exception e)
            {
                return e.ToString();
            }

        }


        /*


        public string kickChatMember(int chat_id,int user_Id)
        {
            string result = "";
            //string s = @"{""file_id"":" + "\"" + response + "\"}"
            string s = @"{""chat_id"":" + "\"" + chat_id + "\"," + @"""user_id"":" + "\"" + user_Id + "\"}";
            Kick kick = new Kick();
            kick.chat_id = chat_id;
            kick.user_id = user_Id;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url + "kickChatMember");

            req.Method = "POST";

            req.Timeout = 800000;//设置请求超时时间，单位为毫秒

            req.ContentType = "application/json";

            byte[] data = Encoding.UTF8.GetBytes(s);

            req.ContentLength = data.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);

                reqStream.Close();
            }
            HttpWebResponse resp = null;
            try
            { resp = (HttpWebResponse)req.GetResponse(); }
            catch (Exception e)
            { return e.ToString()+s; }


            Stream stream = resp.GetResponseStream();

            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            return result.ToString();
        }

    */
    }

}
