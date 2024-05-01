
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
function ValidateEmail() {
    let noError = true;
    var Check = true;
    var email = $('#Email').val();

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
        $.ajax({
            url: '/Dashboard/CheckEmailUpdate',
            type: "POST",
            data: {
                email: email
            },
            async: false,
            success: function (data) {
                // Update DOM elements with retrieved data    
                if (data === 'Existed') {
                    noError = false;
                    $('#emailexist').text('Email already existed!');
                } else {
                    $('#emailexist').text('');
                }
                
            }

        });
    
    var fullname = getValueById('Fullname');
    var phone = getValueById('PhoneNumber');
    var SSN = getValueById('SSN');
    var address = getValueById('LivingAddress');



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
    return noError;
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
// Kiểm tra tính hợp lệ của số điện thoại

