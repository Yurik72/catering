#pragma checksum "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "89e9ffd1b173baa0d919af0d20d7bbc790e4f2a9"
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"89e9ffd1b173baa0d919af0d20d7bbc790e4f2a9", @"/Views/Shared/Components/DayDishComponent/Default.cshtml")]
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
            BeginContext(188, 105, true);
            WriteLiteral("\r\n    <div class=\"well well-lg\">\r\n        <div class=\"row justify-content-md-center\">\r\n\r\n        </div>\r\n");
            EndContext();
#line 15 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
         foreach (var itemcategory in Model)
        {

#line default
#line hidden
            BeginContext(350, 187, true);
            WriteLiteral("            <div class=\"row row-daydish-category row justify-content-md-center\">\r\n                <div class=\"col col-lg-8 col-md-8 col-lg-offset-1 col-md-offset-1\">\r\n                    ");
            EndContext();
            BeginContext(538, 55, false);
#line 19 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
               Write(Html.DisplayFor(modelItem => itemcategory.CategoryName));

#line default
#line hidden
            EndContext();
            BeginContext(593, 48, true);
            WriteLiteral("\r\n                </div>\r\n\r\n            </div>\r\n");
            EndContext();
#line 23 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
             foreach (var item in itemcategory.DayDishes)
            {

#line default
#line hidden
            BeginContext(715, 221, true);
            WriteLiteral("                <div class=\"row row-daydish\">\r\n                    <div class=\"col col-md-2\">\r\n\r\n                    </div>\r\n                    <div class=\"col col-md-1\">\r\n                        <div class=\"checkbox\">\r\n");
            EndContext();
#line 31 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                             if (item.Enabled)
                            {

#line default
#line hidden
            BeginContext(1015, 97, true);
            WriteLiteral("                                <input type=\"checkbox\" checked=\"checked\" cp-name=\"dayselect\" />\r\n");
            EndContext();
#line 34 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                            }
                            else
                            {

#line default
#line hidden
            BeginContext(1208, 79, true);
            WriteLiteral("                                <input type=\"checkbox\" cp-name=\"dayselect\" />\r\n");
            EndContext();
#line 38 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                            }

#line default
#line hidden
            BeginContext(1318, 28, true);
            WriteLiteral("                            ");
            EndContext();
            BeginContext(1347, 40, false);
#line 39 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                       Write(Html.HiddenFor(modelItem => item.DishId));

#line default
#line hidden
            EndContext();
            BeginContext(1387, 30, true);
            WriteLiteral("\r\n                            ");
            EndContext();
            BeginContext(1418, 38, false);
#line 40 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                       Write(Html.HiddenFor(modelItem => item.Date));

#line default
#line hidden
            EndContext();
            BeginContext(1456, 143, true);
            WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"col col-lg-4 col-md-4\">\r\n                        ");
            EndContext();
            BeginContext(1600, 43, false);
#line 44 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                   Write(Html.DisplayFor(modelItem => item.DishName));

#line default
#line hidden
            EndContext();
            BeginContext(1643, 54, true);
            WriteLiteral("\r\n                    </div>\r\n                </div>\r\n");
            EndContext();
#line 47 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
            }

#line default
#line hidden
#line 47 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
             
        }

#line default
#line hidden
            BeginContext(1723, 65, true);
            WriteLiteral("        <div class=\"row dayfoot\">\r\n        </div>\r\n    </div>\r\n\r\n");
            EndContext();
            BeginContext(3443, 2, true);
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
