$('form.update_address_form').submit(function (event) {
        var fullname = getValueById('fullname_update');
        var phone = getValueById('phonenum_update');
        var address = getValueById('address_update');

        var noError = true;

        
        if (!fullname) {
            showError('fullname_update', 'Please enter username');
            noError = false;
        } else if (fullname.length >100) {
            showError('fullname_update', 'You full name must be less than 100 characters!');
            noError = false;
        } else {
            hideError('fullname_update');
        }


        if (!phone) {
            showError('phonenum_update', 'Please enter your phone number!');
            noError = false;
        } else if (!isValidPhoneNumber(phone)) {
            showError('phonenum_update', 'Invalid phone number!');
            noError = false;
        } else {
            hideError('phonenum_update');
        }

       
        if (!address) {
            showError('address_update', 'Please enter address');
            noError = false;
        } else if (address.length < 0) {
            showError('address_update', 'Address must be more than 20 character');
            noError = false;
        } else {
            hideError('address_update');
        }

        // Nếu không có lỗi, kiểm tra sự tồn tại của username bằng cách gửi request AJAX đến server
        if (!noError) {
            event.preventDefault();
        }


});

$('form.add_address_form').submit(function (event) {
    var fullname = getValueById('fullname_address_add');
    var phone = getValueById('phone_address_add');
    var address = getValueById('address_address_add');

    var noError = true;


    if (!fullname) {
        showError('fullname_address_add', 'Please enter username');
        noError = false;
    } else if (fullname.length > 100) {
        showError('fullname_address_add', 'You full name must be less than 100 characters!');
        noError = false;
    } else {
        hideError('fullname_address_add');
    }


    if (!phone) {
        showError('phone_address_add', 'Please enter your phone number!');
        noError = false;
    } else if (!isValidPhoneNumber(phone)) {
        showError('phone_address_add', 'Invalid phone number!');
        noError = false;
    } else {
        hideError('phone_address_add');
    }


    if (!address) {
        showError('address_address_add', 'Please enter address');
        noError = false;
    } else if (address.length < 0) {
        showError('address_address_add', 'Address must be more than 20 character');
        noError = false;
    } else {
        hideError('address_address_add');
    }

    // Nếu không có lỗi, kiểm tra sự tồn tại của username bằng cách gửi request AJAX đến server
    if (!noError) {
        event.preventDefault();
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

function isValidPhoneNumber(phoneNum) {
    const phonePattern = /^0\d{9}$/;
    return phonePattern.test(phoneNum);
}