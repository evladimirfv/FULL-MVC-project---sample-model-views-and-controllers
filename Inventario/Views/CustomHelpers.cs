using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Inventario.Views
{
    public static class CustomHtmlHelepers
    {
        public static IHtmlString ImageActionLink(this HtmlHelper htmlHelper, string linkText, string action, string controller, object routeValues, object htmlAttributes,string imageSrc)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var img = new TagBuilder("img");
            img.Attributes.Add("src", VirtualPathUtility.ToAbsolute(imageSrc));
            var anchor = new TagBuilder("a") { InnerHtml = img.ToString(TagRenderMode.SelfClosing) };
            anchor.Attributes["href"] = urlHelper.Action(action, controller, routeValues);
            anchor.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(anchor.ToString());

        }


        public static MvcHtmlString CheckBoxIntFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, sbyte?>> expression, object htmlAttributes, string isDisabled = "false")
        {
            // get the name of the property
            string[] propertyNameParts = expression.Body.ToString().Split('.');

            // create name and id for the control
            string controlId = String.Join("_", propertyNameParts.Skip(1)).Replace(")","");
            string controlName = String.Join(".", propertyNameParts.Skip(1)).Replace(")", "");

            // get the value of the property

            sbyte? booleanSbyte;
            try
            {
                Func<TModel, sbyte?> compiled = expression.Compile();
                if (html.ViewData.Model == null)
                    booleanSbyte = 0;
                else
                    booleanSbyte = compiled(html.ViewData.Model);
            }
            catch
            {
                booleanSbyte = 0;
            }




            // convert it to a boolean
            bool isChecked = booleanSbyte == 1;

            // build input element
            TagBuilder checkbox = new TagBuilder("input");
            checkbox.MergeAttribute("id", controlId);
            checkbox.MergeAttribute("name", controlName);
            checkbox.MergeAttribute("type", "checkbox");
            if (isDisabled == "true")
            { 
                checkbox.MergeAttribute("disabled", isDisabled);
            }

            if (isChecked)
            {
                checkbox.MergeAttribute("checked", "checked");
                checkbox.MergeAttribute("value", "1");
            }
            else
            {
                checkbox.MergeAttribute("value", "0");
            }
            SetStyle(checkbox, htmlAttributes);

            // script to handle changing selection
            string script = "<script>" +
                                "$('#" + controlId + "').change(function () { " +
                                    "if ($('#" + controlId + "').is(':checked')) " +
                                        "$('#" + controlId + "').val('1'); " +
                                    "else " +
                                        "$('#" + controlId + "').val('0'); " +
                                "}); " +
                            "</script>";

            return MvcHtmlString.Create(checkbox.ToString(TagRenderMode.SelfClosing) + script);
        }

        private static void SetStyle(TagBuilder control, object htmlAttributes)
        {
            if (htmlAttributes == null)
                return;

            // get htmlAttributes
            Type t = htmlAttributes.GetType();
            PropertyInfo classInfo = t.GetProperty("class");
            PropertyInfo styleInfo = t.GetProperty("style");
            string cssClasses = classInfo?.GetValue(htmlAttributes)?.ToString();
            string style = styleInfo?.GetValue(htmlAttributes)?.ToString();

            if (!string.IsNullOrEmpty(style))
                control.MergeAttribute("style", style);
            if (!string.IsNullOrEmpty(cssClasses))
                control.AddCssClass(cssClasses);
        }

    }

}



   