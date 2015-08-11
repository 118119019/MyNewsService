<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArticleDetail.aspx.cs" Inherits="WebApplication2.News.ArticleDetail" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="applicable-device" content="mobile" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <link href="../Content/main.css?v=20150811" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.11.0.min.js"></script>
    <script src="../Scripts/site.js?v=20150811"></script>
   <%--  <%= newsItem.Title %> --%>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <article id="content_show" class="detail">
            <h1><%= newsItem.Title %></h1>
            <div class="userMsg"><%= newsItem.Author  %>  <%= newsItem.CreateTime.ToString() %> | 来源：<%= newsItem.FromSite %> </div>
            <div class="detail_content">
                <%= newsItem.NewsContent %>
            </div>
            <!--share-->
            <a class="button_share" style="display: none;">
                <i class="icons icon_share"></i>
                分享
            </a>
        </article>
        <i id="gotop" class="icons">回到顶部</i>
    </form>
</body>
</html>
