#pragma checksum "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b5326f78ebfbac4c0975afd8d80fc2559a298632"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Reviews_AdminIndex), @"mvc.1.0.view", @"/Views/Reviews/AdminIndex.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Reviews/AdminIndex.cshtml", typeof(AspNetCore.Views_Reviews_AdminIndex))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b5326f78ebfbac4c0975afd8d80fc2559a298632", @"/Views/Reviews/AdminIndex.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4117863597b6ac24aaf098caef0bbe565acb1108", @"/Views/_ViewImports.cshtml")]
    public class Views_Reviews_AdminIndex : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Reviews>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Create", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-success"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Edit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Delete", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            BeginContext(28, 1, true);
            WriteLiteral("\n");
            EndContext();
#line 3 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
  
    var pageName = "Reviews";
    ViewData["Title"] = pageName;

#line default
#line hidden
            BeginContext(98, 5, true);
            WriteLiteral("\n<h2>");
            EndContext();
            BeginContext(104, 8, false);
#line 8 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
Write(pageName);

#line default
#line hidden
            EndContext();
            BeginContext(112, 23, true);
            WriteLiteral("</h2>\n\n    <p>\n        ");
            EndContext();
            BeginContext(135, 61, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b5326f78ebfbac4c0975afd8d80fc2559a2986325460", async() => {
                BeginContext(182, 10, true);
                WriteLiteral("Create New");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(196, 110, true);
            WriteLiteral("\n    </p>\n    <table class=\"table\">\n        <thead>\n            <tr>\n                <th>\n                    ");
            EndContext();
            BeginContext(307, 41, false);
#line 17 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
               Write(Html.DisplayNameFor(model => model.Title));

#line default
#line hidden
            EndContext();
            BeginContext(348, 64, true);
            WriteLiteral("\n                </th>\n                <th>\n                    ");
            EndContext();
            BeginContext(413, 47, false);
#line 20 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
               Write(Html.DisplayNameFor(model => model.Description));

#line default
#line hidden
            EndContext();
            BeginContext(460, 64, true);
            WriteLiteral("\n                </th>\n                <th>\n                    ");
            EndContext();
            BeginContext(525, 41, false);
#line 23 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
               Write(Html.DisplayNameFor(model => model.Grade));

#line default
#line hidden
            EndContext();
            BeginContext(566, 64, true);
            WriteLiteral("\n                </th>\n                <th>\n                    ");
            EndContext();
            BeginContext(631, 40, false);
#line 26 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
               Write(Html.DisplayNameFor(model => model.Date));

#line default
#line hidden
            EndContext();
            BeginContext(671, 64, true);
            WriteLiteral("\n                </th>\n                <th>\n                    ");
            EndContext();
            BeginContext(736, 41, false);
#line 29 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
               Write(Html.DisplayNameFor(model => model.Pizza));

#line default
#line hidden
            EndContext();
            BeginContext(777, 64, true);
            WriteLiteral("\n                </th>\n                <th>\n                    ");
            EndContext();
            BeginContext(842, 40, false);
#line 32 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
               Write(Html.DisplayNameFor(model => model.User));

#line default
#line hidden
            EndContext();
            BeginContext(882, 100, true);
            WriteLiteral("\n                </th>\n                <th></th>\n            </tr>\n        </thead>\n        <tbody>\n");
            EndContext();
#line 38 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
             foreach (var item in Model) {

#line default
#line hidden
            BeginContext(1025, 70, true);
            WriteLiteral("                <tr>\n                    <td>\n                        ");
            EndContext();
            BeginContext(1096, 40, false);
#line 41 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
                   Write(Html.DisplayFor(modelItem => item.Title));

#line default
#line hidden
            EndContext();
            BeginContext(1136, 76, true);
            WriteLiteral("\n                    </td>\n                    <td>\n                        ");
            EndContext();
            BeginContext(1213, 46, false);
#line 44 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
                   Write(Html.DisplayFor(modelItem => item.Description));

#line default
#line hidden
            EndContext();
            BeginContext(1259, 76, true);
            WriteLiteral("\n                    </td>\n                    <td>\n                        ");
            EndContext();
            BeginContext(1336, 40, false);
#line 47 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
                   Write(Html.DisplayFor(modelItem => item.Grade));

#line default
#line hidden
            EndContext();
            BeginContext(1376, 76, true);
            WriteLiteral("\n                    </td>\n                    <td>\n                        ");
            EndContext();
            BeginContext(1453, 39, false);
#line 50 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
                   Write(Html.DisplayFor(modelItem => item.Date));

#line default
#line hidden
            EndContext();
            BeginContext(1492, 76, true);
            WriteLiteral("\n                    </td>\n                    <td>\n                        ");
            EndContext();
            BeginContext(1569, 45, false);
#line 53 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
                   Write(Html.DisplayFor(modelItem => item.Pizza.Name));

#line default
#line hidden
            EndContext();
            BeginContext(1614, 76, true);
            WriteLiteral("\n                    </td>\n                    <td>\n                        ");
            EndContext();
            BeginContext(1691, 45, false);
#line 56 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
                   Write(Html.DisplayFor(modelItem => item.User.Email));

#line default
#line hidden
            EndContext();
            BeginContext(1736, 76, true);
            WriteLiteral("\n                    </td>\n                    <td>\n                        ");
            EndContext();
            BeginContext(1812, 53, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b5326f78ebfbac4c0975afd8d80fc2559a29863212763", async() => {
                BeginContext(1857, 4, true);
                WriteLiteral("Edit");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 59 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
                                               WriteLiteral(item.Id);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1865, 27, true);
            WriteLiteral(" |\n                        ");
            EndContext();
            BeginContext(1892, 59, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b5326f78ebfbac4c0975afd8d80fc2559a29863215132", async() => {
                BeginContext(1940, 7, true);
                WriteLiteral("Details");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 60 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
                                                  WriteLiteral(item.Id);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1951, 27, true);
            WriteLiteral(" |\n                        ");
            EndContext();
            BeginContext(1978, 57, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b5326f78ebfbac4c0975afd8d80fc2559a29863217507", async() => {
                BeginContext(2025, 6, true);
                WriteLiteral("Delete");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 61 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
                                                 WriteLiteral(item.Id);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2035, 49, true);
            WriteLiteral("\n                    </td>\n                </tr>\n");
            EndContext();
#line 64 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Reviews\AdminIndex.cshtml"
}

#line default
#line hidden
            BeginContext(2086, 30, true);
            WriteLiteral("        </tbody>\n    </table>\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Reviews>> Html { get; private set; }
    }
}
#pragma warning restore 1591
