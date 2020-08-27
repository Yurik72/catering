using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CateringPro.Core
{

    public abstract class CatBasePage<TModel> : RazorPage<TModel>
    {
        // [RazorInject]
        public override void Write(string value)
        {
            base.Write(value);
        }
        /*
        public override void Write(object value)
        {
            TagHelperOutput tout = value as TagHelperOutput;
            if (false && tout != null)
            {
                var html = tout.Content.ToString();
                html = REGEX_TAGS.Replace(html, "> <");
                html = REGEX_ALL.Replace(html, " ");
                tout.Content.SetHtmlContent(html);
            }
               
            base.Write(value);
            if (false  && value != null)
            {
                var html = value.ToString();
                html = REGEX_TAGS.Replace(html, "> <");
                html = REGEX_ALL.Replace(html, " ");
              //  if (value is MvcHtmlString) value = new MvcHtmlString(html);
               // else 
                    value = html;
            }
           // base.Write(value);
        }
        */
        public override void WriteLiteral(string  value)
        {
             var  html = REGEX_TAGS.Replace(value, "> <");
              html = REGEX_ALL.Replace(html, " ");
             base.WriteLiteral(html);
           // base.WriteLiteral(value);
        }
        /*
        public override void WriteLiteral(object value)
        {
            if (value != null)
            {
                var html = value.ToString();
                html = REGEX_TAGS.Replace(html, "> <");
                html = REGEX_ALL.Replace(html, " ");
                if (value is MvcHtmlString) value = new MvcHtmlString(html);
               else 
                    value = html;
            }
            base.WriteLiteral(value);
        }
        */
        private static readonly Regex REGEX_TAGS = new Regex(@">\s+<", RegexOptions.Compiled);
        // private static readonly Regex REGEX_ALL = new Regex(@"\s+|\t\s+|\n\s+|\r\s+", RegexOptions.Compiled);

        private static readonly Regex REGEX_ALL = new Regex(@"[ ]+", RegexOptions.Compiled);
    }
}
