<%@ Application Language="C#" %>

<%@ import Namespace="System.IO" %>
<%@ import Namespace="System.Timers" %>
<%@ import Namespace="System.Diagnostics" %>
<%@ import Namespace="MgtNetLog" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        // 在应用程序启动时运行的代码
        Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " : Application_Start");

        System.Timers.Timer aTimer = new System.Timers.Timer();
        aTimer.Elapsed += new ElapsedEventHandler(TimeEvent);
        // 设置引发时间的时间间隔 此处设置为１秒 
        aTimer.Interval = 60000;
        aTimer.Enabled = true;

    }

    void Application_End(object sender, EventArgs e)
    {
        //  在应用程序关闭时运行的代码
        Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " : Application_End");

        System.Threading.Thread.Sleep(5000);
        string strUrl = "http://192.168.0.47/Handler.ashx";
        System.Net.HttpWebRequest _HttpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strUrl);
        System.Net.HttpWebResponse _HttpWebResponse = (System.Net.HttpWebResponse)_HttpWebRequest.GetResponse();
        System.IO.Stream _Stream = _HttpWebResponse.GetResponseStream();//得到回写的字节流 
    }

    private void TimeEvent(object source, ElapsedEventArgs e)
    {/*
        // 得到 hour minute second 如果等于某个值就开始执行 
        int intHour = e.SignalTime.Hour;
        int intMinute = e.SignalTime.Minute;
        int intSecond = e.SignalTime.Second;
        // 定制时间,在00：00：00 的时候执行 
        int iHour = 11;
        int iMinute = 02;
        int iSecond = 00;

        // 设置 每天的00：00：00开始执行程序 
        if (intHour == iHour && intMinute == iMinute && intSecond == iSecond)
        {
            //发送邮件程序                
        }
    */
        Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " : time callback");
        NetLog.WriteTextLog("timer", "timerCallBack", DateTime.Now);
    }

    void Application_Error(object sender, EventArgs e)
    {
        // 在出现未处理的错误时运行的代码

    }

    void Session_Start(object sender, EventArgs e)
    {
        // 在新会话启动时运行的代码

    }

    void Session_End(object sender, EventArgs e)
    {
        // 在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer
        // 或 SQLServer，则不引发该事件。

    }

</script>
