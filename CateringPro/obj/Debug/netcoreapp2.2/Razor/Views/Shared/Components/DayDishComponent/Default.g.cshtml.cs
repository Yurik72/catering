#pragma checksum "C:\work1\CateringPro\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "78956960450210212f01f22caaca88fa1abda5cd"
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
#line 1 "C:\work1\CateringPro\CateringPro\Views\_ViewImports.cshtml"
using CateringPro.Models;

#line default
#line hidden
#line 2 "C:\work1\CateringPro\CateringPro\Views\_ViewImports.cshtml"
using CateringPro.ViewModels;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"78956960450210212f01f22caaca88fa1abda5cd", @"/Views/Shared/Components/DayDishComponent/Default.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4117863597b6ac24aaf098caef0bbe565acb1108", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Components_DayDishComponent_Default : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<CateringPro.Models.DayDishViewModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(57, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\work1\CateringPro\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
  
    ViewData["Title"] = "EditDay";

#line default
#line hidden
#line 6 "C:\work1\CateringPro\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
  
    DateTime daydate = DateTime.Now;

#line default
#line hidden
            BeginContext(147, 115, true);
            WriteLiteral("\r\n<h2>EditDay</h2>\r\n<div class=\"form-group\">\r\n\r\n    <div class=\"col-md-10\">\r\n        <p>�������� ����</p>\r\n        ");
            EndContext();
            BeginContext(263, 90, false);
#line 15 "C:\work1\CateringPro\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
   Write(Html.EditorFor(model => daydate, new { htmlAttributes = new { @class = "form-control" } }));

#line default
#line hidden
            EndContext();
            BeginContext(353, 110, true);
            WriteLiteral("\r\n\r\n    </div>\r\n</div>\r\n\r\n<table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(464, 43, false);
#line 24 "C:\work1\CateringPro\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
           Write(Html.DisplayNameFor(model => model.Enabled));

#line default
#line hidden
            EndContext();
            BeginContext(507, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(563, 44, false);
#line 27 "C:\work1\CateringPro\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
           Write(Html.DisplayNameFor(model => model.DishName));

#line default
#line hidden
            EndContext();
            BeginContext(607, 88, true);
            WriteLiteral("\r\n            </th>\r\n\r\n            <th></th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
            EndContext();
#line 34 "C:\work1\CateringPro\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
         foreach (var item in Model)
        {

#line default
#line hidden
            BeginContext(744, 86, true);
            WriteLiteral("            <tr>\r\n                <td>\r\n                    <div class=\"checkbox\">\r\n\r\n");
            EndContext();
#line 40 "C:\work1\CateringPro\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                         if (item.Enabled)
                        {

#line default
#line hidden
            BeginContext(901, 73, true);
            WriteLiteral("                            <input type=\"checkbox\" checked=\"checked\" />\r\n");
            EndContext();
#line 43 "C:\work1\CateringPro\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                        }
                        else
                        {

#line default
#line hidden
            BeginContext(1058, 55, true);
            WriteLiteral("                            <input type=\"checkbox\" />\r\n");
            EndContext();
#line 47 "C:\work1\CateringPro\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                        }

#line default
#line hidden
            BeginContext(1140, 24, true);
            WriteLiteral("                        ");
            EndContext();
            BeginContext(1165, 40, false);
#line 48 "C:\work1\CateringPro\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
                   Write(Html.HiddenFor(modelItem => item.DishId));

#line default
#line hidden
            EndContext();
            BeginContext(1205, 95, true);
            WriteLiteral("\r\n                    </div>\r\n                </td>\r\n                <td>\r\n                    ");
            EndContext();
            BeginContext(1301, 43, false);
#line 52 "C:\work1\CateringPro\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
               Write(Html.DisplayFor(modelItem => item.DishName));

#line default
#line hidden
            EndContext();
            BeginContext(1344, 46, true);
            WriteLiteral("\r\n                </td>\r\n\r\n            </tr>\r\n");
            EndContext();
#line 56 "C:\work1\CateringPro\CateringPro\Views\Shared\Components\DayDishComponent\Default.cshtml"
        }

#line default
#line hidden
            BeginContext(1401, 26, true);
            WriteLiteral("    </tbody>\r\n</table>\r\n\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(1445, 869, true);
                WriteLiteral(@"


    <script type=""text/javascript"">
            $(document).ready(function () {

                var checkboxes = $(this).find(':checkbox').change(function () {
                    if (this.checked) {
                        // alert(""click"");
                    }
                    var dishid = parseInt($(this).parent().find(""#item_DishId"").val());
                    //alert(JSON.stringify({ DishId: dishid, daydate: 0, enabled: this.checked }));
                    $.ajax({
                        type: ""POST"",
                        data: { DishId: dishid, enabled: this.checked },

                        url: ""/DayDishes/SaveDayDish"",
                        success: function (data) {
                            alert(data);
                        }
                    });
                });
            });
    </script>
");
                EndContext();
            }
            );
            BeginContext(2317, 2, true);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<CateringPro.Models.DayDishViewModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
