
$('input[type="checkbox"].shop').change(function () {
    filterProducts();
});


function RemoveUsedFilter(element, index) {
    // Assuming your checkboxes have unique IDs like "checkbox1", "checkbox2", etc.
    var cateCheckbox = element.getAttribute("data-cateFilter");
    var BrandCheckbox = element.getAttribute("data-brandFilter");
    var SortCheckbox = element.getAttribute("data-sortFilter");
    element.parentNode.removeChild(element);
    if (cateCheckbox) {
        document.getElementById("cat-" + cateCheckbox).checked = false;
    }
    if (BrandCheckbox) {
        document.getElementById("brand-" + BrandCheckbox).checked = false;
    }
    if (SortCheckbox) {
        document.getElementById("sort-" + SortCheckbox).checked = false;
    }

    filterProducts();
    // Add any other desired functionality for removing the used filter here
}


function Order(element) {
    var order = element.getAttribute("data-order");
    var url = window.location.href;

    // Kiểm tra nếu order === 'standard', loại bỏ tham số 'order' trong URL
    if (order === 'standard') {
        url = url.replace(/([&?])order=[^&]+/, ''); // Loại bỏ tham số 'order'
        url = url.replace(/[?&]$/, ''); // Loại bỏ dấu '?' hoặc '&' cuối cùng nếu có
    } else {
        if (url.includes("order=")) {
            // Nếu URL đã có 'order=highest' thì thay thế nó bằng 'order=lowest'
            url = url.replace('order=highest', 'order=lowest');
            if (order === 'highest') {
                if (url.includes("order=lowest")) {
                    url = url.replace('order=lowest', 'order=highest');
                }
            } else if (order === 'lowest') {
                if (url.includes("order=highest")) {
                    url = url.replace('order=highest', 'order=lowest');
                }
            }
        } else if (order.length > 0) {
            // Nếu không có 'order=highest' trong URL, thì kiểm tra xem có '?' trong URL chưa
            url += (url.includes("?") ? "&" : "?") + 'order=' + order;
        }
    }

    window.location.href = url;
}




function filterProducts() {
    var selectedCategories = $('input[id^=cat]:checked').map(function () {
        return this.value;
    }).get();

    var selectedBrands = $('input[id^="brand"]:checked').map(function () {
        return this.value;
    }).get();

    var selectedSort = $('input[id^=sort]:checked').map(function () {
        return this.value;
    }).get();
    // Prepare the URL with selected categories and brands
    console.log(selectedCategories);
    var url = '/Product/Shop';

    if (selectedCategories.length > 0) {
        url += (url.includes("?") ? "&" : "?") + 'category=' + selectedCategories.join(',');
    }

    if (selectedBrands.length > 0) {
        url += (url.includes("?") ? "&" : "?") + 'brand=' + selectedBrands.join(',');
    }
    if (selectedSort.length > 0) {
        url += (url.includes("?") ? "&" : "?") + 'sort=' + selectedSort.join(',');
    }
    window.location.href = url;
}