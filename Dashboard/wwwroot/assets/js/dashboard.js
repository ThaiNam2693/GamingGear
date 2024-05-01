$(function () {
    //======================================
    //Show All
    //======================================
    var recieveIncome = $("#chart_area_data").attr('data-Toalprice');

    var recievePayment = $("#chart_area_data").attr('data-totalPayment');

    var recievecategories = $("#chart_area_data").attr('data-categoriescombide');


    var income = recieveIncome.split(',').map(function (item) {
        return parseInt(item.trim(), 10);
    });

    var payment = recievePayment.split(',').map(function (item) {
        return parseInt(item.trim(), 10);
    });
    var category = recievecategories.split(',').map(function (name) {
        return name.trim(); // Đặt single quotes cho mỗi phần tử
    });
    var chart_area = {
        series: [{
            name: 'Income',
            data: income
        }, {
            name: 'Payment',
            data: payment
        }],
        chart: {
            height: 350,
            type: 'area'
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'smooth'
        },
        xaxis: {
            categories: category
        },

    };

    var chart = new ApexCharts(document.querySelector("#chart_area"), chart_area);
    chart.render();

});
$("#btn_change2").on('click', function () {
    $("#chart").addClass("d-none");
    $("#chart_area").removeClass("d-none");

    var recieveIncome = $("#chart_area_data").attr('data-Toalprice');

    var recievePayment = $("#chart_area_data").attr('data-totalPayment');

    var recievecategories = $("#chart_area_data").attr('data-categoriescombide');

    var income = recieveIncome.split(',').map(function (item) {
        return parseInt(item.trim(), 10);
    });

    var payment = recievePayment.split(',').map(function (item) {
        return parseInt(item.trim(), 10);
    });

    var category = recievecategories.split(',').map(function (name) {
        return name.trim(); // Đặt single quotes cho mỗi phần tử
    });
    var chart_area = {
        series: [{
            name: 'Income',
            data: income
        }, {
            name: 'Payment',
            data: payment
        }],
        chart: {
            height: 350,
            type: 'area'
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'smooth'
        },
        xaxis: {
            categories: category
        },

    };

    var chart = new ApexCharts(document.querySelector("#chart_area"), chart_area);
    chart.render();
});


$("#btn_change").on('click', function () {
    $("#chart_area").addClass("d-none");
    $("#chart").removeClass("d-none");

    var recieveIncome = $("#chart_area_data").attr('data-Toalprice');

    var recievePayment = $("#chart_area_data").attr('data-totalPayment');

    var recieveRevenue = $("#chart_area_data").attr('data-totalRevenue');

    var income = recieveIncome.split(',').map(function (item) {
        return parseInt(item.trim(), 10);
    });

    var payment = recievePayment.split(',').map(function (item) {
        return parseInt(item.trim(), 10);
    });

    var revenue = recieveRevenue.split(',').map(function (item) {
        return parseInt(item.trim(), 10);
    });
    var options = {

        

        series: [{  
            name: 'Income',
            type: 'column',
            data: income
        }, {
            name: 'Cashflow',
            type: 'column',
            data: payment
        }, {
            name: 'Revenue',
            type: 'line',
            data: revenue
        }],
        chart: {
            height: 350,
            type: 'line',
            stacked: false
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            width: [1, 1, 4]
        },

        xaxis: {
            categories: [1, 2, 3, 4, 5, 6, 7, 8, 9 ,10 , 11, 12],
        },
        yaxis: [
            {
                axisTicks: {
                    show: true,
                },
                axisBorder: {
                    show: true,
                    color: '#008FFB'
                },
                labels: {
                    style: {
                        colors: '#008FFB',
                    }
                },
                title: {
                    text: "Income (thousand crores)",
                    style: {
                        color: '#008FFB',
                    }
                },
                tooltip: {
                    enabled: true
                }
            },
            {
                seriesName: 'Income',
                opposite: true,
                axisTicks: {
                    show: true,
                },
                axisBorder: {
                    show: true,
                    color: '#00E396'
                },
                labels: {
                    style: {
                        colors: '#00E396',
                    }
                },
                title: {
                    text: "Operating Cashflow (thousand crores)",
                    style: {
                        color: '#00E396',
                    }
                },
            },
            {
                seriesName: 'Revenue',
                opposite: true,
                axisTicks: {
                    show: true,
                },
                axisBorder: {
                    show: true,
                    color: '#FEB019'
                },
                labels: {
                    style: {
                        colors: '#FEB019',
                    },
                },
                title: {
                    text: "Revenue (thousand crores)",
                    style: {
                        color: '#FEB019',
                    }
                }
            },
        ],
        tooltip: {
            fixed: {
                enabled: true,
                position: 'topLeft', // topRight, topLeft, bottomRight, bottomLeft
                offsetY: 30,
                offsetX: 60
            },
        },
        legend: {
            horizontalAlign: 'left',
            offsetX: 40
        }
    };

    var chart = new ApexCharts(document.querySelector("#chart"), options);
    chart.render();
});
var recieveValue = $("#categoryName_sta").attr('data-cateName');
var recieveQuan = $("#categoryName_sta").attr('data-quantity');
// Chuyển chuỗi thành mảng bằng cách tách nó bằng dấu phẩy
var productNames = recieveValue.split(',');


// Sử dụng mảng mới để cập nhật labels và đặt single quotes cho mỗi phần tử
productNames = productNames.map(function (name) {
    return name.trim(); // Đặt single quotes cho mỗi phần tử
});

var quantity = recieveQuan.split(',').map(function (item) {
    return parseInt(item.trim(), 10);
});
console.log(quantity);

console.log(productNames);


var PieChart = {
    series: quantity,
    chart: {
        width: 500,
        type: 'pie',
    },
    labels: productNames,
    responsive: [{
        breakpoint: 480,
        options: {
            chart: {
                width: 200
            },
            legend: {
                position: 'bottom'
            }
        }
    }]
};

var chart = new ApexCharts(document.querySelector("#PieChart"), PieChart);
chart.render();
