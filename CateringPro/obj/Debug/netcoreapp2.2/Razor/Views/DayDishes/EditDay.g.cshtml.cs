#pragma checksum "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "69a8c63e9348261f6ec4da03996718b8a2db9378"
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"69a8c63e9348261f6ec4da03996718b8a2db9378", @"/Views/DayDishes/EditDay.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4117863597b6ac24aaf098caef0bbe565acb1108", @"/Views/_ViewImports.cshtml")]
    public class Views_DayDishes_EditDay : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<DateTime>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.min.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("text/javascript"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/dishdayedit.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
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
            BeginContext(283, 100, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "69a8c63e9348261f6ec4da03996718b8a2db93785279", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(383, 4078, true);
            WriteLiteral(@"
<style>
    .dayfoot {
        height: 20px;
    }

    .daydish {
        width: 100%;
        border: 1px solid #e3e3e3;
        -moz-border-radius: inherit;
        -webkit-border-radius: inherit;
        border-radius: 4px;
        -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .05);
        box-shadow: inset 0px 1px 1px rgba(0,0,0,0.05);
    }

    .row-daydish-category {
        width: 100%;
        border: 1px solid #e3e3e3;
        -moz-border-radius: inherit;
        -webkit-border-radius: inherit;
        border-radius: 4px;
        -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .05);
        box-shadow: inset 0px 1px 1px rgba(0,0,0,0.05);
        background-color: mintcream;
    }


    .row-daydish > .col {
        margin-top: 10px;
        margin-bottom: 10px;
    }

    .btn-link.daydish[aria-expanded=""true""]:hover, .btn-link.daydish[aria-expanded=""true""]:focus {
        background-color: #f15a29
    }

    .btn-link.daydish:hover, .btn-link.daydish:fo");
            WriteLiteral(@"cus {
        color: #23527c;
        text-decoration: none;
        background: #fff;
    }

    .expand, .dayname {
        float: left;
        display: block;
    }

    .daydish, .daydish:hover .expand {
        background: #fff
    }


        .daydish[aria-expanded=""true""], .expand::after, .daydish[aria-expanded=""true""] .expand::before, .daydish .expand::after, .daydish .expand::before {
            background-color: #f15a29
        }

            .daydish[aria-expanded=""true""] .expand::after, .daydish[aria-expanded=""true""] .expand::before,
            .daydish:hover .expand::after, .daydish:hover .expand::before {
                background-color: #f15a29
            }

        .daydish .expand {
            background: #f15a29;
            border-radius: 50%;
            height: 30px;
            margin: 8px 18px 8px 32px;
            min-width: 30px;
            position: relative;
            -webkit-transition: .25s ease-out;
            transition: .25s ease-out;");
            WriteLiteral(@"
            width: 30px
        }

            .daydish .expand::after, .daydish .expand::before {
                background-color: #fff;
                content: """";
                position: absolute;
                -webkit-transition: -webkit-transform .25s ease-out;
                transition: transform .25s ease-out, -webkit-transform .25s ease-out
            }

            .daydish .expand::before {
                height: 50%;
                left: 50%;
                margin-left: -1px;
                top: 25%;
                width: 2px
            }

            .daydish .expand::after {
                height: 2px;
                left: 25%;
                margin-top: -1px;
                top: 50%;
                width: 50%
            }

    .accordeon dl dt.active .plus, .accordeon dl dt:hover .plus {
        background: #fff
    }

    .daydish[aria-expanded=""true""] .expand::after, .daydish[aria-expanded=""true""] .expand::before, .daydish[aria-expanded=""true");
            WriteLiteral(@"""] .expand::after,
    .daydish[aria-expanded=""true""] .expand::before {
        background-color: #f15a29;
    }

    .daydish[aria-expanded=""true""] .expand {
        background: #fff
    }

        .daydish[aria-expanded=""true""] .expand::before {
            -webkit-transform: rotate(90deg);
            -ms-transform: rotate(90deg);
            transform: rotate(90deg)
        }

        .daydish[aria-expanded=""true""] .expand::after {
            -webkit-transform: rotate(180deg);
            -ms-transform: rotate(180deg);
            transform: rotate(180deg)
        }
</style>
<h2>Dishes to be available on the week</h2>
<div class=""container"">
    <div class=""row justify-content-md-center"">
        <div class=""col-lg-4 col-lmd-4 col-lg-offset-4 col-md-offset-4"">
            <div class=""input-group"">
                <input type=""week"" value=""daydate"" class=""form-control"" id=""Week"" />

            </div>
        </div>
    </div>
</div>
<div id=""accordion"">
");
            EndContext();
#line 147 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
      
        DateTime current = daydate;
    

#line default
#line hidden
            BeginContext(4513, 4, true);
            WriteLiteral("    ");
            EndContext();
#line 150 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
     for (int i = 0; i < 7; i++)
    {

#line default
#line hidden
            BeginContext(4554, 362, true);
            WriteLiteral(@"        <div class=""checkbox"">
            <label>
                <input type=""checkbox""> Check me out
            </label>
        </div>
        <div class=""container"">
            <div class=""row "">
                <div class=""col-lg-8 col-md-8 "">
                    <button class=""btn btn-link collapsed daydish"" data-toggle=""collapse"" data-target=");
            EndContext();
            BeginContext(4918, 15, false);
#line 160 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
                                                                                                  Write("#collapse" + i);

#line default
#line hidden
            EndContext();
            BeginContext(4934, 22, true);
            WriteLiteral(" aria-expanded=\"false\"");
            EndContext();
            BeginWriteAttribute("aria-controls", " aria-controls=", 4956, "", 4988, 1);
#line 160 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
WriteAttributeValue("", 4971, "collapse" + i, 4971, 17, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(4988, 130, true);
            WriteLiteral(">\r\n                        <div class=\"expand\">\r\n\r\n\r\n                        </div>\r\n                        <div class=\"dayname\">");
            EndContext();
            BeginContext(5119, 29, false);
#line 165 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
                                        Write(dtfi.GetDayName((DayOfWeek)i));

#line default
#line hidden
            EndContext();
            BeginContext(5148, 239, true);
            WriteLiteral(" </div>\r\n                    </button>\r\n\r\n                </div>\r\n                <div class=\"col-lg-1 col-md-1 \">\r\n\r\n                </div>\r\n            </div>\r\n            <div class=\"row justify-content-md-center\">\r\n                <div");
            EndContext();
            BeginWriteAttribute("id", " id=", 5387, "", 5408, 1);
#line 174 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
WriteAttributeValue("", 5391, "collapse" + i, 5391, 17, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 5408, "\"", 5480, 6);
            WriteAttributeValue("", 5416, "col", 5416, 3, true);
            WriteAttributeValue(" ", 5419, "col-lg-8", 5420, 9, true);
            WriteAttributeValue(" ", 5428, "col-md-8", 5429, 9, true);
            WriteAttributeValue(" ", 5437, "collapse", 5438, 9, true);
            WriteAttributeValue("  ", 5446, "aria-labelledby=", 5448, 18, true);
#line 174 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
WriteAttributeValue("", 5464, "heading" + i, 5464, 16, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(5481, 50, true);
            WriteLiteral(" data-parent=\"#accordion\">\r\n\r\n                    ");
            EndContext();
            BeginContext(5532, 56, false);
#line 176 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
               Write(await Component.InvokeAsync("DayDishComponent", current));

#line default
#line hidden
            EndContext();
            BeginContext(5588, 48, true);
            WriteLiteral("\r\n\r\n                </div>\r\n            </div>\r\n");
            EndContext();
            BeginContext(6522, 18, true);
            WriteLiteral("\r\n        </div>\r\n");
            EndContext();
#line 200 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
        current = current.AddDays(1);
    }

#line default
#line hidden
            BeginContext(6586, 8, true);
            WriteLiteral("\r\n\r\n    ");
            EndContext();
            BeginContext(6594, 72, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "69a8c63e9348261f6ec4da03996718b8a2db937815333", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("defer", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(6666, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(6690, 281, true);
                WriteLiteral(@"


        <script>
            $(function () {
                //  $(""#accordion"").accordion();
                $('#Week').datepicker({
                    calendarWeeks: true,
                    weekStart: 1
                });
            });
        </script>
    ");
                EndContext();
            }
            );
            BeginContext(6974, 4, true);
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
