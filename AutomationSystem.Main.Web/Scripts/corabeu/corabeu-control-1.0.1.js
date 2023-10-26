/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
CORABEU CONTROLS
Author: Jan Konečný - 2017

Corabeu control encapsulates fundamental operation via HTML forms and other web elements to unify and simplify web development.

Dependencies:
* JQuery
* Boodstrap-DatetimePicker

Content (namespace CorabeuControl):



* FORM CLASSES:

* GUI CLASSES:
 
* HELPER CLASSES:
  
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

// namespace CorabeuControl
var CorabeuControl = window.CorabeuControl || {}


/* * * * * * * * * * * * * * * * * FORM CLASSES  * * * * * * * * * * * * * * * * * * * * * * * * * */

// base component group, implements abstract group algorithms
CorabeuControl.BaseGroup = function () {

    this.id = null;
    this.element = null;
    this.itemArray = [];
    this.itemMap = {};

    // iterates all items and executes some operation
    this.iterate = function (_operation) {
        for (var i = 0; i < this.itemArray.length; i++)
            _operation(this.itemArray[i]);
    }

    // agregates some values
    this.aggregate = function (_seed, _aggregationHandler) {
        var result = _seed;
        this.iterate(function (_item) {
            result = _aggregationHandler(result, _item);
        });
        return result;
    }

    // gets item specified by id
    this.getById = function (_id) {
        return this.itemMap[_id];
    }

    // gets item by specified index
    this.getByIndex = function (index) {
        return this.itemArray[index];
    }

    // initializes group
    this.initializeById = function (_groupElementId, _items) {
        this.id = _groupElementId;
        this.element = $("#" + _groupElementId);
        this.itemArray = _items;
        var this_map = this.itemMap;
        this.iterate(function (_item) {
            this_map[_item.id] = _item;
        });
    }

}

// base class for input controls
CorabeuControl.BaseInput = function () {

    this.id = null;
    this.name = null;
    this.default_forceOnChange = false;             // determines, whether onChange should be raised, when setValue is called
    this.defaultValue = "";
    this.element = null;
    this.onChange = null;


    this.initializeById = function (_elementId, _name, _defaultValue) {
        this.id = _elementId;
        this.element = $("#" + _elementId);
        this.name = CorabeuControl.valueOrDefault(_name, this.name);
        this.defaultValue = CorabeuControl.valueOrDefault(_defaultValue, this.defaultValue);

        var this_object = this;
        this.onChange = new CorabeuControl.Event(this.element, "override_change");
        this.element.change(function () {
            this_object.onChange.trigger(this_object, {});
        });
    }


    this.getValue = function () {
        return this.element.val();
    }

    this.setValue = function (_value, _forceOnChange) {
        _forceOnChange = CorabeuControl.valueOrDefault(_forceOnChange, this.default_forceOnChange);
        this.element.val(_value);
        if (_forceOnChange)
            this.onChange.trigger(this, {});
    }

    this.setDefaultValue = function (_forceOnChange) {
        this.setValue(this.defaultValue, _forceOnChange);
    }

    this.getQueryString = function () {
        var result = "";
        var value = this.getValue();
        if (this.name == null || this.name == "")
            return result;
        if (typeof value === "undefined" || value == null || value == "")
            return result;
        return this.name + "=" + encodeURIComponent(value);
    }


    this.enable = function () {
        this.element.prop("disabled", false);
    }

    this.disable = function () {
        this.element.prop("disabled", true);
    }

}

// Input group
CorabeuControl.InputGroup = function (_gropuElementId, _inputs) {

    this.initializeById(_gropuElementId, _inputs);
    this.onChange = new CorabeuControl.Event(this.element, "override_change");

    // connects onClick handlers
    var this_object = this;
    var onChangeHandler = function (_sender, _state) {
        this_object.onChange.trigger(_sender, _state);
    }
    this.iterate(function (_item) {
        _item.onChange.subscribe(_item, onChangeHandler);
    });

    // agreages base methods
    this.getQueryString = function () {
        return this.aggregate("",
            function (_current, _item) {
                var value = _item.getQueryString();
                if (_current == "")
                    return value;
                return value == "" ? _current : _current + "&" + value;
            });
    };


    this.setDefaultValue = function (_forceOnChange) {
        this.iterate(function (_item) {
            _item.setDefaultValue(_forceOnChange);
        });
    }

    this.enable = function () {
        this.iterate(function (_item) {
            _item.enable();
        });
    }

    this.disable = function () {
        this.iterate(function (_item) {
            _item.disable();
        });
    }


}
CorabeuControl.InputGroup.prototype = new CorabeuControl.BaseGroup();



// TextBox control
CorabeuControl.TextBox = function (_inputId, _name) {

    this.initializeById(_inputId, _name, "");

}
CorabeuControl.TextBox.prototype = new CorabeuControl.BaseInput();

// Hidden input control
CorabeuControl.Hidden = function (_inputId, _name) {

    this.default_forceOnChange = true;
    this.initializeById(_inputId, _name, "");

}
CorabeuControl.Hidden.prototype = new CorabeuControl.BaseInput();


// CheckBox control
CorabeuControl.CheckBox = function (_inputId, _name) {

    this.id = _inputId;
    this.name = _name;
    this.default_forceOnChange = false;             // determines, whether onChange should be raised, when setValue is called
    this.defaultValue = false;
    this.element = $("#" + _inputId);
    this.onChange = new CorabeuControl.Event(this.element, "override_change");

    // registers event
    var this_object = this;
    this.element.change(function () {
        this_object.onChange.trigger(this_object, {});
    });


    // gets value
    this.getValue = function () {
        return this.element.prop("checked");
    }

    // sets value
    this.setValue = function (_value, _forceOnChange) {
        _forceOnChange = CorabeuControl.valueOrDefault(_forceOnChange, this.default_forceOnChange);

        if (_value)
            this.element.prop("checked", true);
        else
            this.element.prop("checked", false);


        if (_forceOnChange)
            this.onChange.trigger(this, {});
    }

    this.setDefaultValue = function (_forceOnChange) {
        this.setValue(this.defaultValue, _forceOnChange);
    }

    this.getQueryString = function () {
        var result = "";
        var value = this.getValue();
        if (this.name == null || this.name == "")
            return result;
        if (typeof value === "undefined" || value == null || value === "")
            return result;
        return this.name + "=" + encodeURIComponent(value);
    }

    this.enable = function () {
        this.element.prop("disabled", false);
    }

    this.disable = function () {
        this.element.prop("disabled", true);
    }

}
CorabeuControl.CheckBox.prototype = new CorabeuControl.BaseInput();


// Spinner input control
CorabeuControl.Spinner = function (_inputId, _name, _modelOptions) {

    this.id = _inputId;
    this.name = _name;
    this.element = $("#" + this.id);
    this.onChange = new CorabeuControl.Event(this.element, "override_change");
    this.plusButton = $(".command-" + this.id + "[data-command='plus']");
    this.minusButton = $(".command-" + this.id + "[data-command='minus']");


    // initializes model and sync functions
    _modelOptions = _modelOptions || {}
    var operators = new CorabeuControl.NumericIntervalOperators(_modelOptions.defaultValue, _modelOptions.notNull);
    this.model = new CorabeuControl.BaseInterval(operators);
    this.updateButtonState = function () {
        var state = this.model.moveState();
        this.plusButton.prop("disabled", !state.canIncrement);
        this.minusButton.prop("disabled", !state.canDecrement);
    }
    this.syncViewValue = function () {
        this.setValue(this.model.value);
    }
    this.syncModelValue = function () {
        if (this.model.setValue(this.getValue()))
            this.syncViewValue();
    }

    // updates range and sets value into model, updates buttons
    this.model.setRanges(this.element.prop("min"), this.element.prop("max"));
    this.syncModelValue();
    this.updateButtonState();


    // sets handlers      
    var _this = this;

    this.plusButton.click(function () {
        _this.model.increment();
        _this.syncViewValue();
        _this.updateButtonState();
        CorabeuControl.validElement(_this.element);
        _this.onChange.trigger(_this, {});
    });
    this.minusButton.click(function () {
        _this.model.decrement();
        _this.syncViewValue();
        _this.updateButtonState();
        CorabeuControl.validElement(_this.element);
        _this.onChange.trigger(_this, {});
    });
    this.element.change(function () {
        _this.syncModelValue();
        _this.updateButtonState();
        CorabeuControl.validElement(_this.element);
        _this.onChange.trigger(_this, {});
    });


    // adjust Input functions
    this.enable = function () {
        this.element.prop("disabled", false);
        this.updateButtonState();
    }

    this.disable = function () {
        this.element.prop("disabled", true);
        this.plusButton.prop("disabled", true);
        this.minusButton.prop("disabled", true);
    }

}
CorabeuControl.Spinner.prototype = new CorabeuControl.BaseInput();

// DateTime Picker input control
CorabeuControl.DateTimePicker = function (_inputId, _name, _pickerOptions) {

    // sets base properties
    this.id = _inputId;
    this.name = _name;
    this.element = $("#" + this.id);
    this.onChange = new CorabeuControl.Event(this.element, "override_change");
    this.dateTimePicker = null;

    // initializes picker
    _pickerOptions = _pickerOptions || {};
    var _this = this;
    //var options = {}

    $(function () {
        var picker = $("#picker-" + _this.id);
        picker.datetimepicker();
        picker.on("dp.change",
            function () {
                CorabeuControl.validElement(_this.element);
                _this.onChange.trigger(_this, {});
            });
        _this.dateTimePicker = picker.data("DateTimePicker");
        if (CorabeuControl.isDefined(_pickerOptions.locale))
            _this.dateTimePicker.locale(_pickerOptions.locale);
        if (CorabeuControl.isDefined(_pickerOptions.format))
            _this.dateTimePicker.format(_pickerOptions.format);
    });


    this.getValueAsDate = function () {
        if (this.dateTimePicker == null) {
            alert("DateTimePicker " + this.name + " is not initialized");
            return null;
        }

        var date = this.dateTimePicker.date();
        if (date !== null && CorabeuControl.isDefined(_pickerOptions.startOf)) {
            date = date.startOf(_pickerOptions.startOf);
        }

        return date;
    }

    this.setValue = function (_value, _forceOnChange) {
        _forceOnChange = CorabeuControl.valueOrDefault(_forceOnChange, this.default_forceOnChange);
        if (!_forceOnChange)
            this.onChange.bypass = true;
        try {
            if (_value == "")
                _value = null;
            this.dateTimePicker.date(_value);
        } catch (e) {
        } finally {
            this.onChange.bypass = false;
        }
    }

}
CorabeuControl.DateTimePicker.prototype = new CorabeuControl.BaseInput();

// Drop down list input control
CorabeuControl.DropDownList = function (_inputId, _name) {

    // sets base properties
    this.id = _inputId;
    this.name = _name;
    this.element = $("#" + this.id);
    this.onChange = new CorabeuControl.Event(this.element, "override_change");
    this.onClick = new CorabeuControl.Event(this.element, "override_click");

    this.selectedStyle = this.element.attr("data-selectedstyle");
    this.button = $("#button-" + this.id);
    this.buttonLabel = $("#caption-" + this.id);
    this.actions = $(".action-" + this.id);

    // default action handlers
    this.itemClick = function (_sender, _id, _text) {
        this.setValue(_id, true);
        if (CorabeuControl.isDefined(this.element.valid))
            this.element.valid();
        this.actions.removeClass(this.selectedStyle);
        _sender.addClass(this.selectedStyle);
        this.buttonLabel.text(_text);
    }


    this.commandClick = function (_sender, _command) {
        this.onClick.trigger(this, _command);
    }

    // process generic action click
    var _this = this;
    this.actions.click(function () {
        var sender = $(this);
        var type = sender.attr("data-type");
        switch (type) {
            case "item":
                var id = sender.attr("data-id");
                var text = sender.attr("data-buttontext");
                _this.itemClick(sender, id, text);
                break;
            case "clearitem":
                text = sender.attr("data-buttontext");
                _this.setDefaultValue(true, text);
                break;
            case "command":
                var command = sender.attr("data-command");
                _this.commandClick(sender, command);
                break;
            default:
                alert("NOT IMPLEMENTED: DropDownList item type " + type);
                break;
        }
    });


    // sets default value
    this.setDefaultValue = function (_forceOnChange, _text) {
        this.setValue(this.defaultValue, _forceOnChange);
        if (CorabeuControl.isDefined(this.element.valid))
            this.element.valid();
        this.actions.removeClass(this.selectedStyle);
        if (!CorabeuControl.isDefined(_text)) {
            var clearItemSelect = "#items-" + this.id + " a[data-type='clearitem']";
            var element = $(clearItemSelect);
            _text = element.attr("data-buttontext");
        }
        this.buttonLabel.text(_text);
    }


    this.enable = function () {
        this.button.prop("disabled", false);
    }

    this.disable = function () {
        this.button.prop("disabled", true);
    }

}
CorabeuControl.DropDownList.prototype = new CorabeuControl.BaseInput();


// Text input with Typeahead functionality
CorabeuControl.TypeaheadTextInput = function (_inputId, _name, _typeaheadSource, _typeaheadOptions) {

    this.initializeById(_inputId, _name, "");
    this.typeaheadSelector = "#typeahead-" + _inputId + " .typeahead";

    _typeaheadOptions = CorabeuControl.valueOrDefault(_typeaheadOptions,
        {
            hint: true,
            highlight: true,
            minLength: 1
        });
    $(this.typeaheadSelector).typeahead(_typeaheadOptions, _typeaheadSource.sourceDefinition);

    // bind event
    var _this = this;
    $(this.typeaheadSelector).bind("typeahead:select", function (ev, suggestion) {
        _this.onChange.trigger(_this, {});
    });

    this.enable = function () {
        this.element.prop("disabled", false);
        this.element.css("background-color", "transparent");
    }

    this.disable = function () {
        this.element.prop("disabled", true);
        this.element.css("background-color", "#eeeeee");
    }

}
CorabeuControl.TypeaheadTextInput.prototype = new CorabeuControl.BaseInput();


// Text input with Typeahead functionality
CorabeuControl.TypeaheadDropDownInput = function (_inputId, _name, _typeaheadSource, _typeaheadOptions) {

    // initializes base properties
    this.id = _inputId;
    this.defaultValue = null;
    this.name = _name;
    this.element = $("#" + this.id);
    this.typeaheadElement = $("#typeahead-" + _inputId + " .typeahead");                // text input element with typeahead
    this.onChange = new CorabeuControl.Event(this.element, "override_change");

    // initializes model, selectors, setters and getters    
    this.objectValue = { text: this.typeaheadElement.val(), value: this.element.val() };
    this.textValueSelector = _typeaheadSource.keyValueSelector;

    this.getObjectValue = function () { return this.objectValue; }
    this.setObjectValue = function (_object, _forceOnChange) {
        this.objectValue = _object == null ? { text: "", value: "" } : _object;
        this.typeaheadElement.val(this.objectValue.text);
        this.setValue(this.objectValue.value, _forceOnChange);
        CorabeuControl.validElement(this.element);
    }

    // initializes typeahead
    _typeaheadOptions = CorabeuControl.valueOrDefault(_typeaheadOptions,
        {
            hint: true,
            highlight: true,
            minLength: 1
        });
    this.typeaheadElement.typeahead(_typeaheadOptions, _typeaheadSource.sourceDefinition);

    // bind events
    var _this = this;
    this.typeaheadElement.change(function () {
        if (_this.typeaheadElement.val() == "" && _this.objectValue != null)
            _this.setObjectValue(null, true);
        else
            _this.setObjectValue(_this.objectValue, false);                 // only resets current value (object must be selected)
    });
    this.typeaheadElement.bind("typeahead:select", function (ev, _suggestion) {
        _this.setObjectValue(_this.textValueSelector(_suggestion), true);
    });
    this.typeaheadElement.bind("typeahead:close", function () {
        _this.setObjectValue(_this.objectValue, false);
    });


    // adjustet BaseInput methods
    this.setDefaultValue = function (_forceOnChange) {
        this.setObjectValue(this.defaultValue, _forceOnChange);
    }

    this.enable = function () {
        this.typeaheadElement.prop("disabled", false);
        this.typeaheadElement.css("background-color", "transparent");
    }

    this.disable = function () {
        this.typeaheadElement.prop("disabled", true);
        this.typeaheadElement.css("background-color", "#eeeeee");
    }

}
CorabeuControl.TypeaheadDropDownInput.prototype = new CorabeuControl.BaseInput();


// base input set
CorabeuControl.BaseSetInput = function () {

    // base properties
    this.animationSpeed = 250;
    this.id = null;
    this.name = null;
    this.default_forceOnChange = false;             // determines, whether onChange should be raised, when setValue is called
    this.defaultValue = "[]";
    this.element = null;
    this.onChange = null;
    this.set = null;

    // set properties
    this.commandSelector = null;
    this.itemContainer = null;
    this.onClickHandler = null;


    // initializes
    this.initializeById = function (_elementId, _name) {
        this.id = _elementId;
        this.element = $("#" + _elementId);
        this.name = CorabeuControl.valueOrDefault(_name, this.name);
        this.onChange = new CorabeuControl.Event(this.element, "override_change");
        this.set = new CorabeuControl.NumericSet();
        this.set.setStringArray(this.getValue());

        // initializes set properties
        var _this = this;
        this.commandSelector = ".command-" + _elementId;
        this.itemContainer = $("#container-" + _elementId);
        this.onClickHandler = function () {
            var sender = $(this);
            var command = sender.attr("data-command");
            var id = sender.attr("data-id");
            _this.commandHandler(sender, command, id);
        }
        $(this.commandSelector).click(this.onClickHandler);
    }

    // set properties
    this.template = null;

    // handles commands
    this.commandHandler = function (_sender, _command, _id) {
        switch (_command) {
            case "remove":
                var itemToRemove = $("#item-" + this.id + "-" + _id);
                itemToRemove.fadeOut(this.animationSpeed, function () {
                    itemToRemove.remove();
                });
                this.set.remove(_id);
                this.setValue(this.set.getStringArray(), true);
                break;
        }
    }


    this.compileTemplate = function (_template) {
        this.template = Handlebars.compile(_template);
    }

    this.addItem = function (_value, _data) {
        if (this.set.contains(_value))
            return;
        _data.mainId = this.id;
        var html = this.template(_data);
        var _this = this;
        $(html).hide().appendTo(this.itemContainer).fadeIn(this.animationSpeed, function () {
            $("#item-" + _this.id + "-" + _value + " .command-" + _this.id).click(_this.onClickHandler);
        });
        this.set.add(_value);
        this.setValue(this.set.getStringArray(), true);
    }

    // overrided baseInput methods
    this.getQueryString = function () {
        var result = "";
        var value = this.getValue();
        if (this.name == null || this.name == "")
            return result;
        if (typeof value === "undefined" || value == null || value == "[]" || value == "")
            return result;
        return this.name + "=" + encodeURIComponent(value);
    }

    this.setDefaultValue = function (_forceOnChange) {
        var itemToRemove = $(".item-" + this.id);
        itemToRemove.fadeOut(this.animationSpeed, function () {
            itemToRemove.remove();
        });
        this.set.clear();
        this.setValue(this.set.getStringArray(), _forceOnChange);
    }

    this.enableItems = function () {
        $(this.commandSelector).prop("disabled", false);
    }

    this.disableItems = function () {
        $(this.commandSelector).prop("disabled", true);
    }

}
CorabeuControl.BaseSetInput.prototype = new CorabeuControl.BaseInput();


// Typehead set input
CorabeuControl.TypeaheadSetInput = function (_inputId, _name, _setOptions) {

    // initializes underlaying set input
    this.initializeById(_inputId, _name);

    var _typeaheadOptions = _setOptions.typeaheadOptions;
    var _typeaheadSource = _setOptions.typeaheadSource;

    // compiles template
    var template = CorabeuControl.valueOrDefault(_setOptions.itemTemplate, $("#" + _setOptions.itemTemplateId).html());
    this.compileTemplate(template);

    this.typeaheadElement = $("#typeahead-" + _inputId + " .typeahead");                // text input element with typeahead
    this.textValueSelector = _typeaheadSource.keyValueSelector;

    // initializes typeahead
    _typeaheadOptions = CorabeuControl.valueOrDefault(_typeaheadOptions,
        {
            hint: true,
            highlight: true,
            minLength: 1
        });
    this.typeaheadElement.typeahead(_typeaheadOptions, _typeaheadSource.sourceDefinition);

    // bind events
    var _this = this;
    this.typeaheadElement.bind("typeahead:select", function (ev, _suggestion) {
        var data = _this.textValueSelector(_suggestion);
        _this.addItem(data.value, data);
    });

    this.enable = function () {
        this.typeaheadElement.prop("disabled", false);
        this.typeaheadElement.css("background-color", "transparent");
        this.enableItems();
    }

    this.disable = function () {
        this.typeaheadElement.prop("disabled", true);
        this.typeaheadElement.css("background-color", "#eeeeee");
        this.disableItems();
    }

}
CorabeuControl.TypeaheadSetInput.prototype = new CorabeuControl.BaseSetInput();


// DropDownList set input
CorabeuControl.DropDownListSetInput = function (_inputId, _name, _setOptions) {

    // initializes underlaying set input
    this.initializeById(_inputId, _name);

    // initializes action links and ddl button
    this.button = $("#button-" + this.id);
    this.actions = $(".action-" + this.id);

    // compiles template
    var template = CorabeuControl.valueOrDefault(_setOptions.itemTemplate, $("#" + _setOptions.itemTemplateId).html());
    this.compileTemplate(template);

    // binds adding of items
    var _this = this;
    this.actions.click(function () {
        var sender = $(this);
        var id = sender.attr("data-id");
        var text = sender.attr("data-buttontext");
        var data = {
            value: id,
            text: text
        };
        _this.addItem(id, data);
    });

    this.enable = function () {
        this.button.prop("disabled", false);
        this.enableItems();
    }

    this.disable = function () {
        this.button.prop("disabled", true);
        this.disableItems();
    }

}
CorabeuControl.DropDownListSetInput.prototype = new CorabeuControl.BaseSetInput();


// Ace editor component
CorabeuControl.AceTextInput = function (_inputId, _name, _options) {

    this.id = null;
    this.name = null;
    this.default_forceOnChange = false;             // determines, whether onChange should be raised, when setValue is called
    this.defaultValue = "";
    this.element = null;
    this.editor = null;
    this.onChange = null;

    // sets values    
    this.id = _inputId;
    this.name = _name;
    this.element = $("#" + _inputId);
    this.onChange = new CorabeuControl.Event(this.element, "override_change");

    // intitializes editor
    var object = this;
    _options = CorabeuControl.valueOrDefault(_options, {});
    if (!CorabeuControl.isDefined(_options.theme))
        _options.theme = "ace/theme/textmate";
    this.editor = ace.edit("editor-" + this.id);
    this.editor.setTheme(_options.theme);

    // setting mode
    if (CorabeuControl.isDefined(_options.mode))
        this.editor.getSession().setMode(_options.mode);

    // event handler
    this.editor.getSession().on('change', function (e) {
        object.element.val(object.getValue());
        CorabeuControl.validElement(object.element);
        object.onChange.trigger(object, {});
    });

    // get value of component
    this.getValue = function () {
        return this.editor.getValue();
    }

    // sets value of component
    this.setValue = function (_value, _forceOnChange) {
        _forceOnChange = CorabeuControl.valueOrDefault(_forceOnChange, this.default_forceOnChange);
        if (!_forceOnChange)
            this.onChange.bypass = true;
        try {
            this.editor.setValue(_value);
        } catch (e) {
        } finally {
            this.onChange.bypass = false;
        }
    }

    // setDefaultValue and getQueryString are inherited from BaseInput

    // enable editing
    this.enable = function () {
        this.editor.setReadOnly(false);
    }

    // enable editing
    this.disable = function () {
        this.editor.setReadOnly(true);
    }

    // inserts text into cursor
    this.insertText = function (_text) {
        this.editor.insert(_text);
    }

    // set focus to ace editor
    this.focus = function () {
        this.editor.focus();
    }

    // sets keywords
    this.setKeywordsInHtml = function (_keyword) {
        // todo: it is difficult to insert this into html syntax}}
        //var session = this.editor.getSession();
        //var rule = {
        //    start: [{
        //        token: "parameter_text",
        //        regex: _keyword                
        //    }]
        //};
        //session.$mode.$highlightRules.addRules(rule, "new-");
        //session.bgTokenizer.start(0);
    }

}
CorabeuControl.AceTextInput.prototype = new CorabeuControl.BaseInput();


// base button implementation
CorabeuControl.BaseButton = function () {

    this.id = null;
    this.element = null;
    this.onClick = null;
    this.state = null;

    this.initializeById = function (_elementId, _state) {
        this.id = _elementId;
        this.element = $("#" + _elementId);
        this.state = CorabeuControl.valueOrDefault(_state, this.state);
        this.onClick = new CorabeuControl.Event(this.element, "override_click");
        var this_object = this;
        this.element.click(function () {
            this_object.onClick.trigger(this_object, this_object.state);
        });
    }

    this.show = function (animationSpeed) {
        this.element.fadeIn(CorabeuControl.valueOrDefault(animationSpeed, 250));
    }

    this.hide = function (animationSpeed) {
        this.element.fadeOut(CorabeuControl.valueOrDefault(animationSpeed, 250));
    }

    this.enable = function () {
        this.element.prop("disabled", false);
    }

    this.disable = function () {
        this.element.prop("disabled", true);
    }

}

// Button group
CorabeuControl.ButtonGroup = function (_groupElementId, _buttons) {

    this.initializeById(_groupElementId, _buttons);
    this.onClick = new CorabeuControl.Event(this.element, "override_click");

    // connects onClick handlers
    var this_object = this;
    var onClickHandler = function (_sender, _state) {
        this_object.onClick.trigger(_sender, _state);
    }
    this.iterate(function (_item) {
        _item.onClick.subscribe(_item, onClickHandler);
    });

    // agregates BaseButton methods
    this.show = function (animationSpeed) {
        this.iterate(function (_item) {
            _item.show(animationSpeed);
        });
    }

    this.hide = function (animationSpeed) {
        this.iterate(function (_item) {
            _item.hide(animationSpeed);
        });
    }

    this.enable = function () {
        this.iterate(function (_item) {
            _item.enable();
        });
    }

    this.disable = function () {
        this.iterate(function (_item) {
            _item.disable();
        });

    }

}
CorabeuControl.ButtonGroup.prototype = new CorabeuControl.BaseGroup();

// standard button with state
CorabeuControl.Button = function (buttonId, state) {

    this.initializeById(buttonId, state);

}
CorabeuControl.Button.prototype = new CorabeuControl.BaseButton();


/* * * * * * * * * * * * * * * * * FORM CLASSES (end)  * * * * * * * * * * * * * * * * * * * * * * */


/* * * * * * * * * * * * * * * * * GUI CLASSES * * * * * * * * * * * * * * * * * * * * * * * * * * */

// manages expansion of page parts
CorabeuControl.ExpansionGroup = function (_id) {

    this.id = _id;
    this.animationSpeed = 250;
    this.commandSelector = ".command-" + this.id;
    this.foldedSelector = ".folded-" + this.id;
    this.unfoldedSelector = ".unfolded-" + this.id;

    // hides unfolded items
    $(this.unfoldedSelector).hide();

    // registers commands
    var _this = this;
    $(this.commandSelector).click(function () {
        var sender = $(this);
        var command = sender.attr("data-command");
        var id = sender.attr("data-id");
        var fs = CorabeuControl.isDefined(id) ? _this.foldedSelector + "-" + id : _this.foldedSelector;
        var us = CorabeuControl.isDefined(id) ? _this.unfoldedSelector + "-" + id : _this.unfoldedSelector;
        switch (command) {
            case "show":
                $(fs).fadeOut(_this.animationSpeed, function () {
                    $(us).fadeIn(_this.animationSpeed);
                });

                break;
            case "hide":
                $(us).fadeOut(_this.animationSpeed, function () {
                    $(fs).fadeIn(_this.animationSpeed);
                });
                break;
        }
    });

}


/* * * * * * * * * * * * * * * * * GUI CLASSES (end) * * * * * * * * * * * * * * * * * * * * * * * */


/* * * * * * * * * * * * * * * * * MODEL CLASSES * * * * * * * * * * * * * * * * * * * * * * * * * * */

// numeric operations for interval
CorabeuControl.NumericIntervalOperators = function (_defaultValue, _notNull) {

    // properties
    this.notNull = CorabeuControl.valueOrDefault(_notNull, false);
    this.defaultValue = CorabeuControl.valueOrDefault(_defaultValue, this.notNull ? 0 : null);


    // parses any value (e.g. input attribute)
    this.parseValue = function (_value) {
        if (isNaN(_value) || _value == "")
            return null;
        return Number(_value);
    }

    // parses input value, can apply adjusting operations, like setting default values
    this.parseNewValue = function (_newValue, _currentValue) {
        var result = {};
        var defaultValue = _currentValue;
        var nullValue = this.notNull ? defaultValue : null;

        // new value is empty, null or default value is used
        if (_newValue == "") {
            result.value = nullValue;
            result.isChanged = this.notNull;                // this.notNull == false -> value is null which is equal to "" in input element value attribute, so, there is no change
            return result;
        }

        // value is not numeric, returns default value
        if (isNaN(_newValue)) {
            result.value = defaultValue;
            result.isChanged = true;
            return result;
        }

        result.value = Number(_newValue);
        result.isChanged = false;
        return result;
    }

    // compares value with ranges
    this.compareWithRanges = function (_value, _min, _max) {
        var result = {
            canIncrement: true,
            canDecrement: true,
            exceedMin: false,
            exceedMax: false
        }
        if (_value == null)
            return result;

        if (_min != null) {
            result.canDecrement = _min < _value;
            result.exceedMin = _value < _min;
        }

        if (_max != null) {
            result.canIncrement = _value < _max;
            result.exceedMax = _max < _value;
        }
        return result;
    }

    // gets predecessor
    this.getPredecessor = function (_value, _min) {
        var result = _value == null ? this.defaultValue : _value - 1;
        if (_min != null && result != null && result < _min)
            result = _min;
        return result;
    }

    // gets successor
    this.getSuccessor = function (_value, _max) {
        var result = _value == null ? this.defaultValue : _value + 1;
        if (_max != null && result != null && _max < result)
            result = _max;
        return result;
    }

}

// base interval model
CorabeuControl.BaseInterval = function (_intervalOperators) {

    // properties
    this.operators = _intervalOperators;
    this.value = this.operators.defaultValue;
    this.min = null;
    this.max = null;

    // fits range boundaries, returns true, when value is changed
    this.fitRange = function () {
        var state = this.operators.compareWithRanges(this.value, this.min, this.max);
        if (state.exceedMin) {
            this.value = this.min;
            return true;
        }
        if (state.exceedMax) {
            this.value = this.max;
            return true;
        }
        return false;
    }

    // sets interval ranges and fits value to range, returns true when value is changed
    this.setRanges = function (_min, _max) {
        this.min = this.operators.parseValue(_min);
        this.max = this.operators.parseValue(_max);
        return this.fitRange();
    }

    // gets button states
    this.moveState = function () {
        var state = this.operators.compareWithRanges(this.value, this.min, this.max);
        return {
            canIncrement: state.canIncrement,
            canDecrement: state.canDecrement
        };
    }

    // sets new value, if value is not fit into interval and is changed, returns true;
    this.setValue = function (_newValue) {
        var parseResult = this.operators.parseNewValue(_newValue, this.value);
        this.value = parseResult.value;
        return parseResult.isChanged || this.fitRange();
    }

    // increments value
    this.increment = function () {
        this.value = this.operators.getSuccessor(this.value, this.max);
    }

    // decrements value
    this.decrement = function () {
        this.value = this.operators.getPredecessor(this.value, this.min);
    }

}


// numeric set
CorabeuControl.NumericSet = function () {

    this.items = new Array();

    this.contains = function (_value) {
        if (_value == "") return false;
        return $.inArray(Number(_value), this.items) >= 0;
    }

    this.clear = function () {
        this.items = new Array();
    }

    this.add = function (_value) {
        if (!this.contains(_value))
            this.items.push(Number(_value));
    }

    this.remove = function (_value) {
        var index = $.inArray(Number(_value), this.items);
        if (index < 0) return;
        this.items.splice(index, 1);
    }

    this.getStringArray = function () {
        return JSON.stringify(this.items);
    }

    this.setStringArray = function (_set) {
        this.clear();
        var temp = JSON.parse(_set);
        for (var i = 0; i < temp.length; i++) {
            this.add(temp[i]);
        }
    }

}



/* * * * * * * * * * * * * * * * * MODEL CLASSES (end) * * * * * * * * * * * * * * * * * * * * * * * */



/* * * * * * * * * * * * * * * * * HELPER CLASSES  * * * * * * * * * * * * * * * * * * * * * * * * */

// Encapsulates set and post request sending
CorabeuControl.RequestHelper = function () {

    this.actions = {};

    // adds new action
    this.addAction = function (key, action) {
        this.actions[key] = action;
    }

    // determines whether action is defined
    this.hasAction = function (key) {
        return CorabeuControl.isDefined(this.actions[key]);
    }


    // gets query string parameters
    this.getQueryStringParameters = function (url) {
        var params = {}, hash;
        var separatorIndex = url.indexOf("?");
        if (separatorIndex < 0) return params;
        var hashes = url.slice(separatorIndex + 1).split("&");
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split("=");
            params[hash[0]] = hash[1];
        }
        return params;
    }

    // combines query string params
    this.concatParameters = function (params, seed) {
        var result = CorabeuControl.valueOrDefault(seed, "");
        for (var key in params) {
            if (params.hasOwnProperty(key)) {
                if (result != "")
                    result += "&";
                result += key + "=" + params[key];
            }
        }
        return result;
    }

    // combines url and query string
    this.concatUrlAndQuery = function (url, query) {
        if (query == "")
            return url;
        return url + "?" + query;
    }

    // creates url
    this.createUrlByActionKey = function (actionKey, params) {
        var url = this.actions[actionKey];
        var queryString = CorabeuControl.isDefined(params) ? this.concatParameters(params) : "";
        return this.concatUrlAndQuery(url, queryString);
    }


    // executes get url
    this.executeGetUrl = function (url) {
        window.location.assign(url);
    }

    // execute post url
    this.executePostUrl = function (url, postParams) {

        // creates form element
        var form = document.createElement("form");
        form.setAttribute("method", "post");
        form.setAttribute("action", url);

        // adds parameters
        if (CorabeuControl.isDefined(postParams)) {
            for (var key in postParams) {
                if (postParams.hasOwnProperty(key)) {
                    var hiddenField = document.createElement("input");
                    hiddenField.setAttribute("type", "hidden");
                    hiddenField.setAttribute("name", key);
                    hiddenField.setAttribute("value", postParams[key]);
                    form.appendChild(hiddenField);
                }
            }
        }

        // append form to body
        document.body.appendChild(form);
        form.submit();
    }

    // executes get defined by action key
    this.executeGet = function (actionKey) {
        var url = this.createUrlByActionKey(actionKey);
        this.executeGetUrl(url);
    }

    // executes get defined by action key that keeps query string context
    this.executeGetWithContext = function (actionKey) {
        var urlParams = this.getQueryStringParameters(window.location.href);
        var url = this.createUrlByActionKey(actionKey, urlParams);
        this.executeGetUrl(url);
    }

    // executes post action
    this.executePost = function (actionKey, postParams) {
        var url = this.createUrlByActionKey(actionKey);
        this.executePostUrl(url, postParams);
    }

    // executes get defined by action key that keeps query string context
    this.executePostWithContext = function (actionKey, postParams) {
        var urlParams = this.getQueryStringParameters(window.location.href);
        var url = this.createUrlByActionKey(actionKey, urlParams);
        this.executePostUrl(url, postParams);
    }

    // submits form to given action
    this.submitFormToAction = function (_form, _actionKey) {
        var url = this.createUrlByActionKey(_actionKey);
        _form.attr("action", url);
        _form.submit();
    }

}


// Encapsulates JQuery events logic
CorabeuControl.Event = function (_element, _eventId) {

    this.bypass = false;
    this.eventId = _eventId;
    this.element = _element;

    this.subscribe = function (_subscriber, _subscriberHandler) {
        var handler = (function (arg, sender, state) {
            _subscriberHandler(sender, state);
        }).bind(_subscriber);
        this.element.on(this.eventId, handler);
    }

    this.trigger = function (sender, state) {
        if (this.bypass) return;
        this.element.triggerHandler(this.eventId, [sender, state]);
    }

}


// Typeahead source
CorabeuControl.TypeaheadSource = function (_name, _text, _value) {

    // defines selector
    this.keyValueSelector = function (_object) {
        return {
            text: (CorabeuControl.isDefined(_text) ? _object[_text] : _object),
            value: (CorabeuControl.isDefined(_value) ? _object[_value] : _object)
        }
    };

    // defines base of source source
    this.datumTokenizer = CorabeuControl.isDefined(_text)
        ? Bloodhound.tokenizers.obj.whitespace(_text)
        : Bloodhound.tokenizers.whitespace;

    this.sourceDefinition = {
        name: _name,
        display: _text
    }

    // adds template
    this.addTemplate = function (_template, _emptyTemplate) {
        this.sourceDefinition.templates = {
            empty: CorabeuControl.valueOrDefault(_emptyTemplate,
                CorabeuControl.TypeaheadSourceFactory.getEmptyTemplate())
        }
        if (CorabeuControl.isDefined(_template))
            this.sourceDefinition.templates.suggesition = Handlebars.compile(_template);
    }

    // adds local source
    this.setLocalSource = function (_values) {
        this.sourceDefinition.source = new Bloodhound({
            datumTokenizer: this.datumTokenizer,
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            local: _values
        });
    }

    // adds prefetch source
    this.setPrefetchSource = function (_url) {
        this.sourceDefinition.source = new Bloodhound({
            datumTokenizer: this.datumTokenizer,
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            prefetch: _url
        });
    }

}


// factory that creates typeahead sources
CorabeuControl.TypeaheadSourceFactory = {

    // gets default template html element template
    getEmptyTemplate: function (_text) {
        return [
            "<div class='tt-empty-message'>",
            CorabeuControl.valueOrDefault(_text, CorabeuControl.Localisation.noItemFound),
            "</div>"
        ].join("\n");
    },

    // gets static local source
    getLocalSource: function (_name, _values, _text, _value, _template, _emptyTemplate) {
        var result = new CorabeuControl.TypeaheadSource(_name, _text, _value);
        result.setLocalSource(_values);
        result.addTemplate(_template, _emptyTemplate);
        return result;
    },

    // gets source that prefetches values from api
    getPrefetchSource: function (_name, _url, _text, _value, _template, _emptyTemplate) {
        var result = new CorabeuControl.TypeaheadSource(_name, _text, _value);
        result.setPrefetchSource(_url);
        result.addTemplate(_template, _emptyTemplate);
        return result;
    }

}


// localisable data
CorabeuControl.Localisation = {

    noItemFound: "No item found"

}

/* * * * * * * * * * * * * * * * * HELPER CLASSES (end)  * * * * * * * * * * * * * * * * * * * * * */


/* * * * * * * * * * * * * * * * * BASE FUNCTIONS  * * * * * * * * * * * * * * * * * * * * * * * * */

// checks whether value is not undefined, otherwise returns default value
CorabeuControl.valueOrDefault = function (_value, _defaultValue) {
    return (typeof _value === "undefined") ? _defaultValue : _value;
}

// checks whether value is not defined
CorabeuControl.isDefined = function (_value) {
    return !(typeof _value === "undefined");
}

// fills leading characters to string
CorabeuControl.fillLeadingCharacter = function (_baseString, _char, _count) {
    if (_baseString == null) return _baseString;
    var countToFill = _count - _baseString.length;
    var result = _baseString;
    for (var i = countToFill; i > 0; i--)
        result = _char + result;
    return result;
}

// valids element
CorabeuControl.validElement = function (_element) {
    if (CorabeuControl.isDefined(_element.valid))
        _element.valid();
}

/* * * * * * * * * * * * * * * * * BASE FUNCTIONS (end)  * * * * * * * * * * * * * * * * * * * * * */




