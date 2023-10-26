namespace AutomationSystem.Shared.Core.HtmlValidation.System
{
    /// <summary>
    /// Factory for html validators
    /// </summary>
    public interface IHtmlValidatorFactory
    {

        // returns app localisation validator
        IHtmlValidator GetAppLocalisationValidator();
        
        // gets email template validator
        IHtmlValidator GetEmailTemplateValidator();

    }

}
