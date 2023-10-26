// namespace AutomationSystem
var AutomationSystem = window.AutomationSystem || {}


// button wrapper with its state
AutomationSystem.ParamButton = function (_element) {

    // constants
    this.defaultClass = "btn-outline-primary";
    this.filledClass = "btn-primary";
    this.notFilledClass = "btn-warning";

    // fileds
    this.element = _element;
    this.insertParam = this.element.attr("data-insert");
    this.currentClass = this.element.attr("data-class");
    this.isRequired = this.element.attr("data-required") == "true";
    this.isFilled = false;
    this.shouldBeFilled = false;

    // update state
    this.redraw = function () {
        if (!this.isRequired) return;
        if (this.isFilled == this.shouldBeFilled) return;
        var newClass = this.shouldBeFilled ? this.filledClass : this.notFilledClass;
        this.element.removeClass(this.currentClass);
        this.element.addClass(newClass);
        this.element.attr("data-class", newClass);
        this.currentClass = newClass;
        this.isFilled = this.shouldBeFilled;
    }

    // sets new state
    this.setState = function (_isFilled) {
        this.shouldBeFilled = _isFilled;
    }

}

// param button pannel component
AutomationSystem.ParamButtonPannel = function (_buttonClass) {

    // fields
    this.elements = $("." + _buttonClass);
    this.insertParamHandler = function () { }
    this.buttonMap = {};
    this.buttonArray = [];

    var _this = this;

    // initialisation
    this.elements.each(function () {
        var button = new AutomationSystem.ParamButton($(this));
        _this.buttonMap[button.insertParam] = button;
        _this.buttonArray.push(button);
    });

    // event binding    
    this.elements.click(function () {
        var button = $(this);
        var insertParam = button.attr("data-insert");
        _this.insertParamHandler(insertParam);
    });


    // FUNCTIONS
    // registers param handler
    this.registerInsertParamHandler = function (_insertParamHandler) {
        this.insertParamHandler = _insertParamHandler;
    }

    // updates buttons state
    this.updateButtonsState = function (_insertedParams) {
        $.each(this.buttonArray, function (_index, _value) { _value.setState(false); });
        for (var i = 0; i < _insertedParams.length; i++) {
            var insertedParam = _insertedParams[i];
            var button = this.buttonMap[insertedParam];
            if (CorabeuControl.isDefined(button))
                button.setState(true);
        }
        $.each(this.buttonArray, function (_index, _value) { _value.redraw(); });
    }

    // gets notAllowed params
    this.getNotAllowed = function (_insertedParams) {
        var notAllowed = [];
        for (var i = 0; i < _insertedParams.length; i++) {
            var insertedParam = _insertedParams[i];
            var button = this.buttonMap[insertedParam];
            if (!CorabeuControl.isDefined(button))
                notAllowed.push(insertedParam);
        }
        return notAllowed;
    }

    // gets missing required params
    this.getMissingRequired = function (_insertedParams) {
        var paramsMap = {};
        var result = [];
        $.each(_insertedParams, function (_index, _value) { paramsMap[_value] = true; });
        $.each(this.buttonArray, function (_index, _value) {
            if (!_value.isRequired) return;
            var paramName = _value.insertParam;
            if (CorabeuControl.isDefined(paramsMap[paramName])) return;
            result.push(paramName);
        });
        return result;
    }


    // enables button pannel
    this.enable = function () {
        $.each(this.buttonArray, function (_index, _value) { _value.element.prop("disabled", false); });
    }

    // disables button pannel
    this.disable = function () {
        $.each(this.buttonArray, function (_index, _value) { _value.element.prop("disabled", true); });
    }

}


// Mediates communication between ParamButtonPannel and AceTextInput
AutomationSystem.AcePannelMediator = function (_subject, _ace, _pannel, _paramRegex) {

    // fields
    this.subject = _subject;
    this.ace = _ace;
    this.pannel = _pannel;
    this.paramRegex = _paramRegex;

    // connects handlers
    var _this = this;
    this.pannel.registerInsertParamHandler(function (_insertParam) {
        _this.ace.insertText(_insertParam);
        _this.ace.focus();
    });
    this.ace.onChange.subscribe(this, function () { _this.processBodyText(); });

    // extracts prarameters from text
    this.extractParameters = function (_text) {
        var regex = new RegExp(this.paramRegex, "g");
        var matchArray;
        var result = [];
        while ((matchArray = regex.exec(_text)) != null) {
            result.push(matchArray[0]);
        }
        return result;
    }

    // initializes mediator
    this.initialize = function () {

        this.processBodyText();
    }

    // processes ace text input content
    this.processBodyText = function () {
        var text = _this.ace.getValue();
        var parameters = this.extractParameters(text);
        this.pannel.updateButtonsState(parameters);
    }

    // validates input value for not allowed params
    this.getNotAllowedParameters = function (_value) {
        var parameters = this.extractParameters(_value);
        var notAllowed = this.pannel.getNotAllowed(parameters);
        return notAllowed;
    }

    // validates input value for missing required params
    this.getMissingRequired = function (_value) {
        var parameters = this.extractParameters(_value);
        var missingRequired = this.pannel.getMissingRequired(parameters);
        return missingRequired;
    }


    // enables email mediator
    this.enable = function () {
        this.subject.enable();
        this.ace.enable();
        this.pannel.enable();
    }

    // disables email mediator
    this.disable = function () {
        this.subject.disable();
        this.ace.disable();
        this.pannel.disable();
    }

}