#pragma checksum "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "92501f052fc9772178acd19c519468e2acee3980"
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"92501f052fc9772178acd19c519468e2acee3980", @"/Views/DayDishes/EditDay.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4117863597b6ac24aaf098caef0bbe565acb1108", @"/Views/_ViewImports.cshtml")]
    public class Views_DayDishes_EditDay : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<DateTime>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "hidden", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("currentdate"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
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
    long ms_since1970 = (long)daydate.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
    CultureInfo ci = new CultureInfo("en-US");
    // Get the DateTimeFormatInfo for the en-US culture.
    DateTimeFormatInfo dtfi = ci.DateTimeFormat;

#line default
#line hidden
            BeginContext(384, 348, true);
            WriteLiteral(@"

<h2>Dishes to be available on the week</h2>

<div class=""container"">
    <div class=""row justify-content-md-center"">
        <div class=""col-lg-1 col-md-1 dayselector dayselectback"">
            <span class="" glyphicon glyphicon-backward"" aria-hidden=""true""></span>

        </div>
        <div class=""col-lg-4 col-md-4 "">
            ");
            EndContext();
            BeginContext(732, 64, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "92501f052fc9772178acd19c519468e2acee39805860", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 24 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => ms_since1970);

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(796, 351, true);
            WriteLiteral(@"
            <div id=""weekselector"">

            </div>
        </div>
        <div class=""col-lg-1 col-md-1 dayselector dayselectforward"">
            <span class="" glyphicon glyphicon-forward"" aria-hidden=""true""></span>

        </div>
    </div>
    <div class=""row"" style=""height:10px"">

    </div>

</div>

<div id=""accordion"">
");
            EndContext();
#line 41 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
      
        DateTime current = daydate;
    

#line default
#line hidden
            BeginContext(1199, 4, true);
            WriteLiteral("    ");
            EndContext();
#line 44 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
     for (int i = 0; i < 7; i++)
    {


#line default
#line hidden
            BeginContext(1242, 217, true);
            WriteLiteral("        <div class=\"container\">\r\n            <div class=\"row \">\r\n                <div class=\"col-lg-8 col-md-8 \">\r\n                    <button class=\"btn btn-link collapsed daydish\" data-toggle=\"collapse\" data-target=");
            EndContext();
            BeginContext(1461, 15, false);
#line 50 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
                                                                                                  Write("#collapse" + i);

#line default
#line hidden
            EndContext();
            BeginContext(1477, 22, true);
            WriteLiteral(" aria-expanded=\"false\"");
            EndContext();
            BeginWriteAttribute("aria-controls", " aria-controls=", 1499, "", 1531, 1);
#line 50 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
WriteAttributeValue("", 1514, "collapse" + i, 1514, 17, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1531, 130, true);
            WriteLiteral(">\r\n                        <div class=\"expand\">\r\n\r\n\r\n                        </div>\r\n                        <div class=\"dayname\">");
            EndContext();
            BeginContext(1662, 29, false);
#line 55 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
                                        Write(dtfi.GetDayName((DayOfWeek)i));

#line default
#line hidden
            EndContext();
            BeginContext(1691, 239, true);
            WriteLiteral(" </div>\r\n                    </button>\r\n\r\n                </div>\r\n                <div class=\"col-lg-1 col-md-1 \">\r\n\r\n                </div>\r\n            </div>\r\n            <div class=\"row justify-content-md-center\">\r\n                <div");
            EndContext();
            BeginWriteAttribute("id", " id=", 1930, "", 1951, 1);
#line 64 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
WriteAttributeValue("", 1934, "collapse" + i, 1934, 17, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 1951, "\"", 2023, 6);
            WriteAttributeValue("", 1959, "col", 1959, 3, true);
            WriteAttributeValue(" ", 1962, "col-lg-8", 1963, 9, true);
            WriteAttributeValue(" ", 1971, "col-md-8", 1972, 9, true);
            WriteAttributeValue(" ", 1980, "collapse", 1981, 9, true);
            WriteAttributeValue("  ", 1989, "aria-labelledby=", 1991, 18, true);
#line 64 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
WriteAttributeValue("", 2007, "heading" + i, 2007, 16, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2024, 40, true);
            WriteLiteral(" data-parent=\"#accordion\" data-daydate=\"");
            EndContext();
            BeginContext(2065, 7, false);
#line 64 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
                                                                                                                                                     Write(current);

#line default
#line hidden
            EndContext();
            BeginContext(2072, 26, true);
            WriteLiteral("\">\r\n\r\n                    ");
            EndContext();
            BeginContext(2099, 56, false);
#line 66 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
               Write(await Component.InvokeAsync("DayDishComponent", current));

#line default
#line hidden
            EndContext();
            BeginContext(2155, 48, true);
            WriteLiteral("\r\n\r\n                </div>\r\n            </div>\r\n");
            EndContext();
            BeginContext(3089, 18, true);
            WriteLiteral("\r\n        </div>\r\n");
            EndContext();
#line 90 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\DayDishes\EditDay.cshtml"
        current = current.AddDays(1);
    }

#line default
#line hidden
            BeginContext(3153, 14, true);
            WriteLiteral("</div>\r\n\r\n    ");
            EndContext();
            BeginContext(3167, 72, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "92501f052fc9772178acd19c519468e2acee398012974", async() => {
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
            BeginContext(3239, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(3263, 4891, true);
                WriteLiteral(@"


        <script>
            $(function () {


                //  $(""#accordion"").accordion();
                /*
                                $('#Week').datepicker({
                                    calendarWeeks: true,
                                    weekStart: 1
                                });

                                var startDate;
                                var endDate;

                                var selectCurrentWeek = function() {
                                    window.setTimeout(function () {
                                        $('.week-picker').find('.ui-datepicker-current-day a').addClass('ui-state-active')
                                    }, 1);
                                }
                */
                var html_loading = '<div class=""spinner-container""> <div>Loading</div><div class=""spinner-border"" role=""status""> <span class=""sr-only"">Loading...</span> </div></div>';
                var reloadDay = function (dayindex");
                WriteLiteral(@") {
                    var ms = parseInt($('#currentdate').val());
                    var dt = new Date(ms);
                    dt.setDate(dt.getDate() - dt.getDay()+dayindex);
                    $('#collapse' + dayindex).html(html_loading);
                    $('#collapse' + dayindex).load('/DayDishes/EditDayComponent?daydate=' + encodeURI(dt.toDateString()));
                }
                var weektext = function () {
                    var ms = parseInt($('#currentdate').val());
                    var dt = new Date(ms);
                    firstweekday = new Date(dt.getTime());
                    lasttweekday = new Date(dt.getTime());
                    firstweekday.setDate(dt.getDate() - dt.getDay());
                    lasttweekday.setDate(dt.getDate() + (6 - dt.getDay()));
                    return firstweekday.toLocaleDateString() + "" - "" + lasttweekday.toLocaleDateString();
                }

                $('#weekselector').text(weektext());
                var incre");
                WriteLiteral(@"mentday = function (d) {
                    var ms = parseInt($('#currentdate').val());
                    var dt = new Date(ms);
                    dt.setDate(dt.getDate() + d);
                    $('#currentdate').val(dt.getTime());
                    $('#weekselector').text(weektext());
                    for (var i = 0; i < 7;i++)
                        reloadDay(i);
                }
                $('.dayselectback').click((e) => {
                    e.preventDefault();
                    incrementday(-7);
                });
                $('.dayselectforward').click((e) => {
                    e.preventDefault();
                    incrementday(7);
                });

                /*
                                $('.week-picker').datepicker( {
                                    showOtherMonths: true,
                                    selectOtherMonths: true,
                                    onSelect: function(dateText, inst) {
                         ");
                WriteLiteral(@"               var date = $(this).datepicker('getDate');
                                        startDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() - date.getDay());
                                        endDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() - date.getDay() + 6);
                                        var dateFormat = inst.settings.dateFormat || $.datepicker._defaults.dateFormat;
                                        $('#startDate').text($.datepicker.formatDate( dateFormat, startDate, inst.settings ));
                                        $('#endDate').text($.datepicker.formatDate( dateFormat, endDate, inst.settings ));

                                        selectCurrentWeek();
                                    },
                                    beforeShowDay: function(date) {
                                        var cssClass = '';
                                        if(date >= startDate && date <= endDate)
            ");
                WriteLiteral(@"                                cssClass = 'ui-datepicker-current-day';
                                        return [true, cssClass];
                                    },
                                    onChangeMonthYear: function(year, month, inst) {
                                        selectCurrentWeek();
                                    }
                                });

                                $('.week-picker .ui-datepicker-calendar tr').live('mousemove', function() { $(this).find('td a').addClass('ui-state-hover'); });
                                $('.week-picker .ui-datepicker-calendar tr').live('mouseleave', function() { $(this).find('td a').removeClass('ui-state-hover'); });
                */
            });

        </script>
    ");
                EndContext();
            }
            );
            BeginContext(8157, 4, true);
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
