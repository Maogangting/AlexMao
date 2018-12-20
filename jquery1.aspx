<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jquery1.aspx.cs" Inherits="jquery1" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>jQuery选项卡标签控制图片滚动切换代码</title>

    <link rel="stylesheet" type="text/css" href="css/style.css" />

    <script src="js/jquery.min.js" type="text/javascript"></script>

</head>
<body>
    <script src="/demos/googlegg.js"></script>

    <div class="xcx_qingqiao">
        <div class="container">
            <div class="xcx_qingqiao_title"><p><span>商城小程序，</span>轻巧的功能依旧强大</p></div>
            <div class="xcx_what_text">是你的第一款微信小程序，具备云起微商城的相同'颜值'商品展示、购买下单、微信<br>支付...同一后台统一管理，抢占第一波小程序红利！</div>
            <div class="xcx_dowhat_main">
                <div class="xcx_phone">
                    <div class="xcx_phone_main">
                        <ul style="">
                            <li><img src="images/xcx/pic_1.jpg"></li>
                            <li><img src="images/xcx/pic_2.jpg"></li>
                            <li><img src="images/xcx/pic_3.jpg"></li>
                            <li><img src="images/xcx/pic_4.jpg"></li>
                            <li><img src="images/xcx/pic_5.jpg"></li>
                            <li><img src="images/xcx/pic_6.jpg"></li>
                        </ul>
                    </div>
                </div>
                <div class="xcx_dowhat_list">
                    <ul>
                        <li class="li_left li_1 current"><div class="title">店铺首页</div><div class="pic"></div></li>
                        <li class="li_left li_2"><div class="title">商品展示</div><div class="pic"></div></li>
                        <li class="li_left li_3"><div class="title">满减、送券</div><div class="pic"></div></li>
                        <li class="li_right li_4"><div class="title">购物车</div><div class="pic"></div></li>
                        <li class="li_right li_5"><div class="pic"></div><div class="title">微信支付</div><div class="info">（需绑定微信服务号，没有服务号请自行申请，或使用小程序打包服务）</div></li>
                        <li class="li_right li_6"><div class="title">订单查询</div><div class="pic"></div></li>
                    </ul>
                </div>
            </div>

        </div>
    </div>

    <script type="text/javascript">
    $(document).ready(function () {
        $(".xcx_dowhat_list ul li").hover(function () {
            $(".xcx_dowhat_list ul li").removeClass("current");
            $i = $(this).index();
            $(this).addClass("current");
            top1 = -$i * 364;
            $(".xcx_phone_main ul").stop(true, false).animate({"top": top1});
        });
    });
    </script>

    <div style="text-align:center;margin:50px 0; font:normal 14px/24px 'MicroSoft YaHei';">
        <p>适用浏览器：IE8、360、FireFox、Chrome、Safari、Opera、傲游、搜狗、世界之窗.</p>
    </div>
</body>
</html>
