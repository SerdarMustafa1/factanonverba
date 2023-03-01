let submitted = false;

let checkIfUserExists = (event) => {
    if (submitted) return;

    event.preventDefault();
    event.stopPropagation();
    let loginInput = $('input#UserNameOrEmailAddress_input').val();
    let passwordInput = $('input#Password_input').val();
    collabed.jobPortal.account.bmtAccount.checkPasswordCredentials(loginInput, passwordInput)
        .then((result) => {
            if (result === true) {
                submitted = true;
                $('form#loginForm').submit();
            }
            else {
                submitted = false;
                passwordError('The email and/or password you entered did not match our records. Please double-check and try again.');
                userNameOrEmailError('');
            }
        }).catch((error) => {
            return console.log(error);
        });
}

let userNameOrEmailError = (message) => {
    $('div#UserNameOrEmailAddress_input-group').addClass('input-group-error');
    $('div#UserNameOrEmailAddress_input-group').removeClass('input-group-correct');
    $('span#UserNameOrEmailAddress_hint').addClass('text-validation-error');
    $('span#UserNameOrEmailAddress_hint').text(message);
}

let passwordError = (message) => {
    $('div#Password_input-group').addClass('input-group-error');
    $('div#Password_input-group').removeClass('input-group-correct');
    $('span#Password_hint').addClass('text-validation-error');
    $('span#Password_hint').text(message);
}

let appropriatePassword = (message) => {
    $('div#Password_input-group').removeClass('input-group-error');
    $('div#Password_input-group').addClass('input-group-correct');
    $('span#Password_hint').removeClass('text-validation-error');
    $('span#Password_hint').text(message);
}

let appropriateUserNameOrLogin = () => {
    $('div#UserNameOrEmailAddress_input-group').removeClass('input-group-error');
    $('div#UserNameOrEmailAddress_input-group').addClass('input-group-correct');
    $('span#UserNameOrEmailAddress_hint').removeClass('text-validation-error');
    $('span#UserNameOrEmailAddress_hint').text('');
}