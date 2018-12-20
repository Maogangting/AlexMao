using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Web.WebSockets;

public class ValuesController1 : ApiController
{
    // GET api/<controller>
   /* public HttpResponseMessage Get()
    {
        if (HttpContext.Current.IsWebSocketRequest)
        {
            HttpContext.Current.AcceptWebSocketRequest(ProcessWSChat);
        }
        return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
    }*/
    private async Task ProcessWSChat(AspNetWebSocketContext arg)
    {
        WebSocket socket = arg.WebSocket;
        while (true)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
            WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
            if (socket.State == WebSocketState.Open)
            {
                string message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                string returnMessage = "You send :" + message + ". at" + DateTime.Now.ToLongTimeString();
                buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(returnMessage));
                await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {
                break;
            }
        }
    }
    // GET api/<controller>/5
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<controller>
    public void Post([FromBody]string value)
    {
    }

    // PUT api/<controller>/5
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/<controller>/5
    public void Delete(int id)
    {
    }
}