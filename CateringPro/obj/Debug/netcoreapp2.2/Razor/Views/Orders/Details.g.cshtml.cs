#pragma checksum "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5d49d1e8ddbb369f0c0906a44f33762764a748f7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Orders_Details), @"mvc.1.0.view", @"/Views/Orders/Details.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Orders/Details.cshtml", typeof(AspNetCore.Views_Orders_Details))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5d49d1e8ddbb369f0c0906a44f33762764a748f7", @"/Views/Orders/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4117863597b6ac24aaf098caef0bbe565acb1108", @"/Views/_ViewImports.cshtml")]
    public class Views_Orders_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Order>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(13, 1, true);
            WriteLiteral("\n");
            EndContext();
#line 3 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
  
    var pageName = "Details";
    ViewData["Title"] = pageName;

#line default
#line hidden
            BeginContext(83, 5, true);
            WriteLiteral("\n<h2>");
            EndContext();
            BeginContext(89, 8, false);
#line 8 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
Write(pageName);

#line default
#line hidden
            EndContext();
            BeginContext(97, 99, true);
            WriteLiteral("</h2>\n\n<div>\n    <h4>Order</h4>\n    <hr />\n    <dl class=\"dl-horizontal\">\n        <dt>\n            ");
            EndContext();
            BeginContext(197, 45, false);
#line 15 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.FirstName));

#line default
#line hidden
            EndContext();
            BeginContext(242, 40, true);
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
            EndContext();
            BeginContext(283, 41, false);
#line 18 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayFor(model => model.FirstName));

#line default
#line hidden
            EndContext();
            BeginContext(324, 40, true);
            WriteLiteral("\n        </dd>\n        <dt>\n            ");
            EndContext();
            BeginContext(365, 44, false);
#line 21 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.LastName));

#line default
#line hidden
            EndContext();
            BeginContext(409, 40, true);
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
            EndContext();
            BeginContext(450, 40, false);
#line 24 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayFor(model => model.LastName));

#line default
#line hidden
            EndContext();
            BeginContext(490, 40, true);
            WriteLiteral("\n        </dd>\n        <dt>\n            ");
            EndContext();
            BeginContext(531, 48, false);
#line 27 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.AddressLine1));

#line default
#line hidden
            EndContext();
            BeginContext(579, 40, true);
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
            EndContext();
            BeginContext(620, 44, false);
#line 30 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayFor(model => model.AddressLine1));

#line default
#line hidden
            EndContext();
            BeginContext(664, 40, true);
            WriteLiteral("\n        </dd>\n        <dt>\n            ");
            EndContext();
            BeginContext(705, 48, false);
#line 33 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.AddressLine2));

#line default
#line hidden
            EndContext();
            BeginContext(753, 40, true);
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
            EndContext();
            BeginContext(794, 44, false);
#line 36 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayFor(model => model.AddressLine2));

#line default
#line hidden
            EndContext();
            BeginContext(838, 40, true);
            WriteLiteral("\n        </dd>\n        <dt>\n            ");
            EndContext();
            BeginContext(879, 43, false);
#line 39 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ZipCode));

#line default
#line hidden
            EndContext();
            BeginContext(922, 40, true);
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
            EndContext();
            BeginContext(963, 39, false);
#line 42 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayFor(model => model.ZipCode));

#line default
#line hidden
            EndContext();
            BeginContext(1002, 40, true);
            WriteLiteral("\n        </dd>\n        <dt>\n            ");
            EndContext();
            BeginContext(1043, 40, false);
#line 45 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.City));

#line default
#line hidden
            EndContext();
            BeginContext(1083, 40, true);
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
            EndContext();
            BeginContext(1124, 36, false);
#line 48 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayFor(model => model.City));

#line default
#line hidden
            EndContext();
            BeginContext(1160, 40, true);
            WriteLiteral("\n        </dd>\n        <dt>\n            ");
            EndContext();
            BeginContext(1201, 41, false);
#line 51 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.State));

#line default
#line hidden
            EndContext();
            BeginContext(1242, 40, true);
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
            EndContext();
            BeginContext(1283, 37, false);
#line 54 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayFor(model => model.State));

#line default
#line hidden
            EndContext();
            BeginContext(1320, 40, true);
            WriteLiteral("\n        </dd>\n        <dt>\n            ");
            EndContext();
            BeginContext(1361, 43, false);
#line 57 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.Country));

#line default
#line hidden
            EndContext();
            BeginContext(1404, 40, true);
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
            EndContext();
            BeginContext(1445, 39, false);
#line 60 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayFor(model => model.Country));

#line default
#line hidden
            EndContext();
            BeginContext(1484, 40, true);
            WriteLiteral("\n        </dd>\n        <dt>\n            ");
            EndContext();
            BeginContext(1525, 47, false);
#line 63 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.PhoneNumber));

#line default
#line hidden
            EndContext();
            BeginContext(1572, 40, true);
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
            EndContext();
            BeginContext(1613, 43, false);
#line 66 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayFor(model => model.PhoneNumber));

#line default
#line hidden
            EndContext();
            BeginContext(1656, 40, true);
            WriteLiteral("\n        </dd>\n        <dt>\n            ");
            EndContext();
            BeginContext(1697, 47, false);
#line 69 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.OrderPlaced));

#line default
#line hidden
            EndContext();
            BeginContext(1744, 40, true);
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
            EndContext();
            BeginContext(1785, 43, false);
#line 72 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayFor(model => model.OrderPlaced));

#line default
#line hidden
            EndContext();
            BeginContext(1828, 40, true);
            WriteLiteral("\n        </dd>\n        <dt>\n            ");
            EndContext();
            BeginContext(1869, 41, false);
#line 75 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.Email));

#line default
#line hidden
            EndContext();
            BeginContext(1910, 40, true);
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
            EndContext();
            BeginContext(1951, 37, false);
#line 78 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayFor(model => model.Email));

#line default
#line hidden
            EndContext();
            BeginContext(1988, 40, true);
            WriteLiteral("\n        </dd>\n        <dt>\n            ");
            EndContext();
            BeginContext(2029, 40, false);
#line 81 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.User));

#line default
#line hidden
            EndContext();
            BeginContext(2069, 40, true);
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
            EndContext();
            BeginContext(2110, 42, false);
#line 84 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Html.DisplayFor(model => model.User.Email));

#line default
#line hidden
            EndContext();
            BeginContext(2152, 138, true);
            WriteLiteral("\n        </dd>\n        <dt>\n            <br />----- Order\n        </dt>\n        <dd>\n            <br /><b>Details -----</b>\n        </dd>\n");
            EndContext();
#line 92 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
          decimal subtotal = 0M;

#line default
#line hidden
            BeginContext(2324, 8, true);
            WriteLiteral("        ");
            EndContext();
#line 93 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
         foreach (OrderDetail orderDetail in ViewBag.OrderDetailsList)
        {

#line default
#line hidden
            BeginContext(2405, 33, true);
            WriteLiteral("            <dt>\n                ");
            EndContext();
            BeginContext(2439, 47, false);
#line 96 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
           Write(Html.DisplayNameFor(model => orderDetail.Pizza));

#line default
#line hidden
            EndContext();
            BeginContext(2486, 52, true);
            WriteLiteral("\n            </dt>\n            <dd>\n                ");
            EndContext();
            BeginContext(2539, 48, false);
#line 99 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
           Write(Html.DisplayFor(model => orderDetail.Pizza.Name));

#line default
#line hidden
            EndContext();
            BeginContext(2587, 52, true);
            WriteLiteral("\n            </dd>\n            <dt>\n                ");
            EndContext();
            BeginContext(2640, 48, false);
#line 102 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
           Write(Html.DisplayNameFor(model => orderDetail.Amount));

#line default
#line hidden
            EndContext();
            BeginContext(2688, 52, true);
            WriteLiteral("\n            </dt>\n            <dd>\n                ");
            EndContext();
            BeginContext(2741, 44, false);
#line 105 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
           Write(Html.DisplayFor(model => orderDetail.Amount));

#line default
#line hidden
            EndContext();
            BeginContext(2785, 52, true);
            WriteLiteral("\n            </dd>\n            <dt>\n                ");
            EndContext();
            BeginContext(2838, 47, false);
#line 108 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
           Write(Html.DisplayNameFor(model => orderDetail.Price));

#line default
#line hidden
            EndContext();
            BeginContext(2885, 56, true);
            WriteLiteral("\n            </dt>\n            <dd>\n                    ");
            EndContext();
            BeginContext(2942, 31, false);
#line 111 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
               Write(orderDetail.Price.ToString("c"));

#line default
#line hidden
            EndContext();
            BeginContext(2973, 97, true);
            WriteLiteral("\n            </dd>\n            <dt>\n                Sub Total\n            </dt>\n            <dd>\n");
            EndContext();
#line 117 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
                  subtotal = orderDetail.Price * orderDetail.Amount;

#line default
#line hidden
            BeginContext(3140, 16, true);
            WriteLiteral("                ");
            EndContext();
            BeginContext(3157, 22, false);
#line 118 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
           Write(subtotal.ToString("c"));

#line default
#line hidden
            EndContext();
            BeginContext(3179, 135, true);
            WriteLiteral("\n            </dd>\n            <dt>\n                <br />\n            </dt>\n            <dd>\n                <br />\n            </dd>\n");
            EndContext();
#line 126 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
        }

#line default
#line hidden
            BeginContext(3324, 76, true);
            WriteLiteral("        <dt>\n            Total Price\n        </dt>\n        <dd>\n            ");
            EndContext();
            BeginContext(3401, 30, false);
#line 131 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Orders\Details.cshtml"
       Write(Model.OrderTotal.ToString("c"));

#line default
#line hidden
            EndContext();
            BeginContext(3431, 47, true);
            WriteLiteral("\n        </dd>\n        \n    </dl>\n</div>\n<div>\n");
            EndContext();
            BeginContext(3548, 4, true);
            WriteLiteral("    ");
            EndContext();
            BeginContext(3552, 38, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "5d49d1e8ddbb369f0c0906a44f33762764a748f718793", async() => {
                BeginContext(3574, 12, true);
                WriteLiteral("Back to List");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3590, 8, true);
            WriteLiteral("\n</div>\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Order> Html { get; private set; }
    }
}
#pragma warning restore 1591
