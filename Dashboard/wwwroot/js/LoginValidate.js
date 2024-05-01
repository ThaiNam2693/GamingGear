function CheckUsernameAndPassword() {
    var Check = true;
    var Username = document.getElementById("username").value.trim();
    var Password = document.getElementById("password").value.trim();

    if (Username != "" && Username.length < 50) {
        document.getElementById("ErrorUsername").innerHTML = "";
    } else {
        Check = false;
        document.getElementById("ErrorUsername").innerHTML = "Username must be from 8 to 20 characters!";
    }

    if (Password != "" && Password.length < 50) {
        document.getElementById("ErrorPassword").innerHTML = "";
    } else {
        Check = false;
        document.getElementById("ErrorPassword").innerHTML = "Password must be from 8 to 20 characters!";
    }

    $.ajax({
        url: '/Login/CheckUsername',
        type: "POST",
        data: {
            username: Username,
            password: Password
        },
        async: false,
        success: function (data) {
            if (data == 'false') {
                Check = false;
                document.getElementById("ErrorPassword").innerHTML = "Username or Password is wrong or not existed!";
            } else {
            }
        }
    });
    return Check;
}