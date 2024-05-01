function EditProductValidate() {
        var pro_name = getValueById('pro_name');
        var pro_price = getValueById('pro_price');
        var pro_discount = getValueById('pro_discount');
        var noError = true;

        // Kiểm tra pro_name không được để trống
        if (!pro_name) {
            showError('pro_name', 'Please insert Product Name!');
            noError = false;
        } else if (pro_name.length > 100) {
            showError('pro_name', 'Product Name can not be more than 100 characters!');
            noError = false;
        } else {
            hideError('pro_name');
    }

    const numberRegex = /^\d+$/;
        if (!pro_price) {
            showError('pro_price', 'Please insert Product Price');
            noError = false;
        } else {
            // Use a regular expression to check if pro_price contains only numbers
            
            if (!numberRegex.test(pro_price)) {
                showError('pro_price', 'Product Price must be a number!');
                noError = false;
            } else {
                hideError('pro_price');
            }
        }

        if (!pro_discount) {
            showError('pro_discount', 'Please insert Product Discount');
            noError = false;
        } else {
            // Use a regular expression to check if pro_price contains only numbers
            if (!numberRegex.test(pro_discount)) {
                showError('pro_discount', 'Discount must be a number!');
                noError = false;
            } else {
                if (pro_discount < 0 || pro_discount > 100) {
                    showError('pro_discount', 'Discount must be between 0 and 100');
                    noError = false;
                } else {
                    hideError('pro_discount');
                }
            }
        }

        // Kiểm tra các input file
        var validExtensions = ['png', 'jpg', 'jpeg'];

        $('.imgFile').each(function () {
            var inputFile = $(this)[0].files[0];
            var errorParagraph = $(this).next('p');

            // Kiểm tra nếu không có file được chọn
            if (!inputFile) {
                errorParagraph.text('Please insert a picture!');
                noError = false;
            } else {
                var fileName = inputFile.name.toLowerCase();
                var fileExtension = fileName.split('.').pop();

                // Kiểm tra đuôi file
                if (!validExtensions.includes(fileExtension)) {
                    errorParagraph.text('file have to be either .png, .jpg or .jpeg!');
                    noError = false;
                } else {
                    errorParagraph.text('');  // Xóa thông báo lỗi nếu hợp lệ
                }
            }
        });

    $('.imgFileExisted').each(function () {
        var inputFile = $(this)[0].files[0];
        var errorParagraph = $(this).next('p');

        // Kiểm tra nếu không có file được chọn
        if (!inputFile) {
        } else {
            var fileName = inputFile.name.toLowerCase();
            var fileExtension = fileName.split('.').pop();

            // Kiểm tra đuôi file
            if (!validExtensions.includes(fileExtension)) {
                errorParagraph.text('file have to be either .png, .jpg or .jpeg!');
                noError = false;
            } else {
                errorParagraph.text('');  // Xóa thông báo lỗi nếu hợp lệ
            }
        }
        
    });

        $('.feature').each(function () {
            var feature = $(this).val().trim();
            var errorParagraph = $(this).next('p');

            // Kiểm tra nếu không có file được chọn
            if (!feature) {
                errorParagraph.text('Please insert attribute description!');
                noError = false;
            } else if (feature.length > 1000) {
                errorParagraph.text('Attribute description can not be over 1000 characters');
                noError = false;
            } else {
                errorParagraph.text('');  // Xóa thông báo lỗi nếu hợp lệ
            }
        });

        $('.description').each(function () {
            var description = $(this).val().trim();

            var errorParagraph = $(this).next('p');
            // Kiểm tra nếu không có file được chọn
            if (!description) {
                errorParagraph.text('Please insert attribute!');
                noError = false;
            } else if (description.length > 100) {
                errorParagraph.text('Attribute description can not be over 1000 characters');
                noError = false;
            } else {
                errorParagraph.text('');  // Xóa thông báo lỗi nếu hợp lệ
            }
        });

        var pro_des = getValueById('pro_des');
        if (!pro_des) {
            showError('pro_des', 'Please insert description!');
            noError = false;
        } else if (pro_des.length > 5000) {
            showError('pro_des', 'Description can not be over 1000 characters!');
            noError = false;
        } else {
            hideError('pro_des');
        }

        // Nếu các kiểm tra đều thành công, tiếp tục submit form
        if (!noError) {
            event.preventDefault();
            // Để form tiếp tục submit, không cần event.preventDefault();
        }
   
}
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