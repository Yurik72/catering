#pragma checksum "C:\work1\CateringPro\CateringPro\Views\Pizzas\ListCategory.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f94ed836b1572e5587de59cf953bea860b3c1b4e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Pizzas_ListCategory), @"mvc.1.0.view", @"/Views/Pizzas/ListCategory.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Pizzas/ListCategory.cshtml", typeof(AspNetCore.Views_Pizzas_ListCategory))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f94ed836b1572e5587de59cf953bea860b3c1b4e", @"/Views/Pizzas/ListCategory.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4117863597b6ac24aaf098caef0bbe565acb1108", @"/Views/_ViewImports.cshtml")]
    public class Views_Pizzas_ListCategory : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Pizzas>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(27, 1, true);
            WriteLiteral("\n");
            EndContext();
#line 3 "C:\work1\CateringPro\CateringPro\Views\Pizzas\ListCategory.cshtml"
  
    var pageName = ViewBag.CurrentCategory;
    ViewData["Title"] = pageName;

#line default
#line hidden
            BeginContext(111, 5, true);
            WriteLiteral("\n<h2>");
            EndContext();
            BeginContext(117, 8, false);
#line 8 "C:\work1\CateringPro\CateringPro\Views\Pizzas\ListCategory.cshtml"
Write(pageName);

#line default
#line hidden
            EndContext();
            BeginContext(125, 14, true);
            WriteLiteral("</h2>\n<hr />\n\n");
            EndContext();
#line 11 "C:\work1\CateringPro\CateringPro\Views\Pizzas\ListCategory.cshtml"
 foreach (var pizza in Model)
{
    

#line default
#line hidden
            BeginContext(176, 35, false);
#line 13 "C:\work1\CateringPro\CateringPro\Views\Pizzas\ListCategory.cshtml"
Write(Html.Partial("PizzaSummary", pizza));

#line default
#line hidden
            EndContext();
#line 13 "C:\work1\CateringPro\CateringPro\Views\Pizzas\ListCategory.cshtml"
                                        
}

#line default
#line hidden
            BeginContext(214, 7, true);
            WriteLiteral("<br />\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Pizzas>> Html { get; private set; }
    }
}
#pragma warning restore 1591