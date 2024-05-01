const search = () => {
    const searchbox = document.getElementById("search-item").value.toUpperCase();
    const storeitems = document.getElementById("product-list");
    const product = document.querySelectorAll(".product");
    const pname = storeitems.getElementsByTagName("h2");

    let hasMatchingProduct = false; // Biến để kiểm tra xem có kết quả tìm kiếm nào không
    if (searchbox.length >= 3) {
        $.ajax({
            url: '/Home/SearchItem',
            type: "POST",
            data: {
                searchbox: searchbox
            },
            success: function (data) {
                var productlist = $('#product-list');
                productlist.empty();

                $.each(data, function (index, product) {
                    var url = '/Product/ShopDetail?ProId=' + product.proId;
                    var productDiv = $('<div class="product" onclick="location.href=\'' + url + '\'">');
                    productDiv.append('<img src="' + product.img[0] + '" alt="">');
                    var pDetailsDiv = $('<div class="p-details">');
                    pDetailsDiv.append('<h2>' + product.proName + '</h2>');
                    if (product.discountPercent > 0) {
                        var priceDiscount = product.proPrice - (product.proPrice * product.discountPercent) / 100;

                        pDetailsDiv.append('<h3>$' + priceDiscount + ' <span class="text-muted ml-2"><del>$' + product.proPrice + '</del></span></h3>');
                    } else {
                        pDetailsDiv.append('<h3>$' + product.proPrice + '</h3>');
                    }

                    productDiv.append(pDetailsDiv);
                    productlist.append(productDiv);
                    storeitems.style.display = "block";
                });
                var seeDetailsDiv = $('<div class="see-details" onclick="location.href=\'/Product/ShopSearch?searchTerm=' + searchbox + '\'">');
                var dividerDiv = $('<div class="divider"></div>');
                var brElement = $('<br>');
                var seeMoreDetails = $('<p>See more details</p> </div>');
                seeDetailsDiv.append(dividerDiv);
                seeDetailsDiv.append(brElement);
                seeDetailsDiv.append(seeMoreDetails);
                productlist.append(seeDetailsDiv);
                storeitems.style.display = "block";
            }
        });
    }
}

