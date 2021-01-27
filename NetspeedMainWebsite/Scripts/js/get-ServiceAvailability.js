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
