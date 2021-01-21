function GetButton(visible, notVisible) {
    //var IDvisible = $(visible).val();
    //var IDnotVisible = $(notVisible).val();
    //var token = $("[name='__RequestVerificationToken']").val();
    if (getElementById('visible')) {

        if (getElementById('visible').style.display == 'block') {
            getElementById('visible').style.display = 'none';
            getElementById('notVisible').style.display = 'block';
        }
        else {
            getElementById('visible').style.display = 'block';
            getElementById('notVisible').style.display = 'none';
        }

    }
}


