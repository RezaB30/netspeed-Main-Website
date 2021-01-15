function GetNext(visible, notVisible) {
    var IDvisible = $(visible).val();
    var IDnotVisible = $(notVisible).val();
    //var token = $("[name='__RequestVerificationToken']").val();

  

        if (document.getElementById('visible')) {

            if (document.getElementById('visible').style.display == 'block') {
                document.getElementById('visible').style.display = 'none';
                document.getElementById('notVisible').style.display = 'block';
            }
            else {
                document.getElementById('visible').style.display = 'block';
                document.getElementById('notVisible').style.display = 'none';
            }
        
    }
}


