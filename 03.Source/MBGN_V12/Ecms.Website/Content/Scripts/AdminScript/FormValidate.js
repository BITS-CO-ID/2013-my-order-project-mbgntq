//Initial bind
$(document).ready(function () {
    BindControlEvents();
});

//Re-bind for callbacks
var prm = Sys.WebForms.PageRequestManager.getInstance();

prm.add_endRequest(function () {
    BindControlEvents();
});

function BindControlEvents() {

    var heightContent = $('#rightContent').height();
    if (heightContent < 500) {
        $('#rightContent').height(500);
    }
        
    $(".accordion").accordion({
        heightStyle: "content",
        collapsible: true
    });

    $('.formMain').validate({
        errorPlacement: function (error, element) {
            //error.appendTo(element.parent("td").next("td"));
            error.insertAfter(element);
        }
    });


    $.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            var re = new RegExp(regexp);
            return this.optional(element) || re.test(value);
        }
    );

    if ($('.txtShipUsa').length) {
        $('.txtShipUsa').rules("add", {
            required: true,
            maxlength: 13,
            messages: {
                required: '*',
                maxlength: '*'
            }
        });
    }

    if ($('.txtShipUsaVn').length) {
        $('.txtShipUsaVn').rules("add", {
            required: true,
            maxlength: 13,
            messages: {
                required: '*',
                maxlength: '*'
            }
        });
    }

    if ($('.txtTaxUsa').length) {
        $('.txtTaxUsa').rules("add", {
            required: true,
            maxlength: 13,
            messages: {
                required: '*',
                maxlength: '*'
            }
        });
    }

    if ($('.txtTaxImport').length) {
        $('.txtTaxImport').rules("add", {
            required: true,
            maxlength: 13,
            messages: {
                required: '*',
                maxlength: '*'
            }
        });
    }

    if ($('.txtEffort').length) {
        $('.txtEffort').rules("add", {
            required: true,
            maxlength: 13,
            messages: {
                required: '*',
                maxlength: '*'
            }
        });
    }

    $('.doubleNumber').keypress(function (e) {
        var charCode = (e.which) ? e.which : e.keyCode;
        if (charCode > 31 && charCode != 45 && charCode != 46 && (charCode < 48 || charCode > 57)) {
            return false;
        } else {
            if (charCode == 45) {
                var pattern = /^-{1}/;
                var money = $('.doubleNumber').val();
                if (pattern.test(money)) {
                    return false;
                }
            }
        }
    });

    $('.doubleNumber').keyup(function () {
        var value = $(this).val();
        value = ReplaceAll(value, ',', '');
        value = numberFormat(value);
        $(this).val(value);
    });

    $("#navigation").toggle();
    $(".toggleMenu").click(function () {
        $("#navigation").toggle("slow");
    });
}

function numberFormat(nStr) {
    nStr += '';
    x = nStr.split(',');
    x1 = x[0];
    x2 = x.length > 1 ? ',' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1))
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    return x1 + x2;
}

function ReplaceAll(Source, stringToFind, stringToReplace) {
    var temp = Source;
    var index = temp.indexOf(stringToFind);
    while (index != -1) {
        temp = temp.replace(stringToFind, stringToReplace);
        index = temp.indexOf(stringToFind);
    }
    return temp;
}

function Confirm(message) {
    var yes = confirm(message);
    if (yes) {
        Page_ClientValidate();
        return Page_IsValid;
    }
    return false;
}