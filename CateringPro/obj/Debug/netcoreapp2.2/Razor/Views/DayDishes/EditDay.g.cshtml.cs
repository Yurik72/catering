#pragma checksum "C:\work1\CateringPro\CateringPro\Views\DayDishes\EditDay.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "38a1c4c935d4292dc98eac55e86f49f9c2e7a1a2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_DayDishes_EditDay), @"mvc.1.0.view", @"/Views/DayDishes/EditDay.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/DayDishes/EditDay.cshtml", typeof(AspNetCore.Views_DayDishes_EditDay))]
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
#line 1 "C:\work1\CateringPro\CateringPro\Views\_ViewImports.cshtml"
using CateringPro.Models;

#line default
#line hidden
#line 2 "C:\work1\CateringPro\CateringPro\Views\_ViewImports.cshtml"
using CateringPro.ViewModels;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"38a1c4c935d4292dc98eac55e86f49f9c2e7a1a2", @"/Views/DayDishes/EditDay.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4117863597b6ac24aaf098caef0bbe565acb1108", @"/Views/_ViewImports.cshtml")]
    public class Views_DayDishes_EditDay : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<DateTime>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(17, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\work1\CateringPro\CateringPro\Views\DayDishes\EditDay.cshtml"
  
    ViewData["Title"] = "EditDay";

#line default
#line hidden
#line 6 "C:\work1\CateringPro\CateringPro\Views\DayDishes\EditDay.cshtml"
  
    DateTime daydate = Model;

#line default
#line hidden
            BeginContext(100, 115, true);
            WriteLiteral("\r\n<h2>EditDay</h2>\r\n<div class=\"form-group\">\r\n\r\n    <div class=\"col-md-10\">\r\n        <p>Выберите дату</p>\r\n        ");
            EndContext();
            BeginContext(216, 90, false);
#line 15 "C:\work1\CateringPro\CateringPro\Views\DayDishes\EditDay.cshtml"
   Write(Html.EditorFor(model => daydate, new { htmlAttributes = new { @class = "form-control" } }));

#line default
#line hidden
            EndContext();
            BeginContext(306, 24, true);
            WriteLiteral("\r\n\r\n    </div>\r\n</div>\r\n");
            EndContext();
            BeginContext(331, 56, false);
#line 19 "C:\work1\CateringPro\CateringPro\Views\DayDishes\EditDay.cshtml"
Write(await Component.InvokeAsync("DayDishComponent", daydate));

#line default
#line hidden
            EndContext();
            BeginContext(387, 2, true);
            WriteLiteral("\r\n");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<DateTime> Html { get; private set; }
    }
}
#pragma warning restore 1591