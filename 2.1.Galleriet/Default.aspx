<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_2._1.Galleriet.Default" ViewStateMode="Disabled"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <link href="~/Css/basic.css" rel="stylesheet" type="text/css" />
        <title>Galleriet</title>
    </head>
    <body>
        <div id="presentation">
            <h1>Galleriet</h1>
            <form id="form1" runat="server">

                <%-- Meddelande om korrekt filuppladdning --%>
                <div id="success" class="hidden">
                    <asp:Label ID="successLabel" runat="server" Text="Bilden har sparats."></asp:Label>
                </div>

                <%-- Sidavdelning för visning av thumbnails och valt foto --%>
                <div id="photoDisplayZone">
                    <asp:DetailsView ID="PhotoCloseUp" runat="server" Height="50px" Width="125px"></asp:DetailsView>
                    <asp:Repeater ID="ThumbnailRepeater" runat="server">
                        <HeaderTemplate>
                            <ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <%-- <asp:HyperLink ID="ThumbnailImageHyperLink" runat="server">--%>
                                    <asp:Image ID="ThumbnailImage" runat="server" ImageUrl='<%# "~/Content/images/thumbnails/" + Item.Name %>' AlternateText="Photo" />
                                <%--</asp:HyperLink>--%>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>

                <%-- Sidavdelning för filuppladdning --%>
                <div id="uploadZone">
                    <asp:Label ID="UploadLabel" runat="server" Text="Ladda upp bild:"></asp:Label><br />

                    <%-- Område för felmeddelande --%>
                    <div id="errorZone" class="hidden">
                        <asp:Label ID="errorLabel" runat="server" Text="Ett fel inträffade. Korrigera felet och försök igen!"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                    </div>
                    <asp:FileUpload ID="ChooseImage" runat="server" /><br />
                    <asp:RequiredFieldValidator ID="ImageUrlRequiredValidator" runat="server" ErrorMessage="Du måste välja en fil att ladda upp." ControlToValidate="ChooseImage" Display="None"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="ValidFileTypeValidator" runat="server" ErrorMessage="Filen måste vara av typen .jpg, .jpeg, .gif eller .png." ControlToValidate="ChooseImage" Display="None" ValidationExpression="(?i)\.(jpg|jpeg|png|gif)$"></asp:RegularExpressionValidator>
                    <asp:Button ID="UploadPhoto" runat="server" Text="Ladda upp" OnClick="UploadPhoto_Click" />
                </div>
            </form>
        </div>
    </body>
</html>
