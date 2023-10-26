using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Gui.Helpers;
using CorabeuControl.Helpers;

namespace AutomationSystem.Main.Web.Helpers
{
    /// <summary>
    /// Web HTML extensions
    /// </summary>
    public static class WebHtmlExtensions
    {

        /// <summary>
        /// Creates class category badge
        /// </summary>
        public static IHtmlString ClassCategoryBadge(this HtmlHelper html, ClassCategoryEnum classCategoryId)
        {
            var tagBuilder = new TagBuilder("small");
            switch (classCategoryId)
            {
                case ClassCategoryEnum.DistanceClass:
                    tagBuilder.MergeAttribute("class", "badge badge-secondary");
                    tagBuilder.SetInnerText("WWA");
                    break;

                case ClassCategoryEnum.PrivateMaterialClass:
                    tagBuilder.MergeAttribute("class", "badge badge-dark");
                    tagBuilder.SetInnerText("PMC");
                    break;

                default:
                    return new HtmlString("");

            }

            return html.Raw(tagBuilder.ToString(TagRenderMode.Normal));
        }


        /// <summary>
        /// Creates class state badge
        /// </summary>
        public static IHtmlString ClassStateBadge(this HtmlHelper html, ClassState classState)
        {
            var tagBuilder = new TagBuilder("small");
            tagBuilder.MergeAttribute("class", GetClassStateBadgeStyle(classState).Render());
            tagBuilder.SetInnerText(MainTextHelper.GetClassStateText(classState));
            return html.Raw(tagBuilder.ToString(TagRenderMode.Normal));

        }

        /// <summary>
        /// Creates distance class template state badge
        /// </summary>
        public static IHtmlString DistanceClassTemplateStateBadge(this HtmlHelper html, DistanceClassTemplateState templateState)
        {
            var tagBuilder = new TagBuilder("small");
            tagBuilder.MergeAttribute("class", GetDistanceClassTemplateStateBadgeStyle(templateState).Render());
            tagBuilder.SetInnerText(MainTextHelper.GetDistanceClassTemplateStateText(templateState));
            return html.Raw(tagBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Creates registration state badge
        /// </summary>
        public static IHtmlString RegistrationStateBadge(this HtmlHelper html, RegistrationState registrationState)
        {
            var tagBuilder = new TagBuilder("small");
            tagBuilder.MergeAttribute("class", GetRegistrationStateBadgeStyle(registrationState).Render());
            tagBuilder.SetInnerText(MainTextHelper.GetRegistrationStateText(registrationState));
            return html.Raw(tagBuilder.ToString(TagRenderMode.Normal));

        }

        /// <summary>
        /// Creates small loader
        /// </summary>
        public static IHtmlString LoaderSmall(this HtmlHelper html, string id, string cssClass)
        {
            var result = new StringBuilder();
            var divSpinner = new TagBuilder("div");
            divSpinner.MergeAttribute("class", cssClass);
            divSpinner.MergeAttribute("id", id);
            result.AppendLine(divSpinner.ToString(TagRenderMode.StartTag));

            var divFirstDot = new TagBuilder("div");
            divFirstDot.MergeAttribute("class", "dot3");
            result.AppendLine(divFirstDot.ToString(TagRenderMode.Normal));
            
            var divSecondDot = new TagBuilder("div");
            divSecondDot.MergeAttribute("class", "dot4");
            result.AppendLine(divSecondDot.ToString(TagRenderMode.Normal));            

            result.AppendLine(divSpinner.ToString(TagRenderMode.EndTag));
            return html.Raw(result.ToString());
        }

        /// <summary>
        /// Creates large loader
        /// </summary>
        public static IHtmlString LoaderLarge(this HtmlHelper html, string id, string cssClass)
        {
            var result = new StringBuilder();
            var divSpinner = new TagBuilder("div");
            divSpinner.MergeAttribute("class", cssClass);
            divSpinner.MergeAttribute("id", id);
            result.AppendLine(divSpinner.ToString(TagRenderMode.StartTag));

            var divFirstDot = new TagBuilder("div");
            divFirstDot.MergeAttribute("class", "dot1");
            result.AppendLine(divFirstDot.ToString(TagRenderMode.StartTag));
            result.AppendLine(divFirstDot.ToString(TagRenderMode.EndTag));

            var divSecondDot = new TagBuilder("div");
            divSecondDot.MergeAttribute("class", "dot2");
            result.AppendLine(divSecondDot.ToString(TagRenderMode.StartTag));
            result.AppendLine(divSecondDot.ToString(TagRenderMode.EndTag));

            result.AppendLine(divSpinner.ToString(TagRenderMode.EndTag));
            return html.Raw(result.ToString());
        }


        #region private methods

        // gets class state badge style
        private static CssClassHelper GetClassStateBadgeStyle(ClassState classState)
        {
            var result = CssClassHelper.New().Add("badge");
            switch (classState)
            {
                case ClassState.New:
                    result.Add("badge-light");
                    break;
                case ClassState.InRegistration:
                    result.Add("badge-success");
                    break;
                case ClassState.AfterRegistration:
                    result.Add("badge-warning");
                    break;
                case ClassState.Canceled:
                    result.Add("badge-danger");
                    break;
                case ClassState.Completed:
                    result.Add("badge-primary");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(classState), classState, null);
            }
            return result;
        }

        // gets distance class template state badge style
        private static CssClassHelper GetDistanceClassTemplateStateBadgeStyle(DistanceClassTemplateState templateState)
        {
            var result = CssClassHelper.New().Add("badge");
            switch (templateState)
            {
                case DistanceClassTemplateState.New:
                    result.Add("badge-light");
                    break;
                case DistanceClassTemplateState.Approved:
                    result.Add("badge-success");
                    break;
                case DistanceClassTemplateState.Completed:
                    result.Add("badge-primary");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(templateState), templateState, null);
            }
            return result;
        }

        // gets registraton state badge style
        private static CssClassHelper GetRegistrationStateBadgeStyle(RegistrationState registrationState)
        {
            var result = CssClassHelper.New().Add("badge");
            switch (registrationState)
            {
                case RegistrationState.Temporary:
                    result.Add("badge-secondary");
                    break;
                case RegistrationState.New:
                    result.Add("badge-light");
                    break;
                case RegistrationState.Approved:
                    result.Add("badge-primary");
                    break;
                                    
                case RegistrationState.Canceled:
                    result.Add("badge-danger");
                    break;             
                default:
                    throw new ArgumentOutOfRangeException(nameof(registrationState), registrationState, null);
            }
            return result;
        }

        #endregion

    }

}