#pragma checksum "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Deny.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3675e201b5633823a78ea29b0f5debb9ecf89120"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Login_Deny), @"mvc.1.0.view", @"/Views/Login/Deny.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3675e201b5633823a78ea29b0f5debb9ecf89120", @"/Views/Login/Deny.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2bd25cc622ebf4ed25b205a6d3ee0806c0243058", @"/Views/_ViewImports.cshtml")]
    public class Views_Login_Deny : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Deny.cshtml"
   
    ViewBag.Title = "Unauthorized";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Access Denied</h1> <br /> <br />\r\n<a class=\"btn btn-primary\"");
            BeginWriteAttribute("href", " href=\"", 111, "\"", 145, 1);
#nullable restore
#line 6 "C:\Users\NoMin\source\repos\VNPOST\VNPOSTWebUI\Views\Login\Deny.cshtml"
WriteAttributeValue("", 118, Url.Action("Index","Home"), 118, 27, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" >Home</a>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
