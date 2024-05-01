$(document).ready(function () {
    $('form.CreateAccount').submit(function (event) {
        var Username, Password, Fullname, PhoneNumber, Email, SSN, LivingAddress;
        Username = getValueById('Username');
        Password = getValueById('Password');
        Fullname = getValueById('Fullname');        
        PhoneNumber = getValueById('PhoneNumber');
        Email = getValueById('Email');
        SSN = getValueById('SSN');
        LivingAddress = getValueById('LivingAddress');

        var noError = true; 


        //Username
        if (!Username) {
            showError('Username', 'Username Required!');
            noError = false;
        } else if (Username.length > 50) {
            showError('Username', 'Username Can not be longer than 50 characters! ');
            noError = false;
        } else {
            hideError('Username');
        }


        //Password
        var uppercaseRegex = /[A-Z]/;
        var specialCharRegex = /[!@#$%^&*()_+[\]{};':"\\|,.<>/?]+/;
        if (Password.length > 8 && uppercaseRegex.test(Password) && specialCharRegex.test(Password)) {
            showError('Password', 'Password must have at least 8 characters, 1 uppercase letter, and 1 special character!');
            noError = false;
        } 

        //Fullname
        if (!Fullname && Fullname.length > 100) {
            showError('Fullname', 'Fullname can not be blank or longer than 100 characters!');
            noError = false;
        }

        //Email
        var emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
        if (!Email && emailRegex.test(Email)) {
            showError('Email', 'Email is invalid!');
            noError = false;
        }
    }
});

