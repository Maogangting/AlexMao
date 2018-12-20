<%@ WebHandler Language="C#" Class="HandlerFlurlHttp" %>

using System;
using System.Web;
using System.Diagnostics;
using Flurl;
using Flurl.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class HandlerFlurlHttp : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
        Newtonsoft.Json.Linq.JObject jo1 = Newtonsoft.Json.Linq.JObject.Parse("{\"msgtype\": \"text\",\"text\": {\"content\": \"httpclient -> あなたのことが好きです!\"}}");
        Debug.WriteLine("11");
        Task<string> res = FlurlPostAsync("https://oapi.dingtalk.com/robot/send?access_token=5de05218ebb592ecf7027f07a972cfd89f892231370d64b9d6c5f9052b5bc360",
            jo1);
        Debug.WriteLine("33");
        Debug.WriteLine("55");
        Debug.WriteLine("66");
        context.Response.Write(res.Result);
        Debug.WriteLine("77");
        /*
                Task<string> res = FlurlGetAsync("http://api.map.baidu.com/geocoder?location=39.990998,116.645966&output=json&key=28bcdd84fae25699606ffad27f8da77b");
                Debug.WriteLine("hello world");
                context.Response.Write(res.Result);
                Debug.WriteLine("hello world22");
        */
    }

private async Task<string> FlurlGetAsync(string url)
{
    // "GET"
    var responseString = await url.GetStringAsync().ConfigureAwait(false);
    Debug.WriteLine("hello world333");
    Debug.WriteLine("第三方GET方式获取结果：" + responseString);
    return responseString;

}

private async Task<string> FlurlPostAsync(string url, object postData)
{
        // Json "POST"
        Debug.WriteLine("22");
    var responseString = await url.PostJsonAsync(postData).ReceiveString().ConfigureAwait(false);
    Debug.WriteLine("44");
    return responseString;
}

public bool IsReusable {
    get {
        return false;
    }
}

}