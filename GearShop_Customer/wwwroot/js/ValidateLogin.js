$(document).ready(function () {
    $('form.LoginFunction').submit(function (event) {
        var username = getValueById('typeEmailX-2');
        var password = getValueById('typePasswordX-2');

        var noError = true;

        //tài khoản
        if (!username) {
            showError('typeEmailX-2', 'Please enter username');
            noError = false;
        } else if (username.length > 20) {
            showError('typeEmailX-2', 'Username can not more than 20 characters');
            noError = false;
        } else {
            hideError('typeEmailX-2');
        }




        //Password
        if (!password) {
            showError('typePasswordX-2', 'Please enter password');
            noError = false;
        } else if (password.length < 0) {
            showError('typePasswordX-2', 'Password must be at least 8 characters');
            noError = false;
        } else {
            hideError('typePasswordX-2');
        }

        // Nếu không có lỗi, kiểm tra sự tồn tại của username bằng cách gửi request AJAX đến server
        if (!noError) {
            event.preventDefault();
        }


    });
});

$('#typeEmailX-2').on('blur', function () {
    var username = getValueById('typeEmailX-2');
    if (username !== null && username.trim().length > 0) {
        $.ajax({
            url: '/Login/CheckUsername',
            type: "POST",
            data: {
                username: username
            },
            success: function (data) {
                // Update DOM elements with retrieved data
                if (data == 'false') {
                    $('#BtnSubmitLogin').css('pointer-events', 'none');
                    showError('typeEmailX-2', 'Username does not exist');
                } else {
                    $('#BtnSubmitLogin').css('pointer-events', 'auto');
                    hideError('typeEmailX-2');
                }
            }
        });
    }

})

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
