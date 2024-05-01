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
            $('#Brand_Name_edit').val(data.brandName);
            $('#Brand_Image_tmp').val(data.brandLogo);
            $('#Brand_ID_edit').val(data.brandId);
            $('#brand_img_edit').attr('src', data.brandLogo);
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
            $('#Cat_Name_edit').val(data.catName);
            $('#Cat_ID_edit').val(data.catId);
            $('#contactFormEditCategory').fadeToggle();

        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', textStatus, errorThrown);
        }
    });
}


function ShowIR(element) {
    var IRid = element.getAttribute("data-ViewIR");

    // Check if brandID is not null or empty
    if (!IRid) {
        console.error('Receipt ID is invalid.');
        return;
    }
    
    $.ajax({
        url: '/Dashboard/GetIRInfo',
        type: "POST",
        data: {
            ID: IRid
        },
        dataType: 'json',
        success: function (data) {
            $('#IR_Name').html(''); // Clear the Name element
            $('#IR_ID').html('');   // Clear the ID element
            $('#IR_Date').html(''); // Clear the Date element
            $('#IR_Payment').html(''); // Clear the Payment element

            // Update DOM elements with retrieved data
            $('#IR_Name').html(data.person_In_Charge );
            $('#IR_ID').html(data.reciept_ID);
            var rawDate = new Date(data.date_Import);

            // Format the date as "MM/DD/YYYY"
            var formattedDate = (rawDate.getMonth() + 1) + '/' + rawDate.getDate() + '/' + rawDate.getFullYear();

            // Update the HTML element with the formatted date
            $('#IR_Date').html(formattedDate);
            $('#IR_Payment').html('$' + data.payment);
            

        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', textStatus, errorThrown);
        }
    });
}

function ShowRP(element) {
    var IRid = element.getAttribute("data-ViewIR");

    // Check if brandID is not null or empty
    if (!IRid) {
        console.error('Receipt ID is invalid.');
        return;
    }

    $.ajax({
        url: '/Dashboard/GetRPInfo',
        type: "POST",
        data: {
            ID: IRid
        },
        dataType: 'json',
        success: function (data) {
            var tableBody = $('#receiptTable tbody');
            tableBody.empty();
            for (var i = 0; i < data.length; i++) {
                var receipt = data[i];

                // Create a new row for the receipt data
                var newRow = $('<tr>');

                newRow.append($('<td>').text(i+1));
                // Append columns for Name, ID, Date, and Payment
                newRow.append($('<td>').text(receipt.pro_name));
                // Append a dollar sign "$" in front of the payment
                newRow.append($('<td>').text('$' + receipt.price));
                // Format the date as "MM/DD/YYYY"
                newRow.append($('<td>').text(receipt.amount));

               

                // Append the row to the table body
                tableBody.append(newRow);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', textStatus, errorThrown);
        }
    });
}

function ShowStaff(element) {
    var StaffID = element.getAttribute("data-employeeid");

    if (!StaffID) {
        console.error('Receipt ID is invalid.');
        return;
    }

    $.ajax({
        url: '/Dashboard/GetStaffInfo',
        type: "POST",
        data: {
            ID: StaffID
        },
        dataType: 'json',
        success: function (data) {
            $('#Staff_Name').html('');
            $('#Staff_PhoneNum').html('');
            $('#Staff_Email').html('');
            $('#Staff_SSN').html('');
            $('#Staff_LivingAddress').html('');
            $('#Staff_Username').html('');

            $('#Staff_Name').html(data.fullname);
            $('#Staff_PhoneNum').html(data.phone);
            $('#Staff_Email').html(data.email);
            $('#Staff_SSN').html(data.ssn);
            $('#Staff_LivingAddress').html(data.address);
            $('#Staff_Username').html(data.username);
            var modal = document.getElementById("myModal");            
            modal.style.display = "block";
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', textStatus, errorThrown);
        }
    });
    var span = document.getElementsByClassName("close")[0];
    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = "none";
    }

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
}

function GetOrderReceipt(element) {
    var OrderID = element.getAttribute("data-orderid");
    if (!OrderID) {
        console.error('Order ID is invalid.');
        return;
    }

    $.ajax({
        url: '/Dashboard/GetOrderInfo',
        type: "POST",
        data: {
            ID: OrderID
        },
        dataType: 'json',
        success: function (data) {
            $('#Order_ID').html('');
            $('#Created_Date').html('');
            $('#Shipped_Date').html('');
            $('#Name').html('');
            $('#PhoneNumber').html('');
            $('#Email').html('');
            $('#Address').html('');
            $('#Staff_Name').html('');
            $('#Description').html('');
            $('#TotalPrice').html('');

            $('#Order_ID').html(data.order.orderId);
            $('#TotalPrice').html("$" + data.order.totalPrice);
            var dateString = data.order.startDate;

            // Parse the date and time
            var date = new Date(dateString);

            // Format the date as 'dd/mm/yyyy'
            var day = date.getDate();
            var month = date.getMonth() + 1; // Months are 0-based
            var year = date.getFullYear();

            // Ensure single-digit day and month are formatted with leading zeros
            if (day < 10) {
                day = "0" + day;
            }
            if (month < 10) {
                month = "0" + month;
            }

            var formattedDate = day + "/" + month + "/" + year; // Result: '29/10/2023'


            $('#Created_Date').html(formattedDate);

            var dateString2 = data.order.endDate;

            // Parse the date and time
            var date2 = new Date(dateString2);

            // Format the date as 'dd/mm/yyyy'
            var day2 = date2.getDate();
            var month2 = date2.getMonth() + 1; // Months are 0-based
            var year2 = date2.getFullYear();

            // Ensure single-digit day and month are formatted with leading zeros
            if (day2 < 10) {
                day2 = "0" + day2;
            }
            if (month2 < 10) {
                month2 = "0" + month2;
            }

            var formattedDate2 = day2 + "/" + month2 + "/" + year2; // Result: '29/10/2023'
            if (data.order.endDate === null) {
                $('#Shipped_Date').html('To be Decided');
            } else {
                $('#Shipped_Date').html(formattedDate2);
            }


            $('#Name').html(data.address.fullname);
            $('#PhoneNumber').html(data.address.phoneNum);
            $('#Email').html(data.email);
            $('#Address').html(data.address.address);
            if (data.order.staffId === null) {
                $('#Staff_Name').html('To be Decided');
            } else {
                $('#Staff_Name').html(data.staff.fullname);
            }

            $('#Description').html(data.order.description);

            var table = document.getElementById("OrderReceipt"); 
            var rowsToRemove = table.querySelectorAll(".item");

            rowsToRemove.forEach(function (row) {
                row.remove();
            });

            // Create a new table row (tr) with the "item" class
            // Define the number of rows you want to create
            var numRowsToCreate = data.orderDetail.length;

            for (var i = 0; i < numRowsToCreate; i++) {
                var receipt = data.orderDetail[i];
                // Create a new table row (tr) with the "item" class
                var newRow = document.createElement("tr");
                newRow.className = "item";
                // Create three table cells (td) for each column
                var cell1 = document.createElement("td");
                var cell2 = document.createElement("td");
                var cell3 = document.createElement("td");

                // Set the text content of the cells
                cell1.textContent = receipt.proName;
                cell2.textContent = receipt.quantity;
                cell3.textContent = "$" +receipt.price;

                // Append the cells to the row
                newRow.appendChild(cell1);
                newRow.appendChild(cell2);
                newRow.appendChild(cell3);

                // Find the "total" row
                var shippingRow = document.querySelector(".shipping");

                // Insert the new row before the "total" row
                shippingRow.parentNode.insertBefore(newRow, shippingRow);
            }

            var order_status = data.order.status; // Replace with the actual order_status value

            var buttonsContainer = document.getElementById("buttonsContainer");

            // Clear the content of the buttonsContainer
            buttonsContainer.innerHTML = '';

            // Check the order_status and append anchor tags accordingly
            if (order_status === 1) {
                // Append "Accept Order" and "Cancel Order" anchor tags
                var acceptLink = document.createElement("a");
                acceptLink.className = "btn btn-primary";
                acceptLink.textContent = "Accept Order";
                acceptLink.href = "/Dashboard/AcceptOrder/" + data.order.orderId;
                acceptLink.style.marginBottom = "10px"; // Add padding to the link


                var cancelLink = document.createElement("a");
                cancelLink.className = "btn btn-danger";
                cancelLink.textContent = "Cancel Order";
                cancelLink.href = "/Dashboard/CancelOrder/" + data.order.orderId;

                buttonsContainer.appendChild(acceptLink);
                buttonsContainer.appendChild(cancelLink);
            } else if (order_status === 3) {
                // Append "Complete Order" anchor tag
                var completeLink = document.createElement("a");
                completeLink.className = "btn btn-primary";
                completeLink.textContent = "Complete Order";
                completeLink.href = "/Dashboard/CompletedOrder/" + data.order.orderId;

                buttonsContainer.appendChild(completeLink);
            } else if (order_status === 2) {
                var AcceptedLink = document.createElement("a");
                AcceptedLink.className = "btn btn-primary";
                AcceptedLink.textContent = "Shipped Order";
                AcceptedLink.href = "/Dashboard/ShippedOrder/" + data.order.orderId;
                buttonsContainer.appendChild(AcceptedLink);
            }




            var modal = document.getElementById("myModal");
            modal.style.display = "block";
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', textStatus, errorThrown);
        }
    });
    var span = document.getElementsByClassName("close")[0];
    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = "none";
    }

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
}

function CheckCategoryAdd() {
    event.preventDefault();
    document.getElementById("ErrorCategoryName").innerHTML = "";
    document.getElementById("ErrorCategoryKeyword").innerHTML = "";
    var Name = document.getElementById("Cat_Name").value.trim();
    var Keyword = document.getElementById("keyword").value.trim();
    var Check = true;

    if (Name != "" && Name.length < 50) {
        // Continue with other checks
    } else {
        Check = false;
        document.getElementById("ErrorCategoryName").innerHTML = "Invalid Category Name!";
    }

    if (Keyword != null && Keyword.length == 2) {
        // Continue with other checks
    } else {
        Check = false;
        document.getElementById("ErrorCategoryKeyword").innerHTML = "Keyword must be 2 characters long!";
    }

    // Call CheckKeyWordExisted and pass a callback function
    CheckKeyWordExisted(Keyword, function (exists) {
        if (exists) {
            // Keyword exists, handle accordingly
            Check = false;
            document.getElementById("ErrorCategoryKeyword").innerHTML = "Keyword already existed!";
        } else {

        }
    });

    // Return the initial value of Check
    if (Check == true) {
        document.getElementById("AddCate").submit();
    }
}

function CheckKeyWordExisted(keyword, callback) {
    $.ajax({
        url: '/Dashboard/KeywordExisted',
        type: "POST",
        data: {
            keyword: keyword
        },
        async: false,
        success: function (data) {
            if (data == 'Existed') {
                // Keyword exists
                callback(true);
            }
        }
    });
}

function CheckCateAdd() {
    var isOtherValid = CheckCategoryAdd();
    var isKeywordExisted = CheckKeyWordExisted();
    return isOtherValid && isKeywordExisted;
}


function CheckEditCategory() {
    var Check = true;
    var Name = document.getElementById("Cat_Name_edit").value.trim();
    if (Name != "" && Name.length < 50) {
        document.getElementById("ErrorCategoryNameEdit").innerHTML = "";
    } else {
        Check = false;
        document.getElementById("ErrorCategoryNameEdit").innerHTML = "Invalid Category Name!";
    }
    return Check;
}

function CheckBrandAdd() {
    document.getElementById("ErrorBrandName").innerHTML = "";
    document.getElementById("ErrorBrandLogo").innerHTML = "";
    var Name = document.getElementById("Brand_Name").value.trim();
    var inputFile = document.getElementById("Brand_Logo");
    var Check = true;

    if (Name != "" && Name.length < 50) {

    } else {
        Check = false;
        document.getElementById("ErrorBrandName").innerHTML = "Invalid Brand Name!";
    }
    if (inputFile.files.length === 0) {
        document.getElementById("ErrorBrandLogo").innerHTML = "Please insert an image!";
        Check = false; // Prevent form submission
    } else {
        var validExtensions = ["png", "jpg", "jpeg"];
        var fileName = inputFile.files[0].name;
        var fileExtension = fileName.slice(((fileName.lastIndexOf(".") - 1) >>> 0) + 2).toLowerCase();

        if (!validExtensions.includes(fileExtension)) {
            document.getElementById("ErrorBrandLogo").innerHTML = "File must have an extension of .png, .jpg, or .jpeg!";
            event.preventDefault(); // Prevent the default form submission
        }

        // Additional validation logic can go here

        // If all validation passes, the form will be submitted as the default behavior
    }
    return Check;
}

function CheckBrandEdit() {
    var Check = true;
    var Name = document.getElementById("Brand_Name_edit").value.trim();

    if (Name != "" && Name.length < 50) {
        document.getElementById("ErrorBrandNameEdit").innerHTML = "";
    } else {
        Check = false;
        document.getElementById("ErrorBrandNameEdit").innerHTML = "Invalid Brand Name!";
    }

    var errorElement = document.getElementById("ErrorBrandLogoEdit").innerHTML = "";
    var inputFile = document.getElementById("Brand_Image_tmp");

    var validExtensions = ["png", "jpg", "jpeg"];
    $('#Brand_Image_Change').each(function () {
        var inputFile = $(this)[0].files[0];
        // Kiểm tra nếu không có file được chọn
        if (!inputFile) {
        } else {
            var fileName = inputFile.name.toLowerCase();
            var fileExtension = fileName.split('.').pop();

            // Kiểm tra đuôi file
            if (!validExtensions.includes(fileExtension)) {
                errorElement.innerHTML = "File must have an extension of .png, .jpg, or .jpeg!";
                Check = false;
            } else {
                errorElement.innerHTML = "";
            }
        }

    });
    return Check;
}

function ShowIR(element) {
    var IRid = element.getAttribute("data-ViewIR");

    // Check if brandID is not null or empty
    if (!IRid) {
        console.error('Receipt ID is invalid.');
        return;
    }

    $.ajax({
        url: '/Dashboard/GetIRInfo',
        type: "POST",
        data: {
            ID: IRid
        },
        dataType: 'json',
        success: function (data) {
            $('#IR_Name').html(''); // Clear the Name element
            $('#IR_ID').html('');   // Clear the ID element
            $('#IR_Date').html(''); // Clear the Date element
            $('#IR_Payment').html(''); // Clear the Payment element

            // Update DOM elements with retrieved data
            $('#IR_Name').html(data.personInCharge);
            $('#IR_ID').html(data.receiptId);
            var rawDate = new Date(data.dateImport);

            // Format the date as "MM/DD/YYYY"
            var formattedDate = (rawDate.getMonth() + 1) + '/' + rawDate.getDate() + '/' + rawDate.getFullYear();

            // Update the HTML element with the formatted date
            $('#IR_Date').html(formattedDate);
            $('#IR_Payment').html('$' + data.payment);


        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', textStatus, errorThrown);
        }
    });
}

function ShowRP(element) {
    var IRid = element.getAttribute("data-ViewIR");

    // Check if brandID is not null or empty
    if (!IRid) {
        console.error('Receipt ID is invalid.');
        return;
    }

    $.ajax({
        url: '/Dashboard/GetRPInfo',
        type: "POST",
        data: {
            ID: IRid
        },
        dataType: 'json',
        success: function (data) {
            var tableBody = $('#receiptTable tbody');
            tableBody.empty();
            for (var i = 0; i < data.length; i++) {
                var receipt = data[i];

                // Create a new row for the receipt data
                var newRow = $('<tr>');

                newRow.append($('<td>').text(i + 1));
                // Append columns for Name, ID, Date, and Payment
                newRow.append($('<td>').text(receipt.proName));
                // Append a dollar sign "$" in front of the payment
                newRow.append($('<td>').text('$' + receipt.price));
                // Format the date as "MM/DD/YYYY"
                newRow.append($('<td>').text(receipt.amount));



                // Append the row to the table body
                tableBody.append(newRow);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', textStatus, errorThrown);
        }
    });
}