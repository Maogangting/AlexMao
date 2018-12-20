<%@ WebHandler Language="C#" Class="CodeToDevice" %>

using System;
using System.Web;
using System.Text;
using System.Diagnostics;

using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Iot.Model.V20170420;

public class CodeToDevice : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
        // ALi Demo (远程控制设备)
        // 设置1，区域，2,acessKey, 3,密钥
        IClientProfile clientProfile = DefaultProfile.GetProfile("cn-shanghai", "LTAIH4yk1x6vIYVX", "ylaz2q3Z5x4E9leb40DI83mtB2yOE9");
        DefaultAcsClient client = new DefaultAcsClient(clientProfile);
        
        PubRequest request = new PubRequest();
        request.ProductKey = "a1JoWeN3xAo";
        request.TopicFullName = "/" + request.ProductKey + "/temp1/data";
        byte[] payload = Encoding.Default.GetBytes("{\"devId\":\"temp1\",\"param\":{\"cmd\":\"REQUEST\"},\"systime\":\"20180708061705\"} ");
        string payloadStr = Convert.ToBase64String(payload);
        request.MessageContent = payloadStr;
        request.Qos = 0;
        try
        {
            PubResponse response = client.GetAcsResponse(request);
            Debug.WriteLine("publish message result: " + response.Success);
            Debug.WriteLine(response.ErrorMessage);
        }
        catch (ServerException e)
        {
            Debug.WriteLine(e.ErrorCode);
            Debug.WriteLine(e.ErrorMessage);
        }
        catch (ClientException e)
        {
            Debug.WriteLine(e.ErrorCode);
            Debug.WriteLine(e.ErrorMessage);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}