"use strict";

// Class definition
var KTWizard5 = function () {
    // Base elements
    var _wizardEl;
    var _formEl;
    var _wizardObj;
    var _validations = [];


    // Private functions
    var _initWizard = function () {
        // Initialize form wizard
        _wizardObj = new KTWizard(_wizardEl, {
            startStep: 1, // initial active step number
            clickableSteps: false  // allow step clicking
        });

        // Validation before going to next page
        _wizardObj.on('change', function (wizard) {
            // load validations again //
            if (wizard.getStep() > wizard.getNewStep()) {
                return; // Skip if stepped back
            }

            // Validate form before change wizard step
            //_validations = [];
            //_initValidation();
            var validator = _validations[wizard.getStep() - 1]; // get validator for currnt step

            if (validator) {
                var curFields = validator.fields;
                var isInvalid = 0;
                if (curFields.tcno && curFields.anneadi && curFields.annekizliksoyad && curFields.cinsiyet && curFields.babaadi && curFields.dogumgun) {
                    if (!$('input[name="kimlikmahalle"]').val()) {
                        ClearValidations($('input[name="kimlikmahalle"]'));
                        $('input[name="kimlikmahalle"]').parent("div").append('<div class="fv-plugins-message-container"><div data-field="kimlikmahalle" data-validator="notEmpty" class="fv-help-block">' + 'Kayıtlı Olduğu Mahalle Zorunludur' + '</div></div>');
                        $('input[name="kimlikmahalle"]').addClass("is-invalid");
                        isInvalid++;
                    }
                    if (!$('input[name="kimlikil"]').val()) {
                        ClearValidations($('input[name="kimlikil"]'));
                        $('input[name="kimlikil"]').parent("div").append('<div class="fv-plugins-message-container"><div data-field="kimlikil" data-validator="notEmpty" class="fv-help-block">' + 'Kayıtlı Olduğu İl Zorunludur' + '</div></div>');
                        $('input[name="kimlikil"]').addClass("is-invalid");
                        isInvalid++;
                    }
                    if (!$('input[name="kimlikilce"]').val()) {
                        ClearValidations($('input[name="kimlikilce"]'));
                        $('input[name="kimlikilce"]').parent("div").append('<div class="fv-plugins-message-container"><div data-field="kimlikilce" data-validator="notEmpty" class="fv-help-block">' + 'Kayıtlı Olduğu İlçe Zorunludur' + '</div></div>');
                        $('input[name="kimlikilce"]').addClass("is-invalid");
                        isInvalid++;
                    }
                    if (!$('input[name="ailesirano"]').val()) {
                        ClearValidations($('input[name="ailesirano"]'));
                        $('input[name="ailesirano"]').parent("div").append('<div class="fv-plugins-message-container"><div data-field="ailesirano" data-validator="notEmpty" class="fv-help-block">' + 'Aile Sıra Numarası Zorunludur' + '</div></div>');
                        $('input[name="ailesirano"]').addClass("is-invalid");
                        isInvalid++;
                    }
                    if (!$('input[name="ciltno"]').val()) {
                        ClearValidations($('input[name="ciltno"]'));
                        $('input[name="ciltno"]').parent("div").append('<div class="fv-plugins-message-container"><div data-field="ciltno" data-validator="notEmpty" class="fv-help-block">' + 'Cilt Numarası Zorunludur' + '</div></div>');
                        $('input[name="ciltno"]').addClass("is-invalid");
                        isInvalid++;
                    }
                    if (!$('input[name="sirano"]').val()) {
                        ClearValidations($('input[name="sirano"]'));
                        $('input[name="sirano"]').parent("div").append('<div class="fv-plugins-message-container"><div data-field="sirano" data-validator="notEmpty" class="fv-help-block">' + 'Sıra Numarası Zorunludur' + '</div></div>');
                        $('input[name="sirano"]').addClass("is-invalid");
                        isInvalid++;
                    }
                }
                validator.validate().then(function (status) {
                    if (status == 'Valid') {

                        if (curFields.gsmno) {
                            SendValidationSMS(wizard);
                        }
                        if (curFields.smscode) {
                            CheckSMSCode(wizard);
                        }
                        if (curFields.firstname && curFields.lastname && curFields.email) { // iletişim bilgileri
                            wizard.goTo(wizard.getNewStep()); // next step
                        }
                        if (curFields.sehir && curFields.ilce && curFields.hizmetnosu) { // başvuru bilgileri
                            var registerStepCount = 0;
                            ClearValidations($('input[name="evtelno"]'));
                            ClearValidations($('input[name="hizmetnosu"]'));
                            ClearValidations($('input[name="mevcutoprad"]'));
                            if ($('#housephone').val() == 2) {
                                if (!$('input[name="evtelno"]').val()) {
                                    registerStepCount++;                                    
                                    $('input[name="evtelno"]').parent("div").append('<div class="fv-plugins-message-container"><div data-field="evtelno" data-validator="notEmpty" class="fv-help-block">' + 'Ev Telefon Numarası Zorunludur' + '</div></div>');
                                    $('input[name="evtelno"]').addClass("is-invalid");
                                } else {
                                    var _tempEvTelNo = $('input[name="evtelno"]').val().replace("(", "").replace(")", "").replace(" ", "").replace("-", "").replace("_","");
                                    if (_tempEvTelNo.length != 10) {
                                        registerStepCount++;
                                        $('input[name="evtelno"]').parent("div").append('<div class="fv-plugins-message-container"><div data-field="evtelno" data-validator="notEmpty" class="fv-help-block">' + 'Lütfen Geçerli Telefon Numarası Giriniz' + '</div></div>');
                                        $('input[name="evtelno"]').addClass("is-invalid");
                                    }
                                }
                            }
                            if ($('select[name="applicationType"]').val() == 2) {
                                if (!$('input[name="hizmetnosu"]').val()) {
                                    registerStepCount++;                                    
                                    $('input[name="hizmetnosu"]').parent("div").append('<div class="fv-plugins-message-container"><div data-field="hizmetnosu" data-validator="notEmpty" class="fv-help-block">' + 'Hizmet Numarası Zorunludur' + '</div></div>');
                                    $('input[name="hizmetnosu"]').addClass("is-invalid");
                                } else {
                                    if ($('input[name="hizmetnosu"]').val().replace("_","").length != 10) {
                                        registerStepCount++;
                                        $('input[name="hizmetnosu"]').parent("div").append('<div class="fv-plugins-message-container"><div data-field="hizmetnosu" data-validator="notEmpty" class="fv-help-block">' + 'Lütfen Geçerli Hizmet Numarası Giriniz' + '</div></div>');
                                        $('input[name="hizmetnosu"]').addClass("is-invalid");
                                    }
                                }
                                if (!$('input[name="mevcutoprad"]').val()) {
                                    registerStepCount++;                                    
                                    $('input[name="mevcutoprad"]').parent("div").append('<div class="fv-plugins-message-container"><div data-field="mevcutoprad" data-validator="notEmpty" class="fv-help-block">' + 'Operatör Adı Zorunludur' + '</div></div>');
                                    $('input[name="mevcutoprad"]').addClass("is-invalid");
                                }
                            }
                            if (registerStepCount != 0) {
                                Swal.fire({
                                    text: "Hata Bulundu, Lütfen kontrol edin",
                                    icon: "error",
                                    buttonsStyling: false,
                                    confirmButtonText: "Tamam",
                                    customClass: {
                                        confirmButton: "btn font-weight-bold btn-light"
                                    }
                                }).then(function () {
                                    KTUtil.scrollTop();
                                });
                            } else {
                                GetTariffsPrefers(wizard);
                            }
                        }
                        if (curFields.xstatikip && curFields.xphone && curFields.xemail && curFields.tariff) { // tarife ve tercihler
                            CheckTariffsPrefers(wizard);
                        }
                        if (curFields.tcno && curFields.anneadi && curFields.annekizliksoyad && curFields.cinsiyet && curFields.babaadi && curFields.dogumgun) { // kimlik bilgileri                            
                            if ($('#idCardType').val() == 2) {
                                if (isInvalid == 0) {
                                    IDCardValidation(wizard);
                                } else {
                                    Swal.fire({
                                        text: "Hata Bulundu, Lütfen kontrol edin",
                                        icon: "error",
                                        buttonsStyling: false,
                                        confirmButtonText: "Tamam",
                                        customClass: {
                                            confirmButton: "btn font-weight-bold btn-light"
                                        }
                                    }).then(function () {
                                        KTUtil.scrollTop();
                                    });
                                }
                            }
                            else {
                                IDCardValidation(wizard);
                            }
                        }
                        // do something for functions
                        KTUtil.scrollTop();
                    } else {
                        Swal.fire({
                            text: "Hata Bulundu, Lütfen kontrol edin",
                            icon: "error",
                            buttonsStyling: false,
                            confirmButtonText: "Tamam",
                            customClass: {
                                confirmButton: "btn font-weight-bold btn-light"
                            }
                        }).then(function () {
                            KTUtil.scrollTop();
                        });
                    }
                });
            }

            return false;  // Do not change wizard step, further action will be handled by he validator
        });

        // Change event
        _wizardObj.on('changed', function (wizard) {
            KTUtil.scrollTop();
        });
        // Submit event
        _wizardObj.on('submit', function (wizard) {
            var isValidContractCheckbox = 0;
            $('input[name="cccvv"]').each(function () {
                if (!$(this).is(":checked")) {
                    isValidContractCheckbox++;
                }
            });
            if (isValidContractCheckbox != 0) {
                ShowErrorMessage("Lütfen onay kutucuklarını boş bırakmayınız");
            }
            else {
                Swal.fire({
                    text: "Herşey Tamam!, Başvurunuzu Onaylıyor musunuz?",
                    icon: "success",
                    showCancelButton: true,
                    buttonsStyling: false,
                    confirmButtonText: "Onaylıyorum",
                    cancelButtonText: "İptal",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-primary",
                        cancelButton: "btn font-weight-bold btn-default"
                    }
                }).then(function (result) {
                    if (result.value) {
                        var datastring = $('form[dataform="register"]').serialize();
                        $.ajax({
                            url: '/basvur/Index',
                            method: 'POST',
                            data: datastring,
                            complete: function (data, status) {
                                RegisterResult(data, status);
                            }
                        })
                        //$('form[dataform="register"]').submit(); // Submit form
                    } else if (result.dismiss === 'cancel') {
                        Swal.fire({
                            text: "Başvuru Formu Gönderimi İptal Edildi",
                            icon: "error",
                            buttonsStyling: false,
                            confirmButtonText: "Tamam",
                            customClass: {
                                confirmButton: "btn font-weight-bold btn-primary",
                            }
                        });
                    }
                });
            }
        });
    }
    //$('button[data-wizard-type="action-submit"]').click(function (event) {        

    //});    
    function RegisterResult(data, status) {
        if (status == "success") {
            var result = data.responseJSON;
            if (result.errorCode == 0) {
                //Swal.fire({
                //    text: result.message,
                //    icon: "success",
                //    buttonsStyling: false,
                //    confirmButtonText: "Tamam",
                //    customClass: {
                //        confirmButton: "btn font-weight-bold btn-light"
                //    }
                //}).then(function () {
                //    window.location.href = "/";
                //});
                window.location.href = '/destek/tesekkurederiz';
            } else {
                ShowErrorMessage(result.message);
            }
        } else {
            ShowErrorMessage("Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
        }
    }
    var _initValidation = function () {
        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        // Step 1
        _validations.push(FormValidation.formValidation(
            _formEl,
            {
                fields: {
                    gsmno: {
                        validators: {
                            notEmpty: {
                                message: 'GSM numaranızı yazınız'
                            },
                            regexp: {
                                regexp: /^[(][0-9][0-9][0-9][)][ ][0-9][0-9][0-9][-][0-9]+$/,
                                message: 'Lütfen geçerli bir telefon numarası giriniz'
                            }
                        }
                    },
                    kvkk: {
                        validators: {
                            choice: {
                                min: 1,
                                message: 'Onay veriniz'
                            },

                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    // Bootstrap Framework Integration
                    bootstrap: new FormValidation.plugins.Bootstrap({
                        //eleInvalidClass: '',
                        eleValidClass: '',
                    })
                }
            }
        ));

        // Step 2
        _validations.push(FormValidation.formValidation(
            _formEl,
            {
                fields: {
                    smscode: {
                        validators: {
                            notEmpty: {
                                message: 'SMS Doğrulama Kodunuzu Yazınız'
                            },
                            regexp: {
                                regexp: /^[0-9]+$/,
                                message: 'Doğrulama kodu eksik veya hatalı'
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    // Bootstrap Framework Integration
                    bootstrap: new FormValidation.plugins.Bootstrap({
                        //eleInvalidClass: '',
                        eleValidClass: '',
                    })
                }
            }
        ));

        // Step 3
        _validations.push(FormValidation.formValidation(
            _formEl,
            {
                fields: {
                    firstname: {
                        validators: {
                            notEmpty: {
                                message: 'Adınızı Yazınız'

                            }
                        }
                    },
                    lastname: {
                        validators: {
                            notEmpty: {
                                message: 'Soyadınızı Yazınız'
                            }
                        }
                    },
                    email: {
                        validators: {
                            notEmpty: {
                                message: 'E-Posta Gerekli'
                            },
                            regexp: {
                                regexp: /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/,
                                message:'Lütfen geçerli bir e-posta yazınız'
                            }
                        }

                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    // Bootstrap Framework Integration
                    bootstrap: new FormValidation.plugins.Bootstrap({
                        //eleInvalidClass: '',
                        eleValidClass: '',
                    })
                }
            }
        ));
        // Step 4
        _validations.push(FormValidation.formValidation(
            _formEl,
            {
                fields: {
                    sehir: {
                        validators: {
                            notEmpty: {
                                message: 'Şehir Seçiniz'
                            }
                        }
                    },


                    hizmetnosu: {
                        validators: {
                            stringLength: {
                                min: 10,
                                max: 10,
                                message: 'Hizmet No 10 haneli olmalıdır'
                            }
                        }
                    },

                    ilce: {
                        validators: {
                            notEmpty: {
                                message: 'İlçe Seçiniz'
                            }
                        }
                    },
                    semt: {
                        validators: {
                            notEmpty: {
                                message: 'Semt Seçiniz'
                            }
                        }
                    },
                    mahalle: {
                        validators: {
                            notEmpty: {
                                message: 'Mahalle Seçiniz'
                            }
                        }
                    },
                    sokak: {
                        validators: {
                            notEmpty: {
                                message: 'Sokak veya Cadde Seçiniz'
                            }
                        }
                    },
                    kapino: {
                        validators: {
                            notEmpty: {
                                message: 'Kapı No Seçiniz'
                            }
                        }
                    },
                    katno: {
                        validators: {
                            notEmpty: {
                                message: 'Kat No Seçiniz'
                            }
                        }
                    },
                    postakodu: {
                        validators: {
                            notEmpty: {
                                message: 'Posta Kodu Zorunludur'
                            }
                        }
                    },
                    aptbina: {
                        validators: {
                            notEmpty: {
                                message: 'Bina No Seçiniz'
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    // Bootstrap Framework Integration
                    bootstrap: new FormValidation.plugins.Bootstrap({
                        //eleInvalidClass: '',
                        eleValidClass: '',
                    })
                }
            }
        ));

        // Step 5
        _validations.push(FormValidation.formValidation(
            _formEl,
            {
                fields: {
                    xstatikip: {
                        validators: {
                            notEmpty: {
                                message: 'Adınızı Yazınız'

                            }
                        }
                    },
                    xlastname: {
                        validators: {
                            notEmpty: {
                                message: 'Soyadınızı Yazınız'
                            }
                        }
                    },
                    xemail: {
                        validators: {
                            notEmpty: {
                                message: 'E-Posta Gerekli'
                            },
                            emailAddress: {
                                message: 'Lütfen geçerli bir e-posta yazınız'
                            }
                        }
                    },
                    xphone: {
                        validators: {
                            notEmpty: {
                                message: 'Phone is required'
                            }
                        }
                    },
                    tariff: {
                        validators: {
                            notEmpty: {
                                message: 'Lütfen Tarife Seçiniz'
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    // Bootstrap Framework Integration
                    bootstrap: new FormValidation.plugins.Bootstrap({
                        //eleInvalidClass: '',
                        eleValidClass: '',
                    })
                }
            }
        ));

        // Step 6
        _validations.push(FormValidation.formValidation(
            _formEl,
            {
                fields: {
                    tcno: {
                        validators: {
                            notEmpty: {
                                message: 'T.C Zorunludur'
                            }
                        }

                    },
                    dogumyili: {
                        validators: {
                            notEmpty: {
                                message: 'Doğum Yılı Zorunludur'
                            }
                        }

                    },
                    annekizliksoyad: {
                        validators: {
                            notEmpty: {
                                message: 'Anne Kızlık Soyadı Zorunludur'
                            }
                        }

                    },

                    dogumyeri: {
                        validators: {
                            notEmpty: {
                                message: 'Doğum Yeri Zorunludur'
                            }
                        }

                    },

                    cinsiyet: {
                        validators: {
                            notEmpty: {
                                message: 'Cinsiyet Seçiniz'
                            }
                        }

                    },

                    babaadi: {
                        validators: {
                            notEmpty: {
                                message: 'Baba Adı Zorunludur'
                            }
                        }

                    },

                    anneadi: {
                        validators: {
                            notEmpty: {
                                message: 'Anne Adı Zorunludur'
                            }
                        }

                    },

                    dogumay: {
                        validators: {
                            notEmpty: {
                                message: 'Doğum Ayı Zorunludur'
                            }
                        }

                    },

                    dogumgun: {
                        validators: {
                            notEmpty: {
                                message: 'Doğum Günü Zorunludur'
                            }
                        }

                    },
                    serino: {
                        validators: {
                            notEmpty: {
                                message: 'Kimlik Seri Numarası Zorunludur'
                            }
                        }
                    },
                    kimlikyil: {
                        validators: {
                            notEmpty: {
                                message: 'Kimlik Tarihi Zorunludur'
                            }
                        }
                    },
                    kimlikay: {
                        validators: {
                            notEmpty: {
                                message: 'Kimlik Tarihi Zorunludur'
                            }
                        }
                    },
                    kimlikgun: {
                        validators: {
                            notEmpty: {
                                message: 'Kimlik Tarihi Zorunludur'
                            }
                        }
                    }
                    //xcountry: {
                    //    validators: {
                    //        notEmpty: {
                    //            message: 'Country is required'
                    //        }
                    //    }
                    //}
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    // Bootstrap Framework Integration
                    bootstrap: new FormValidation.plugins.Bootstrap({
                        //eleInvalidClass: '',
                        eleValidClass: '',
                    })
                }
            }

        ));

    }
    return {
        // public functions
        init: function () {
            _wizardEl = KTUtil.getById('kt_wizard');
            _formEl = KTUtil.getById('kt_form');

            _initWizard();
            _initValidation();
        }
    };
}();

jQuery(document).ready(function () {
    KTWizard5.init();
});

// functions
function ShowErrorMessage(errorMsg) {
    Swal.fire({
        text: errorMsg,
        icon: "error",
        buttonsStyling: false,
        confirmButtonText: "Tamam",
        customClass: {
            confirmButton: "btn font-weight-bold btn-light"
        }
    }).then(function () {
        KTUtil.scrollTop();
    });
}

function SendValidationSMS(wizard) {
    var phoneNo = $('input[name="gsmno"]').val();
    phoneNo = phoneNo.replace("(", "").replace(")", "").replace(" ", "").replace("-", "");
    $.ajax({
        url: '/basvur/SendValidationSMS',
        method: 'POST',
        data: { phoneNo: phoneNo },
        complete: function (data, status) {
            if (status == "success") {
                var response = data.responseJSON;
                if (response == "invalid") {
                    ShowErrorMessage("İşlem yapmak istediğiniz numaraya ait mevcut abonelik bulunmaktadır. Yeni abonelik için online işlem merkezinden başvuru yapabilirsiniz");
                }
                else if (response == "error") {
                    ShowErrorMessage("Bir hata oluştu. Daha sonra tekrar deneyiniz.");
                }
                else {
                    wizard.goTo(wizard.getNewStep()); // next step
                }
            } else {
                ShowErrorMessage("Bir hata oluştu. Daha sonra tekrar deneyiniz.");
            }
        }
    });
}
function CheckSMSCode(wizard) {
    var smsCode = $('input[name="smscode"]').val();
    $.ajax({
        url: '/basvur/CheckValidationSMS',
        method: 'POST',
        data: { smsCode: smsCode },
        complete: function (data, status) {
            if (status == "success") {
                var response = data.responseJSON;
                if (response.includes("error")) {
                    ShowErrorMessage("Doğrulama kodu hatalı. Lütfen tekrar deneyiniz");
                } else {
                    if (response == "failed") {
                        window.location.reload();
                    } else {
                        wizard.goTo(wizard.getNewStep()); // next step
                    }
                }
            } else {
                ShowErrorMessage("Bir hata oluştu. Daha sonra tekrar deneyiniz.");
            }
        }
    });
}
function GetTariffsPrefers(wizard) {
    var apartmentCode = $('select[name="kapino"]').val();
    $.ajax({
        url: '/basvur/GetTariffs',
        method: 'POST',
        data: { apartmentCode: apartmentCode },
        complete: function (data, status) {
            if (status == "success") {
                var response = data.responseText;
                if (response != "error") {
                    $('#customer-tariffs').html(response);
                    wizard.goTo(wizard.getNewStep()); // next step
                }
            } else {
                ShowErrorMessage("Bir hata oluştu. Daha sonra tekrar deneyiniz.");
            }
        }
    });
}
function SubscriptionSummary() {
    var fullname = $('input[name="firstname"]').val() + " " + $('input[name="lastname"]').val();
    var email = $('input[name="email"]').val();
    var phoneNo = $('input[name="gsmno"]').val();
    var addressText = $('#address-text').text();
    var applicationType = $('select[name="applicationType"]').val();
    var applicationTypeText = $('select[name="applicationType"] option:selected').text();
    var xdslNo = $('input[name="hizmetnosu"]').val();
    var transferISS = $('input[name="mevcutoprad"]').val();
    if (applicationType == 2) {
        xdslNo = "Mevcut Operatör Hizmet No : " + xdslNo;
        transferISS = "Geçiş Yapılacak Operatör Adı : " + transferISS;
    } else {
        xdslNo = "";
        transferISS = "";
    }
    var tariffName = "-";
    $('input[name="tariff"]').each(function (e) {
        if ($(this).is(":checked")) {
            tariffName = $('#tariff-generic-info-' + $(this).val() + '').text();
        }
    });
    var modem = "Modem Talebi : " + $('select[name="modem"] option:selected').text();
    //
    $("#summary-fullname").text(fullname);
    $("#summary-email").text(email);
    $("#summary-phoneNo").text(phoneNo);
    $("#summary-addressText").text(addressText);
    $("#summary-applicationType").text(applicationTypeText);
    $("#summary-xdslNo").text(xdslNo);
    $("#summary-transferISS").text(transferISS);
    $("#summary-tariffName").text(tariffName);
    $("#summary-modem").text(modem);
}
function IDCardValidation(wizard) {
    var idCardType = $('select[name="idCardType"]').val();
    var tckNo = $('input[name="tcno"]').val();
    var firstname = $('input[name="firstname"]').val();
    var lastname = $('input[name="lastname"]').val();
    var birthDateDay = $('select[name="dogumgun"]').val();
    var birthDateMonth = $('select[name="dogumay"]').val();
    var birthDateYear = $('select[name="dogumyili"]').val();
    var serialNumber = $('input[name="serino"]').val();
    $.ajax({
        url: '/basvur/IDCardValidation',
        method: 'POST',
        data: { idCardType, tckNo, firstname, lastname, birthDateDay, birthDateMonth, birthDateYear, serialNumber },
        complete: function (data, status) {
            if (status == "success") {
                var response = data.responseJSON;
                if (response == "success") {
                    SubscriptionSummary();
                    wizard.goTo(wizard.getNewStep()); // next step
                } else {
                    ShowErrorMessage("Kimlik doğrulama başarısız. Lütfen bilgilerinizi tekrar giriniz");
                }
            } else {
                ShowErrorMessage("Bir hata oluştu. Daha sonra tekrar deneyiniz.");
            }
        }
    });
}
function CheckTariffsPrefers(wizard) {
    var hasSelected = false;
    $('input[name="tariff"]').each(function (e) {
        if ($(this).is(":checked")) {
            hasSelected = true;
        }
    });
    if (hasSelected) {
        wizard.goTo(wizard.getNewStep()); // next step
    } else {
        ShowErrorMessage("Lütfen Tarifenizi Seçiniz");
    }
}