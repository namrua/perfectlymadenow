  
namespace AutomationSystem.Base.Contract.Enums
{

	/// <summary>
	/// Determines type of approvement
	/// </summary>
	public enum ApprovementTypeEnum 
	{

		/// <summary>
		/// No approvement needed
		/// </summary>
		None = 1,
			
		/// <summary>
		/// Invitation approvement
		/// </summary>
		InvitationApprovement = 2,
			
		/// <summary>
		/// Manual creation approvement
		/// </summary>
		ManualApprovement = 3,
			
		/// <summary>
		/// Review of student approvement
		/// </summary>
		ManualReview = 4,
			
	}


	/// <summary>
	/// Type async request
	/// </summary>
	public enum AsyncRequestTypeEnum 
	{

		/// <summary>
		/// Sending of email
		/// </summary>
		SendEmail = 1,
			
		/// <summary>
		/// Reporting of incident
		/// </summary>
		ReportIncident = 2,
			
		/// <summary>
		/// Adding attendee to outer system
		/// </summary>
		AddToOuterSystem = 2001,
			
		/// <summary>
		/// Removing attendee from outer system
		/// </summary>
		RemoveFromOuterSystem = 2002,
			
		/// <summary>
		/// Updating outer system state of attendee
		/// </summary>
		UpdateOuterSystemState = 2003,
			
		/// <summary>
		/// Synchronizing outer system state of attendee
		/// </summary>
		SyncOuterSystemState = 2004,
			
		/// <summary>
		/// Sending of final reports
		/// </summary>
		SendFinalReports = 2005,
			
		/// <summary>
		/// Creating of deposit
		/// </summary>
		CreateDeposit = 2006,
			
		/// <summary>
		/// Sending invitation from outer system
		/// </summary>
		SendOuterSystemInvitation = 2007,
			
		/// <summary>
		/// Generating of certificates
		/// </summary>
		GenerateCertificates = 2008,
			
		/// <summary>
		/// Generating of finacial forms
		/// </summary>
		GenerateFinancialForms = 2009,
			
		/// <summary>
		/// Complete distance classes for template
		/// </summary>
		CompleteDistanceClassesForTemplate = 2010,
			
	}


	/// <summary>
	/// Type of batch upload operation
	/// </summary>
	public enum BatchUploadOperationTypeEnum 
	{

		/// <summary>
		/// Create new record
		/// </summary>
		New = 1,
			
		/// <summary>
		/// Update existing record
		/// </summary>
		Update = 2,
			
		/// <summary>
		/// Ignore record
		/// </summary>
		Ignore = 3,
			
	}


	/// <summary>
	/// State of batch upload workflow
	/// </summary>
	public enum BatchUploadStateEnum 
	{

		/// <summary>
		/// In uploading process
		/// </summary>
		InUploading = 1,
			
		/// <summary>
		/// In validation process
		/// </summary>
		InValidation = 2,
			
		/// <summary>
		/// In merging process
		/// </summary>
		InMerging = 3,
			
		/// <summary>
		/// Complete
		/// </summary>
		Complete = 4,
			
		/// <summary>
		/// Discarded
		/// </summary>
		Discarded = 5,
			
	}


	/// <summary>
	/// Type of Batch upload
	/// </summary>
	public enum BatchUploadTypeEnum 
	{

		/// <summary>
		/// Former student upload sheet
		/// </summary>
		FormerStudentExcel = 2001,
			
		/// <summary>
		/// Former student CVS
		/// </summary>
		FormerStudentCSV = 2002,
			
		/// <summary>
		/// Student registration upload sheet
		/// </summary>
		StudentRegistrationExcel = 2010,
			
	}


	/// <summary>
	/// Determines type of class action
	/// </summary>
	public enum ClassActionTypeEnum 
	{

		/// <summary>
		/// Completion of Class/Lecture
		/// </summary>
		Completion = 1,
			
		/// <summary>
		/// Cancelation of Class/Lecture
		/// </summary>
		Cancelation = 2,
			
		/// <summary>
		/// Change notification
		/// </summary>
		Change = 3,
			
		/// <summary>
		/// WWA change notification
		/// </summary>
		WwaChange = 4,
			
	}


	/// <summary>
	/// Category of class
	/// </summary>
	public enum ClassCategoryEnum 
	{

		/// <summary>
		/// Class
		/// </summary>
		Class = 1,
			
		/// <summary>
		/// Lecture
		/// </summary>
		Lecture = 2,
			
		/// <summary>
		/// Distance class
		/// </summary>
		DistanceClass = 3,
			
		/// <summary>
		/// Private material class
		/// </summary>
		PrivateMaterialClass = 4,
			
	}


	/// <summary>
	/// Determines type of expenses computed for class
	/// </summary>
	public enum ClassExpenseTypeEnum 
	{

		/// <summary>
		/// Custom expense
		/// </summary>
		Custom = 1,
			
		/// <summary>
		/// PayPal Processing Fee
		/// </summary>
		PayPalFeeClass = 2,
			
		/// <summary>
		/// PayPal Processing Fee for WWA and Lecture
		/// </summary>
		PayPalFeeWwaLecture = 3,
			
		/// <summary>
		/// FOI Royalty Fee
		/// </summary>
		FoiRoyaltyFee = 4,
			
	}


	/// <summary>
	/// Determines coordinated class type
	/// </summary>
	public enum ClassTypeEnum 
	{

		/// <summary>
		/// SITH® Basic I Ho’oponopono ONLINE class
		/// </summary>
		BasicOnline = 1,
			
		/// <summary>
		/// SITH® Business Ho’oponopono ONLINE class
		/// </summary>
		BusinessOnline = 2,
			
		/// <summary>
		/// SITH® Health Ho’oponopono ONLINE class
		/// </summary>
		HealthOnline = 3,
			
		/// <summary>
		/// SITH® Basic II Ho’oponopono ONLINE class
		/// </summary>
		Basic2Online = 4,
			
		/// <summary>
		/// SITH® Business Ho’oponopono ONLINE lecture
		/// </summary>
		LectureBusinessOnline = 5,
			
		/// <summary>
		/// SITH® Basic I Ho’oponopono ONLINE lecture
		/// </summary>
		LectureBasicOnline = 6,
			
		/// <summary>
		/// SITH® Basic II Ho’oponopono ONLINE lecture
		/// </summary>
		LectureBasic2Online = 7,
			
		/// <summary>
		/// SITH® Health Ho’oponopono ONLINE lecture
		/// </summary>
		LectureHealthOnline = 8,
			
		/// <summary>
		/// SITH® Basic I Ho’oponopono IN PERSON class
		/// </summary>
		Basic = 9,
			
		/// <summary>
		/// SITH® Business Ho’oponopono IN PERSON class
		/// </summary>
		Business = 10,
			
		/// <summary>
		/// SITH® Health Ho’oponopono IN PERSON class
		/// </summary>
		Health = 11,
			
		/// <summary>
		/// SITH® Basic II Ho’oponopono IN PERSON class
		/// </summary>
		Basic2 = 12,
			
		/// <summary>
		/// SITH® Business Ho’oponopono IN PERSON lecture
		/// </summary>
		LectureBusiness = 13,
			
		/// <summary>
		/// SITH® Basic I Ho’oponopono IN PERSON lecture
		/// </summary>
		LectureBasic = 14,
			
		/// <summary>
		/// SITH® Basic II Ho’oponopono IN PERSON lecture
		/// </summary>
		LectureBasic2 = 15,
			
		/// <summary>
		/// SITH® Health Ho’oponopono IN PERSON lecture
		/// </summary>
		LectureHealth = 16,
			
	}


	/// <summary>
	/// Type of conference account
	/// </summary>
	public enum ConferenceAccountTypeEnum 
	{

		/// <summary>
		/// WebEx account
		/// </summary>
		WebEx = 1,
			
	}


	/// <summary>
	/// Countries
	/// </summary>
	public enum CountryEnum 
	{

		/// <summary>
		/// Afghanistan
		/// </summary>
		AF = 1,
			
		/// <summary>
		/// Albania
		/// </summary>
		AL = 2,
			
		/// <summary>
		/// Algeria
		/// </summary>
		DZ = 3,
			
		/// <summary>
		/// American Samoa
		/// </summary>
		AS = 4,
			
		/// <summary>
		/// Andorra
		/// </summary>
		AD = 5,
			
		/// <summary>
		/// Angola
		/// </summary>
		AO = 6,
			
		/// <summary>
		/// Anguilla
		/// </summary>
		AI = 7,
			
		/// <summary>
		/// Antarctica
		/// </summary>
		AQ = 8,
			
		/// <summary>
		/// Antigua And Barbuda
		/// </summary>
		AG = 9,
			
		/// <summary>
		/// Argentina
		/// </summary>
		AR = 10,
			
		/// <summary>
		/// Armenia
		/// </summary>
		AM = 11,
			
		/// <summary>
		/// Aruba
		/// </summary>
		AW = 12,
			
		/// <summary>
		/// Australia
		/// </summary>
		AU = 13,
			
		/// <summary>
		/// Austria
		/// </summary>
		AT = 14,
			
		/// <summary>
		/// Azerbaijan
		/// </summary>
		AZ = 15,
			
		/// <summary>
		/// Bahamas
		/// </summary>
		BS = 16,
			
		/// <summary>
		/// Bahrain
		/// </summary>
		BH = 17,
			
		/// <summary>
		/// Bangladesh
		/// </summary>
		BD = 18,
			
		/// <summary>
		/// Barbados
		/// </summary>
		BB = 19,
			
		/// <summary>
		/// Belarus
		/// </summary>
		BY = 20,
			
		/// <summary>
		/// Belgium
		/// </summary>
		BE = 21,
			
		/// <summary>
		/// Belize
		/// </summary>
		BZ = 22,
			
		/// <summary>
		/// Benin
		/// </summary>
		BJ = 23,
			
		/// <summary>
		/// Bermuda
		/// </summary>
		BM = 24,
			
		/// <summary>
		/// Bhutan
		/// </summary>
		BT = 25,
			
		/// <summary>
		/// Bolivia
		/// </summary>
		BO = 26,
			
		/// <summary>
		/// Bosnia And Herzegovina
		/// </summary>
		BA = 27,
			
		/// <summary>
		/// Botswana
		/// </summary>
		BW = 28,
			
		/// <summary>
		/// Bouvet Island
		/// </summary>
		BV = 29,
			
		/// <summary>
		/// Brazil
		/// </summary>
		BR = 30,
			
		/// <summary>
		/// British Indian Ocean Territory
		/// </summary>
		IO = 31,
			
		/// <summary>
		/// Brunei Darussalam
		/// </summary>
		BN = 32,
			
		/// <summary>
		/// Bulgaria
		/// </summary>
		BG = 33,
			
		/// <summary>
		/// Burkina Faso
		/// </summary>
		BF = 34,
			
		/// <summary>
		/// Burundi
		/// </summary>
		BI = 35,
			
		/// <summary>
		/// Cambodia
		/// </summary>
		KH = 36,
			
		/// <summary>
		/// Cameroon
		/// </summary>
		CM = 37,
			
		/// <summary>
		/// Canada
		/// </summary>
		CA = 38,
			
		/// <summary>
		/// Cape Verde
		/// </summary>
		CV = 39,
			
		/// <summary>
		/// Cayman Islands
		/// </summary>
		KY = 40,
			
		/// <summary>
		/// Central African Republic
		/// </summary>
		CF = 41,
			
		/// <summary>
		/// Chad
		/// </summary>
		TD = 42,
			
		/// <summary>
		/// Chile
		/// </summary>
		CL = 43,
			
		/// <summary>
		/// China
		/// </summary>
		CN = 44,
			
		/// <summary>
		/// Christmas Island
		/// </summary>
		CX = 45,
			
		/// <summary>
		/// Cocos (keeling) Islands
		/// </summary>
		CC = 46,
			
		/// <summary>
		/// Colombia
		/// </summary>
		CO = 47,
			
		/// <summary>
		/// Comoros
		/// </summary>
		KM = 48,
			
		/// <summary>
		/// Congo
		/// </summary>
		CG = 49,
			
		/// <summary>
		/// Congo, The Democratic Republic Of The
		/// </summary>
		CD = 50,
			
		/// <summary>
		/// Cook Islands
		/// </summary>
		CK = 51,
			
		/// <summary>
		/// Costa Rica
		/// </summary>
		CR = 52,
			
		/// <summary>
		/// Cote D''ivoire
		/// </summary>
		CI = 53,
			
		/// <summary>
		/// Croatia
		/// </summary>
		HR = 54,
			
		/// <summary>
		/// Cuba
		/// </summary>
		CU = 55,
			
		/// <summary>
		/// Cyprus
		/// </summary>
		CY = 56,
			
		/// <summary>
		/// Czech Republic
		/// </summary>
		CZ = 57,
			
		/// <summary>
		/// Denmark
		/// </summary>
		DK = 58,
			
		/// <summary>
		/// Djibouti
		/// </summary>
		DJ = 59,
			
		/// <summary>
		/// Dominica
		/// </summary>
		DM = 60,
			
		/// <summary>
		/// Dominican Republic
		/// </summary>
		DO = 61,
			
		/// <summary>
		/// East Timor
		/// </summary>
		TP = 62,
			
		/// <summary>
		/// Ecuador
		/// </summary>
		EC = 63,
			
		/// <summary>
		/// Egypt
		/// </summary>
		EG = 64,
			
		/// <summary>
		/// El Salvador
		/// </summary>
		SV = 65,
			
		/// <summary>
		/// Equatorial Guinea
		/// </summary>
		GQ = 66,
			
		/// <summary>
		/// Eritrea
		/// </summary>
		ER = 67,
			
		/// <summary>
		/// Estonia
		/// </summary>
		EE = 68,
			
		/// <summary>
		/// Ethiopia
		/// </summary>
		ET = 69,
			
		/// <summary>
		/// Falkland Islands (malvinas)
		/// </summary>
		FK = 70,
			
		/// <summary>
		/// Faroe Islands
		/// </summary>
		FO = 71,
			
		/// <summary>
		/// Fiji
		/// </summary>
		FJ = 72,
			
		/// <summary>
		/// Finland
		/// </summary>
		FI = 73,
			
		/// <summary>
		/// France
		/// </summary>
		FR = 74,
			
		/// <summary>
		/// French Guiana
		/// </summary>
		GF = 75,
			
		/// <summary>
		/// French Polynesia
		/// </summary>
		PF = 76,
			
		/// <summary>
		/// French Southern Territories
		/// </summary>
		TF = 77,
			
		/// <summary>
		/// Gabon
		/// </summary>
		GA = 78,
			
		/// <summary>
		/// Gambia
		/// </summary>
		GM = 79,
			
		/// <summary>
		/// Georgia
		/// </summary>
		GE = 80,
			
		/// <summary>
		/// Germany
		/// </summary>
		DE = 81,
			
		/// <summary>
		/// Ghana
		/// </summary>
		GH = 82,
			
		/// <summary>
		/// Gibraltar
		/// </summary>
		GI = 83,
			
		/// <summary>
		/// Greece
		/// </summary>
		GR = 84,
			
		/// <summary>
		/// Greenland
		/// </summary>
		GL = 85,
			
		/// <summary>
		/// Grenada
		/// </summary>
		GD = 86,
			
		/// <summary>
		/// Guadeloupe
		/// </summary>
		GP = 87,
			
		/// <summary>
		/// Guam
		/// </summary>
		GU = 88,
			
		/// <summary>
		/// Guatemala
		/// </summary>
		GT = 89,
			
		/// <summary>
		/// Guinea
		/// </summary>
		GN = 90,
			
		/// <summary>
		/// Guinea-bissau
		/// </summary>
		GW = 91,
			
		/// <summary>
		/// Guyana
		/// </summary>
		GY = 92,
			
		/// <summary>
		/// Haiti
		/// </summary>
		HT = 93,
			
		/// <summary>
		/// Heard Island And Mcdonald Islands
		/// </summary>
		HM = 94,
			
		/// <summary>
		/// Holy See (vatican City State)
		/// </summary>
		VA = 95,
			
		/// <summary>
		/// Honduras
		/// </summary>
		HN = 96,
			
		/// <summary>
		/// Hong Kong
		/// </summary>
		HK = 97,
			
		/// <summary>
		/// Hungary
		/// </summary>
		HU = 98,
			
		/// <summary>
		/// Iceland
		/// </summary>
		IS = 99,
			
		/// <summary>
		/// India
		/// </summary>
		IN = 100,
			
		/// <summary>
		/// Indonesia
		/// </summary>
		ID = 101,
			
		/// <summary>
		/// Iran, Islamic Republic Of
		/// </summary>
		IR = 102,
			
		/// <summary>
		/// Iraq
		/// </summary>
		IQ = 103,
			
		/// <summary>
		/// Ireland
		/// </summary>
		IE = 104,
			
		/// <summary>
		/// Israel
		/// </summary>
		IL = 105,
			
		/// <summary>
		/// Italy
		/// </summary>
		IT = 106,
			
		/// <summary>
		/// Jamaica
		/// </summary>
		JM = 107,
			
		/// <summary>
		/// Japan
		/// </summary>
		JP = 108,
			
		/// <summary>
		/// Jordan
		/// </summary>
		JO = 109,
			
		/// <summary>
		/// Kazakstan
		/// </summary>
		KZ = 110,
			
		/// <summary>
		/// Kenya
		/// </summary>
		KE = 111,
			
		/// <summary>
		/// Kiribati
		/// </summary>
		KI = 112,
			
		/// <summary>
		/// Korea, Democratic People''s Republic Of
		/// </summary>
		KP = 113,
			
		/// <summary>
		/// Korea, Republic Of
		/// </summary>
		KR = 114,
			
		/// <summary>
		/// Kosovo
		/// </summary>
		KV = 115,
			
		/// <summary>
		/// Kuwait
		/// </summary>
		KW = 116,
			
		/// <summary>
		/// Kyrgyzstan
		/// </summary>
		KG = 117,
			
		/// <summary>
		/// Lao People''s Democratic Republic
		/// </summary>
		LA = 118,
			
		/// <summary>
		/// Latvia
		/// </summary>
		LV = 119,
			
		/// <summary>
		/// Lebanon
		/// </summary>
		LB = 120,
			
		/// <summary>
		/// Lesotho
		/// </summary>
		LS = 121,
			
		/// <summary>
		/// Liberia
		/// </summary>
		LR = 122,
			
		/// <summary>
		/// Libyan Arab Jamahiriya
		/// </summary>
		LY = 123,
			
		/// <summary>
		/// Liechtenstein
		/// </summary>
		LI = 124,
			
		/// <summary>
		/// Lithuania
		/// </summary>
		LT = 125,
			
		/// <summary>
		/// Luxembourg
		/// </summary>
		LU = 126,
			
		/// <summary>
		/// Macau
		/// </summary>
		MO = 127,
			
		/// <summary>
		/// Macedonia, The Former Yugoslav Republic Of
		/// </summary>
		MK = 128,
			
		/// <summary>
		/// Madagascar
		/// </summary>
		MG = 129,
			
		/// <summary>
		/// Malawi
		/// </summary>
		MW = 130,
			
		/// <summary>
		/// Malaysia
		/// </summary>
		MY = 131,
			
		/// <summary>
		/// Maldives
		/// </summary>
		MV = 132,
			
		/// <summary>
		/// Mali
		/// </summary>
		ML = 133,
			
		/// <summary>
		/// Malta
		/// </summary>
		MT = 134,
			
		/// <summary>
		/// Marshall Islands
		/// </summary>
		MH = 135,
			
		/// <summary>
		/// Martinique
		/// </summary>
		MQ = 136,
			
		/// <summary>
		/// Mauritania
		/// </summary>
		MR = 137,
			
		/// <summary>
		/// Mauritius
		/// </summary>
		MU = 138,
			
		/// <summary>
		/// Mayotte
		/// </summary>
		YT = 139,
			
		/// <summary>
		/// Mexico
		/// </summary>
		MX = 140,
			
		/// <summary>
		/// Micronesia, Federated States Of
		/// </summary>
		FM = 141,
			
		/// <summary>
		/// Moldova, Republic Of
		/// </summary>
		MD = 142,
			
		/// <summary>
		/// Monaco
		/// </summary>
		MC = 143,
			
		/// <summary>
		/// Mongolia
		/// </summary>
		MN = 144,
			
		/// <summary>
		/// Montserrat
		/// </summary>
		MS = 145,
			
		/// <summary>
		/// Montenegro
		/// </summary>
		ME = 146,
			
		/// <summary>
		/// Morocco
		/// </summary>
		MA = 147,
			
		/// <summary>
		/// Mozambique
		/// </summary>
		MZ = 148,
			
		/// <summary>
		/// Myanmar
		/// </summary>
		MM = 149,
			
		/// <summary>
		/// Namibia
		/// </summary>
		NA = 150,
			
		/// <summary>
		/// Nauru
		/// </summary>
		NR = 151,
			
		/// <summary>
		/// Nepal
		/// </summary>
		NP = 152,
			
		/// <summary>
		/// Netherlands
		/// </summary>
		NL = 153,
			
		/// <summary>
		/// Netherlands Antilles
		/// </summary>
		AN = 154,
			
		/// <summary>
		/// New Caledonia
		/// </summary>
		NC = 155,
			
		/// <summary>
		/// New Zealand
		/// </summary>
		NZ = 156,
			
		/// <summary>
		/// Nicaragua
		/// </summary>
		NI = 157,
			
		/// <summary>
		/// Niger
		/// </summary>
		NE = 158,
			
		/// <summary>
		/// Nigeria
		/// </summary>
		NG = 159,
			
		/// <summary>
		/// Niue
		/// </summary>
		NU = 160,
			
		/// <summary>
		/// Norfolk Island
		/// </summary>
		NF = 161,
			
		/// <summary>
		/// Northern Mariana Islands
		/// </summary>
		MP = 162,
			
		/// <summary>
		/// Norway
		/// </summary>
		NO = 163,
			
		/// <summary>
		/// Oman
		/// </summary>
		OM = 164,
			
		/// <summary>
		/// Pakistan
		/// </summary>
		PK = 165,
			
		/// <summary>
		/// Palau
		/// </summary>
		PW = 166,
			
		/// <summary>
		/// Palestinian Territory, Occupied
		/// </summary>
		PS = 167,
			
		/// <summary>
		/// Panama
		/// </summary>
		PA = 168,
			
		/// <summary>
		/// Papua New Guinea
		/// </summary>
		PG = 169,
			
		/// <summary>
		/// Paraguay
		/// </summary>
		PY = 170,
			
		/// <summary>
		/// Peru
		/// </summary>
		PE = 171,
			
		/// <summary>
		/// Philippines
		/// </summary>
		PH = 172,
			
		/// <summary>
		/// Pitcairn
		/// </summary>
		PN = 173,
			
		/// <summary>
		/// Poland
		/// </summary>
		PL = 174,
			
		/// <summary>
		/// Portugal
		/// </summary>
		PT = 175,
			
		/// <summary>
		/// Puerto Rico
		/// </summary>
		PR = 176,
			
		/// <summary>
		/// Qatar
		/// </summary>
		QA = 177,
			
		/// <summary>
		/// Reunion
		/// </summary>
		RE = 178,
			
		/// <summary>
		/// Romania
		/// </summary>
		RO = 179,
			
		/// <summary>
		/// Russian Federation
		/// </summary>
		RU = 180,
			
		/// <summary>
		/// Rwanda
		/// </summary>
		RW = 181,
			
		/// <summary>
		/// Saint Helena
		/// </summary>
		SH = 182,
			
		/// <summary>
		/// Saint Kitts And Nevis
		/// </summary>
		KN = 183,
			
		/// <summary>
		/// Saint Lucia
		/// </summary>
		LC = 184,
			
		/// <summary>
		/// Saint Pierre And Miquelon
		/// </summary>
		PM = 185,
			
		/// <summary>
		/// Saint Vincent And The Grenadines
		/// </summary>
		VC = 186,
			
		/// <summary>
		/// Samoa
		/// </summary>
		WS = 187,
			
		/// <summary>
		/// San Marino
		/// </summary>
		SM = 188,
			
		/// <summary>
		/// Sao Tome And Principe
		/// </summary>
		ST = 189,
			
		/// <summary>
		/// Saudi Arabia
		/// </summary>
		SA = 190,
			
		/// <summary>
		/// Senegal
		/// </summary>
		SN = 191,
			
		/// <summary>
		/// Serbia
		/// </summary>
		RS = 192,
			
		/// <summary>
		/// Seychelles
		/// </summary>
		SC = 193,
			
		/// <summary>
		/// Sierra Leone
		/// </summary>
		SL = 194,
			
		/// <summary>
		/// Singapore
		/// </summary>
		SG = 195,
			
		/// <summary>
		/// Slovakia
		/// </summary>
		SK = 196,
			
		/// <summary>
		/// Slovenia
		/// </summary>
		SI = 197,
			
		/// <summary>
		/// Solomon Islands
		/// </summary>
		SB = 198,
			
		/// <summary>
		/// Somalia
		/// </summary>
		SO = 199,
			
		/// <summary>
		/// South Africa
		/// </summary>
		ZA = 200,
			
		/// <summary>
		/// South Georgia And The South Sandwich Islands
		/// </summary>
		GS = 201,
			
		/// <summary>
		/// Spain
		/// </summary>
		ES = 202,
			
		/// <summary>
		/// Sri Lanka
		/// </summary>
		LK = 203,
			
		/// <summary>
		/// Sudan
		/// </summary>
		SD = 204,
			
		/// <summary>
		/// Suriname
		/// </summary>
		SR = 205,
			
		/// <summary>
		/// Svalbard And Jan Mayen
		/// </summary>
		SJ = 206,
			
		/// <summary>
		/// Swaziland
		/// </summary>
		SZ = 207,
			
		/// <summary>
		/// Sweden
		/// </summary>
		SE = 208,
			
		/// <summary>
		/// Switzerland
		/// </summary>
		CH = 209,
			
		/// <summary>
		/// Syrian Arab Republic
		/// </summary>
		SY = 210,
			
		/// <summary>
		/// Taiwan, Province Of China
		/// </summary>
		TW = 211,
			
		/// <summary>
		/// Tajikistan
		/// </summary>
		TJ = 212,
			
		/// <summary>
		/// Tanzania, United Republic Of
		/// </summary>
		TZ = 213,
			
		/// <summary>
		/// Thailand
		/// </summary>
		TH = 214,
			
		/// <summary>
		/// Togo
		/// </summary>
		TG = 215,
			
		/// <summary>
		/// Tokelau
		/// </summary>
		TK = 216,
			
		/// <summary>
		/// Tonga
		/// </summary>
		TO = 217,
			
		/// <summary>
		/// Trinidad And Tobago
		/// </summary>
		TT = 218,
			
		/// <summary>
		/// Tunisia
		/// </summary>
		TN = 219,
			
		/// <summary>
		/// Turkey
		/// </summary>
		TR = 220,
			
		/// <summary>
		/// Turkmenistan
		/// </summary>
		TM = 221,
			
		/// <summary>
		/// Turks And Caicos Islands
		/// </summary>
		TC = 222,
			
		/// <summary>
		/// Tuvalu
		/// </summary>
		TV = 223,
			
		/// <summary>
		/// Uganda
		/// </summary>
		UG = 224,
			
		/// <summary>
		/// Ukraine
		/// </summary>
		UA = 225,
			
		/// <summary>
		/// United Arab Emirates
		/// </summary>
		AE = 226,
			
		/// <summary>
		/// United Kingdom
		/// </summary>
		GB = 227,
			
		/// <summary>
		/// USA
		/// </summary>
		US = 228,
			
		/// <summary>
		/// United States Minor Outlying Islands
		/// </summary>
		UM = 229,
			
		/// <summary>
		/// Uruguay
		/// </summary>
		UY = 230,
			
		/// <summary>
		/// Uzbekistan
		/// </summary>
		UZ = 231,
			
		/// <summary>
		/// Vanuatu
		/// </summary>
		VU = 232,
			
		/// <summary>
		/// Venezuela
		/// </summary>
		VE = 233,
			
		/// <summary>
		/// Viet Nam
		/// </summary>
		VN = 234,
			
		/// <summary>
		/// Virgin Islands, British
		/// </summary>
		VG = 235,
			
		/// <summary>
		/// Virgin Islands, U.s.
		/// </summary>
		VI = 236,
			
		/// <summary>
		/// Wallis And Futuna
		/// </summary>
		WF = 237,
			
		/// <summary>
		/// Western Sahara
		/// </summary>
		EH = 238,
			
		/// <summary>
		/// Yemen
		/// </summary>
		YE = 239,
			
		/// <summary>
		/// Zambia
		/// </summary>
		ZM = 240,
			
		/// <summary>
		/// Zimbabwe
		/// </summary>
		ZW = 241,
			
	}


	/// <summary>
	/// Currencies
	/// </summary>
	public enum CurrencyEnum 
	{

		/// <summary>
		/// US Dollar
		/// </summary>
		USD = 1,
			
		/// <summary>
		/// Mexican peso
		/// </summary>
		MXN = 2,
			
		/// <summary>
		/// Romanian leu
		/// </summary>
		RON = 3,
			
	}


	/// <summary>
	/// Type of email parameter
	/// </summary>
	public enum EmailParameterTypeEnum 
	{

		/// <summary>
		/// Text
		/// </summary>
		Text = 1,
			
		/// <summary>
		/// Multiline text
		/// </summary>
		Multiline = 2,
			
		/// <summary>
		/// Globalized date and time
		/// </summary>
		DateTime = 3,
			
		/// <summary>
		/// URL link to the system
		/// </summary>
		URL = 4,
			
		/// <summary>
		/// HTML block
		/// </summary>
		HTML = 5,
			
	}


	/// <summary>
	/// Email type
	/// </summary>
	public enum EmailTypeEnum 
	{

		/// <summary>
		/// Incident warning
		/// </summary>
		CoreIncidentWarning = 1,
			
		/// <summary>
		/// Confirmation of registration
		/// </summary>
		RegistrationConfirmation = 2001,
			
		/// <summary>
		/// Invitation to registration
		/// </summary>
		RegistrationInvitation = 2002,
			
		/// <summary>
		/// Manual verification notification
		/// </summary>
		ManualVerificationNotification = 2003,
			
		/// <summary>
		/// Class change notification
		/// </summary>
		ConversationChanged = 2004,
			
		/// <summary>
		/// Class cancelation notification
		/// </summary>
		ConversationCanceled = 2005,
			
		/// <summary>
		/// Class completion notification
		/// </summary>
		ConversationCompleted = 2006,
			
		/// <summary>
		/// Cancelation of registration
		/// </summary>
		RegistrationCanceled = 2007,
			
		/// <summary>
		/// Payment request
		/// </summary>
		PaymentRequest = 2008,
			
		/// <summary>
		/// Materials notification
		/// </summary>
		MaterialsNotification = 2009,
			
		/// <summary>
		/// WWA confirmation of registration
		/// </summary>
		WwaRegistrationConfirmation = 2021,
			
		/// <summary>
		/// WWA invitation to registration
		/// </summary>
		WwaRegistrationInvitation = 2022,
			
		/// <summary>
		/// WWA class change notification
		/// </summary>
		WwaConversationChanged = 2023,
			
		/// <summary>
		/// WWA class cancelation notification
		/// </summary>
		WwaConversationCanceled = 2024,
			
		/// <summary>
		/// WWA class completion notification
		/// </summary>
		WwaConversationCompleted = 2025,
			
		/// <summary>
		/// WWA cancelation of registration
		/// </summary>
		WwaRegistrationCanceled = 2026,
			
		/// <summary>
		/// Invitation filled in notification
		/// </summary>
		InvitationFilledIn = 2041,
			
		/// <summary>
		/// WWA invitation filled in notification
		/// </summary>
		WwaInvitationFilledIn = 2042,
			
		/// <summary>
		/// Request for manual verification
		/// </summary>
		ManualVerificationRequest = 2043,
			
		/// <summary>
		/// Outer system synchronisation report
		/// </summary>
		OuterSystemSyncJobReport = 2044,
			
		/// <summary>
		/// WWA student registration notification
		/// </summary>
		WwaStudentRegistrationNotification = 2045,
			
		/// <summary>
		/// Database clearing job report
		/// </summary>
		DatabaseClearingJobReport = 2046,
			
		/// <summary>
		/// Distance class completion job report
		/// </summary>
		DistanceClassCompletionJobReport = 2047,
			
		/// <summary>
		/// Registration list
		/// </summary>
		RegistrationList = 2061,
			
		/// <summary>
		/// Registration list for master coordinator
		/// </summary>
		RegistrationListMaster = 2062,
			
		/// <summary>
		/// Request for new deposit
		/// </summary>
		NewDepositRequest = 2063,
			
		/// <summary>
		/// Class daily reports
		/// </summary>
		ClassDailyReports = 2064,
			
		/// <summary>
		/// Class final reports
		/// </summary>
		ClassFinalReports = 2065,
			
		/// <summary>
		/// Contacts notification
		/// </summary>
		ContactNotification = 2080,
			
	}


	/// <summary>
	/// Type of entity
	/// </summary>
	public enum EntityTypeEnum 
	{

		/// <summary>
		/// Email
		/// </summary>
		CoreEmail = 1,
			
		/// <summary>
		/// Task run
		/// </summary>
		CoreJobRun = 2,
			
		/// <summary>
		/// Asynchronous request
		/// </summary>
		CoreAsyncRequest = 3,
			
		/// <summary>
		/// Incident
		/// </summary>
		CoreIncident = 4,
			
		/// <summary>
		/// Coordinated class
		/// </summary>
		MainClass = 2001,
			
		/// <summary>
		/// Coordinated class action
		/// </summary>
		MainClassAction = 2002,
			
		/// <summary>
		/// Registration
		/// </summary>
		MainClassRegistration = 2005,
			
		/// <summary>
		/// Invitation
		/// </summary>
		MainClassRegistrationInvitation = 2006,
			
		/// <summary>
		/// Former class
		/// </summary>
		MainFormerClass = 2009,
			
		/// <summary>
		/// Former student
		/// </summary>
		MainFormerStudent = 2010,
			
		/// <summary>
		/// Person
		/// </summary>
		MainPerson = 2011,
			
		/// <summary>
		/// Coordinator's profile
		/// </summary>
		MainProfile = 2012,
			
		/// <summary>
		/// Distance class template
		/// </summary>
		MainDistanceClassTemplate = 2022,
			
		/// <summary>
		/// Distance profile
		/// </summary>
		MainDistanceProfile = 2023,
			
		/// <summary>
		/// Contact list
		/// </summary>
		MainContactList = 2024,
			
	}


	/// <summary>
	/// Type of enumeration
	/// </summary>
	public enum EnumTypeEnum 
	{

		/// <summary>
		/// Supported languages
		/// </summary>
		Language = 1,
			
		/// <summary>
		/// Currencies
		/// </summary>
		Currency = 2,
			
		/// <summary>
		/// Countries
		/// </summary>
		Country = 3,
			
		/// <summary>
		/// Time zones
		/// </summary>
		TimeZone = 4,
			
		/// <summary>
		/// Class type
		/// </summary>
		MainClassType = 2001,
			
		/// <summary>
		/// Registration type
		/// </summary>
		MainRegistrationType = 2002,
			
	}


	/// <summary>
	/// Encapsulates types of environments and purposes
	/// </summary>
	public enum EnvironmentTypeEnum 
	{

		/// <summary>
		/// Production environment
		/// </summary>
		Production = 1,
			
		/// <summary>
		/// Test environment
		/// </summary>
		Test = 2,
			
	}


	/// <summary>
	/// Type of stored file
	/// </summary>
	public enum FileTypeEnum 
	{

		/// <summary>
		/// Generic file
		/// </summary>
		Generic = 1,
			
		/// <summary>
		/// Excel file
		/// </summary>
		Excel = 2,
			
		/// <summary>
		/// Word file
		/// </summary>
		Word = 3,
			
		/// <summary>
		/// CSV file
		/// </summary>
		Csv = 4,
			
		/// <summary>
		/// Jpg image
		/// </summary>
		Jpg = 5,
			
		/// <summary>
		/// Png image
		/// </summary>
		Png = 6,
			
		/// <summary>
		/// Gif image
		/// </summary>
		Gif = 7,
			
		/// <summary>
		/// PDF file
		/// </summary>
		Pdf = 8,
			
	}


	/// <summary>
	/// Type of incident
	/// </summary>
	public enum IncidentTypeEnum 
	{

		/// <summary>
		/// Unknown error type
		/// </summary>
		UnknownType = 1,
			
		/// <summary>
		/// Unsuccessful interaction with outer system
		/// </summary>
		OuterSystemError = 2,
			
		/// <summary>
		/// Unsuccessful PayPal operation
		/// </summary>
		PayPalError = 3,
			
		/// <summary>
		/// Email sending error
		/// </summary>
		EmailError = 4,
			
		/// <summary>
		/// Report creating error
		/// </summary>
		ReportError = 5,
			
		/// <summary>
		/// AsyncRequest execution error
		/// </summary>
		AsyncRequestError = 6,
			
		/// <summary>
		/// Web rendering error
		/// </summary>
		WebRenderingError = 7,
			
		/// <summary>
		/// Zendesk integration error
		/// </summary>
		ZendeskError = 8,
			
		/// <summary>
		/// JobRun execution error
		/// </summary>
		JobRunError = 9,
			
		/// <summary>
		/// Maintenance error
		/// </summary>
		MaintenanceError = 10,
			
		/// <summary>
		/// Materials distribution error
		/// </summary>
		MaterialError = 2001,
			
	}


	/// <summary>
	/// Type of WebEx integration state
	/// </summary>
	public enum IntegrationStateTypeEnum 
	{

		/// <summary>
		/// Error occured when attempting to get WebEx data
		/// </summary>
		Error = 1,
			
		/// <summary>
		/// The person with attendee Id is not in the WebEx
		/// </summary>
		NotInWebEx = 2,
			
		/// <summary>
		/// The person is in the WebEx
		/// </summary>
		InWebEx = 3,
			
	}


	/// <summary>
	/// Determines conference integration type
	/// </summary>
	public enum IntegrationTypeEnum 
	{

		/// <summary>
		/// No integration
		/// </summary>
		NoIntegration = 1,
			
		/// <summary>
		/// WebEx program
		/// </summary>
		WebExProgram = 2001,
			
	}


	/// <summary>
	/// Type of job
	/// </summary>
	public enum JobTypeEnum 
	{

		/// <summary>
		/// Synchronisation unpropagated incidents
		/// </summary>
		ZendeskSyncJob = 1,
			
		/// <summary>
		/// Job that sends emails with low priority
		/// </summary>
		EmailSendJob = 2,
			
		/// <summary>
		/// Synchronisation data with outer system
		/// </summary>
		OuterSystemSyncJob = 3,
			
		/// <summary>
		/// Test job serves for testing JobRunsEnvelope
		/// </summary>
		TestJob = 4,
			
		/// <summary>
		/// Job that sends periodical reports
		/// </summary>
		ReportSendJob = 5,
			
		/// <summary>
		/// Job that processes reports for active classes
		/// </summary>
		MainActiveClassReportJob = 2001,
			
		/// <summary>
		/// Job that clears deleted or obsoleted records in database
		/// </summary>
		MainDatabaseClearingJob = 2002,
			
		/// <summary>
		/// Job that completes distance classes
		/// </summary>
		MainDistanceClassCompletionJob = 2003,
			
	}


	/// <summary>
	/// Supported languages
	/// </summary>
	public enum LanguageEnum 
	{

		/// <summary>
		/// English
		/// </summary>
		En = 1,
			
		/// <summary>
		/// Čeština
		/// </summary>
		Cs = 2,
			
		/// <summary>
		/// Español
		/// </summary>
		Es = 3,
			
		/// <summary>
		/// Français
		/// </summary>
		Fr = 4,
			
		/// <summary>
		/// Română
		/// </summary>
		Ro = 5,
			
		/// <summary>
		/// Português
		/// </summary>
		Pt = 6,
			
	}


	/// <summary>
	/// Determines possible person type of roles
	/// </summary>
	public enum PersonRoleTypeEnum 
	{

		/// <summary>
		/// Coordinator
		/// </summary>
		Coordinator = 1,
			
		/// <summary>
		/// Distance coordinator
		/// </summary>
		DistanceDoordinator = 2,
			
		/// <summary>
		/// Instructor
		/// </summary>
		Instructor = 3,
			
		/// <summary>
		/// Approved staff
		/// </summary>
		ApprovedStaff = 4,
			
		/// <summary>
		/// Contact person
		/// </summary>
		Contact = 5,
			
		/// <summary>
		/// Guest instructor
		/// </summary>
		GuestInstructor = 6,
			
	}


	/// <summary>
	/// Determines pricelist type
	/// </summary>
	public enum PriceListTypeEnum 
	{

		/// <summary>
		/// Class price list
		/// </summary>
		Class = 1,
			
		/// <summary>
		/// Lecture price list
		/// </summary>
		Lecture = 2,
			
		/// <summary>
		/// WWA class price list
		/// </summary>
		WwaClass = 3,
			
		/// <summary>
		/// Material class price list
		/// </summary>
		MaterialClass = 4,
			
	}


	/// <summary>
	/// State of processing e.g. job
	/// </summary>
	public enum ProcessingStateEnum 
	{

		/// <summary>
		/// New
		/// </summary>
		New = 1,
			
		/// <summary>
		/// Skipped
		/// </summary>
		Skipped = 2,
			
		/// <summary>
		/// In process
		/// </summary>
		InProcess = 3,
			
		/// <summary>
		/// Finished
		/// </summary>
		Finished = 4,
			
		/// <summary>
		/// Error
		/// </summary>
		Error = 5,
			
	}


	/// <summary>
	/// Determines color and style scheme for public registration pages
	/// </summary>
	public enum RegistrationColorSchemeEnum 
	{

		/// <summary>
		/// Default limet-green scheme
		/// </summary>
		Limet = 1,
			
		/// <summary>
		/// Ocean-blue scheme
		/// </summary>
		Ocean = 2,
			
	}


	/// <summary>
	/// Determines type of registration form and filled parameters
	/// </summary>
	public enum RegistrationFormTypeEnum 
	{

		/// <summary>
		/// Only student's fields required
		/// </summary>
		Adult = 1,
			
		/// <summary>
		/// Both fields required, registrant email optional
		/// </summary>
		Child = 2,
			
		/// <summary>
		/// Both fields required, Student LN, Student address optional, student Country required
		/// </summary>
		WWA = 3,
			
	}


	/// <summary>
	/// Determines student status and registration type
	/// </summary>
	public enum RegistrationTypeEnum 
	{

		/// <summary>
		/// New Student – Adult (14 yrs. & older) Pre-Registration
		/// </summary>
		NewAdult = 1,
			
		/// <summary>
		/// New Student – Adult (14 yrs. & older) Registration Week of Class
		/// </summary>
		NewAdultWeekOfClass = 2,
			
		/// <summary>
		/// New Student – Child (birth – 13 yrs.)
		/// </summary>
		NewChild = 3,
			
		/// <summary>
		/// Review Student – Adult (14 yrs. & older)
		/// </summary>
		ReviewAdult = 4,
			
		/// <summary>
		/// Review Student – Child (birth – 13 yrs.)
		/// </summary>
		ReviewChild = 5,
			
		/// <summary>
		/// World Wide Absentee – Any I-Dentity
		/// </summary>
		WWA = 6,
			
		/// <summary>
		/// Approved Guest
		/// </summary>
		ApprovedGuest = 7,
			
		/// <summary>
		/// Online Lecture Registration
		/// </summary>
		LectureRegistration = 8,
			
		/// <summary>
		/// Material registration
		/// </summary>
		MaterialRegistration = 9,
			
	}


	/// <summary>
	/// Royalty FeeRate Type
	/// </summary>
	public enum RoyaltyFeeRateTypeEnum 
	{

		/// <summary>
		/// New Student
		/// </summary>
		NewStudent = 1,
			
		/// <summary>
		/// New Child
		/// </summary>
		NewChild = 2,
			
		/// <summary>
		/// Review Student (excluding Absentee)
		/// </summary>
		ReviewStudent = 3,
			
		/// <summary>
		/// Review Child (excluding Absentee)
		/// </summary>
		ReviewChild = 4,
			
	}


	/// <summary>
	/// Severity of item, e.g. email
	/// </summary>
	public enum SeverityEnum 
	{

		/// <summary>
		/// Fatal severity
		/// </summary>
		Fatal = 10,
			
		/// <summary>
		/// High severity
		/// </summary>
		High = 20,
			
		/// <summary>
		/// Normal severity
		/// </summary>
		Normal = 30,
			
		/// <summary>
		/// Low severity
		/// </summary>
		Low = 40,
			
		/// <summary>
		/// Trivial severity
		/// </summary>
		Trivial = 50,
			
	}


	/// <summary>
	/// TimeZones
	/// </summary>
	public enum TimeZoneEnum 
	{

		/// <summary>
		/// (UTC-12:00) International Date Line West
		/// </summary>
		DatelineStandardTime = 1,
			
		/// <summary>
		/// (UTC-11:00) Coordinated Universal Time-11
		/// </summary>
		UTC11 = 2,
			
		/// <summary>
		/// (UTC-10:00) Aleutian Islands
		/// </summary>
		AleutianStandardTime = 3,
			
		/// <summary>
		/// (UTC-10:00) Hawaii
		/// </summary>
		HawaiianStandardTime = 4,
			
		/// <summary>
		/// (UTC-09:30) Marquesas Islands
		/// </summary>
		MarquesasStandardTime = 5,
			
		/// <summary>
		/// (UTC-09:00) Alaska
		/// </summary>
		AlaskanStandardTime = 6,
			
		/// <summary>
		/// (UTC-09:00) Coordinated Universal Time-09
		/// </summary>
		UTC09 = 7,
			
		/// <summary>
		/// (UTC-08:00) Baja California
		/// </summary>
		PacificStandardTimeMexico = 8,
			
		/// <summary>
		/// (UTC-08:00) Coordinated Universal Time-08
		/// </summary>
		UTC08 = 9,
			
		/// <summary>
		/// (UTC-08:00) Pacific Time (US & Canada)
		/// </summary>
		PacificStandardTime = 10,
			
		/// <summary>
		/// (UTC-07:00) Arizona
		/// </summary>
		USMountainStandardTime = 11,
			
		/// <summary>
		/// (UTC-07:00) Chihuahua, La Paz, Mazatlan
		/// </summary>
		MountainStandardTimeMexico = 12,
			
		/// <summary>
		/// (UTC-07:00) Mountain Time (US & Canada)
		/// </summary>
		MountainStandardTime = 13,
			
		/// <summary>
		/// (UTC-07:00) Yukon
		/// </summary>
		YukonStandardTime = 14,
			
		/// <summary>
		/// (UTC-06:00) Central America
		/// </summary>
		CentralAmericaStandardTime = 15,
			
		/// <summary>
		/// (UTC-06:00) Central Time (US & Canada)
		/// </summary>
		CentralStandardTime = 16,
			
		/// <summary>
		/// (UTC-06:00) Easter Island
		/// </summary>
		EasterIslandStandardTime = 17,
			
		/// <summary>
		/// (UTC-06:00) Guadalajara, Mexico City, Monterrey
		/// </summary>
		CentralStandardTimeMexico = 18,
			
		/// <summary>
		/// (UTC-06:00) Saskatchewan
		/// </summary>
		CanadaCentralStandardTime = 19,
			
		/// <summary>
		/// (UTC-05:00) Bogota, Lima, Quito, Rio Branco
		/// </summary>
		SAPacificStandardTime = 20,
			
		/// <summary>
		/// (UTC-05:00) Chetumal
		/// </summary>
		EasternStandardTimeMexico = 21,
			
		/// <summary>
		/// (UTC-05:00) Eastern Time (US & Canada)
		/// </summary>
		EasternStandardTime = 22,
			
		/// <summary>
		/// (UTC-05:00) Haiti
		/// </summary>
		HaitiStandardTime = 23,
			
		/// <summary>
		/// (UTC-05:00) Havana
		/// </summary>
		CubaStandardTime = 24,
			
		/// <summary>
		/// (UTC-05:00) Indiana (East)
		/// </summary>
		USEasternStandardTime = 25,
			
		/// <summary>
		/// (UTC-05:00) Turks and Caicos
		/// </summary>
		TurksAndCaicosStandardTime = 26,
			
		/// <summary>
		/// (UTC-04:00) Asuncion
		/// </summary>
		ParaguayStandardTime = 27,
			
		/// <summary>
		/// (UTC-04:00) Atlantic Time (Canada)
		/// </summary>
		AtlanticStandardTime = 28,
			
		/// <summary>
		/// (UTC-04:00) Caracas
		/// </summary>
		VenezuelaStandardTime = 29,
			
		/// <summary>
		/// (UTC-04:00) Cuiaba
		/// </summary>
		CentralBrazilianStandardTime = 30,
			
		/// <summary>
		/// (UTC-04:00) Georgetown, La Paz, Manaus, San Juan
		/// </summary>
		SAWesternStandardTime = 31,
			
		/// <summary>
		/// (UTC-04:00) Santiago
		/// </summary>
		PacificSAStandardTime = 32,
			
		/// <summary>
		/// (UTC-03:30) Newfoundland
		/// </summary>
		NewfoundlandStandardTime = 33,
			
		/// <summary>
		/// (UTC-03:00) Araguaina
		/// </summary>
		TocantinsStandardTime = 34,
			
		/// <summary>
		/// (UTC-03:00) Brasilia
		/// </summary>
		ESouthAmericaStandardTime = 35,
			
		/// <summary>
		/// (UTC-03:00) Cayenne, Fortaleza
		/// </summary>
		SAEasternStandardTime = 36,
			
		/// <summary>
		/// (UTC-03:00) City of Buenos Aires
		/// </summary>
		ArgentinaStandardTime = 37,
			
		/// <summary>
		/// (UTC-03:00) Greenland
		/// </summary>
		GreenlandStandardTime = 38,
			
		/// <summary>
		/// (UTC-03:00) Montevideo
		/// </summary>
		MontevideoStandardTime = 39,
			
		/// <summary>
		/// (UTC-03:00) Punta Arenas
		/// </summary>
		MagallanesStandardTime = 40,
			
		/// <summary>
		/// (UTC-03:00) Saint Pierre and Miquelon
		/// </summary>
		SaintPierreStandardTime = 41,
			
		/// <summary>
		/// (UTC-03:00) Salvador
		/// </summary>
		BahiaStandardTime = 42,
			
		/// <summary>
		/// (UTC-02:00) Coordinated Universal Time-02
		/// </summary>
		UTC02 = 43,
			
		/// <summary>
		/// (UTC-02:00) Mid-Atlantic - Old
		/// </summary>
		MidAtlanticStandardTime = 44,
			
		/// <summary>
		/// (UTC-01:00) Azores
		/// </summary>
		AzoresStandardTime = 45,
			
		/// <summary>
		/// (UTC-01:00) Cabo Verde Is.
		/// </summary>
		CapeVerdeStandardTime = 46,
			
		/// <summary>
		/// (UTC) Coordinated Universal Time
		/// </summary>
		UTC = 47,
			
		/// <summary>
		/// (UTC+00:00) Dublin, Edinburgh, Lisbon, London
		/// </summary>
		GMTStandardTime = 48,
			
		/// <summary>
		/// (UTC+00:00) Monrovia, Reykjavik
		/// </summary>
		GreenwichStandardTime = 49,
			
		/// <summary>
		/// (UTC+00:00) Sao Tome
		/// </summary>
		SaoTomeStandardTime = 50,
			
		/// <summary>
		/// (UTC+01:00) Casablanca
		/// </summary>
		MoroccoStandardTime = 51,
			
		/// <summary>
		/// (UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna
		/// </summary>
		WEuropeStandardTime = 52,
			
		/// <summary>
		/// (UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague
		/// </summary>
		CentralEuropeStandardTime = 53,
			
		/// <summary>
		/// (UTC+01:00) Brussels, Copenhagen, Madrid, Paris
		/// </summary>
		RomanceStandardTime = 54,
			
		/// <summary>
		/// (UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb
		/// </summary>
		CentralEuropeanStandardTime = 55,
			
		/// <summary>
		/// (UTC+01:00) West Central Africa
		/// </summary>
		WCentralAfricaStandardTime = 56,
			
		/// <summary>
		/// (UTC+02:00) Amman
		/// </summary>
		JordanStandardTime = 57,
			
		/// <summary>
		/// (UTC+02:00) Athens, Bucharest
		/// </summary>
		GTBStandardTime = 58,
			
		/// <summary>
		/// (UTC+02:00) Beirut
		/// </summary>
		MiddleEastStandardTime = 59,
			
		/// <summary>
		/// (UTC+02:00) Cairo
		/// </summary>
		EgyptStandardTime = 60,
			
		/// <summary>
		/// (UTC+02:00) Chisinau
		/// </summary>
		EEuropeStandardTime = 61,
			
		/// <summary>
		/// (UTC+02:00) Damascus
		/// </summary>
		SyriaStandardTime = 62,
			
		/// <summary>
		/// (UTC+02:00) Gaza, Hebron
		/// </summary>
		WestBankStandardTime = 63,
			
		/// <summary>
		/// (UTC+02:00) Harare, Pretoria
		/// </summary>
		SouthAfricaStandardTime = 64,
			
		/// <summary>
		/// (UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius
		/// </summary>
		FLEStandardTime = 65,
			
		/// <summary>
		/// (UTC+02:00) Jerusalem
		/// </summary>
		IsraelStandardTime = 66,
			
		/// <summary>
		/// (UTC+02:00) Juba
		/// </summary>
		SouthSudanStandardTime = 67,
			
		/// <summary>
		/// (UTC+02:00) Kaliningrad
		/// </summary>
		KaliningradStandardTime = 68,
			
		/// <summary>
		/// (UTC+02:00) Khartoum
		/// </summary>
		SudanStandardTime = 69,
			
		/// <summary>
		/// (UTC+02:00) Tripoli
		/// </summary>
		LibyaStandardTime = 70,
			
		/// <summary>
		/// (UTC+02:00) Windhoek
		/// </summary>
		NamibiaStandardTime = 71,
			
		/// <summary>
		/// (UTC+03:00) Baghdad
		/// </summary>
		ArabicStandardTime = 72,
			
		/// <summary>
		/// (UTC+03:00) Istanbul
		/// </summary>
		TurkeyStandardTime = 73,
			
		/// <summary>
		/// (UTC+03:00) Kuwait, Riyadh
		/// </summary>
		ArabStandardTime = 74,
			
		/// <summary>
		/// (UTC+03:00) Minsk
		/// </summary>
		BelarusStandardTime = 75,
			
		/// <summary>
		/// (UTC+03:00) Moscow, St. Petersburg
		/// </summary>
		RussianStandardTime = 76,
			
		/// <summary>
		/// (UTC+03:00) Nairobi
		/// </summary>
		EAfricaStandardTime = 77,
			
		/// <summary>
		/// (UTC+03:00) Volgograd
		/// </summary>
		VolgogradStandardTime = 78,
			
		/// <summary>
		/// (UTC+03:30) Tehran
		/// </summary>
		IranStandardTime = 79,
			
		/// <summary>
		/// (UTC+04:00) Abu Dhabi, Muscat
		/// </summary>
		ArabianStandardTime = 80,
			
		/// <summary>
		/// (UTC+04:00) Astrakhan, Ulyanovsk
		/// </summary>
		AstrakhanStandardTime = 81,
			
		/// <summary>
		/// (UTC+04:00) Baku
		/// </summary>
		AzerbaijanStandardTime = 82,
			
		/// <summary>
		/// (UTC+04:00) Izhevsk, Samara
		/// </summary>
		RussiaTimeZone3 = 83,
			
		/// <summary>
		/// (UTC+04:00) Port Louis
		/// </summary>
		MauritiusStandardTime = 84,
			
		/// <summary>
		/// (UTC+04:00) Saratov
		/// </summary>
		SaratovStandardTime = 85,
			
		/// <summary>
		/// (UTC+04:00) Tbilisi
		/// </summary>
		GeorgianStandardTime = 86,
			
		/// <summary>
		/// (UTC+04:00) Yerevan
		/// </summary>
		CaucasusStandardTime = 87,
			
		/// <summary>
		/// (UTC+04:30) Kabul
		/// </summary>
		AfghanistanStandardTime = 88,
			
		/// <summary>
		/// (UTC+05:00) Ashgabat, Tashkent
		/// </summary>
		WestAsiaStandardTime = 89,
			
		/// <summary>
		/// (UTC+05:00) Ekaterinburg
		/// </summary>
		EkaterinburgStandardTime = 90,
			
		/// <summary>
		/// (UTC+05:00) Islamabad, Karachi
		/// </summary>
		PakistanStandardTime = 91,
			
		/// <summary>
		/// (UTC+05:00) Qyzylorda
		/// </summary>
		QyzylordaStandardTime = 92,
			
		/// <summary>
		/// (UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi
		/// </summary>
		IndiaStandardTime = 93,
			
		/// <summary>
		/// (UTC+05:30) Sri Jayawardenepura
		/// </summary>
		SriLankaStandardTime = 94,
			
		/// <summary>
		/// (UTC+05:45) Kathmandu
		/// </summary>
		NepalStandardTime = 95,
			
		/// <summary>
		/// (UTC+06:00) Astana
		/// </summary>
		CentralAsiaStandardTime = 96,
			
		/// <summary>
		/// (UTC+06:00) Dhaka
		/// </summary>
		BangladeshStandardTime = 97,
			
		/// <summary>
		/// (UTC+06:00) Omsk
		/// </summary>
		OmskStandardTime = 98,
			
		/// <summary>
		/// (UTC+06:30) Yangon (Rangoon)
		/// </summary>
		MyanmarStandardTime = 99,
			
		/// <summary>
		/// (UTC+07:00) Bangkok, Hanoi, Jakarta
		/// </summary>
		SEAsiaStandardTime = 100,
			
		/// <summary>
		/// (UTC+07:00) Barnaul, Gorno-Altaysk
		/// </summary>
		AltaiStandardTime = 101,
			
		/// <summary>
		/// (UTC+07:00) Hovd
		/// </summary>
		WMongoliaStandardTime = 102,
			
		/// <summary>
		/// (UTC+07:00) Krasnoyarsk
		/// </summary>
		NorthAsiaStandardTime = 103,
			
		/// <summary>
		/// (UTC+07:00) Novosibirsk
		/// </summary>
		NCentralAsiaStandardTime = 104,
			
		/// <summary>
		/// (UTC+07:00) Tomsk
		/// </summary>
		TomskStandardTime = 105,
			
		/// <summary>
		/// (UTC+08:00) Beijing, Chongqing, Hong Kong, Urumqi
		/// </summary>
		ChinaStandardTime = 106,
			
		/// <summary>
		/// (UTC+08:00) Irkutsk
		/// </summary>
		NorthAsiaEastStandardTime = 107,
			
		/// <summary>
		/// (UTC+08:00) Kuala Lumpur, Singapore
		/// </summary>
		SingaporeStandardTime = 108,
			
		/// <summary>
		/// (UTC+08:00) Perth
		/// </summary>
		WAustraliaStandardTime = 109,
			
		/// <summary>
		/// (UTC+08:00) Taipei
		/// </summary>
		TaipeiStandardTime = 110,
			
		/// <summary>
		/// (UTC+08:00) Ulaanbaatar
		/// </summary>
		UlaanbaatarStandardTime = 111,
			
		/// <summary>
		/// (UTC+08:45) Eucla
		/// </summary>
		AusCentralWStandardTime = 112,
			
		/// <summary>
		/// (UTC+09:00) Chita
		/// </summary>
		TransbaikalStandardTime = 113,
			
		/// <summary>
		/// (UTC+09:00) Osaka, Sapporo, Tokyo
		/// </summary>
		TokyoStandardTime = 114,
			
		/// <summary>
		/// (UTC+09:00) Pyongyang
		/// </summary>
		NorthKoreaStandardTime = 115,
			
		/// <summary>
		/// (UTC+09:00) Seoul
		/// </summary>
		KoreaStandardTime = 116,
			
		/// <summary>
		/// (UTC+09:00) Yakutsk
		/// </summary>
		YakutskStandardTime = 117,
			
		/// <summary>
		/// (UTC+09:30) Adelaide
		/// </summary>
		CenAustraliaStandardTime = 118,
			
		/// <summary>
		/// (UTC+09:30) Darwin
		/// </summary>
		AUSCentralStandardTime = 119,
			
		/// <summary>
		/// (UTC+10:00) Brisbane
		/// </summary>
		EAustraliaStandardTime = 120,
			
		/// <summary>
		/// (UTC+10:00) Canberra, Melbourne, Sydney
		/// </summary>
		AUSEasternStandardTime = 121,
			
		/// <summary>
		/// (UTC+10:00) Guam, Port Moresby
		/// </summary>
		WestPacificStandardTime = 122,
			
		/// <summary>
		/// (UTC+10:00) Hobart
		/// </summary>
		TasmaniaStandardTime = 123,
			
		/// <summary>
		/// (UTC+10:00) Vladivostok
		/// </summary>
		VladivostokStandardTime = 124,
			
		/// <summary>
		/// (UTC+10:30) Lord Howe Island
		/// </summary>
		LordHoweStandardTime = 125,
			
		/// <summary>
		/// (UTC+11:00) Bougainville Island
		/// </summary>
		BougainvilleStandardTime = 126,
			
		/// <summary>
		/// (UTC+11:00) Chokurdakh
		/// </summary>
		RussiaTimeZone10 = 127,
			
		/// <summary>
		/// (UTC+11:00) Magadan
		/// </summary>
		MagadanStandardTime = 128,
			
		/// <summary>
		/// (UTC+11:00) Norfolk Island
		/// </summary>
		NorfolkStandardTime = 129,
			
		/// <summary>
		/// (UTC+11:00) Sakhalin
		/// </summary>
		SakhalinStandardTime = 130,
			
		/// <summary>
		/// (UTC+11:00) Solomon Is., New Caledonia
		/// </summary>
		CentralPacificStandardTime = 131,
			
		/// <summary>
		/// (UTC+12:00) Anadyr, Petropavlovsk-Kamchatsky
		/// </summary>
		RussiaTimeZone11 = 132,
			
		/// <summary>
		/// (UTC+12:00) Auckland, Wellington
		/// </summary>
		NewZealandStandardTime = 133,
			
		/// <summary>
		/// (UTC+12:00) Coordinated Universal Time+12
		/// </summary>
		UTC12 = 134,
			
		/// <summary>
		/// (UTC+12:00) Fiji
		/// </summary>
		FijiStandardTime = 135,
			
		/// <summary>
		/// (UTC+12:00) Petropavlovsk-Kamchatsky - Old
		/// </summary>
		KamchatkaStandardTime = 136,
			
		/// <summary>
		/// (UTC+12:45) Chatham Islands
		/// </summary>
		ChathamIslandsStandardTime = 137,
			
		/// <summary>
		/// (UTC+13:00) Coordinated Universal Time+13
		/// </summary>
		UTC13 = 138,
			
		/// <summary>
		/// (UTC+13:00) Nuku''alofa
		/// </summary>
		TongaStandardTime = 139,
			
		/// <summary>
		/// (UTC+13:00) Samoa
		/// </summary>
		SamoaStandardTime = 140,
			
		/// <summary>
		/// (UTC+14:00) Kiritimati Island
		/// </summary>
		LineIslandsStandardTime = 141,
			
	}


	/// <summary>
	/// Defines type of user group where an entity can be incorporated
	/// </summary>
	public enum UserGroupTypeEnum 
	{

		/// <summary>
		/// Coordinator's profile
		/// </summary>
		MainProfile = 2001,
			
	}


	/// <summary>
	/// Type of identity role
	/// </summary>
	public enum UserRoleTypeEnum 
	{

		/// <summary>
		/// Administrator
		/// </summary>
		Administrator = 1,
			
		/// <summary>
		/// Coordinator
		/// </summary>
		Coordinator = 2,
			
		/// <summary>
		/// Super Coordinator
		/// </summary>
		SuperCoordinator = 3,
			
		/// <summary>
		/// Distance coordinator
		/// </summary>
		DistanceCoordinator = 4,
			
		/// <summary>
		/// Translator
		/// </summary>
		Translator = 5,
			
	}

}
