using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using CorabeuControl.Components;
using CorabeuControl.Helpers;
using Newtonsoft.Json;


namespace AutomationSystem.Main.Web.Helpers
{
    /// <summary>
    /// Form extensions for HtmlHelper class
    /// </summary>
    public static class FormExtensions
    {

        // writes display name for java string
        // todo: ugly - remove to Ajax helper, utilize metadata for dispay name obtaining
        public static IHtmlString DisplayNameForJava<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> field)
        {
            var name = HttpUtility.HtmlDecode(html.DisplayNameFor(field).ToString());
            var javaString = HttpUtility.JavaScriptStringEncode(name);
            return html.Raw(javaString);
        }

        // creates table checkbox
        public static HtmlString TableCheckBox(this HtmlHelper html, string name, object value, bool isChecked, bool isDisabled = false, object htmlAttributes = null)
        {
            var result = new TagBuilder("input");
            result.MergeAttribute("type", "checkbox");
            result.MergeAttribute("name", name);
            result.MergeAttribute("value", value?.ToString());
            if (isChecked)
                result.MergeAttribute("checked", null);
            if (isDisabled)
                result.MergeAttribute("disabled", null);
            MergeAttributes(result, htmlAttributes);

            return new HtmlString(result.ToString(TagRenderMode.Normal));
        }


        // creates table checkbox for
        public static HtmlString TableCheckBoxFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> field, object value, bool isChecked, bool isDisabled = false, object htmlAttributes = null)
        {
            var fullName = html.NameFor(field).ToString();
            var name = TextHelper.GetNameFromHtmlFieldPrefix(fullName);
            return html.TableCheckBox(name, value, isChecked, isDisabled, htmlAttributes);
        }


        // adds constant into form
        public static HtmlString HiddenConstant(this HtmlHelper html, string name, object value)
        {
            var result = new TagBuilder("input");
            result.MergeAttribute("name", name);
            result.MergeAttribute("type", "hidden");
            result.MergeAttribute("value", value?.ToString());
            return new HtmlString(result.ToString(TagRenderMode.SelfClosing));
        }

        // adds constant into form
        public static HtmlString HiddenConstantWithId(this HtmlHelper html, string id, string name, object value)
        {
            var result = new TagBuilder("input");
            result.MergeAttribute("id", id);
            result.MergeAttribute("name", name);
            result.MergeAttribute("type", "hidden");
            result.MergeAttribute("value", value?.ToString());
            return new HtmlString(result.ToString(TagRenderMode.SelfClosing));
        }

        // adds constant containing array into form
        public static HtmlString HiddenLongArrayConstantWithId(this HtmlHelper html, string id, string name, IEnumerable<long> values)
        {
            var encoded = values == null ? "[]" : $"[{string.Join(",", values)}]";
            return html.HiddenConstantWithId(id, name, encoded);
        }


        // creates ddl typeahead source 
        public static HtmlString TypeaheadDropDownSource(this HtmlHelper html, string sourceName, IEnumerable<PickerItem> items)
        {
            var data = JsonConvert.SerializeObject(items.ToArray());
            var result = $"var {sourceName} = CorabeuControl.TypeaheadSourceFactory.getLocalSource(\"{sourceName}\", {data}, \"Text\", \"Value\");";
            return new HtmlString(result);
        }

        #region private helpers 

        // todo: move to some common helper
        // merges htmlAttributes that should be object or RouteValueDictionry
        private static void MergeAttributes(TagBuilder tagBuidler, object htmlAttributes)
        {
            if (htmlAttributes == null) return;
            var routeHtmlAttributes = (htmlAttributes as RouteValueDictionary) ?? new RouteValueDictionary(htmlAttributes);
            tagBuidler.MergeAttributes(routeHtmlAttributes);
        }

        #endregion

    }

}