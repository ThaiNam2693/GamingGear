var isWrong = true;
$('#pwrs').on('blur', function () {
    var pw = $('#pwrs').val();
    if (pw !== '') {

        $.ajax({
            url: '/Account/CheckPassword',
            type: "POST",
            data: {
                pw: pw
            },
            success: function (data) {
                // Update DOM elements with retrieved data
                if (data === "Fail") {
                    isWrong = false;
                    $('#smspw').text('Password does not correct');
                } else {
                    isWrong = true;
                    $('#smspw').text('');
                }
            }
        });
    }
});
$(document).ready(function () {
    $('form.changepw').submit(function (event) {
        var pw = $('#pwrs').val();
        var pwn = $('#newPass').val();
        var pwc = $('#newPassConfirm').val();
        var noError = true;
        if (pw === '') {
            noError = false;
            $('#smspw').text('Please enter password');
        }



        if (pwn === '') {
            noError = false;
            $('#smsErrpw1').text('Please enter new password');
        } else {
            $('#smsErrpw1').text('');
        }


        if (pwc === '') {
            noError = false;
            $('#smsErrpw2').text('Please enter confirm password');
        } else if (pwc !== pwn) {
            noError = false;
            $('#smsErrpw2').text('Confirm password does not match');
        } else {
            $('#smsErrpw2').text('');
        }


        if (noError == false || isWrong ==false) {
            event.preventDefault();
        }
    });
});
