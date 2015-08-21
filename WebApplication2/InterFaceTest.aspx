<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InterFaceTest.aspx.cs" Inherits="WebApplication2.InterFaceTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnTest" OnClick="btnTest_Click" runat="server" Text="测试" />
        </div>
        <div>
            <label>搜索：</label>
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            <asp:Button ID="btnQuery" OnClick="btnQuery_Click" Text="搜索" runat="server" />
        </div>
        <div class="table-box2">
            <table width="100%" class="table-2" id="tablesort">
                <thead>
                    <tr>
                        <th style="width: 10%">标题</th>
                        <th style="width: 10%">来源地址</th>
                        <th style="width: 45%">内容</th>
                        <th style="width: 10%">创建时间</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptList1" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Eval("Title") %>
                                </td>
                                <td>
                                    <%#Eval("SourceUrl") %>
                                </td>
                                <td>
                                    <%#Eval("NewsText") %>                                   
                                </td>
                                <td>
                                    <%#Eval("CreateTime") %>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>

    </form>
</body>
</html>
