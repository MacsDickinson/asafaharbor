using System;
using System.Collections.Generic;
using System.Linq;
using Asafaharbor.Web.Models;
using Nancy.ViewEngines.Razor;

namespace Asafaharbor.Web.HTMLExtensions
{
    public static class HtmlExtensions
    {
        public static IHtmlString CheckBox<T>(this HtmlHelpers<T> helper, string name, dynamic modelProperty)
        {
            string input;
            bool checkedState;

            if (!bool.TryParse(modelProperty.ToString(), out checkedState))
            {
                input = "<input name=\"" + name + "\" type=\"checkbox\" value=\"true\" />";
            }
            else
            {
                if (checkedState)
                    input = "<input name=\"" + name + "\" type=\"checkbox\" value=\"true\" checked />";
                else
                    input = "<input name=\"" + name + "\" type=\"checkbox\" value=\"true\" />";
            }


            return new NonEncodedHtmlString(input);
        }

        public static IHtmlString ValidationSummary<T>(this HtmlHelpers<T> helper, List<ErrorModel> errors)
        {

            if (!errors.Any())
                return new NonEncodedHtmlString("");

            string div = errors.Aggregate("<div class=\"error\">", (current, error) => current + ("<span class='error-message error'>" + error.ErrorMessage + "</span>"));

            div += "</div>";

            return new NonEncodedHtmlString(div);
        }

        public static IHtmlString ValidationMessageFor<T>(this HtmlHelpers<T> helper, List<ErrorModel> errors, string propertyName)
        {
            if (!errors.Any())
                return new NonEncodedHtmlString("");

            string span = String.Empty;

            foreach (var item in errors.Where(item => item.Name == propertyName))
            {
                span += "<div class=\"error\"><span class=\"error-message error\">" + item.ErrorMessage + "</span></div>";
                break;
            }

            return new NonEncodedHtmlString(span);
        }

        public static bool IsDebug<T>(this HtmlHelpers<T> helper)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}