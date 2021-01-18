//var oAH = {

//    ValidateSubmit: function (self) {

//        // send request to handler
//        return $.ajax({
//            type: "POST",
//            url: "",
//            data: aData,
//            cache: false
//        })
//            .then(function (data) {
//                return data.Status === 1;
//            });
//    }
//}


//$('#form').on('submit', function (e) {
//    var form = this;
//    e.preventDefault()

//    return oAH.ValidateSubmit(this)
//        .then(function (canSubmit) {
//            if (canSubmit) {
//                form.submit();
//            }
//        });
//});

function GetService(apartmentId, url, callback) {
    var token = $("[name='__RequestVerificationToken']").val();
    //ClearList(responseId);

    //alert("Yükleniyor");
    $.ajax({
        type: 'POST',
        url: url,
        dataType: 'json',
        data: { apartmentId: apartmentId, __RequestVerificationToken: token },

        complete: function (data, status) {
            if (status == "success") {
                results = data.responseJSON;
                callback(results);
            }
            else {
                alert(status);
            }
        }
    
        //success: function (data) {
        //    console.log('success', data);
        //},
        //error: function (exception) { alert('Exeption:' + exception); }
    });
}
