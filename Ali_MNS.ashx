<%@ WebHandler Language="C#" Class="Ali_MNS" %>

using System;
using System.Web;

public class Ali_MNS : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Write("Hello World");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}