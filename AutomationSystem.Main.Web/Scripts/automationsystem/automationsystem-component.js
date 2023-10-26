// namespace AutomationSystem
var AutomationSystem = window.AutomationSystem || {}


// address localisation template
AutomationSystem.AddressLocalisationTemplate = function() {
    this.firstName = "First name";
    this.lastName = "Last name";
    this.street = "Address line 1";
    this.street2 = "Address line 2";
    this.city = "City";
    this.state = "State";
    this.country = "Country";
    this.zipCode = "Zip code";
}

// address logic
AutomationSystem.Address = function(_form, _name, _options, _localisation) {

    this.form = _form;
    this.name = _name;

    var isRequired = true;
    if (CorabeuControl.isDefined(_options)) {
        isRequired = !CorabeuControl.valueOrDefault(_options.isIncomplete, false);
    }

    this.countryId = new CorabeuControl.DropDownList(this.getId("countryId"), this.getName("CountryId"));

    // initializes localisation
    var localisation = CorabeuControl.valueOrDefault(_localisation, new AutomationSystem.AddressLocalisationTemplate());

    this.form.validateControl($(this.getIdSelector("firstName")), localisation.firstName, { required: true, maxlength: 64 });
    this.form.validateControl($(this.getIdSelector("lastName")), localisation.lastName, { required: isRequired, maxlength: 64 });
    this.form.validateControl($(this.getIdSelector("street")), localisation.street, { required: isRequired, maxlength: 64 });
    this.form.validateControl($(this.getIdSelector("street2")), localisation.street2, { maxlength: 64 });
    this.form.validateControl($(this.getIdSelector("city")), localisation.city, { required: isRequired, maxlength: 64 });
    this.form.validateControl($(this.getIdSelector("state")), localisation.state, { maxlength: 64 });
    this.form.validateControl(this.countryId.element, localisation.country, { notSelected: true });
    this.form.validateControl($(this.getIdSelector("zipCode")), localisation.zipCode, { required: isRequired, maxlength: 16 });

}
AutomationSystem.Address.prototype = new CorabeuControl.Component();


AutomationSystem.createAdminMessage = function(_alertType, _message) {
    var messageBox =
        '<div class="' + 'alert alert-dismissible fade show ' + _alertType + '" role="alert">' +
            '<button type= "button" class="close" data-dismiss="alert" aria-label="Close">' +
            '<span aria-hidden="true">&times;</span></button >' + _message + '</div>';
    $("#alert-placeholder").append(messageBox);
}