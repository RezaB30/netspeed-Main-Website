function GetValueDays(requestIdYear, requestIdMonth, responseId, url) {
    var IDYear = $(requestIdYear).val();
    var IDMonth = $(requestIdMonth).val();
    var token = $("[name='__RequestVerificationToken']").val();
    ClearList(responseId);
    $(responseId).trigger('change');
    if (IDYear == '' || IDMonth == '') {

    }

    else {
        DisableSelectList();
        $.ajax({
            dataType: 'json',
            method: 'POST',
            url: url,
            data: { year: IDYear, month: IDMonth, __RequestVerificationToken: token },
            complete: function (data, status) {
                if (status == "success") {
                    var results = data.responseJSON;
                    for (var i = 0; i < results.length; i++) {
                        $(responseId).append('<option value="' + results[i].Value + '">' + results[i].Text + '</option>');
                    }
                    EnableSelectList();
                }
                else {
                    EnableSelectList();
                }
            }
        });
    }
}

function DisableSelectList() {
    $('div').each(function () {
        $(this).find("select").attr("disabled", "disabled");
    })
}
function EnableSelectList() {
    $('div').each(function () {
        $(this).find("select").removeAttr("disabled");
    })
}
function ClearList(responseId) {
    var container = $(responseId);
    container.find('option').each(function () {
        if ($(this).val() != '') {
            $(this).remove();
        }
    });
}
// Numeric only control handler
jQuery.fn.ForceNumericOnly =
    function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;
                // allow backspace, tab, delete, enter, arrows, numbers and keypad numbers ONLY
                // home, end, period, and numpad decimal
                return (
                    key == 8 ||
                    key == 9 ||
                    key == 13 ||
                    key == 46 ||
                    key == 110 ||
                    key == 190 ||
                    (key >= 35 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                    (key >= 96 && key <= 105));
            });
        });
    };

