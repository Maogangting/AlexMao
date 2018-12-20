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
            // Post��ʽ�£�ȡ��client�˴�����������
            if ("post".Equals(context.Request.HttpMethod.ToLower()))
            {
                StreamReader reader = new StreamReader(context.Request.InputStream);
                string json = HttpUtility.UrlDecode(reader.ReadToEnd());// ע�⣬�������Ҫ�����
                string uri = "https://oapi.dingtalk.com/robot/send?access_token=5de05218ebb592ecf7027f07a972cfd89f892231370d64b9d6c5f9052b5bc360";
                //post������  
                string postdata = json; // "{\"msgtype\": \"text\",\"text\": {\"content\": \"ASP -> hello world!\"}}";
                byte[] bytes = Encoding.UTF8.GetBytes(postdata);

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json; charset=UTF-8";
                httpWebRequest.ContentLength = bytes.Length;
                httpWebRequest.Timeout = 20000;
                //request.Headers.Add("Cookie", cookieStr);  // ��ѡ��չ
                
                
                Stream sendStream = httpWebRequest.GetRequestStream();
                sendStream.Write(bytes, 0, bytes.Length);
                sendStream.Close();

                //�õ�����ֵ  
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                string responseContent = streamReader.ReadToEnd();
                // return responseContent;
                //ת����json������  
                //List<GetOrderQuantity> getOrderQuantity = sr.Deserialize<List<GetOrderQuantity>>(OrderQuantity); 
                context.Response.Write(responseContent);
                
                streamReader.Close();
                httpWebResponse.Close();
                httpWebRequest.Abort();
            }
            // Get��ʽ�£�ȡ��client�˴�����������
            else
            {
                // ע�⣬�������Ҫ�����
                //string json = HttpUtility.UrlDecode(context.Request.QueryString.ToString());
                string uri = "http://192.168.0.146";

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);//ʹ��HttpWebRequest���Create��������һ������uri�Ķ��� 
                request.Method = "GET";//ָ������ķ�ʽΪGet��ʽ 
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();//��ȡ����������Ӧ��������Դ����ǿתΪHttpWebResponse��Ӧ���� 
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));//��ȡ����Ӧ����Ŀɶ��� 
                string responseContent = reader.ReadToEnd(); //�����ı���ȡ��ɲ���ֵ��str 
                // return responseContent;
                context.Response.Write(responseContent);
                reader.Close();
                response.Close();
                request.Abort();
            }
        }
    }
}
