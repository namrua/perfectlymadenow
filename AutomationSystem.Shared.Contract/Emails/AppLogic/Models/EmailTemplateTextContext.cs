using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    /// <summary>
    /// Encapsulates context information for integrating email text edit administration to other administration pages
    /// </summary>
    public class EmailTemplateTextContext
    {

        // public properties       
        public string Title { get; set; }
        public string ActiveMainMenuItemId { get; set; }
        public EntityTypeEnum? TestEmailEntityTypeId { get; set; }          // value for EmailTestSendInfo.EntityTypeId (fills test Email.EntityTypeId in DB)
        public long? TestEmailEntityId { get; set; }                        // value for EmailTestSendInfo.EntityId (fills text Email.EntityId in DB)
        public EntityTypeEnum? ParameterEntityTypeId { get; set; }          // defines entityTypeId for underlying parameter resolver used by test email sending
        public long? ParameterEntityId { get; set; }                        // defines entityId for underlying parameter resolver used by test email sending


        #region static factory

        // new object factory
        public static EmailTemplateTextContext New()
        {
            return new EmailTemplateTextContext();
        }

        #endregion


        #region extensions

        // adds source page title and main menu item id
        public EmailTemplateTextContext AddTitleAndMenuItem(string title, string mainMenuItemId)
        {
            Title = title;
            ActiveMainMenuItemId = mainMenuItemId;
            return this;
        }

        // adds test email entity 
        public EmailTemplateTextContext AddTestEmailEnity(EntityTypeEnum entityTypeId, long entityId)
        {
            TestEmailEntityTypeId = entityTypeId;
            TestEmailEntityId = entityId;
            return this;
        }

        // adds parameter entity
        public EmailTemplateTextContext AddParameterEntity(EntityTypeEnum entityTypeId, long entityId)
        {
            ParameterEntityTypeId = entityTypeId;
            ParameterEntityId = entityId;
            return this;
        }

        #endregion

    }
}
