/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
CORABEU CONTROLS
Author: Jan Konečný - 2017

Corabeu control encapsulates fundamental operation via HTML forms and other web elements to unify and simplify web development.

Dependencies:
* TODO:

Content (namespace CorabeuControl):



* FORM CLASSES:

* COMPONENT CLASSES:

* VALIDATION CLASSES:
  
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

// namespace CorabeuControl
var CorabeuControl = window.CorabeuControl || {}


/* * * * * * * * * * * * * * * * * FORM CLASSES  * * * * * * * * * * * * * * * * * * * * * * * * * */

// corabeu form, manages validations
CorabeuControl.Form = function (_formId) {
    
    this.id = _formId;
    this.element = $("#" + _formId);
    this.messageProvider = new CorabeuControl.ValidationMessageProvider();
   
    // inits validation   
    this.element.validate({
        ignore: ["not:hidden"],
        errorPlacement: function (error, element) {
            var placeholderSelector = "#error-placeholder-" + element.attr("id");
            error.appendTo(placeholderSelector);
        }
    });
    

    // REGISTRATION OF VALIDATORS

    // validation settings, registration of validation methods, and error placement configuration
    CorabeuControl.ValidationHelper.registerNotSelected();
    CorabeuControl.ValidationHelper.registerNotEqual();

    // registers date time validations
    this.registerDateTimeValidators = function()
    {
        CorabeuControl.ValidationHelper.registerDateTimeValidator("dateMin",
            function (value, param) {
                return param.isSameOrBefore(value);
            }
        );
        CorabeuControl.ValidationHelper.registerDateTimeValidator("dateMinPicker",
            function (value, picker) {
                var pickerValue = picker.getValueAsDate();
                return pickerValue == null || pickerValue.isSameOrBefore(value);
            }
        );
        CorabeuControl.ValidationHelper.registerDateTimeValidator("dateMinEqualPicker",
            function (value, picker) {
                var pickerValue = picker.getValueAsDate();
                return pickerValue == null || pickerValue.isBefore(value);
            }
        );
    }


    // VALIDATION

    // validates fundamental controls
    this.validateControl = function (_element, _name, _options) {        
        var params = { name: _name };

        // required
        if (CorabeuControl.isDefined(_options.required) && _options.required)
            CorabeuControl.ValidationHelper.addRule(_element, "required", true, this.messageProvider.getMessage("required", params));

        // email address
        if (CorabeuControl.isDefined(_options.email) && _options.email) {
            CorabeuControl.ValidationHelper.addRule(_element, "email", true, this.messageProvider.getMessage("email", params));
        }

        // not selected 
        if (CorabeuControl.isDefined(_options.notSelected) && _options.notSelected) {
            CorabeuControl.ValidationHelper.addRule(_element, "notSelected", true, this.messageProvider.getMessage("notSelected", params));
        }

        // number
        if (CorabeuControl.isDefined(_options.number) && _options.number) {
            CorabeuControl.ValidationHelper.addRule(_element, "number", true, this.messageProvider.getMessage("number", params));
        }

        // min length
        if (CorabeuControl.isDefined(_options.minlength)) {
            params.minlength = _options.minlength;
            CorabeuControl.ValidationHelper.addRule(_element, "minlength", _options.minlength, this.messageProvider.getMessage("minlength", params));
        }

        // max length
        if (CorabeuControl.isDefined(_options.maxlength)) {
            params.maxlength = _options.maxlength;
            CorabeuControl.ValidationHelper.addRule(_element, "maxlength", _options.maxlength, this.messageProvider.getMessage("maxlength", params));
        }

        // min 
        if (CorabeuControl.isDefined(_options.min)) {
            params.min = _options.min;
            CorabeuControl.ValidationHelper.addRule(_element, "min", _options.min, this.messageProvider.getMessage("min", params));
        }

        // max 
        if (CorabeuControl.isDefined(_options.max)) {
            params.max = _options.max;
            CorabeuControl.ValidationHelper.addRule(_element, "max", _options.max, this.messageProvider.getMessage("max", params));
        }   

        // digits
        if (CorabeuControl.isDefined(_options.digits) && _options.digits) {
            CorabeuControl.ValidationHelper.addRule(_element, "digits", true, this.messageProvider.getMessage("digits", params));
        }

    }

    // validates date time control
    this.validateDateTimeControl = function (_component, _name, _options) {
        var params = { name: _name };

        // executes fundamental validation
        this.validateControl(_component.element, _name, _options);

        // required
        if (CorabeuControl.isDefined(_options.dateMin)) {
            params.dateMinText = _options.dateMinText;
            CorabeuControl.ValidationHelper.addDateTimeRule(_component, "dateMin", _options.dateMin, this.messageProvider.getMessage("dateMin", params));
        }

        // dateMinPicker
        if (CorabeuControl.isDefined(_options.dateMinPicker)) {
            params.dateMinPickerText = _options.dateMinPickerText;
            CorabeuControl.ValidationHelper.addDateTimeRule(_component, "dateMinPicker", _options.dateMinPicker, this.messageProvider.getMessage("dateMinPicker", params));
        }

        // dateMinEqualPicker
        if (CorabeuControl.isDefined(_options.dateMinEqualPicker)) {
            params.dateMinEqualPickerText = _options.dateMinEqualPickerText;
            CorabeuControl.ValidationHelper.addDateTimeRule(_component, "dateMinEqualPicker", _options.dateMinEqualPicker, this.messageProvider.getMessage("dateMinEqualPicker", params));
        }
    }


    // VALIDATION GROUP BINDING

    // bind validation of one component to change of another
    this.validationBindOne = function(_triggerComponent, _influencedComponent) {
        _triggerComponent.onChange.subscribe(this,
            function () {
                CorabeuControl.validElement(_influencedComponent.element);
            });
    }


    // executes validation of form
    this.valid = function () {
        var _element = this.element;
        $(function() {
            return _element.valid();
        });
    }

}


/* * * * * * * * * * * * * * * * * FORM CLASSES (end)  * * * * * * * * * * * * * * * * * * * * * * */

/* * * * * * * * * * * * * * * * * COMPONENT CLASSES  * * * * * * * * * * * * * * * * * * * * * * * * * */

// prototype for corabeu component
CorabeuControl.Component = function () {

    this.form = null;
    this.name = "";

    // gets id selector
    this.getIdSelector = function (_controlName) {
        var result = "#" + this.getId(_controlName);
        return result;
    }

    // gets id 
    this.getId = function (_controlName) {
        var result = CorabeuControl.getIdPrefixForId(this.name) + "-" + _controlName;
        return result;
    }

    // gets name
    this.getName = function (_controlName) {
        var result = this.name + "." + _controlName;
        return result;
    }  

}

/* * * * * * * * * * * * * * * * * COMPONENT CLASSES (end)  * * * * * * * * * * * * * * * * * * * * * * */

/* * * * * * * * * * * * * * * * * VALIDATION CLASSES  * * * * * * * * * * * * * * * * * * * * * * * * * */

// validation helper - provides fundamental validation functionality
CorabeuControl.ValidationHelper = {

    // register notSelected method
    registerNotSelected: function () {
        jQuery.validator.addMethod("notSelected",
            function (value, element, param) {
                return !(value == null || value == "" || value == "0");
            },
            "Please select value");
    },

    // register notEqual method
    registerNotEqual: function () {
        jQuery.validator.addMethod("notEqual",
            function (value, element, param) {
                return !(value == param);
            },
            "Not equal validation error");
    },

    // registers datetime picker validation method
    registerDateTimeValidator: function (method, validator) {
        jQuery.validator.addMethod(method,
            function (value, element, param) {
                var dateValue = param.picker.getValueAsDate();
                return dateValue == null || validator(dateValue, param.input);
            },
            method + " validation failed");
    },


    // adds rule
    addRule: function (_element, _method, _parameter, _message) {
        var ruleMessage = {};
        ruleMessage[_method] = _message;
        var rule = {}
        rule[_method] = _parameter;
        rule["messages"] = ruleMessage;
        _element.rules("add", rule);
    },

    // adds rule without message
    addRuleWithoutMessage: function (_element, _method, _parameter) {
        var rule = {}
        rule[_method] = _parameter;
        _element.rules("add", rule);
    },

    // adds datetime rule
    addDateTimeRule: function (_picker, _method, _input, _message) {
        CorabeuControl.ValidationHelper.addRule(_picker.element, _method, { picker: _picker, input: _input }, _message);
    }

}


// valids element
CorabeuControl.validElement = function (_element) {
    if (CorabeuControl.isDefined(_element.valid))
        _element.valid();
}


// lazy validation message provider
CorabeuControl.ValidationMessageProvider = function(_messages) {

    this.message = CorabeuControl.valueOrDefault(_messages, CorabeuControl.ValidationMessages);
    this.templates = {};

    this.getMessage = function (_type, _params) {
        var messageTemplate = this.templates[_type];
        if (!CorabeuControl.isDefined(messageTemplate)) {
            var templateText = CorabeuControl.valueOrDefault(this.message[_type], "#Missing validation message");
            messageTemplate = Handlebars.compile(templateText);
            this.templates[_type] = messageTemplate;
        }                
        var message = messageTemplate(_params);
        return message;
    }

}



// localisable data
CorabeuControl.ValidationMessages = {

    required: "Please enter {{name}}",
    maxlength: "Max length of {{name}} is {{maxlength}} characters",
    minlength: "Min length of {{name}} is {{minlength}} characters",
    notSelected: "Please select {{name}}",    
    email: "{{name}} is not valid email address",
    number: "{{name}} is not a valid number",
    min: "{{name}} must be greater or equal {{min}}",
    max: "{{name}} must be lower or equal {{max}}",
    digits: "{{name}} can contain only digits",

    // date messages
    dateMin: "{{name}} must be greater or equal {{dateMinText}}",
    dateMinPicker: "{{name}} must be greater or equal than {{dateMinPickerText}}",
    dateMinEqualPicker: "{{name}} must be greater than {{dateMinEqualPickerText}}"
}

/* * * * * * * * * * * * * * * * * VALIDATION CLASSES (end)  * * * * * * * * * * * * * * * * * * * * * * */

/* * * * * * * * * * * * * * * * * BASE FUNCTIONS  * * * * * * * * * * * * * * * * * * * * * * * * */

// gets id prefix for encapsulated form
CorabeuControl.getIdPrefixForId = function(_name) {
    var result = _name;
    var first = result.substring(0, 1);
    first = first.toLowerCase();
    var last = result.substring(1);
    result = first + last;
    return result;
}



