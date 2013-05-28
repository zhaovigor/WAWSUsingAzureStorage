<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WAWSUsingAzureStorage.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:FileUpload ID="FileUploadCtrl" runat="server" />
        <asp:Button ID="BtnUpload" runat="server" OnClick="BtnUpload_Click" Text="Upload" />
    </div>
        <div>

            <asp:Literal ID="BlobsLiteral" runat="server"></asp:Literal>

        </div>
        <div>
            <a href="WAWSUsingAzureStorage.zip" title="SourceCode">SourceCode</a>
        </div>
    </form>
</body>
</html>
