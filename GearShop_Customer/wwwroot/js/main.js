(function ($) {
    "use strict";

    // Dropdown on mouse hover
    $(document).ready(function () {
        function toggleNavbarMethod() {
            if ($(window).width() > 992) {
                $('.navbar .dropdown').on('mouseover', function () {
                    $('.dropdown-toggle', this).trigger('click');
                }).on('mouseout', function () {
                    $('.dropdown-toggle', this).trigger('click').blur();
                });
            } else {
                $('.navbar .dropdown').off('mouseover').off('mouseout');
            }
        }
        toggleNavbarMethod();
        $(window).resize(toggleNavbarMethod);
    });


    // Back to top button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
        return false;
    });


    // Vendor carousel
    $('.vendor-carousel').owlCarousel({
        loop: true,
        margin: 29,
        nav: false,
        autoplay: true,
        smartSpeed: 1000,
        responsive: {
            0: {
                items: 2
            },
            576: {
                items: 3
            },
            768: {
                items: 4
            },
            992: {
                items: 5
            },
            1200: {
                items: 6
            }
        }
    });


    // Related carousel
    $('.related-carousel').owlCarousel({
        loop: true,
        margin: 29,
        nav: false,
        autoplay: true,
        smartSpeed: 1000,
        responsive: {
            0: {
                items: 1
            },
            576: {
                items: 2
            },
            768: {
                items: 3
            },
            992: {
                items: 4
            }
        }
    });


    // Product Quantity
    $('.quantity button').on('click', function () {
        var button = $(this);
        var oldValue = parseInt(button.parent().parent().find('input').val());
        var pro_quan_available = document.querySelector('.number-product').getAttribute('data-product_quan');
        if (button.hasClass('btn-plus')) {
            if (oldValue < pro_quan_available) {
                var newVal = parseInt(parseInt(oldValue) + 1);
            } else {
                var newVal = parseInt(pro_quan_available);
            }
        } else {
            if (oldValue > 1) {
                var newVal = parseInt(parseInt(oldValue) - 1);
            } else {
                newVal = 1;
            }
        }

        var cart_quan = parseInt($('#shopDetail_quan').attr('data-cart_quan_current'))

        if ((newVal + cart_quan) > parseInt(pro_quan_available)) {
            newVal = parseInt(pro_quan_available - cart_quan);
        }

        button.parent().parent().find('input').val(newVal);

        $('#shopDetail_quan').attr('data-quan_input', newVal);
    });

    $('#quan_input').on('blur', function () {
        var pro_quan_available = document.querySelector('.number-product').getAttribute('data-product_quan');

        var cart_quan = parseInt($('#shopDetail_quan').attr('data-cart_quan_current'))
        var quan = parseInt($('#quan_input').val());
        $('#shopDetail_quan').attr('data-quan_input', $('#quan_input').val());

        if ((quan + cart_quan) > pro_quan_available) {
            var total = pro_quan_available - cart_quan;
            $('#quan_input').val(total);
            $('#shopDetail_quan').attr('data-quan_input', total);
        }

        if (isNaN(quan)) {
            $('#quan_input').val(1);
            $('#shopDetail_quan').attr('data-quan_input', 1);
        }
    });

})(jQuery);

function isValidInput(event) {
    if ((event.keyCode >= 49 && event.keyCode <= 57) ||  // Số từ 1 đến 9
        (event.keyCode == 48 && event.target.selectionStart !== 0) ||  // Số 0 không ở đầu
        event.keyCode == 8) {  // Phím xóa
        return true;
    } else {
        return false;
    }
}