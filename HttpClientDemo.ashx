<%@ WebHandler Language="C#" Class="HttpClientDemo" %>

using System;
using System.Web;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

public class HttpClientDemo : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        // sync Get
        /*        string _address = "http://api.worldbank.org/countries?format=json";
                // Create an HttpClient instance  
                HttpClient client = new HttpClient();
                //远程获取数据
                var task = client.GetAsync(_address);
                var rep = task.Result;//在这里会等待task返回。
                //读取响应内容
                var task2 = rep.Content.ReadAsStringAsync();
                var ret = task2.Result;//在这里会等待task2返回。
                Debug.WriteLine(ret);
        */

        // async get
               Debug.WriteLine("11");
               Task<string> task = httpGetAsync("http://api.map.baidu.com/geocoder?location=39.990998,116.645966&output=json&key=28bcdd84fae25699606ffad27f8da77b");
               Debug.WriteLine("44");

               context.Response.Write(task.Result);
               Debug.WriteLine("55");
       
        // Async Post
/*        Debug.WriteLine("11");
        Task<string> task = HttpPostAsync("https://oapi.dingtalk.com/robot/send?access_token=5de05218ebb592ecf7027f07a972cfd89f892231370d64b9d6c5f9052b5bc360",
            "{\"msgtype\": \"text\",\"text\": {\"content\": \"httpclient -> あなたのことが好きです!\"}}");
        Debug.WriteLine("44");
        context.Response.Write(task.Result);
        Debug.WriteLine("55");
*/
    }

    private static async Task<string> httpGetAsync(string url)
    {
        HttpClient client = new HttpClient();
        Debug.WriteLine("22");
        string body = await client.GetStringAsync(url).ConfigureAwait(false);
        Debug.WriteLine("33");
        return body;
    }

    private static async Task<string> HttpPostAsync(string url, string postData)
    {
        HttpContent httpContent = new StringContent(postData);
        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        httpContent.Headers.ContentType.CharSet = "utf-8";

        HttpClient client = new HttpClient();

        Debug.WriteLine("22");
        HttpResponseMessage response = await client.PostAsync(url, httpContent).ConfigureAwait(false);
        Debug.WriteLine("33");
        string result = await response.Content.ReadAsStringAsync();
        Debug.WriteLine("66");
        return result;
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}