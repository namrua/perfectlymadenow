  
using System;
using System.Web;
using AutomationSystem.Main.Web;
using AutomationSystem.Shared.Contract.Localisation.System;

namespace Resources
{

	/// <summary>
	/// Resource module Metadata
	/// </summary>
	public static class MetadataTexts 
	{
		private static readonly Lazy<ILocalisationService> localisationService = new Lazy<ILocalisationService>(IocProvider.Get<ILocalisationService>);

		/// <summary>
		/// Resource text HtmlStringInstructors
		/// </summary>
		public static HtmlString Instructors => localisationService.Value.GetLocalisedHtmlString("Metadata", "Instructors"); 		

		/// <summary>
		/// Resource text Instructors
		/// </summary>
		public static string InstructorsString => localisationService.Value.GetLocalisedString("Metadata", "Instructors");		
			
		/// <summary>
		/// Resource text HtmlStringPhone
		/// </summary>
		public static HtmlString Phone => localisationService.Value.GetLocalisedHtmlString("Metadata", "Phone"); 		

		/// <summary>
		/// Resource text Phone
		/// </summary>
		public static string PhoneString => localisationService.Value.GetLocalisedString("Metadata", "Phone");		
			
		/// <summary>
		/// Resource text HtmlStringFirstName
		/// </summary>
		public static HtmlString FirstName => localisationService.Value.GetLocalisedHtmlString("Metadata", "FirstName"); 		

		/// <summary>
		/// Resource text FirstName
		/// </summary>
		public static string FirstNameString => localisationService.Value.GetLocalisedString("Metadata", "FirstName");		
			
		/// <summary>
		/// Resource text HtmlStringLastName
		/// </summary>
		public static HtmlString LastName => localisationService.Value.GetLocalisedHtmlString("Metadata", "LastName"); 		

		/// <summary>
		/// Resource text LastName
		/// </summary>
		public static string LastNameString => localisationService.Value.GetLocalisedString("Metadata", "LastName");		
			
		/// <summary>
		/// Resource text HtmlStringName
		/// </summary>
		public static HtmlString Name => localisationService.Value.GetLocalisedHtmlString("Metadata", "Name"); 		

		/// <summary>
		/// Resource text Name
		/// </summary>
		public static string NameString => localisationService.Value.GetLocalisedString("Metadata", "Name");		
			
		/// <summary>
		/// Resource text HtmlStringEmail
		/// </summary>
		public static HtmlString Email => localisationService.Value.GetLocalisedHtmlString("Metadata", "Email"); 		

		/// <summary>
		/// Resource text Email
		/// </summary>
		public static string EmailString => localisationService.Value.GetLocalisedString("Metadata", "Email");		
			
		/// <summary>
		/// Resource text HtmlStringStreet
		/// </summary>
		public static HtmlString Street => localisationService.Value.GetLocalisedHtmlString("Metadata", "Street"); 		

		/// <summary>
		/// Resource text Street
		/// </summary>
		public static string StreetString => localisationService.Value.GetLocalisedString("Metadata", "Street");		
			
		/// <summary>
		/// Resource text HtmlStringStreet2
		/// </summary>
		public static HtmlString Street2 => localisationService.Value.GetLocalisedHtmlString("Metadata", "Street2"); 		

		/// <summary>
		/// Resource text Street2
		/// </summary>
		public static string Street2String => localisationService.Value.GetLocalisedString("Metadata", "Street2");		
			
		/// <summary>
		/// Resource text HtmlStringCity
		/// </summary>
		public static HtmlString City => localisationService.Value.GetLocalisedHtmlString("Metadata", "City"); 		

		/// <summary>
		/// Resource text City
		/// </summary>
		public static string CityString => localisationService.Value.GetLocalisedString("Metadata", "City");		
			
		/// <summary>
		/// Resource text HtmlStringState
		/// </summary>
		public static HtmlString State => localisationService.Value.GetLocalisedHtmlString("Metadata", "State"); 		

		/// <summary>
		/// Resource text State
		/// </summary>
		public static string StateString => localisationService.Value.GetLocalisedString("Metadata", "State");		
			
		/// <summary>
		/// Resource text HtmlStringZipCode
		/// </summary>
		public static HtmlString ZipCode => localisationService.Value.GetLocalisedHtmlString("Metadata", "ZipCode"); 		

		/// <summary>
		/// Resource text ZipCode
		/// </summary>
		public static string ZipCodeString => localisationService.Value.GetLocalisedString("Metadata", "ZipCode");		
			
		/// <summary>
		/// Resource text HtmlStringCountry
		/// </summary>
		public static HtmlString Country => localisationService.Value.GetLocalisedHtmlString("Metadata", "Country"); 		

		/// <summary>
		/// Resource text Country
		/// </summary>
		public static string CountryString => localisationService.Value.GetLocalisedString("Metadata", "Country");		
			
		/// <summary>
		/// Resource text HtmlStringCountryPlaceholder
		/// </summary>
		public static HtmlString CountryPlaceholder => localisationService.Value.GetLocalisedHtmlString("Metadata", "CountryPlaceholder"); 		

		/// <summary>
		/// Resource text CountryPlaceholder
		/// </summary>
		public static string CountryPlaceholderString => localisationService.Value.GetLocalisedString("Metadata", "CountryPlaceholder");		
			
		/// <summary>
		/// Resource text HtmlStringCountryNoItemText
		/// </summary>
		public static HtmlString CountryNoItemText => localisationService.Value.GetLocalisedHtmlString("Metadata", "CountryNoItemText"); 		

		/// <summary>
		/// Resource text CountryNoItemText
		/// </summary>
		public static string CountryNoItemTextString => localisationService.Value.GetLocalisedString("Metadata", "CountryNoItemText");		
			
		/// <summary>
		/// Resource text HtmlStringLanguage
		/// </summary>
		public static HtmlString Language => localisationService.Value.GetLocalisedHtmlString("Metadata", "Language"); 		

		/// <summary>
		/// Resource text Language
		/// </summary>
		public static string LanguageString => localisationService.Value.GetLocalisedString("Metadata", "Language");		
			
		/// <summary>
		/// Resource text HtmlStringLanguagePlaceholder
		/// </summary>
		public static HtmlString LanguagePlaceholder => localisationService.Value.GetLocalisedHtmlString("Metadata", "LanguagePlaceholder"); 		

		/// <summary>
		/// Resource text LanguagePlaceholder
		/// </summary>
		public static string LanguagePlaceholderString => localisationService.Value.GetLocalisedString("Metadata", "LanguagePlaceholder");		
			
		/// <summary>
		/// Resource text HtmlStringLanguageNoItemText
		/// </summary>
		public static HtmlString LanguageNoItemText => localisationService.Value.GetLocalisedHtmlString("Metadata", "LanguageNoItemText"); 		

		/// <summary>
		/// Resource text LanguageNoItemText
		/// </summary>
		public static string LanguageNoItemTextString => localisationService.Value.GetLocalisedString("Metadata", "LanguageNoItemText");		
			
		/// <summary>
		/// Resource text HtmlStringClass
		/// </summary>
		public static HtmlString Class => localisationService.Value.GetLocalisedHtmlString("Metadata", "Class"); 		

		/// <summary>
		/// Resource text Class
		/// </summary>
		public static string ClassString => localisationService.Value.GetLocalisedString("Metadata", "Class");		
			
		/// <summary>
		/// Resource text HtmlStringClassType
		/// </summary>
		public static HtmlString ClassType => localisationService.Value.GetLocalisedHtmlString("Metadata", "ClassType"); 		

		/// <summary>
		/// Resource text ClassType
		/// </summary>
		public static string ClassTypeString => localisationService.Value.GetLocalisedString("Metadata", "ClassType");		
			
		/// <summary>
		/// Resource text HtmlStringLocation
		/// </summary>
		public static HtmlString Location => localisationService.Value.GetLocalisedHtmlString("Metadata", "Location"); 		

		/// <summary>
		/// Resource text Location
		/// </summary>
		public static string LocationString => localisationService.Value.GetLocalisedString("Metadata", "Location");		
			
		/// <summary>
		/// Resource text HtmlStringDate
		/// </summary>
		public static HtmlString Date => localisationService.Value.GetLocalisedHtmlString("Metadata", "Date"); 		

		/// <summary>
		/// Resource text Date
		/// </summary>
		public static string DateString => localisationService.Value.GetLocalisedString("Metadata", "Date");		
			
		/// <summary>
		/// Resource text HtmlStringMonth
		/// </summary>
		public static HtmlString Month => localisationService.Value.GetLocalisedHtmlString("Metadata", "Month"); 		

		/// <summary>
		/// Resource text Month
		/// </summary>
		public static string MonthString => localisationService.Value.GetLocalisedString("Metadata", "Month");		
			
		/// <summary>
		/// Resource text HtmlStringMonthPlaceholder
		/// </summary>
		public static HtmlString MonthPlaceholder => localisationService.Value.GetLocalisedHtmlString("Metadata", "MonthPlaceholder"); 		

		/// <summary>
		/// Resource text MonthPlaceholder
		/// </summary>
		public static string MonthPlaceholderString => localisationService.Value.GetLocalisedString("Metadata", "MonthPlaceholder");		
			
		/// <summary>
		/// Resource text HtmlStringMonthNoItemText
		/// </summary>
		public static HtmlString MonthNoItemText => localisationService.Value.GetLocalisedHtmlString("Metadata", "MonthNoItemText"); 		

		/// <summary>
		/// Resource text MonthNoItemText
		/// </summary>
		public static string MonthNoItemTextString => localisationService.Value.GetLocalisedString("Metadata", "MonthNoItemText");		
			
		/// <summary>
		/// Resource text HtmlStringYear
		/// </summary>
		public static HtmlString Year => localisationService.Value.GetLocalisedHtmlString("Metadata", "Year"); 		

		/// <summary>
		/// Resource text Year
		/// </summary>
		public static string YearString => localisationService.Value.GetLocalisedString("Metadata", "Year");		
			
		/// <summary>
		/// Resource text HtmlStringYearPlaceholder
		/// </summary>
		public static HtmlString YearPlaceholder => localisationService.Value.GetLocalisedHtmlString("Metadata", "YearPlaceholder"); 		

		/// <summary>
		/// Resource text YearPlaceholder
		/// </summary>
		public static string YearPlaceholderString => localisationService.Value.GetLocalisedString("Metadata", "YearPlaceholder");		
			
		/// <summary>
		/// Resource text HtmlStringYearNoItemText
		/// </summary>
		public static HtmlString YearNoItemText => localisationService.Value.GetLocalisedHtmlString("Metadata", "YearNoItemText"); 		

		/// <summary>
		/// Resource text YearNoItemText
		/// </summary>
		public static string YearNoItemTextString => localisationService.Value.GetLocalisedString("Metadata", "YearNoItemText");		
			
		/// <summary>
		/// Resource text HtmlStringRegistrationStart
		/// </summary>
		public static HtmlString RegistrationStart => localisationService.Value.GetLocalisedHtmlString("Metadata", "RegistrationStart"); 		

		/// <summary>
		/// Resource text RegistrationStart
		/// </summary>
		public static string RegistrationStartString => localisationService.Value.GetLocalisedString("Metadata", "RegistrationStart");		
			
		/// <summary>
		/// Resource text HtmlStringRegistrationEnd
		/// </summary>
		public static HtmlString RegistrationEnd => localisationService.Value.GetLocalisedHtmlString("Metadata", "RegistrationEnd"); 		

		/// <summary>
		/// Resource text RegistrationEnd
		/// </summary>
		public static string RegistrationEndString => localisationService.Value.GetLocalisedString("Metadata", "RegistrationEnd");		
			
		/// <summary>
		/// Resource text HtmlStringRegistrationType
		/// </summary>
		public static HtmlString RegistrationType => localisationService.Value.GetLocalisedHtmlString("Metadata", "RegistrationType"); 		

		/// <summary>
		/// Resource text RegistrationType
		/// </summary>
		public static string RegistrationTypeString => localisationService.Value.GetLocalisedString("Metadata", "RegistrationType");		
			
		/// <summary>
		/// Resource text HtmlStringPrice
		/// </summary>
		public static HtmlString Price => localisationService.Value.GetLocalisedHtmlString("Metadata", "Price"); 		

		/// <summary>
		/// Resource text Price
		/// </summary>
		public static string PriceString => localisationService.Value.GetLocalisedString("Metadata", "Price");		
			
		/// <summary>
		/// Resource text HtmlStringAcceptAgreement
		/// </summary>
		public static HtmlString AcceptAgreement => localisationService.Value.GetLocalisedHtmlString("Metadata", "AcceptAgreement"); 		

		/// <summary>
		/// Resource text AcceptAgreement
		/// </summary>
		public static string AcceptAgreementString => localisationService.Value.GetLocalisedString("Metadata", "AcceptAgreement");		
			
		/// <summary>
		/// Resource text HtmlStringAddress
		/// </summary>
		public static HtmlString Address => localisationService.Value.GetLocalisedHtmlString("Metadata", "Address"); 		

		/// <summary>
		/// Resource text Address
		/// </summary>
		public static string AddressString => localisationService.Value.GetLocalisedString("Metadata", "Address");		
			
		/// <summary>
		/// Resource text HtmlStringStudentAddress
		/// </summary>
		public static HtmlString StudentAddress => localisationService.Value.GetLocalisedHtmlString("Metadata", "StudentAddress"); 		

		/// <summary>
		/// Resource text StudentAddress
		/// </summary>
		public static string StudentAddressString => localisationService.Value.GetLocalisedString("Metadata", "StudentAddress");		
			
		/// <summary>
		/// Resource text HtmlStringParentAddress
		/// </summary>
		public static HtmlString ParentAddress => localisationService.Value.GetLocalisedHtmlString("Metadata", "ParentAddress"); 		

		/// <summary>
		/// Resource text ParentAddress
		/// </summary>
		public static string ParentAddressString => localisationService.Value.GetLocalisedString("Metadata", "ParentAddress");		
			
		/// <summary>
		/// Resource text HtmlStringChildAddress
		/// </summary>
		public static HtmlString ChildAddress => localisationService.Value.GetLocalisedHtmlString("Metadata", "ChildAddress"); 		

		/// <summary>
		/// Resource text ChildAddress
		/// </summary>
		public static string ChildAddressString => localisationService.Value.GetLocalisedString("Metadata", "ChildAddress");		
			
		/// <summary>
		/// Resource text HtmlStringParticipantAddress
		/// </summary>
		public static HtmlString ParticipantAddress => localisationService.Value.GetLocalisedHtmlString("Metadata", "ParticipantAddress"); 		

		/// <summary>
		/// Resource text ParticipantAddress
		/// </summary>
		public static string ParticipantAddressString => localisationService.Value.GetLocalisedString("Metadata", "ParticipantAddress");		
			
		/// <summary>
		/// Resource text HtmlStringRegistrationAddress
		/// </summary>
		public static HtmlString RegistrationAddress => localisationService.Value.GetLocalisedHtmlString("Metadata", "RegistrationAddress"); 		

		/// <summary>
		/// Resource text RegistrationAddress
		/// </summary>
		public static string RegistrationAddressString => localisationService.Value.GetLocalisedString("Metadata", "RegistrationAddress");		
			
	}


	/// <summary>
	/// Resource module Error
	/// </summary>
	public static class ErrorTexts 
	{
		private static readonly Lazy<ILocalisationService> localisationService = new Lazy<ILocalisationService>(IocProvider.Get<ILocalisationService>);

		/// <summary>
		/// Resource text HtmlStringMaterialsNotAvailableTitle
		/// </summary>
		public static HtmlString MaterialsNotAvailableTitle => localisationService.Value.GetLocalisedHtmlString("Error", "MaterialsNotAvailableTitle"); 		

		/// <summary>
		/// Resource text MaterialsNotAvailableTitle
		/// </summary>
		public static string MaterialsNotAvailableTitleString => localisationService.Value.GetLocalisedString("Error", "MaterialsNotAvailableTitle");		
			
		/// <summary>
		/// Resource text HtmlStringMaterialsNotAvailable
		/// </summary>
		public static HtmlString MaterialsNotAvailable => localisationService.Value.GetLocalisedHtmlString("Error", "MaterialsNotAvailable"); 		

		/// <summary>
		/// Resource text MaterialsNotAvailable
		/// </summary>
		public static string MaterialsNotAvailableString => localisationService.Value.GetLocalisedString("Error", "MaterialsNotAvailable");		
			
		/// <summary>
		/// Resource text HtmlStringClassClosedTitle
		/// </summary>
		public static HtmlString ClassClosedTitle => localisationService.Value.GetLocalisedHtmlString("Error", "ClassClosedTitle"); 		

		/// <summary>
		/// Resource text ClassClosedTitle
		/// </summary>
		public static string ClassClosedTitleString => localisationService.Value.GetLocalisedString("Error", "ClassClosedTitle");		
			
		/// <summary>
		/// Resource text HtmlStringClassClosed
		/// </summary>
		public static HtmlString ClassClosed => localisationService.Value.GetLocalisedHtmlString("Error", "ClassClosed"); 		

		/// <summary>
		/// Resource text ClassClosed
		/// </summary>
		public static string ClassClosedString => localisationService.Value.GetLocalisedString("Error", "ClassClosed");		
			
		/// <summary>
		/// Resource text HtmlStringClassNotStartedTitle
		/// </summary>
		public static HtmlString ClassNotStartedTitle => localisationService.Value.GetLocalisedHtmlString("Error", "ClassNotStartedTitle"); 		

		/// <summary>
		/// Resource text ClassNotStartedTitle
		/// </summary>
		public static string ClassNotStartedTitleString => localisationService.Value.GetLocalisedString("Error", "ClassNotStartedTitle");		
			
		/// <summary>
		/// Resource text HtmlStringClassNotStarted
		/// </summary>
		public static HtmlString ClassNotStarted => localisationService.Value.GetLocalisedHtmlString("Error", "ClassNotStarted"); 		

		/// <summary>
		/// Resource text ClassNotStarted
		/// </summary>
		public static string ClassNotStartedString => localisationService.Value.GetLocalisedString("Error", "ClassNotStarted");		
			
		/// <summary>
		/// Resource text HtmlStringRegistrationCompleteTitle
		/// </summary>
		public static HtmlString RegistrationCompleteTitle => localisationService.Value.GetLocalisedHtmlString("Error", "RegistrationCompleteTitle"); 		

		/// <summary>
		/// Resource text RegistrationCompleteTitle
		/// </summary>
		public static string RegistrationCompleteTitleString => localisationService.Value.GetLocalisedString("Error", "RegistrationCompleteTitle");		
			
		/// <summary>
		/// Resource text HtmlStringRegistrationComplete
		/// </summary>
		public static HtmlString RegistrationComplete => localisationService.Value.GetLocalisedHtmlString("Error", "RegistrationComplete"); 		

		/// <summary>
		/// Resource text RegistrationComplete
		/// </summary>
		public static string RegistrationCompleteString => localisationService.Value.GetLocalisedString("Error", "RegistrationComplete");		
			
		/// <summary>
		/// Resource text HtmlStringInvalidStepTitle
		/// </summary>
		public static HtmlString InvalidStepTitle => localisationService.Value.GetLocalisedHtmlString("Error", "InvalidStepTitle"); 		

		/// <summary>
		/// Resource text InvalidStepTitle
		/// </summary>
		public static string InvalidStepTitleString => localisationService.Value.GetLocalisedString("Error", "InvalidStepTitle");		
			
		/// <summary>
		/// Resource text HtmlStringInvalidStep
		/// </summary>
		public static HtmlString InvalidStep => localisationService.Value.GetLocalisedHtmlString("Error", "InvalidStep"); 		

		/// <summary>
		/// Resource text InvalidStep
		/// </summary>
		public static string InvalidStepString => localisationService.Value.GetLocalisedString("Error", "InvalidStep");		
			
		/// <summary>
		/// Resource text HtmlStringRegistrationNotAllowedTitle
		/// </summary>
		public static HtmlString RegistrationNotAllowedTitle => localisationService.Value.GetLocalisedHtmlString("Error", "RegistrationNotAllowedTitle"); 		

		/// <summary>
		/// Resource text RegistrationNotAllowedTitle
		/// </summary>
		public static string RegistrationNotAllowedTitleString => localisationService.Value.GetLocalisedString("Error", "RegistrationNotAllowedTitle");		
			
		/// <summary>
		/// Resource text HtmlStringRegistrationNotAllowed
		/// </summary>
		public static HtmlString RegistrationNotAllowed => localisationService.Value.GetLocalisedHtmlString("Error", "RegistrationNotAllowed"); 		

		/// <summary>
		/// Resource text RegistrationNotAllowed
		/// </summary>
		public static string RegistrationNotAllowedString => localisationService.Value.GetLocalisedString("Error", "RegistrationNotAllowed");		
			
		/// <summary>
		/// Resource text HtmlStringPreRegistrationClosedTitle
		/// </summary>
		public static HtmlString PreRegistrationClosedTitle => localisationService.Value.GetLocalisedHtmlString("Error", "PreRegistrationClosedTitle"); 		

		/// <summary>
		/// Resource text PreRegistrationClosedTitle
		/// </summary>
		public static string PreRegistrationClosedTitleString => localisationService.Value.GetLocalisedString("Error", "PreRegistrationClosedTitle");		
			
		/// <summary>
		/// Resource text HtmlStringPreRegistrationClosed
		/// </summary>
		public static HtmlString PreRegistrationClosed => localisationService.Value.GetLocalisedHtmlString("Error", "PreRegistrationClosed"); 		

		/// <summary>
		/// Resource text PreRegistrationClosed
		/// </summary>
		public static string PreRegistrationClosedString => localisationService.Value.GetLocalisedString("Error", "PreRegistrationClosed");		
			
		/// <summary>
		/// Resource text HtmlStringInvalidPageTitle
		/// </summary>
		public static HtmlString InvalidPageTitle => localisationService.Value.GetLocalisedHtmlString("Error", "InvalidPageTitle"); 		

		/// <summary>
		/// Resource text InvalidPageTitle
		/// </summary>
		public static string InvalidPageTitleString => localisationService.Value.GetLocalisedString("Error", "InvalidPageTitle");		
			
		/// <summary>
		/// Resource text HtmlStringInvalidPage
		/// </summary>
		public static HtmlString InvalidPage => localisationService.Value.GetLocalisedHtmlString("Error", "InvalidPage"); 		

		/// <summary>
		/// Resource text InvalidPage
		/// </summary>
		public static string InvalidPageString => localisationService.Value.GetLocalisedString("Error", "InvalidPage");		
			
		/// <summary>
		/// Resource text HtmlStringGenericTitle
		/// </summary>
		public static HtmlString GenericTitle => localisationService.Value.GetLocalisedHtmlString("Error", "GenericTitle"); 		

		/// <summary>
		/// Resource text GenericTitle
		/// </summary>
		public static string GenericTitleString => localisationService.Value.GetLocalisedString("Error", "GenericTitle");		
			
		/// <summary>
		/// Resource text HtmlStringGeneric
		/// </summary>
		public static HtmlString Generic => localisationService.Value.GetLocalisedHtmlString("Error", "Generic"); 		

		/// <summary>
		/// Resource text Generic
		/// </summary>
		public static string GenericString => localisationService.Value.GetLocalisedString("Error", "Generic");		
			
		/// <summary>
		/// Resource text HtmlStringInvitationExpiredTitle
		/// </summary>
		public static HtmlString InvitationExpiredTitle => localisationService.Value.GetLocalisedHtmlString("Error", "InvitationExpiredTitle"); 		

		/// <summary>
		/// Resource text InvitationExpiredTitle
		/// </summary>
		public static string InvitationExpiredTitleString => localisationService.Value.GetLocalisedString("Error", "InvitationExpiredTitle");		
			
		/// <summary>
		/// Resource text HtmlStringInvitationExpired
		/// </summary>
		public static HtmlString InvitationExpired => localisationService.Value.GetLocalisedHtmlString("Error", "InvitationExpired"); 		

		/// <summary>
		/// Resource text InvitationExpired
		/// </summary>
		public static string InvitationExpiredString => localisationService.Value.GetLocalisedString("Error", "InvitationExpired");		
			
	}


	/// <summary>
	/// Resource module Title
	/// </summary>
	public static class TitleTexts 
	{
		private static readonly Lazy<ILocalisationService> localisationService = new Lazy<ILocalisationService>(IocProvider.Get<ILocalisationService>);

		/// <summary>
		/// Resource text HtmlStringClassMaterials
		/// </summary>
		public static HtmlString ClassMaterials => localisationService.Value.GetLocalisedHtmlString("Title", "ClassMaterials"); 		

		/// <summary>
		/// Resource text ClassMaterials
		/// </summary>
		public static string ClassMaterialsString => localisationService.Value.GetLocalisedString("Title", "ClassMaterials");		
			
		/// <summary>
		/// Resource text HtmlStringWwaRegistration
		/// </summary>
		public static HtmlString WwaRegistration => localisationService.Value.GetLocalisedHtmlString("Title", "WwaRegistration"); 		

		/// <summary>
		/// Resource text WwaRegistration
		/// </summary>
		public static string WwaRegistrationString => localisationService.Value.GetLocalisedString("Title", "WwaRegistration");		
			
		/// <summary>
		/// Resource text HtmlStringIntroduction
		/// </summary>
		public static HtmlString Introduction => localisationService.Value.GetLocalisedHtmlString("Title", "Introduction"); 		

		/// <summary>
		/// Resource text Introduction
		/// </summary>
		public static string IntroductionString => localisationService.Value.GetLocalisedString("Title", "Introduction");		
			
		/// <summary>
		/// Resource text HtmlStringOnlineClasses
		/// </summary>
		public static HtmlString OnlineClasses => localisationService.Value.GetLocalisedHtmlString("Title", "OnlineClasses"); 		

		/// <summary>
		/// Resource text OnlineClasses
		/// </summary>
		public static string OnlineClassesString => localisationService.Value.GetLocalisedString("Title", "OnlineClasses");		
			
		/// <summary>
		/// Resource text HtmlStringRegistrationInfo
		/// </summary>
		public static HtmlString RegistrationInfo => localisationService.Value.GetLocalisedHtmlString("Title", "RegistrationInfo"); 		

		/// <summary>
		/// Resource text RegistrationInfo
		/// </summary>
		public static string RegistrationInfoString => localisationService.Value.GetLocalisedString("Title", "RegistrationInfo");		
			
		/// <summary>
		/// Resource text HtmlStringRegistrationForm
		/// </summary>
		public static HtmlString RegistrationForm => localisationService.Value.GetLocalisedHtmlString("Title", "RegistrationForm"); 		

		/// <summary>
		/// Resource text RegistrationForm
		/// </summary>
		public static string RegistrationFormString => localisationService.Value.GetLocalisedString("Title", "RegistrationForm");		
			
		/// <summary>
		/// Resource text HtmlStringRegistrationSummary
		/// </summary>
		public static HtmlString RegistrationSummary => localisationService.Value.GetLocalisedHtmlString("Title", "RegistrationSummary"); 		

		/// <summary>
		/// Resource text RegistrationSummary
		/// </summary>
		public static string RegistrationSummaryString => localisationService.Value.GetLocalisedString("Title", "RegistrationSummary");		
			
		/// <summary>
		/// Resource text HtmlStringAgreement
		/// </summary>
		public static HtmlString Agreement => localisationService.Value.GetLocalisedHtmlString("Title", "Agreement"); 		

		/// <summary>
		/// Resource text Agreement
		/// </summary>
		public static string AgreementString => localisationService.Value.GetLocalisedString("Title", "Agreement");		
			
		/// <summary>
		/// Resource text HtmlStringAgreementAcceptance
		/// </summary>
		public static HtmlString AgreementAcceptance => localisationService.Value.GetLocalisedHtmlString("Title", "AgreementAcceptance"); 		

		/// <summary>
		/// Resource text AgreementAcceptance
		/// </summary>
		public static string AgreementAcceptanceString => localisationService.Value.GetLocalisedString("Title", "AgreementAcceptance");		
			
		/// <summary>
		/// Resource text HtmlStringPaymentSummary
		/// </summary>
		public static HtmlString PaymentSummary => localisationService.Value.GetLocalisedHtmlString("Title", "PaymentSummary"); 		

		/// <summary>
		/// Resource text PaymentSummary
		/// </summary>
		public static string PaymentSummaryString => localisationService.Value.GetLocalisedString("Title", "PaymentSummary");		
			
		/// <summary>
		/// Resource text HtmlStringReviewStudent
		/// </summary>
		public static HtmlString ReviewStudent => localisationService.Value.GetLocalisedHtmlString("Title", "ReviewStudent"); 		

		/// <summary>
		/// Resource text ReviewStudent
		/// </summary>
		public static string ReviewStudentString => localisationService.Value.GetLocalisedString("Title", "ReviewStudent");		
			
		/// <summary>
		/// Resource text HtmlStringFormerSuggestion
		/// </summary>
		public static HtmlString FormerSuggestion => localisationService.Value.GetLocalisedHtmlString("Title", "FormerSuggestion"); 		

		/// <summary>
		/// Resource text FormerSuggestion
		/// </summary>
		public static string FormerSuggestionString => localisationService.Value.GetLocalisedString("Title", "FormerSuggestion");		
			
		/// <summary>
		/// Resource text HtmlStringFormerSummary
		/// </summary>
		public static HtmlString FormerSummary => localisationService.Value.GetLocalisedHtmlString("Title", "FormerSummary"); 		

		/// <summary>
		/// Resource text FormerSummary
		/// </summary>
		public static string FormerSummaryString => localisationService.Value.GetLocalisedString("Title", "FormerSummary");		
			
		/// <summary>
		/// Resource text HtmlStringRegistrationComplete
		/// </summary>
		public static HtmlString RegistrationComplete => localisationService.Value.GetLocalisedHtmlString("Title", "RegistrationComplete"); 		

		/// <summary>
		/// Resource text RegistrationComplete
		/// </summary>
		public static string RegistrationCompleteString => localisationService.Value.GetLocalisedString("Title", "RegistrationComplete");		
			
		/// <summary>
		/// Resource text HtmlStringFormerClassForm
		/// </summary>
		public static HtmlString FormerClassForm => localisationService.Value.GetLocalisedHtmlString("Title", "FormerClassForm"); 		

		/// <summary>
		/// Resource text FormerClassForm
		/// </summary>
		public static string FormerClassFormString => localisationService.Value.GetLocalisedString("Title", "FormerClassForm");		
			
		/// <summary>
		/// Resource text HtmlStringFormerClassSummary
		/// </summary>
		public static HtmlString FormerClassSummary => localisationService.Value.GetLocalisedHtmlString("Title", "FormerClassSummary"); 		

		/// <summary>
		/// Resource text FormerClassSummary
		/// </summary>
		public static string FormerClassSummaryString => localisationService.Value.GetLocalisedString("Title", "FormerClassSummary");		
			
		/// <summary>
		/// Resource text HtmlStringManualReview
		/// </summary>
		public static HtmlString ManualReview => localisationService.Value.GetLocalisedHtmlString("Title", "ManualReview"); 		

		/// <summary>
		/// Resource text ManualReview
		/// </summary>
		public static string ManualReviewString => localisationService.Value.GetLocalisedString("Title", "ManualReview");		
			
	}


	/// <summary>
	/// Resource module Text
	/// </summary>
	public static class TextTexts 
	{
		private static readonly Lazy<ILocalisationService> localisationService = new Lazy<ILocalisationService>(IocProvider.Get<ILocalisationService>);

		/// <summary>
		/// Resource text HtmlStringMaterialLanguageInstruction
		/// </summary>
		public static HtmlString MaterialLanguageInstruction => localisationService.Value.GetLocalisedHtmlString("Text", "MaterialLanguageInstruction"); 		

		/// <summary>
		/// Resource text MaterialLanguageInstruction
		/// </summary>
		public static string MaterialLanguageInstructionString => localisationService.Value.GetLocalisedString("Text", "MaterialLanguageInstruction");		
			
		/// <summary>
		/// Resource text HtmlStringAcceptQuestion
		/// </summary>
		public static HtmlString AcceptQuestion => localisationService.Value.GetLocalisedHtmlString("Text", "AcceptQuestion"); 		

		/// <summary>
		/// Resource text AcceptQuestion
		/// </summary>
		public static string AcceptQuestionString => localisationService.Value.GetLocalisedString("Text", "AcceptQuestion");		
			
		/// <summary>
		/// Resource text HtmlStringPaymentInstruction
		/// </summary>
		public static HtmlString PaymentInstruction => localisationService.Value.GetLocalisedHtmlString("Text", "PaymentInstruction"); 		

		/// <summary>
		/// Resource text PaymentInstruction
		/// </summary>
		public static string PaymentInstructionString => localisationService.Value.GetLocalisedString("Text", "PaymentInstruction");		
			
		/// <summary>
		/// Resource text HtmlStringReviewInstruction
		/// </summary>
		public static HtmlString ReviewInstruction => localisationService.Value.GetLocalisedHtmlString("Text", "ReviewInstruction"); 		

		/// <summary>
		/// Resource text ReviewInstruction
		/// </summary>
		public static string ReviewInstructionString => localisationService.Value.GetLocalisedString("Text", "ReviewInstruction");		
			
		/// <summary>
		/// Resource text HtmlStringNoFormerStudent
		/// </summary>
		public static HtmlString NoFormerStudent => localisationService.Value.GetLocalisedHtmlString("Text", "NoFormerStudent"); 		

		/// <summary>
		/// Resource text NoFormerStudent
		/// </summary>
		public static string NoFormerStudentString => localisationService.Value.GetLocalisedString("Text", "NoFormerStudent");		
			
		/// <summary>
		/// Resource text HtmlStringManualReviewInstruction
		/// </summary>
		public static HtmlString ManualReviewInstruction => localisationService.Value.GetLocalisedHtmlString("Text", "ManualReviewInstruction"); 		

		/// <summary>
		/// Resource text ManualReviewInstruction
		/// </summary>
		public static string ManualReviewInstructionString => localisationService.Value.GetLocalisedString("Text", "ManualReviewInstruction");		
			
		/// <summary>
		/// Resource text HtmlStringRegistrationComplete
		/// </summary>
		public static HtmlString RegistrationComplete => localisationService.Value.GetLocalisedHtmlString("Text", "RegistrationComplete"); 		

		/// <summary>
		/// Resource text RegistrationComplete
		/// </summary>
		public static string RegistrationCompleteString => localisationService.Value.GetLocalisedString("Text", "RegistrationComplete");		
			
		/// <summary>
		/// Resource text HtmlStringManualReview
		/// </summary>
		public static HtmlString ManualReview => localisationService.Value.GetLocalisedHtmlString("Text", "ManualReview"); 		

		/// <summary>
		/// Resource text ManualReview
		/// </summary>
		public static string ManualReviewString => localisationService.Value.GetLocalisedString("Text", "ManualReview");		
			
		/// <summary>
		/// Resource text HtmlStringPayPalError
		/// </summary>
		public static HtmlString PayPalError => localisationService.Value.GetLocalisedHtmlString("Text", "PayPalError"); 		

		/// <summary>
		/// Resource text PayPalError
		/// </summary>
		public static string PayPalErrorString => localisationService.Value.GetLocalisedString("Text", "PayPalError");		
			
		/// <summary>
		/// Resource text HtmlStringNoClass
		/// </summary>
		public static HtmlString NoClass => localisationService.Value.GetLocalisedHtmlString("Text", "NoClass"); 		

		/// <summary>
		/// Resource text NoClass
		/// </summary>
		public static string NoClassString => localisationService.Value.GetLocalisedString("Text", "NoClass");		
			
		/// <summary>
		/// Resource text HtmlStringShippmentAddressInfo
		/// </summary>
		public static HtmlString ShippmentAddressInfo => localisationService.Value.GetLocalisedHtmlString("Text", "ShippmentAddressInfo"); 		

		/// <summary>
		/// Resource text ShippmentAddressInfo
		/// </summary>
		public static string ShippmentAddressInfoString => localisationService.Value.GetLocalisedString("Text", "ShippmentAddressInfo");		
			
	}


	/// <summary>
	/// Resource module Content
	/// </summary>
	public static class ContentTexts 
	{
		private static readonly Lazy<ILocalisationService> localisationService = new Lazy<ILocalisationService>(IocProvider.Get<ILocalisationService>);

		/// <summary>
		/// Resource text HtmlStringMaterialInstruction
		/// </summary>
		public static HtmlString MaterialInstruction => localisationService.Value.GetLocalisedHtmlString("Content", "MaterialInstruction"); 		

		/// <summary>
		/// Resource text MaterialInstruction
		/// </summary>
		public static string MaterialInstructionString => localisationService.Value.GetLocalisedString("Content", "MaterialInstruction");		
			
		/// <summary>
		/// Resource text HtmlStringDistanceClassesInstruction
		/// </summary>
		public static HtmlString DistanceClassesInstruction => localisationService.Value.GetLocalisedHtmlString("Content", "DistanceClassesInstruction"); 		

		/// <summary>
		/// Resource text DistanceClassesInstruction
		/// </summary>
		public static string DistanceClassesInstructionString => localisationService.Value.GetLocalisedString("Content", "DistanceClassesInstruction");		
			
		/// <summary>
		/// Resource text HtmlStringHomepageDistanceClasses
		/// </summary>
		public static HtmlString HomepageDistanceClasses => localisationService.Value.GetLocalisedHtmlString("Content", "HomepageDistanceClasses"); 		

		/// <summary>
		/// Resource text HomepageDistanceClasses
		/// </summary>
		public static string HomepageDistanceClassesString => localisationService.Value.GetLocalisedString("Content", "HomepageDistanceClasses");		
			
		/// <summary>
		/// Resource text HtmlStringIntroduction
		/// </summary>
		public static HtmlString Introduction => localisationService.Value.GetLocalisedHtmlString("Content", "Introduction"); 		

		/// <summary>
		/// Resource text Introduction
		/// </summary>
		public static string IntroductionString => localisationService.Value.GetLocalisedString("Content", "Introduction");		
			
		/// <summary>
		/// Resource text HtmlStringRegistrationInfo1
		/// </summary>
		public static HtmlString RegistrationInfo1 => localisationService.Value.GetLocalisedHtmlString("Content", "RegistrationInfo1"); 		

		/// <summary>
		/// Resource text RegistrationInfo1
		/// </summary>
		public static string RegistrationInfo1String => localisationService.Value.GetLocalisedString("Content", "RegistrationInfo1");		
			
		/// <summary>
		/// Resource text HtmlStringRegistrationInfo2
		/// </summary>
		public static HtmlString RegistrationInfo2 => localisationService.Value.GetLocalisedHtmlString("Content", "RegistrationInfo2"); 		

		/// <summary>
		/// Resource text RegistrationInfo2
		/// </summary>
		public static string RegistrationInfo2String => localisationService.Value.GetLocalisedString("Content", "RegistrationInfo2");		
			
		/// <summary>
		/// Resource text HtmlStringAgreement1
		/// </summary>
		public static HtmlString Agreement1 => localisationService.Value.GetLocalisedHtmlString("Content", "Agreement1"); 		

		/// <summary>
		/// Resource text Agreement1
		/// </summary>
		public static string Agreement1String => localisationService.Value.GetLocalisedString("Content", "Agreement1");		
			
		/// <summary>
		/// Resource text HtmlStringAgreement2
		/// </summary>
		public static HtmlString Agreement2 => localisationService.Value.GetLocalisedHtmlString("Content", "Agreement2"); 		

		/// <summary>
		/// Resource text Agreement2
		/// </summary>
		public static string Agreement2String => localisationService.Value.GetLocalisedString("Content", "Agreement2");		
			
	}


	/// <summary>
	/// Resource module Button
	/// </summary>
	public static class ButtonTexts 
	{
		private static readonly Lazy<ILocalisationService> localisationService = new Lazy<ILocalisationService>(IocProvider.Get<ILocalisationService>);

		/// <summary>
		/// Resource text HtmlStringDownload
		/// </summary>
		public static HtmlString Download => localisationService.Value.GetLocalisedHtmlString("Button", "Download"); 		

		/// <summary>
		/// Resource text Download
		/// </summary>
		public static string DownloadString => localisationService.Value.GetLocalisedString("Button", "Download");		
			
		/// <summary>
		/// Resource text HtmlStringDistanceClasses
		/// </summary>
		public static HtmlString DistanceClasses => localisationService.Value.GetLocalisedHtmlString("Button", "DistanceClasses"); 		

		/// <summary>
		/// Resource text DistanceClasses
		/// </summary>
		public static string DistanceClassesString => localisationService.Value.GetLocalisedString("Button", "DistanceClasses");		
			
		/// <summary>
		/// Resource text HtmlStringRegister
		/// </summary>
		public static HtmlString Register => localisationService.Value.GetLocalisedHtmlString("Button", "Register"); 		

		/// <summary>
		/// Resource text Register
		/// </summary>
		public static string RegisterString => localisationService.Value.GetLocalisedString("Button", "Register");		
			
		/// <summary>
		/// Resource text HtmlStringBackToHome
		/// </summary>
		public static HtmlString BackToHome => localisationService.Value.GetLocalisedHtmlString("Button", "BackToHome"); 		

		/// <summary>
		/// Resource text BackToHome
		/// </summary>
		public static string BackToHomeString => localisationService.Value.GetLocalisedString("Button", "BackToHome");		
			
		/// <summary>
		/// Resource text HtmlStringSelect
		/// </summary>
		public static HtmlString Select => localisationService.Value.GetLocalisedHtmlString("Button", "Select"); 		

		/// <summary>
		/// Resource text Select
		/// </summary>
		public static string SelectString => localisationService.Value.GetLocalisedString("Button", "Select");		
			
		/// <summary>
		/// Resource text HtmlStringModify
		/// </summary>
		public static HtmlString Modify => localisationService.Value.GetLocalisedHtmlString("Button", "Modify"); 		

		/// <summary>
		/// Resource text Modify
		/// </summary>
		public static string ModifyString => localisationService.Value.GetLocalisedString("Button", "Modify");		
			
		/// <summary>
		/// Resource text HtmlStringSave
		/// </summary>
		public static HtmlString Save => localisationService.Value.GetLocalisedHtmlString("Button", "Save"); 		

		/// <summary>
		/// Resource text Save
		/// </summary>
		public static string SaveString => localisationService.Value.GetLocalisedString("Button", "Save");		
			
		/// <summary>
		/// Resource text HtmlStringConfirm
		/// </summary>
		public static HtmlString Confirm => localisationService.Value.GetLocalisedHtmlString("Button", "Confirm"); 		

		/// <summary>
		/// Resource text Confirm
		/// </summary>
		public static string ConfirmString => localisationService.Value.GetLocalisedString("Button", "Confirm");		
			
		/// <summary>
		/// Resource text HtmlStringBack
		/// </summary>
		public static HtmlString Back => localisationService.Value.GetLocalisedHtmlString("Button", "Back"); 		

		/// <summary>
		/// Resource text Back
		/// </summary>
		public static string BackString => localisationService.Value.GetLocalisedString("Button", "Back");		
			
		/// <summary>
		/// Resource text HtmlStringManualReview
		/// </summary>
		public static HtmlString ManualReview => localisationService.Value.GetLocalisedHtmlString("Button", "ManualReview"); 		

		/// <summary>
		/// Resource text ManualReview
		/// </summary>
		public static string ManualReviewString => localisationService.Value.GetLocalisedString("Button", "ManualReview");		
			
		/// <summary>
		/// Resource text HtmlStringChangeFormer
		/// </summary>
		public static HtmlString ChangeFormer => localisationService.Value.GetLocalisedHtmlString("Button", "ChangeFormer"); 		

		/// <summary>
		/// Resource text ChangeFormer
		/// </summary>
		public static string ChangeFormerString => localisationService.Value.GetLocalisedString("Button", "ChangeFormer");		
			
		/// <summary>
		/// Resource text HtmlStringModifyPersonal
		/// </summary>
		public static HtmlString ModifyPersonal => localisationService.Value.GetLocalisedHtmlString("Button", "ModifyPersonal"); 		

		/// <summary>
		/// Resource text ModifyPersonal
		/// </summary>
		public static string ModifyPersonalString => localisationService.Value.GetLocalisedString("Button", "ModifyPersonal");		
			
		/// <summary>
		/// Resource text HtmlStringKeepSelection
		/// </summary>
		public static HtmlString KeepSelection => localisationService.Value.GetLocalisedHtmlString("Button", "KeepSelection"); 		

		/// <summary>
		/// Resource text KeepSelection
		/// </summary>
		public static string KeepSelectionString => localisationService.Value.GetLocalisedString("Button", "KeepSelection");		
			
		/// <summary>
		/// Resource text HtmlStringResetStudent
		/// </summary>
		public static HtmlString ResetStudent => localisationService.Value.GetLocalisedHtmlString("Button", "ResetStudent"); 		

		/// <summary>
		/// Resource text ResetStudent
		/// </summary>
		public static string ResetStudentString => localisationService.Value.GetLocalisedString("Button", "ResetStudent");		
			
	}


	/// <summary>
	/// Resource module Layout
	/// </summary>
	public static class LayoutTexts 
	{
		private static readonly Lazy<ILocalisationService> localisationService = new Lazy<ILocalisationService>(IocProvider.Get<ILocalisationService>);

		/// <summary>
		/// Resource text HtmlStringFooter
		/// </summary>
		public static HtmlString Footer => localisationService.Value.GetLocalisedHtmlString("Layout", "Footer"); 		

		/// <summary>
		/// Resource text Footer
		/// </summary>
		public static string FooterString => localisationService.Value.GetLocalisedString("Layout", "Footer");		
			
		/// <summary>
		/// Resource text HtmlStringHeadline
		/// </summary>
		public static HtmlString Headline => localisationService.Value.GetLocalisedHtmlString("Layout", "Headline"); 		

		/// <summary>
		/// Resource text Headline
		/// </summary>
		public static string HeadlineString => localisationService.Value.GetLocalisedString("Layout", "Headline");		
			
		/// <summary>
		/// Resource text HtmlStringPageTitleHome
		/// </summary>
		public static HtmlString PageTitleHome => localisationService.Value.GetLocalisedHtmlString("Layout", "PageTitleHome"); 		

		/// <summary>
		/// Resource text PageTitleHome
		/// </summary>
		public static string PageTitleHomeString => localisationService.Value.GetLocalisedString("Layout", "PageTitleHome");		
			
		/// <summary>
		/// Resource text HtmlStringPageTitleMain
		/// </summary>
		public static HtmlString PageTitleMain => localisationService.Value.GetLocalisedHtmlString("Layout", "PageTitleMain"); 		

		/// <summary>
		/// Resource text PageTitleMain
		/// </summary>
		public static string PageTitleMainString => localisationService.Value.GetLocalisedString("Layout", "PageTitleMain");		
			
		/// <summary>
		/// Resource text HtmlStringPageTitleError
		/// </summary>
		public static HtmlString PageTitleError => localisationService.Value.GetLocalisedHtmlString("Layout", "PageTitleError"); 		

		/// <summary>
		/// Resource text PageTitleError
		/// </summary>
		public static string PageTitleErrorString => localisationService.Value.GetLocalisedString("Layout", "PageTitleError");		
			
		/// <summary>
		/// Resource text HtmlStringPageTitleRegistration
		/// </summary>
		public static HtmlString PageTitleRegistration => localisationService.Value.GetLocalisedHtmlString("Layout", "PageTitleRegistration"); 		

		/// <summary>
		/// Resource text PageTitleRegistration
		/// </summary>
		public static string PageTitleRegistrationString => localisationService.Value.GetLocalisedString("Layout", "PageTitleRegistration");		
			
	}


	/// <summary>
	/// Resource module Common
	/// </summary>
	public static class CommonTexts 
	{
		private static readonly Lazy<ILocalisationService> localisationService = new Lazy<ILocalisationService>(IocProvider.Get<ILocalisationService>);

		/// <summary>
		/// Resource text HtmlStringWith
		/// </summary>
		public static HtmlString With => localisationService.Value.GetLocalisedHtmlString("Common", "With"); 		

		/// <summary>
		/// Resource text With
		/// </summary>
		public static string WithString => localisationService.Value.GetLocalisedString("Common", "With");		
			
		/// <summary>
		/// Resource text HtmlStringAnd
		/// </summary>
		public static HtmlString And => localisationService.Value.GetLocalisedHtmlString("Common", "And"); 		

		/// <summary>
		/// Resource text And
		/// </summary>
		public static string AndString => localisationService.Value.GetLocalisedString("Common", "And");		
			
		/// <summary>
		/// Resource text HtmlStringTo
		/// </summary>
		public static HtmlString To => localisationService.Value.GetLocalisedHtmlString("Common", "To"); 		

		/// <summary>
		/// Resource text To
		/// </summary>
		public new static string ToString => localisationService.Value.GetLocalisedString("Common", "To");		
			
		/// <summary>
		/// Resource text HtmlStringLanguageTranslation
		/// </summary>
		public static HtmlString LanguageTranslation => localisationService.Value.GetLocalisedHtmlString("Common", "LanguageTranslation"); 		

		/// <summary>
		/// Resource text LanguageTranslation
		/// </summary>
		public static string LanguageTranslationString => localisationService.Value.GetLocalisedString("Common", "LanguageTranslation");		
			
	}


	/// <summary>
	/// Resource module Validation
	/// </summary>
	public static class ValidationTexts 
	{
		private static readonly Lazy<ILocalisationService> localisationService = new Lazy<ILocalisationService>(IocProvider.Get<ILocalisationService>);

		/// <summary>
		/// Resource text HtmlStringAgreement
		/// </summary>
		public static HtmlString Agreement => localisationService.Value.GetLocalisedHtmlString("Validation", "Agreement"); 		

		/// <summary>
		/// Resource text Agreement
		/// </summary>
		public static string AgreementString => localisationService.Value.GetLocalisedString("Validation", "Agreement");		
			
		/// <summary>
		/// Resource text HtmlStringRequired
		/// </summary>
		public static HtmlString Required => localisationService.Value.GetLocalisedHtmlString("Validation", "Required"); 		

		/// <summary>
		/// Resource text Required
		/// </summary>
		public static string RequiredString => localisationService.Value.GetLocalisedString("Validation", "Required");		
			
		/// <summary>
		/// Resource text HtmlStringMaxLength
		/// </summary>
		public static HtmlString MaxLength => localisationService.Value.GetLocalisedHtmlString("Validation", "MaxLength"); 		

		/// <summary>
		/// Resource text MaxLength
		/// </summary>
		public static string MaxLengthString => localisationService.Value.GetLocalisedString("Validation", "MaxLength");		
			
		/// <summary>
		/// Resource text HtmlStringNotSelected
		/// </summary>
		public static HtmlString NotSelected => localisationService.Value.GetLocalisedHtmlString("Validation", "NotSelected"); 		

		/// <summary>
		/// Resource text NotSelected
		/// </summary>
		public static string NotSelectedString => localisationService.Value.GetLocalisedString("Validation", "NotSelected");		
			
		/// <summary>
		/// Resource text HtmlStringEmail
		/// </summary>
		public static HtmlString Email => localisationService.Value.GetLocalisedHtmlString("Validation", "Email"); 		

		/// <summary>
		/// Resource text Email
		/// </summary>
		public static string EmailString => localisationService.Value.GetLocalisedString("Validation", "Email");		
			
	}


	/// <summary>
	/// Resource module EmailTemplate
	/// </summary>
	public static class EmailTemplateTexts 
	{
		private static readonly Lazy<ILocalisationService> localisationService = new Lazy<ILocalisationService>(IocProvider.Get<ILocalisationService>);

		/// <summary>
		/// Resource text HtmlStringRegistrationListNote
		/// </summary>
		public static HtmlString RegistrationListNote => localisationService.Value.GetLocalisedHtmlString("EmailTemplate", "RegistrationListNote"); 		

		/// <summary>
		/// Resource text RegistrationListNote
		/// </summary>
		public static string RegistrationListNoteString => localisationService.Value.GetLocalisedString("EmailTemplate", "RegistrationListNote");		
			
		/// <summary>
		/// Resource text HtmlStringWith
		/// </summary>
		public static HtmlString With => localisationService.Value.GetLocalisedHtmlString("EmailTemplate", "With"); 		

		/// <summary>
		/// Resource text With
		/// </summary>
		public static string WithString => localisationService.Value.GetLocalisedString("EmailTemplate", "With");		
			
		/// <summary>
		/// Resource text HtmlStringAnd
		/// </summary>
		public static HtmlString And => localisationService.Value.GetLocalisedHtmlString("EmailTemplate", "And"); 		

		/// <summary>
		/// Resource text And
		/// </summary>
		public static string AndString => localisationService.Value.GetLocalisedString("EmailTemplate", "And");		
			
		/// <summary>
		/// Resource text HtmlStringTo
		/// </summary>
		public static HtmlString To => localisationService.Value.GetLocalisedHtmlString("EmailTemplate", "To"); 		

		/// <summary>
		/// Resource text To
		/// </summary>
		public new static string ToString => localisationService.Value.GetLocalisedString("EmailTemplate", "To");		
			
		/// <summary>
		/// Resource text HtmlStringClassHeader
		/// </summary>
		public static HtmlString ClassHeader => localisationService.Value.GetLocalisedHtmlString("EmailTemplate", "ClassHeader"); 		

		/// <summary>
		/// Resource text ClassHeader
		/// </summary>
		public static string ClassHeaderString => localisationService.Value.GetLocalisedString("EmailTemplate", "ClassHeader");		
			
	}

}
