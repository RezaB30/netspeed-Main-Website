function GetRequiredMessage(buttonClass, idVisible, idNotVisble) {
    //var token = $("[name='__RequestVerificationToken']").val();

    //alert("Yükleniyor");
    var buttonClass = buttonClass;
    var idVisible = $(idVisible);
    var idNotVisble = $(idNotVisble);


    //$('button.buttonClass').click(function () {
    //var clickedButton = $(this);
    var clickedButton = $(buttonClass);
    var currentContainer = clickedButton.closest('div.wizard-step');
    var isValid = true;
    var requiredSelects = currentContainer.find('.form-group.is-required').find('select[name]');
    var requiredTexts = currentContainer.find('.form-group.is-required').find('input[name]');
    //var requiredRadioButtons = currentContainer.find('.form-group.is-required').find('check[name]');

    requiredSelects.each(function () {
        var currentSelect = $(this);
        var hasSelectedOption = currentSelect.find('option[value!=""]:selected').length > 0;

        if (!hasSelectedOption /*|| !hasText || ! HasRadioButtons*/) {
            isValid = false;
            //currentSelect.closest('.form-group').find('span.required-error').show();
            currentSelect.closest('.form-group.is-required').find('span.required-error').show();
        }
        else {
            //currentSelect.closest('.form-group').find('span.required-error').hide();
            currentSelect.closest('.form-group.is-required').find('span.required-error').hide();
        }
    });

    requiredTexts.each(function () {
        var currentInput = $(this);
        //var hasText = currentSelect.find('input[value!=""]:text').length > 0
        var hasText = currentInput.val() != "";

        if (!hasText) {
            isValid = false;
            //currentInput.closest('.form-group').find('span.required-error').show();
            currentSelect.closest('.form-group.is-required').find('span.required-error').show();
        }
        else {
            //currentInput.closest('.form-group').find('span.required-error').hide();
            currentSelect.closest('.form-group.is-required').find('span.required-error').hide();
        }
    });

    //requiredRadioButtons.each(function () {
    //    var currentSelect = $(this);
    //    var HasRadioButtons = currentSelect.find('input[value!=""]:checked').length > 0;

    //    if (!HasRadioButtons) {
    //        isValid = false;
    //        currentSelect.closest('.form-group').find('span.required-error').show();
    //        //currentSelect.closest('.form-group.is-required').find('span.required-error').show();
    //    }
    //    else {
    //        currentSelect.closest('.form-group').find('span.required-error').hide();
    //    }
    //});

    if (isValid == true) {
        var k = $('#InfrastructureInquiryForApplication.wizard-step');
        if (idVisible == k) {
            $.ajax({
                url: '@Url.Action("GetServiceAvailability","Application")',
                method: 'POST',
                data: { apartmentId: $("#ApartmentId").val() },
                complete: function (data, status) {
                    if (status == "success") {
                        //$('#tariffs-content').html(data.responseText);
                        $('#InfrastructureAndTariffContent').html(data.responseText);
                        var foundButton = $('#InfrastructureAndTariffContent').find('.HasNotInfrastucture');
                        if (foundButton.length == 0) {
                            $('#visibleButton').show();
                        }
                    }
                }
            })
        }


        if ($('#idVisible').is(':visible')) {
            $('#idVisible').hide();
            $('#idNotVisible').show();
        }
    }
    //})
}