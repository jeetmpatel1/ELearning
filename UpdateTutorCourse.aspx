<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateTutorCourse.aspx.cs" Inherits="Elearning.UpdateTutorCourse" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
   <form id="form1" runat="server">
    <div>
     <asp:Button ID="btnUserAccount" runat="server" PostBackUrl="~/UserAccount.aspx" 
        style="z-index: 1; left: 104px; top: 15px; position: absolute" 
        Text="User Account" />
     
     <asp:Label ID="lblRegister" runat="server" 
            style="z-index: 1; left: 102px; top: 56px; position: absolute" 
            Text="Update Tutor Course" Font-Bold="True" Font-Size="Larger" 
            Font-Underline="True"></asp:Label>
     
     <asp:Label ID="lblCourse" runat="server" 
        style="z-index: 1; left: 106px; top: 103px; position: absolute" 
            Text="Current Course:"></asp:Label>
    <asp:TextBox ID="txtCourse" runat="server" ReadOnly="True" 
        style="z-index: 1; left: 224px; top: 102px; position: absolute"></asp:TextBox>
     
     <asp:Label ID="lblSwitchToCourse" runat="server" 
        style="z-index: 1; left: 106px; top: 136px; position: absolute" 
        Text="Switch to Course:"></asp:Label>       
     
    <asp:ListBox ID="lstCourses" runat="server"  
        
            
            style="z-index: 1; left: 106px; top: 165px; position: absolute; height: 72px; width: 180px">
    </asp:ListBox>
       
    
   
    <asp:Button ID="btnUpdateCourse" runat="server" onclick="btnUpdateCourse_Click" 
        style="z-index: 1; left: 106px; top: 254px; position: absolute" 
        Text="Update Course" />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" 
        style="z-index: 1; left: 106px; top: 294px; position: absolute"></asp:Label>
        
         </div>
    </form>
</body>
</html>
