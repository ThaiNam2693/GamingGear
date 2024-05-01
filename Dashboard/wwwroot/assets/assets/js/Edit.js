function EditBrand(element) {
    var brandID = element.getAttribute("data-brandedit");

    // Check if brandID is not null or empty
    if (!brandID) {
        console.error('Brand ID is invalid.');
        return;
    }

    $.ajax({
        url: '/Dashboard/GetBrandInfo',
        type: "POST",
        data: {
            brand_id: brandID
        },
        dataType: 'json',
        success: function (data) {
            // Update DOM elements with retrieved data
            $('#Brand_Name_edit').val(data.brand_name);
            $('#Brand_Image_tmp').val(data.brand_img);
            $('#Brand_ID_edit').val(data.brand_id);
            $('#brand_img_edit').attr('src', data.brand_img);
            $('#contactFormBrandEdit').fadeToggle();

        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', textStatus, errorThrown);
        }
    });
}


function EditCat(element) {
    var cateID = element.getAttribute("data-CatEdit");

    // Check if brandID is not null or empty
    if (!cateID) {
        console.error('Category ID is invalid.');
        return;
    }

    $.ajax({
        url: '/Dashboard/GetCategoryInfo',
        type: "POST",
        data: {
            cate_id: cateID
        },
        dataType: 'json',
        success: function (data) {
            // Update DOM elements with retrieved data
            $('#Cat_Name_edit').val(data.cate_name);
            $('#Cat_ID_edit').val(data.cate_id);
            $('#contactFormEditCategory').fadeToggle();

        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', textStatus, errorThrown);
        }
    });
}
