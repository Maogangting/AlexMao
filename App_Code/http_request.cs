using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using System.IO;
// using RestSharp;

namespace MyHandlers
{
    public class MyJsonHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            // Post方式下，取得client端传过来的数据
            if ("post".Equals(context.Request.HttpMethod.ToLower()))
            {
                StreamReader reader = new StreamReader(context.Request.InputStream);
                string json = HttpUtility.UrlDecode(reader.ReadToEnd());// 注意，这个是需要解码的
                string uri = "https://oapi.dingtalk.com/robot/send?access_token=5de05218ebb592ecf7027f07a972cfd89f892231370d64b9d6c5f9052b5bc360";
                //post传参数  
                string postdata = json; // "{\"msgtype\": \"text\",\"text\": {\"content\": \"ASP -> hello world!\"}}";
                byte[] bytes = Encoding.UTF8.GetBytes(postdata);

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json; charset=UTF-8";
                httpWebRequest.ContentLength = bytes.Length;
                httpWebRequest.Timeout = 20000;
                //request.Headers.Add("Cookie", cookieStr);  // 可选扩展
                
                
                Stream sendStream = httpWebRequest.GetRequestStream();
                sendStream.Write(bytes, 0, bytes.Length);
                sendStream.Close();

                //得到返回值  
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                string responseContent = streamReader.ReadToEnd();
                // return responseContent;
                //转化成json对象处理  
                //List<GetOrderQuantity> getOrderQuantity = sr.Deserialize<List<GetOrderQuantity>>(OrderQuantity); 
                context.Response.Write(responseContent);
                
                streamReader.Close();
                httpWebResponse.Close();
                httpWebRequest.Abort();
            }
            // Get方式下，取得client端传过来的数据
            else
            {
                // 注意，这个是需要解码的
                //string json = HttpUtility.UrlDecode(context.Request.QueryString.ToString());
                string uri = "http://192.168.0.146";

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);//使用HttpWebRequest类的Create方法创建一个请求到uri的对象。 
                request.Method = "GET";//指定请求的方式为Get方式 
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();//获取该请求所响应回来的资源，并强转为HttpWebResponse响应对象 
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));//获取该响应对象的可读流 
                string responseContent = reader.ReadToEnd(); //将流文本读取完成并赋值给str 
                // return responseContent;
                context.Response.Write(responseContent);
                reader.Close();
                response.Close();
                request.Abort();
            }
        }
    }
}
