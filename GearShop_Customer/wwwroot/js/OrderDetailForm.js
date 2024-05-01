function openForm(element) {
    var id = element.getAttribute('data-orderId');
    $.ajax({
        url: '/Account/OrderDetail',
        type: "POST",
        data: {
            id: id
        },
        success: function (data) {
            var listOrderD = $('#listOrderDetail');
            listOrderD.empty();
            var totalPrice = 0;
            $.each(data.orderDetails, function (index, item) {
                var htmlCode = '<div class="row">' +
                    '<div class="col-9">' +
                    '<span id="name">' + item.productName + " x " + item.quantity + '</span>' +
                    '</div>' +
                    '<div class="col-3">' +
                    '<span id="price">$' + item.price + '</span>' +
                    '</div>' +
                    '</div>';
                listOrderD.append(htmlCode);
                totalPrice += item.price;
            });

            var dancer = $('#ODDancer');
            dancer.empty();
            var htmlCode = "";
            switch (data.orderDick.status) {
                case 1:
                    htmlCode = '<ul id="progressbar">' +
                        '<li class="step0 active" id="step1">Pending</li>' +
                        '<li class="step0 text-center" id="step2">Accepted</li>' +
                        '<li class="step0 text-right" id="step3">Delivering</li>' +
                        '<li class="step0 text-right" id="step4">Completed</li>' +
                        '</ul>';
                    dancer.append(htmlCode);
                    break;
                case 2:
                    htmlCode = '<ul id="progressbar">' +
                        '<li class="step0 active" id="step1">Pending</li>' +
                        '<li class="step0 active text-center" id="step2">Accepted</li>' +
                        '<li class="step0 text-right" id="step3">Delivering</li>' +
                        '<li class="step0 text-right" id="step4">Completed</li>' +
                        '</ul>';
                    dancer.append(htmlCode);
                    break;
                case 3:
                    htmlCode = '<ul id="progressbar">' +
                        '<li class="step0 active" id="step1">Pending</li>' +
                        '<li class="step0 active text-center" id="step2">Accepted</li>' +
                        '<li class="step0 active text-right" id="step3">Delivering</li>' +
                        '<li class="step0 text-right" id="step4">Completed</li>' +
                        '</ul>';
                    dancer.append(htmlCode);
                    break;
                case 4:
                    htmlCode = '<ul id="progressbar">' +
                        '<li class="step0 active" id="step1">Pending</li>' +
                        '<li class="step0 active text-center" id="step2">Accepted</li>' +
                        '<li class="step0 active text-right" id="step3">Delivering</li>' +
                        '<li class="step0 active text-right" id="step4">Completed</li>' +
                        '</ul>';
                    dancer.append(htmlCode);
                    break;
            }

            totalPrice += 10;
            $('#totalPriceRecipt').empty();
            $('#totalPriceRecipt').append("<big>$" + totalPrice + "</big>");

            $('#orderDateOD').text(data.orderDick.startDay);
            $('#fullnameOD').text(data.addresses.fullname);
            $('#phonenumOD').text(data.addresses.phonenum);
            $('#addressOD').text(data.addresses.address);
        }
    });
    document.getElementById("myForm").style.display = "block";
}

function closeForm() {
    document.getElementById("myForm").style.display = "none";
}

function UpdateAddress(element) {

    var ID = element.getAttribute('data-ID');
    var fullname = element.getAttribute('data-fullname');
    var phonenum = element.getAttribute('data-phonenum');
    var address = element.getAttribute('data-address');
    var retrievedValue = $("#update-address").attr('data-addressID');
    var hasShowClass = $("#update-address").hasClass("show");
    $("#update-address").attr("data-addressID", ID);
    if (ID !== retrievedValue || !hasShowClass) {

        $('#fullname_update').val(fullname);
        $('#phonenum_update').val(phonenum);
        $('#address_update').val(address);
        $('#ID_update').val(ID);
        $("#update-address").removeClass("collapse").addClass("collapsing");
        if ($("#update-address").hasClass("collapsing")) {
            // Thêm style "height" vào phần tử
            $("#update-address").css("height", "292px");
        }
        setTimeout(function () {
            $("#update-address").removeClass("collapsing").addClass("collapse show");
        }, 500);

        if ($("#update-address").hasClass("show")) {
            // Thêm style "height" vào phần tử
            $("#update-address").css("height", "");
        }
    } else {
        $("#update-address").removeClass("collapse show").addClass("collapsing");
            if ($("#update-address").hasClass("collapsing")) {
                // Thêm style "height" vào phần tử
                $("#update-address").css("height", "");
            }
        setTimeout(function () {
            $("#update-address").removeClass("collapsing").addClass("collapse");
        }, 500);
    }



}

function DeleteAddress(element) {
    var id = element.getAttribute('data-ID');
    $.ajax({
        url: '/Account/DeleteAddress',
        type: "POST",
        data: {
            id: id
        },
        success: function (data) {
            window.location.reload();
        }
    });
}
