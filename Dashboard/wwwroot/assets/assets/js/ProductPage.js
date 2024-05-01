$(document).ready(function () {
    // Lắng nghe sự kiện thay đổi của select box
    $("#cate_id").change(function () {
        // Lấy giá trị đã chọn
        var selectedValue = $(this).val();

        $.ajax({
            url: '/Dashboard/GetNewProductID',
            type: "POST",
            data: {
                cate_id: selectedValue
            },
            success: function (data) {
                // Update DOM elements with retrieved data
                $('#pro_id').val(data);

            }
        });
    });
});
