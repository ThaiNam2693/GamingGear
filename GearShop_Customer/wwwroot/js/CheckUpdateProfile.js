var noError = true;
$('#emailUpdate').on('blur', function () {
    var fullname = $('#fullnameUpdate').val();
    var phone = $('#phoneUpdate').val();
    var email = $('#emailUpdate').val();
    if (email !== '') {
        $.ajax({
            url: '/Account/UpdateProfile',
            type: "POST",
            data: {
                fullname: fullname,
                phone: phone,
                email: email
            },
            success: function (data) {
                // Update DOM elements with retrieved data
                if (data === "Existed") {
                    $('#smsEmail').text('Email already existed!');
                    noError = false;
                } else {
                    $('#smsEmail').text('');
                    noError = true;
                }
            }
        });
    }
});

$(document).ready(function () {
    $('form.update_profile').submit(function (event) {
        var fullname = $('#fullnameUpdate').val();
        var phone = $('#phoneUpdate').val();
        var email = $('#emailUpdate').val();

        if (!fullname) {
            $('#fullnameUpdate').next('p').text('Enter your full name');
            noError = false;
        } else if (fullname.length > 100) {
            $('#fullnameUpdate').next('p').text('You full name must be less than 100 characters!');
            noError = false;
        } else {
            $('#fullnameUpdate').next('p').text('');
        }


        if (!phone) {
            $('#phoneUpdate').next('p').text('Enter your phone number');
            noError = false;
        } else if (!isValidPhoneNumber(phone)) {
            $('#phoneUpdate').next('p').text('Phone number is invalid!');
            noError = false;
        } else {
            $('#phoneUpdate').next('p').text('');
        }

        if (!email) {
            $('#emailUpdate').next('p').text('Enter your email');
            noError = false;
        } else if (!isValidEmail(email)) {
            $('#emailUpdate').next('p').text('Email is invalid');
            noError = false;
        } else {
            $('#emailUpdate').next('p').text('');
        }

        if (!noError) {
            event.preventDefault();
        }
    });
});

function isValidPhoneNumber(phoneNum) {
    const phonePattern = /^0\d{9}$/;
    return phonePattern.test(phoneNum);
}

function isValidEmail(email) {
    const emailPattern = /^[a-zA-Z][a-zA-Z0-9._%+-]+@[^\s@]+\.[^\s@]{2,}$/;
    return emailPattern.test(email);
}