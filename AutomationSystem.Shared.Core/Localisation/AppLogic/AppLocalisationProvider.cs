using System;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;
using AutomationSystem.Shared.Contract.Localisation.System;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Shared.Core.Localisation.AppLogic
{
    /// <summary>
    /// App Localisation provider
    /// </summary>
    public class AppLocalisationProvider : ICorabeuLocalisationProvider
    {
        private readonly ILocalisationService localisationService;

        // provider type
        public Type AttributeType => typeof(LocalisedTextAttribute);

        // provider name       
        public string Name => LocalisedTextAttribute.ProviderKey;

        public AppLocalisationProvider(ILocalisationService localisationService)
        {
            this.localisationService = localisationService;
        }

        // provides localised text by localisation attribute
        public string GetText(ICorabeuLocalisationAttribute localisationAttribute)
        {
            if (!(localisationAttribute is LocalisedTextAttribute localisedText))
                return null;
            var result = localisationService.GetLocalisedString(localisedText.Module, localisedText.Label);            
            return result;
        }

    }

}
