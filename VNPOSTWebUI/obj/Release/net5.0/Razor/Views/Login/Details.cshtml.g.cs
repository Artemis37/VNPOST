#pragma checksum "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8c8a5590d7c9cac00aa7f6f45f17c5faa858e44f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Login_Details), @"mvc.1.0.view", @"/Views/Login/Details.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\_ViewImports.cshtml"
using VNPOSTWebUI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\_ViewImports.cshtml"
using VNPOSTWebUI.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8c8a5590d7c9cac00aa7f6f45f17c5faa858e44f", @"/Views/Login/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2bd25cc622ebf4ed25b205a6d3ee0806c0243058", @"/Views/_ViewImports.cshtml")]
    public class Views_Login_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Microsoft.AspNetCore.Identity.IdentityUser>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
  
    ViewData["Title"] = "Details";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Details</h1>\r\n<table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
#nullable restore
#line 12 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
           Write(Html.DisplayNameFor(model => model.UserName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 15 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
           Write(Html.DisplayNameFor(model => model.Email));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 18 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
           Write(Html.DisplayNameFor(model => model.EmailConfirmed));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 21 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
           Write(Html.DisplayNameFor(model => model.PhoneNumber));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 24 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
           Write(Html.DisplayNameFor(model => model.PhoneNumberConfirmed));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 27 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
           Write(Html.DisplayNameFor(model => model.TwoFactorEnabled));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th></th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n        <tr>\r\n            <td>\r\n                ");
#nullable restore
#line 35 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
           Write(Html.DisplayFor(model => model.UserName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 38 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
           Write(Html.DisplayFor(model => model.Email));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 41 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
           Write(Html.DisplayFor(model => model.EmailConfirmed));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 44 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
           Write(Html.DisplayFor(model => model.PhoneNumber));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 47 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
           Write(Html.DisplayFor(model => model.PhoneNumberConfirmed));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 50 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
           Write(Html.DisplayFor(model => model.TwoFactorEnabled));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n        </tr>\r\n    </tbody>\r\n</table>\r\n<a");
            BeginWriteAttribute("href", " href=\"", 1514, "\"", 1547, 1);
#nullable restore
#line 55 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
WriteAttributeValue("", 1521, Url.Action("ListAccount"), 1521, 26, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"btn btn-primary\">Back to list</a>\r\n");
#nullable restore
#line 56 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
 if (Model.EmailConfirmed) { 

#line default
#line hidden
#nullable disable
            WriteLiteral("    <a");
            BeginWriteAttribute("href", " href=\"", 1628, "\"", 1680, 1);
#nullable restore
#line 57 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
WriteAttributeValue("", 1635, Url.Action("Disable", new { id = Model.Id }), 1635, 45, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"btn btn-danger\">Disable</a>\r\n");
#nullable restore
#line 58 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
}else { 

#line default
#line hidden
#nullable disable
            WriteLiteral("    <a");
            BeginWriteAttribute("href", " href=\"", 1734, "\"", 1785, 1);
#nullable restore
#line 59 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
WriteAttributeValue("", 1741, Url.Action("Enable", new { id = Model.Id }), 1741, 44, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"btn btn-primary\">Enable</a>\r\n");
#nullable restore
#line 60 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Details.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Microsoft.AspNetCore.Identity.IdentityUser> Html { get; private set; }
    }
}
#pragma warning restore 1591