var usernameExist = true;
var emailExist = true;

$('#Username').on('blur', function () {
    var username = $('#Username').val();
    if (username !== '') {

        $.ajax({
            url: '/Dashboard/CheckUsername',
            type: "POST",
            data: {
                username: username
            },
            success: function (data) {
                // Update DOM elements with retrieved data
                if (data === "Fail") {
                    usernameExist = true;
                    $('#usexist').text('Username has already been taken!');
                } else {
                    usernameExist = false;
                    $('#usexist').text('');
                }
            }
        });
    }
});

$('#Email').on('blur', function () {
    var email = $('#Email').val();
    if (email !== '') {

        $.ajax({
            url: '/Dashboard/CheckEmail',
            type: "POST",
            data: {
                email: email
            },
            success: function (data) {
                // Update DOM elements with retrieved data
                if (data === "Fail") {
                    emailExist = true;
                    $('#emailexist').text('Email has already been used!');
                } else {
                    emailExist = false;
                    $('#emailexist').text('');
                }
            }
        });
    }
});

// Lấy giá tin input
function getValueById(id) {
    return $('#' + id).val();
}

// Hiển thị thông báo lỗi cho một trường
function showError(id, message) {
    $('#' + id).next('p').text(message);
}

// Ẩn thông báo lỗi của một trường
function hideError(id) {
    $('#' + id).next('p').text('');
}
// Kiểm tra tính hợp lệ của số điện thoại
function isValidPhoneNumber(phoneNum) {
    const phonePattern = /^0\d{9}$/;
    return phonePattern.test(phoneNum);
}
// Kiểm tra tính hợp lệ của SSN
function isValidSSN(SSN) {
    const SSNPattern = /^\d{12}$/;
    return SSNPattern.test(SSN);
}

// Kiểm tra tính hợp lệ của địa chỉ email
function isValidEmail(email) {
    const emailPattern = /^[a-zA-Z][a-zA-Z0-9._%+-]+@[^\s@]+\.[^\s@]{2,}$/;
    return emailPattern.test(email);
}



$(document).ready(function () {
    $('form.createAccount').submit(function (event) {

        var username = getValueById('Username');
        var password = getValueById('Password');
        var fullname = getValueById('Fullname');
        var phone = getValueById('PhoneNumber');
        var email = getValueById('Email');
        var SSN = getValueById('SSN');
        var address = getValueById('LivingAddress');

        var noError = true;

        //tài khoản
        if (!username) {
            showError('Username', 'Please enter your username!');
            noError = false;
        } else if (username.length > 20 || username.length < 6) {
            showError('Username', 'Username must be from 6 to 20 characters!');
            noError = false;
        } else {
            hideError('Username');
        }


        //Họ và tên
        if (!fullname) {
            showError('Fullname', 'Please enter your full name!');
            noError = false;
        } else if (fullname.length > 100) {
            showError('Fullname', 'You full name must be less than or equal to 100 characters!');
            noError = false;
        } else {
            hideError('Fullname');
        }

        // Số điện thoại
        if (!phone) {
            showError('PhoneNumber', 'Please enter your phone number!');
            noError = false;
        } else if (!isValidPhoneNumber(phone)) {
            showError('PhoneNumber', 'Phone number format must be 0XXXXXXXXX');
            noError = false;
        } else {
            hideError('PhoneNumber');
        }

        //Email
        if (!email) {
            showError('Email', 'Please enter your email address!');
            noError = false;
        } else if (!isValidEmail(email)) {
            showError('Email', 'Invalid Email address!');
            noError = false;
        } else {
            hideError('Email');
        }

        //Password
        if (!password) {
            showError('Password', 'Please enter a password!');
            noError = false;
        } else if (password.length < 8) {
            showError('Password', 'Password must have at least 1 uppercase letter, 1 special character, and be at least 8 characters long!');
            noError = false;
        } else if (!/(?=.*[A-Z])(?=.*[!@#$%^&*])(.{8,})/.test(password)) {
            showError('Password', 'Password must have at least 1 uppercase letter, 1 special character, and be at least 8 characters long!');
            noError = false;
        } else {
            hideError('Password');
        }

        //SSN
        if (!SSN) {
            showError('SSN', 'Please enter your SSN!');
            noError = false;
        } else if (!isValidSSN(SSN)) {
            showError('SSN', 'Invalid SSN!');
            noError = false;
        } else {
            hideError('SSN');
        }

        //Địa chỉ
        if (!address) {
            showError('LivingAddress', 'Please enter your address!');
            noError = false;
        } else if (address.length > 100) {
            showError('LivingAddress', 'address must be less than or equal to 100 characters!');
            noError = false;
        } else {
            hideError('LivingAddress');
        }

        //If there is error, block form submit
        if (!noError || usernameExist || emailExist) {
            event.preventDefault();
        }
    });
});