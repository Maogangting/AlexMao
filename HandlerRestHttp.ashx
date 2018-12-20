<%@ WebHandler Language="C#" Class="HandlerRestHttp" %>

using System;
using System.Web;
using System.Diagnostics;
using RestSharp;
using System.Threading.Tasks;

/*
 *      RestSharp - Simple .NET REST Client
 *      官方代码示例详见：https://github.com/restsharp/RestSharp
 */

public class HandlerRestHttp : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        // Async Get
        /*        Debug.WriteLine("11");
                Task<string> task = RestGetAsync("http://api.map.baidu.com/geocoder?location=39.990998,116.645966&output=json&key=28bcdd84fae25699606ffad27f8da77b");
                Debug.WriteLine("44");

                context.Response.Write(task.Result);
                Debug.WriteLine("66");
        */
        // Async Post
        Debug.WriteLine("11");
        Task<string> task = RestPostAsync("https://oapi.dingtalk.com/robot/send?access_token=5de05218ebb592ecf7027f07a972cfd89f892231370d64b9d6c5f9052b5bc360",
            "{\"msgtype\": \"text\",\"text\": {\"content\": \"httpclient -> あなたのことが好きです!\"}}");
        Debug.WriteLine("44");

        context.Response.Write(task.Result);
        Debug.WriteLine("66");


        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");

        // RestSharp http sync "GET"
        //var client = new RestSharp.RestClient("http://api.map.baidu.com");
        /*             var client = new RestClient();
        client.BaseUrl = new Uri("http://api.map.baidu.com/geocoder?location=39.990998,116.645966&output=json&key=28bcdd84fae25699606ffad27f8da77b");
                        var requestGet = new RestRequest();
                        //requestGet.AddParameter("location", "39.990998,116.645966");
                        //requestGet.AddParameter("output", "json");
                        //requestGet.AddParameter("key", "28bcdd84fae25699606ffad27f8da77b");
                        IRestResponse response = client.Execute(requestGet);
                    Debug.WriteLine(response.Content);
                        context.Response.Write(response.Content);
                        */
        /*
                        // RestSharp http sync "POST"
                        var client = new RestSharp.RestClient("https://oapi.dingtalk.com");
                        var requestPost = new RestRequest("robot/send?access_token=5de05218ebb592ecf7027f07a972cfd89f892231370d64b9d6c5f9052b5bc360", Method.POST);
                        //requestPost.AddParameter("access_token", "5de05218ebb592ecf7027f07a972cfd89f892231370d64b9d6c5f9052b5bc360");
                        string postdata = "{\"msgtype\": \"text\",\"text\": {\"content\": \"RestSharp -> hello world!\"}}";
                        requestPost.AddParameter("application/json", postdata, ParameterType.RequestBody);
                        IRestResponse responsePost = client.Execute(requestPost);
                        Debug.WriteLine("第三方POST方式获取结果：" + responsePost.Content);
                */
        //http://api.map.baidu.com/geocoder?location=39.990998,116.645966&output=json&key=28bcdd84fae25699606ffad27f8da77b
        // RestSharp http Async "GET"
        //var client = new RestSharp.RestClient("http://api.map.baidu.com");
        //var requestGet = new RestRequest("geocoder", Method.GET);
        //requestGet.AddParameter("location", "39.990998,116.645966");
        //requestGet.AddParameter("output", "json");
        //requestGet.AddParameter("key", "28bcdd84fae25699606ffad27f8da77b");
        ////requestGet.AddUrlSegment("name", "...");  // 添加每级路由段
        //string res = "hello\r\n";
        //await client.ExecuteAsync(requestGet);
        //    , response => {
        //          Debug.WriteLine("第三方GET方式获取结果：" + response.Content);
        //          //context.Response.Write(response.Content);
        //          res = response.Content;
        //      });
        //Debug.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");      // 异步，所以先输出 "$$$$$$$$$$$$$$$$$$$$$$$$$$$$$"

        //context.Response.Write(res);
        /*
                        // RestSharp http Async "POST"
                        var client = new RestSharp.RestClient("https://oapi.dingtalk.com");
                        var requestPost = new RestRequest("robot/send?access_token=5de05218ebb592ecf7027f07a972cfd89f892231370d64b9d6c5f9052b5bc360", Method.POST);
                        //requestPost.AddParameter("access_token", "5de05218ebb592ecf7027f07a972cfd89f892231370d64b9d6c5f9052b5bc360");
                        string postdata = "{\"msgtype\": \"text\",\"text\": {\"content\": \"RestSharp -> hello world!\"}}";
                        requestPost.AddParameter("application/json", postdata, ParameterType.RequestBody);
                        client.ExecuteAsync(requestPost, response => {
                            Debug.WriteLine("第三方GET方式获取结果：" + response.Content);
                        });*/
    }

    private static async Task<string> RestGetAsync(string url)
    {
        //var client = new RestClient("http://api.map.baidu.com");
        //var requestGet = new RestRequest("geocoder", Method.GET);
        //Task<string> res = client.GetAsync(requestGet);
        //    , (response) => {
        //    Debug.WriteLine("第三方GET方式获取结果：" + response.Content);
        //    //context.Response.Write(response.Content);
        //    //    res = response.Content;
        //});
        //requestGet.AddParameter("location", "39.990998,116.645966");
        //requestGet.AddParameter("output", "json");
        //requestGet.AddParameter("key", "28bcdd84fae25699606ffad27f8da77b");
        ////requestGet.AddUrlSegment("name", "...");  // 添加每级路由段
        //string res;
        //await client.ExecuteAsync(requestGet, response => {
        //    Debug.WriteLine("第三方GET方式获取结果：" + response.Content);
        //    //context.Response.Write(response.Content);
        //    res = response.Content;
        //});
        //return res;
        Debug.WriteLine("22");
        var client = new RestClient();
        client.BaseUrl = new Uri(url);
        var request = new RestRequest();
        Debug.WriteLine("33");
        var content = await RestSharpEx.GetContentAsync(client, request).ConfigureAwait(false);
        Debug.WriteLine("55");
        return content;
    }
    // Async post
    private static async Task<string> RestPostAsync(string url, string postbody)
    {
        Debug.WriteLine("22");
        var client = new RestClient();
        client.BaseUrl = new Uri(url);
        var request = new RestRequest(Method.POST);
        Debug.WriteLine("33");
        request.AddParameter("application/json", postbody , ParameterType.RequestBody);
        var content = await RestSharpEx.GetContentAsync(client, request).ConfigureAwait(false);
        Debug.WriteLine("55");
        return content;
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}

// 创建一个返回是 TASK<T>的任务函数，原API没有封装 mgt
public static class RestSharpEx
{
    public static Task<string> GetContentAsync(this RestClient client, IRestRequest request)
    {
        return SelectAsync(client,request, r => r.Content);
    }
    private static Task<T> SelectAsync<T>(this RestClient client, IRestRequest request, Func<IRestResponse, T> selector)
    {
        var tcs = new TaskCompletionSource<T>();
        var loginResponse = client.ExecuteAsync(request, r =>
        {
            if (r.ErrorException == null)
            {
                tcs.SetResult(selector(r));
            }
            else
            {
                tcs.SetException(r.ErrorException);
            }
        });
        return tcs.Task;
    }
}