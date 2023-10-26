using System;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Shared.Contract.Localisation.AppLogic
{
    /// <summary>
    /// Localised text attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class LocalisedTextAttribute : Attribute, ICorabeuLocalisationAttribute
    {
        public const string ProviderKey = "AppLocalisationProvider";

        // text type
        public string TextType { get; set; }
       
        // app licalisation item key
        public string Module { get; set; }        
        public string Label { get; set; }

        // provider name
        public string ProviderName => ProviderKey;


        // constructor
        public LocalisedTextAttribute(string module, string label, string textType = EditorTemplateOptions.DisplayName)
        {
            Module = module;
            Label = label;
            TextType = textType;
        }

        // allows multiple attribute processing
        private readonly object _typeId = new object();
        public override object TypeId => _typeId;

    }

}
