#pragma checksum "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3ac0d82b2c8c29774c6ebd060e2cec6c1d66c94e"
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
#line 1 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\_ViewImports.cshtml"
using CateringPro.Models;

#line default
#line hidden
#line 2 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\_ViewImports.cshtml"
using CateringPro.ViewModels;

#line default
#line hidden
#line 2 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
using System.Globalization;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3ac0d82b2c8c29774c6ebd060e2cec6c1d66c94e", @"/Views/DayDishes/EditDay.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4117863597b6ac24aaf098caef0bbe565acb1108", @"/Views/_ViewImports.cshtml")]
    public class Views_DayDishes_EditDay : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<DateTime>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
  
    ViewData["Title"] = "EditDay";

#line default
#line hidden
#line 6 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
  
    DateTime daydate = Model;
        CultureInfo ci = new CultureInfo("en-US");
// Get the DateTimeFormatInfo for the en-US culture.
    DateTimeFormatInfo dtfi = ci.DateTimeFormat;

#line default
#line hidden
            BeginContext(283, 42, true);
            WriteLiteral("\r\n<h2>EditDay</h2>\r\n<div id=\"accordion\">\r\n");
            EndContext();
#line 15 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
      
        DateTime current = daydate;
    

#line default
#line hidden
            BeginContext(377, 4, true);
            WriteLiteral("    ");
            EndContext();
#line 18 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
     for (int i = 0; i < 7; i++)
    {

#line default
#line hidden
            BeginContext(418, 64, true);
            WriteLiteral("        <div class=\"card\">\r\n            <div class=\"card-header\"");
            EndContext();
            BeginWriteAttribute("id", " id=", 482, "", 500, 1);
#line 21 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
WriteAttributeValue("", 486, "heading"+i, 486, 14, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(500, 132, true);
            WriteLiteral(">\r\n                <h5 class=\"mb-0\">\r\n                    <button class=\"btn btn-link collapsed\" data-toggle=\"collapse\" data-target=");
            EndContext();
            BeginContext(634, 13, false);
#line 23 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
                                                                                          Write("#collapse"+i);

#line default
#line hidden
            EndContext();
            BeginWriteAttribute("aria-expanded", " aria-expanded=", 648, "", 685, 1);
#line 23 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
WriteAttributeValue("", 663, i==0?"true":"false", 663, 22, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginWriteAttribute("aria-controls", " aria-controls=", 685, "", 715, 1);
#line 23 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
WriteAttributeValue("", 700, "collapse"+i, 700, 15, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(715, 27, true);
            WriteLiteral(">\r\n                        ");
            EndContext();
            BeginContext(743, 29, false);
#line 24 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
                   Write(dtfi.GetDayName((DayOfWeek)i));

#line default
#line hidden
            EndContext();
            BeginContext(772, 96, true);
            WriteLiteral("\r\n\r\n                    </button>\r\n                </h5>\r\n            </div>\r\n\r\n            <div");
            EndContext();
            BeginWriteAttribute("id", " id=", 868, "", 887, 1);
#line 30 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
WriteAttributeValue("", 872, "collapse"+i, 872, 15, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 887, "\"", 935, 3);
            WriteAttributeValue("", 895, "collapse", 895, 8, true);
            WriteAttributeValue("  ", 903, "aria-labelledby=", 905, 18, true);
#line 30 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
WriteAttributeValue("", 921, "heading"+i, 921, 14, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(936, 89, true);
            WriteLiteral(" data-parent=\"#accordion\">\r\n                <div class=\"card-body\">\r\n                    ");
            EndContext();
            BeginContext(1026, 56, false);
#line 32 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
               Write(await Component.InvokeAsync("DayDishComponent", current));

#line default
#line hidden
            EndContext();
            BeginContext(1082, 62, true);
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n        </div>\r\n");
            EndContext();
#line 36 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
        current =current.AddDays(1);
    }

#line default
#line hidden
            BeginContext(1189, 17, true);
            WriteLiteral("</div>\r\n\r\n \r\n\r\n\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(1228, 143, true);
                WriteLiteral("\r\n\r\n\r\n        <script>\r\n            $(function () {\r\n                //  $(\"#accordion\").accordion();\r\n            });\r\n        </script>\r\n    ");
                EndContext();
            }
            );
            BeginContext(1374, 4, true);
            WriteLiteral("\r\n\r\n");
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
