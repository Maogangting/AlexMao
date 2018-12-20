<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebSocketDemo.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0"/>
    <title></title>
    <script src="http://code.jquery.com/jquery-1.4.1.min.js"></script>
    <script src="https://unpkg.com/flyio/dist/fly.min.js"></script>
    <script>
        var fly = new Fly();
        var ws;
        $().ready(function () {
            $('#conn').click(function () {
                ws = new WebSocket('ws://' + window.location.hostname + ':' + window.location.port + '/Handler.ashx?user=' + $("#user").val());
                //ws = new WebSocket('ws://192.168.3.3:8086/Handler1.ashx?user=' + $("#user").val());
                //var host = 'ws://192.168.85.128:8085/api/WSChat?user='+$("#user").val();
                //var host = "ws://192.168.85.128:8085/api/WSChat";
                //webSocket = new WebSocket(host);

                $('#msg').append('<p>正在连接</p>');

                ws.onopen = function () {
                    $('#msg').append('<p>已经连接</p>');
                }
                ws.onmessage = function (evt) {
                    $('#msg').append('<p>' + evt.data + '</p>');
                }
                ws.onerror = function (evt) {
                    $('#msg').append('<p>' + JSON.stringify(evt) + '</p>');
                }
                ws.onclose = function () {
                    $('#msg').append('<p>已经关闭</p>');
                }
            });

            $('#close').click(function () {
                ws.close();
            });

            $('#send').click(function () {
                if (ws.readyState == WebSocket.OPEN) {
                    ws.send($("#to").val() + "|" + $('#content').val());
                }
                else {
                    $('#tips').text('连接已经关闭');
                }
            });
            $('#flybtn').click(function () {
                // get
                /*
                fly.get('http://192.168.0.39/WebSocketDemo.aspx')
                .then(function (response) {
                    $('#tips').text('hi...' + response.data);
                })*/
                // post
                fly.request("http://192.168.0.39/HttpClientDemo.ashx",
                    {"msgtype": "text","text": {"content": "httpclient -> あなたのことが好きです!"}}, {
                    method: "post",
                    timeout: 5000 //超时设置为5s
                })
                .then(d=> { console.log("request result:", d) })
                .catch((e) => console.log("error", e))
            });
        });
    </script>
</head>
<body>
    <div>
        <input id="user" type="text" />
        <input id="conn" type="button" value="连接" />
        <input id="close" type="button"  value="关闭"/>
        &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
        <input id="flybtn" type="button" value="Fly Test"/><br />
        
        <input id="content" type="text" />
        <input id="send" type="button"  value="发送"/><br />
        <input id="to" type="text" />:目的用户 <br />
        <span id="tips"></span>
        <div id="msg">
        </div>
    </div>
</body>
</html>