#pragma checksum "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ec52a346df453d9f3e991e040d51a035a82dee07"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Components_DayDishComponent_Default), @"mvc.1.0.view", @"/Views/Shared/Components/DayDishComponent/Default.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/Components/DayDishComponent/Default.cshtml", typeof(AspNetCore.Views_Shared_Components_DayDishComponent_Default))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ec52a346df453d9f3e991e040d51a035a82dee07", @"/Views/Shared/Components/DayDishComponent/Default.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4117863597b6ac24aaf098caef0bbe565acb1108", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Components_DayDishComponent_Default : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<CateringPro.Models.DayDishViewModelPerGategory>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(68, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
  
    ViewData["Title"] = "EditDay";

#line default
#line hidden
#line 6 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
  
    DateTime daydate = DateTime.Now;

#line default
#line hidden
            BeginContext(188, 101, true);
            WriteLiteral("\r\n<div class=\"well well-lg\">\r\n    <div class=\"row justify-content-md-center\">\r\n\r\n    </div>\r\n</div>\r\n");
            EndContext();
#line 16 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
  
    int idx = 0;

#line default
#line hidden
            BeginContext(314, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 20 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
 foreach (var itemcategory in Model)
{

#line default
#line hidden
            BeginContext(357, 214, true);
            WriteLiteral("    <div class=\"row row-daydish-category row justify-content-md-center\">\r\n        <div class=\"container\">\r\n            <div class=\"row\">\r\n                <div class=\"col col-lg-12 col-md-12 \">\r\n                    ");
            EndContext();
            BeginContext(572, 55, false);
#line 26 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
               Write(Html.DisplayFor(modelItem => itemcategory.CategoryName));

#line default
#line hidden
            EndContext();
            BeginContext(627, 48, true);
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n\r\n");
            EndContext();
#line 30 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                 foreach (var item in itemcategory.DayDishes)
                {
                    string id_checkbox = daydate.Millisecond.ToString() + "_" + idx;

#line default
#line hidden
            BeginContext(843, 257, true);
            WriteLiteral(@"                    <div class=""row row-daydish"">
                        <div class=""col col-md-2"">

                        </div>
                        <div class=""col col-md-1"">
                            <div class=""checkbox checkbox-info"">

");
            EndContext();
#line 40 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                                 if (item.Enabled)
                                {

#line default
#line hidden
            BeginContext(1187, 120, true);
            WriteLiteral("                                    <input type=\"checkbox\" class=\"checkbox styled\" checked=\"checked\" cp-name=\"dayselect\"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 1307, "\"", 1324, 1);
#line 42 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
WriteAttributeValue("", 1312, id_checkbox, 1312, 12, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1325, 5, true);
            WriteLiteral(" />\r\n");
            EndContext();
#line 43 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                                }
                                else
                                {

#line default
#line hidden
            BeginContext(1438, 102, true);
            WriteLiteral("                                    <input type=\"checkbox\" class=\"checkbox styled\" cp-name=\"dayselect\"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 1540, "\"", 1557, 1);
#line 46 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
WriteAttributeValue("", 1545, id_checkbox, 1545, 12, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1558, 5, true);
            WriteLiteral(" />\r\n");
            EndContext();
#line 47 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                                }

#line default
#line hidden
            BeginContext(1598, 38, true);
            WriteLiteral("                                <label");
            EndContext();
            BeginWriteAttribute("for", " for=\"", 1636, "\"", 1654, 1);
#line 48 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
WriteAttributeValue("", 1642, id_checkbox, 1642, 12, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1655, 79, true);
            WriteLiteral(">\r\n\r\n                                </label>\r\n                                ");
            EndContext();
            BeginContext(1735, 40, false);
#line 51 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                           Write(Html.HiddenFor(modelItem => item.DishId));

#line default
#line hidden
            EndContext();
            BeginContext(1775, 34, true);
            WriteLiteral("\r\n                                ");
            EndContext();
            BeginContext(1810, 38, false);
#line 52 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                           Write(Html.HiddenFor(modelItem => item.Date));

#line default
#line hidden
            EndContext();
            BeginContext(1848, 159, true);
            WriteLiteral("\r\n                            </div>\r\n                        </div>\r\n                        <div class=\"col col-lg-4 col-md-4\">\r\n                            ");
            EndContext();
            BeginContext(2008, 43, false);
#line 56 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                       Write(Html.DisplayFor(modelItem => item.DishName));

#line default
#line hidden
            EndContext();
            BeginContext(2051, 62, true);
            WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n");
            EndContext();
#line 59 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                    idx++;
                }

#line default
#line hidden
            BeginContext(2160, 105, true);
            WriteLiteral("\r\n                <div class=\"row dayfoot\">\r\n                </div>\r\n            </div>\r\n        </div>\r\n");
            EndContext();
#line 66 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
   
}

#line default
#line hidden
            BeginContext(3771, 2, true);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<CateringPro.Models.DayDishViewModelPerGategory>> Html { get; private set; }
    }
}
#pragma warning restore 1591
