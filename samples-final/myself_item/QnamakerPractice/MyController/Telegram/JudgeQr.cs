using System;
using System.Drawing;
using ZXing;
using System.IO;
using System.Net;

namespace Mycontroller_Telegram_JudgeQr
{
    public class judgeqr
    {                
        public string CodeDecoder(string path)
        {
            Bitmap bitMap;
            MemoryStream ms;
            WebClient Client = new WebClient();
            string url = @"https://api.telegram.org/file/bot649665785:AAF3lm9sagdFTtiF4t5p0uBhwr1PhabLSCs/";
            url = url + path;
            try
            {
                System.Net.WebRequest request =
                    System.Net.WebRequest.Create(
                    url);
                System.Net.WebResponse response = request.GetResponse();
                System.IO.Stream responseStream =
                    response.GetResponseStream();
                bitMap = new Bitmap(responseStream);
                
                ms = new MemoryStream();//实例化内存流

                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);//把位图信息保存到内存流里面

                byte[] bytes = ms.GetBuffer();//把颜色信息转化为byte数据
                
                LuminanceSource source = new RGBLuminanceSource(bytes, bitMap.Width, bitMap.Height);//得到位图的像素数值内容

                BinaryBitmap bb = new BinaryBitmap(new ZXing.Common.HybridBinarizer(source));//处理像素值内容信息
                
                MultiFormatReader mutireader = new ZXing.MultiFormatReader();//实例化MultiFormatReader

                Result str = mutireader.decode(bb);//通过mutireader.decode（）得到解析后的结果

                ms.Close();//关闭内存流

                if (str == null)
                    return null;
                else
                    return str.Text;//返回解析结果
                
            }
            catch (System.Net.WebException e)
            {
                return e.ToString();
            }
            
        }

    }
}
