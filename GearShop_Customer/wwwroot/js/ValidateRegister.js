
$(document).ready(function () {
    $('form.RegisterForm').submit(function (event) {
        $(window).on('unload', function () {// Reset giá trị các biến về trạng thái ban đầu
            noError = true;
        });

        var username = getValueById('ustxt');
        var fullname = getValueById('nametxt');
        var phone = getValueById('phonetxt');
        var email = getValueById('emailtxt');
        var password = getValueById('pwdtxt');
        var repassword = getValueById('re_pwdtxt');

        var noError = true;
        var errorEmail = true;
        var errorUsername = true;
        //tài khoản
        if (!username) {
            showError('ustxt', 'Please enter your username!');
            noError = false;
        } else if (username.length > 20 || username.length < 6) {
            showError('ustxt', 'Username must be from 6 to 20 characters!');
            noError = false;
            errorUsername = false;
        } else {
            hideError('ustxt');
            errorUsername = true;
        }


        //Họ và tên
        if (!fullname) {
            showError('nametxt', 'Please enter your full name!');
            noError = false;
        } else if (fullname.length > 100) {
            showError('nametxt', 'You full name must be less than 100 characters!');
            noError = false;
        } else {
            hideError('nametxt');
        }

        // Số điện thoại
        if (!phone) {
            showError('phonetxt', 'Please enter your phone number!');
            noError = false;
        } else if (!isValidPhoneNumber(phone)) {
            showError('phonetxt', 'Invalid phone number!');
            noError = false;
        } else {
            hideError('phonetxt');
        }

        //Email
        if (!email) {
            showError('emailtxt', 'Please enter your email address!');
            noError = false;
        } else if (!isValidEmail(email)) {
            showError('emailtxt', 'Invalid Email address!');
            noError = false;
            errorEmail = false;
        } else {
            errorEmail = true;
            hideError('emailtxt');
        }

        //Password
        if (!password) {
            showError('pwdtxt', 'Please enter a password!');
            noError = false;
        } else if (password.length < 8) {
            showError('pwdtxt', 'Password must be at least 8 characters!');
            noError = false;
        } else if (!/(?=.*[A-Z])(?=.*[!@#$%^&*])(.{8,})/.test(password)) {
            showError('pwdtxt', 'Password must have uppercase, special character!');
            noError = false;
        } else {
            hideError('pwdtxt');
        }


        //Repassword
        if (!repassword) {
            showError('re_pwdtxt', 'Please confirm your password!');
            noError = false;
        } else if (repassword !== password) {
            showError('re_pwdtxt', 'The confirm password does not matches!');
            noError = false;
        } else {
            hideError('re_pwdtxt');
        }

        if (!noError) {
            event.preventDefault();
        }
    });
});



function validateEmail() {
    var email = getValueById('emailtxt');
    document.getElementById("ErrorEmailExist").innerHTML = "";
    var Check = false;
    if (email !== null && email !== '') {
        $.ajax({
            url: '/Login/CheckEmail',
            type: "POST",
            data: {
                email: email
            },
            async: false,
            success: function (data) {
                // Update DOM elements with retrieved data
                if (data == 'true') {
                    document.getElementById("ErrorEmailExist").innerHTML = "Email already existed!";
                    Check = false;
                } else {
                    document.getElementById("ErrorEmailExist").innerHTML = "";
                    Check = true; // Prevent the form from submitting
                }
            }
        });
    }
    // If you reach here, the form submission will be allowed
    return Check;
}



function validateForm() {
    var username = getValueById('ustxt');
    document.getElementById("ErrorUsernameExist").innerHTML = "";
    var Check = false;
    hideError('ustxt');
    if (username !== null && username !== '') {
        $.ajax({
            url: '/Login/CheckUsername',
            type: "POST",
            data: {
                username: username
            },
            async: false,
            success: function (data) {
                if (data == 'true') {
                    document.getElementById("ErrorUsernameExist").innerHTML = "Username already existed!";
                    Check = false; // Prevent the form from submitting
                } else {
                    // Registration logic here if username is available                   
                    document.getElementById("ErrorUsernameExist").innerHTML = "";
                    Check = true; // Prevent the form from submitting
                    
                }
            }
        });
    }
    // If you reach here, the form submission will be allowed
    return Check;
} 

function Validate() {
    var isFormValid = validateForm();
    var isEmailValid = validateEmail();

    // Combine the results of both validations using logical AND (&&)
    return isFormValid && isEmailValid;
}


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

function isValidPhoneNumber(phoneNum) {
    const phonePattern = /^0\d{9}$/;
    return phonePattern.test(phoneNum);
}

// Kiểm tra tính hợp lệ của địa chỉ email
function isValidEmail(email) {
    const emailPattern = /^[a-zA-Z][a-zA-Z0-9._%+-]+@[^\s@]+\.[^\s@]{2,}$/;
    return emailPattern.test(email);
}

