#pragma checksum "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Dishes\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "569d6a1ef54e3289df85ef98eb2d9295b99c200a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Dishes_Index), @"mvc.1.0.view", @"/Views/Dishes/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Dishes/Index.cshtml", typeof(AspNetCore.Views_Dishes_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"569d6a1ef54e3289df85ef98eb2d9295b99c200a", @"/Views/Dishes/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4117863597b6ac24aaf098caef0bbe565acb1108", @"/Views/_ViewImports.cshtml")]
    public class Views_Dishes_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<CateringPro.Models.Dish>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(45, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Dishes\Index.cshtml"
  
    ViewData["Title"] = "Index";

#line default
#line hidden
            BeginContext(88, 1078, true);
            WriteLiteral(@"

      
<div class=""container"">
            <div class=""row"">
                <div id=""create-btn"" class=""col-lg-3 col-md-3 create"">
                    <span class="" glyphicon glyphicon-plus"" aria-hidden=""true"">Create</span>

                </div>
            </div>
</div>
<div class=""container"">
    <div class=""row"">
        <div class=""col-md-6"">
            <div id=""custom-search-input"">
                <div class=""input-group col-md-12"">
                    <input type=""text"" id=""search-val"" class=""form-control input-lg"" placeholder=""Dish"" />
                    <span class=""input-group-btn"">
                        <button class=""btn btn-info btn-lg"" id=""search-btn"" type=""button"">
                            <i class=""glyphicon glyphicon-search""></i>
                        </button>
                    </span>
                </div>
            </div>
        </div>
    </div>
</div>
<div id=""modDialog"" class=""modal fade"">
    <div id=""dialogContent"" class=""modal-dialog""><");
            WriteLiteral("/div>\r\n</div>\r\n<div id=\"table-content\">\r\n \r\n</div>\r\n\r\n");
            EndContext();
            DefineSection("scripts", async() => {
                BeginContext(1185, 311, true);
                WriteLiteral(@"
    <script type=""text/javascript"">

        $(function () {

            var reload = function () {
                 $('#table-content').load('/Dishes/ListItems?searchcriteria='+$('#search-val').val());
            }
            $.ajaxSetup({ cache: false });
            //$('#table-content').load('");
                EndContext();
                BeginContext(1497, 61, false);
#line 50 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Dishes\Index.cshtml"
                                   Write(Url.Action("ListItems","Dishes", new { searchcriteria = "" }));

#line default
#line hidden
                EndContext();
                BeginContext(1558, 452, true);
                WriteLiteral(@"');
            reload();
            $('#search-btn').click(function (e) {
                reload();
            });
            $('#custom-search-input').keydown((event) => {
                if (event.which == 13) {
                    event.preventDefault();
                    reload();
                }
            });
            $('#create-btn').click(function (e) {
                e.preventDefault();
                var url = '");
                EndContext();
                BeginContext(2011, 35, false);
#line 63 "C:\Users\yurik\source\repos\Ferolka\catering\CateringPro\Views\Dishes\Index.cshtml"
                      Write(Url.Content("~/Dishes/CreateModal"));

#line default
#line hidden
                EndContext();
                BeginContext(2046, 1835, true);
                WriteLiteral(@"';
                $.get(url, function (data) {
                    $('#dialogContent').html(data);
                    $('#modDialog').modal('show');
                });
            });
            $(document).on( ""click"", ""a.dishitem"", function(e) {
               e.preventDefault();

                $.get(this.href, function (data) {
                    $('#dialogContent').html(data);
                    $('#modDialog').modal('show');
                });
            });
             $(document).on( ""click"", ""a.dishitem"", function(e) {
               e.preventDefault();

                $.get(this.href, function (data) {
                    $('#dialogContent').html(data);
                    $('#modDialog').modal('show');
                });
             });
              $(document).on('click', '[data-save=""modal""]', function (event) {
                    event.preventDefault();

                    var form = $(this).parents('.modal-body').find('form');
                    var ac");
                WriteLiteral(@"tionUrl = form.attr('action');
                    var dataToSend = form.serialize();

                  $.post(actionUrl, dataToSend).done(function (data) {
                      var isValid = false;

                      if (data && data.res && data.res== ""OK"")
                                isValid = true;

                      if (isValid) {
                          $('#modDialog').modal('hide');
                          $('#dialogContent').empty();
                          reload();
                      }
                      else {
                          var newBody = $('.modal-body', data);
                          $(document).find('.modal-body').replaceWith(newBody);
                      }
                    });
                });
        })
    </script>
");
                EndContext();
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<CateringPro.Models.Dish>> Html { get; private set; }
    }
}
#pragma warning restore 1591
