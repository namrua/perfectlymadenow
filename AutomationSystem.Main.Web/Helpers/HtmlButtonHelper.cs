using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CorabeuControl.Components;
using CorabeuControl.Helpers;

namespace AutomationSystem.Main.Web.Helpers
{
    /// <summary>
    /// Creates html buttons
    /// </summary>
    public class HtmlButtonHelper : IDisposable
    {

        // private fields
        private readonly TextWriter requestOutput;
        private readonly TagBuilder groupTag;
        private readonly ButtonGroupType buttonGroupType;       

        // constructor
        public HtmlButtonHelper(TextWriter requestOutput, ButtonGroupType buttonGroupType)
        {
            this.requestOutput = requestOutput;
            this.buttonGroupType = buttonGroupType;
            this.groupTag = CreateGroupTag();

            WriteStartTag();           
        }


        // creates submit button
        public HtmlString Submit(string text, ButtonStyle style = ButtonStyle.Primary, object htmlAttributes = null)
        {
            var result = CreateButtonTag(text, "submit", style, false, htmlAttributes);
            return new HtmlString(result.ToString(TagRenderMode.Normal));
        }

        // creates submit button
        public HtmlString Submit(string text, ButtonStyle style, bool isDisabled, object htmlAttributes = null)
        {
            var result = CreateButtonTag(text, "submit", style, isDisabled, htmlAttributes);
            return new HtmlString(result.ToString(TagRenderMode.Normal));
        }

        // returns link
        public HtmlString Link(string text, ButtonStyle style, string url, bool isDisabled = false,
            object htmlAttributes = null)
        {
            var result = CreateLinkTag(text, url, style, isDisabled, htmlAttributes);          
            return new HtmlString(result.ToString(TagRenderMode.Normal));
        }

        // returns link
        public HtmlString Link(string text, string linkGroup, ButtonStyle style, string url, bool isDisabled = false,
            object htmlAttributes = null)
        {
            var result = CreateLinkTag(text, url, style, isDisabled, htmlAttributes);

            if (!string.IsNullOrEmpty(linkGroup))
                result.AddCssClass(TextHelper.DecorateName(linkGroup, "link"));

            return new HtmlString(result.ToString(TagRenderMode.Normal));
        }


        // returns command button
        public HtmlString Command(string text, ButtonStyle style, bool isDisabled, object htmlAttributes = null)
        {
            return Command(text, style, isDisabled, null, null, htmlAttributes);
        }

        // returns command button
        public HtmlString Command(string text, ButtonStyle style, bool isDisabled, string commandName = null,
            object commandId = null, object htmlAttributes = null)
        {
            return Command(text, null, style, isDisabled, commandName, commandId, htmlAttributes);
        }

        // returns command button with command class
        public HtmlString Command(string text, string commandGroup, ButtonStyle style, bool isDisabled,
            string commandName = null, object commandId = null, object htmlAttributes = null)
        {
            var result = CreateButtonTag(text, "button", style, isDisabled, htmlAttributes);

            if (!string.IsNullOrEmpty(commandGroup))
                result.AddCssClass(TextHelper.DecorateName(commandGroup, "command"));
            if (!string.IsNullOrEmpty(commandName))
                result.MergeAttribute("data-command", commandName);
            if (commandId != null)
                result.MergeAttribute("data-id", commandId.ToString());

            return new HtmlString(result.ToString(TagRenderMode.Normal));
        }

        // returns dropdown list with links button
        public HtmlString DropdownLink(string text, ButtonStyle style, string id, bool isDisabled, IEnumerable<PickerItem> items, bool forcePopup = false)
        {
            var itemsEnumerated = items.ToList();
            if (isDisabled || !itemsEnumerated.Any())
            {
                var disabledResult = CreateLinkTag(text, "#", style, true, new { id });
                return new HtmlString(disabledResult.ToString(TagRenderMode.Normal));
            }

            // when there is only one item, popup us omitted
            if (!forcePopup && itemsEnumerated.Count == 1)
            {
                return Link(text, style, itemsEnumerated[0].Value, false, new { id });
            }
                
            // constructs dropdown link button            
            var tagItems = new List<TagBuilder>();
            foreach (var item in itemsEnumerated)
            {
                var linkItem = new TagBuilder("a");
                linkItem.MergeAttribute("class", "dropdown-item");
                linkItem.MergeAttribute("href", item.Value);
                linkItem.SetInnerText(item.Text);
                tagItems.Add(linkItem);
            }
            var result = CreateDropdownButton(id, text, style, tagItems);            
            return result;
        }


        // returns dropdown list with links button
        public HtmlString DropdownCommand(string text, ButtonStyle style, string id, string commandGroup, bool isDisabled, IEnumerable<PickerItem> items)
        {
            if (isDisabled)
            {
                var disabledResult = CreateButtonTag(text, "button", style, true, new { id});
                return new HtmlString(disabledResult.ToString(TagRenderMode.Normal));
            }

            // constructs dropdown link button
            var tagItems = new List<TagBuilder>();
            foreach (var item in items)
            {
                var commandItem = new TagBuilder("a");
                commandItem.MergeAttribute("class", $"dropdown-item {TextHelper.DecorateName(commandGroup, "command")}");
                commandItem.MergeAttribute("href", "#");
                commandItem.MergeAttribute("data-id", item.Value);
                commandItem.SetInnerText(item.Text);
                tagItems.Add(commandItem);
            }
            var result = CreateDropdownButton(id, text, style, tagItems);
            return result;
        }


        #region open-close tags

        // writes start tag to view
        private void WriteStartTag()
        {
            var startTag = groupTag.ToString(TagRenderMode.StartTag);
            requestOutput.WriteLine(startTag);
        }


        // writes end tag to view
        public void Dispose()
        {
            var endTag = groupTag.ToString(TagRenderMode.EndTag);
            requestOutput.WriteLine(endTag);
        }

        #endregion


        #region html factory

        // encapsulates creation of dropdown button
        private HtmlString CreateDropdownButton(string id, string text, ButtonStyle style, List<TagBuilder> items)
        {
            // constructs dropdown link button
            var result = new StringBuilder();
            var divGroup = new TagBuilder("div");
            divGroup.MergeAttribute("class", "btn-group");
            divGroup.MergeAttribute("role", "group");
            result.AppendLine(divGroup.ToString(TagRenderMode.StartTag));

            var buttonTag = new TagBuilder("button");
            buttonTag.MergeAttribute("id", id);
            buttonTag.MergeAttribute("type", "button");
            buttonTag.MergeAttribute("class", "dropdown-toggle");
            AddButtonCss(buttonTag, style);
            buttonTag.MergeAttribute("data-toggle", "dropdown");
            buttonTag.MergeAttribute("aria-haspopup", "true");
            buttonTag.MergeAttribute("aria-expanded", "false");
            buttonTag.SetInnerText(text);
            result.AppendLine(buttonTag.ToString(TagRenderMode.Normal));

            var divDropdown = new TagBuilder("div");
            divDropdown.MergeAttribute("class", "dropdown-menu");
            divDropdown.MergeAttribute("aria-labelledby", id);
            result.AppendLine(divDropdown.ToString(TagRenderMode.StartTag));

            foreach (var item in items)
            {               
                result.AppendLine(item.ToString(TagRenderMode.Normal));
            }

            result.AppendLine(divDropdown.ToString(TagRenderMode.EndTag));
            result.AppendLine(divGroup.ToString(TagRenderMode.EndTag));
            return new HtmlString(result.ToString());
        }

        #endregion


        #region tag factory        

        // creates button
        private TagBuilder CreateButtonTag(string text, string type, ButtonStyle style, bool isDisabled, object htmlAttributes = null)
        {
            var result = new TagBuilder("button");
            result.MergeAttribute("type", type);
            if (isDisabled)
                result.MergeAttribute("disabled", null);

            AddButtonCss(result, style);
            if (htmlAttributes != null)
            {
                var dictionaryAttributes = (htmlAttributes as IDictionary<string, object>) ?? new RouteValueDictionary(htmlAttributes);
                result.MergeAttributes(dictionaryAttributes);
            }

            result.SetInnerText(text);
            return result;
        }

        // creates link
        private TagBuilder CreateLinkTag(string text, string url, ButtonStyle style, bool isDisabled, object htmlAttributes = null)
        {
            var result = new TagBuilder("a");
            result.MergeAttribute("href", isDisabled ? "#" : url);
            result.MergeAttribute("role", "button");

            if (isDisabled)
            {
                result.MergeAttribute("aria-disabled", "true");
                result.AddCssClass("disabled");                
            }

            AddButtonCss(result, style);
            if (htmlAttributes != null)
                result.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            result.SetInnerText(text);
            return result;
        }


        // adds button styles
        private void AddButtonCss(TagBuilder tag, ButtonStyle style)
        {
            // adds styles           
            switch (style)
            {
                case ButtonStyle.Primary:
                    tag.AddCssClass("btn-primary");
                    break;
                case ButtonStyle.Normal:
                    tag.AddCssClass("btn-outline-primary");
                    break;
                case ButtonStyle.FilterPrimary:
                    tag.AddCssClass("btn-info");
                    break;
                case ButtonStyle.FilterNormal:
                    tag.AddCssClass("btn-outline-info");
                    break;
                case ButtonStyle.PublicPrimary:
                    tag.AddCssClass("btn-public");
                    break;
                case ButtonStyle.PublicNormal:
                    tag.AddCssClass("btn-outline-public");
                    break;
            }
            tag.AddCssClass("btn");

        }


        // creates group tag
        private TagBuilder CreateGroupTag()
        {           
            var result = new TagBuilder("div");                              
            switch (buttonGroupType)
            {
                case ButtonGroupType.BlockItemGroup:
                    result.AddCssClass("btn-group-item-block");
                    result.AddCssClass("btn-group-sm");
                    break;
                case ButtonGroupType.ItemRightGroup:
                    result.AddCssClass("btn-group-item-right");
                    result.AddCssClass("btn-group-sm");
                    break;
                case ButtonGroupType.TopCommandsGroup:
                    result.AddCssClass("btn-group-top-command");
                    result.AddCssClass("btn-group");
                    break;
                case ButtonGroupType.PagePartGroup:
                    result.AddCssClass("btn-group-page-part");                   
                    break;
                case ButtonGroupType.FilterGroup:
                    result.AddCssClass("btn-group-filter");
                    result.AddCssClass("btn-group-sm");
                    break;
            }
            result.AddCssClass("btn-group");
            return result;
        }

        #endregion

    }


}
