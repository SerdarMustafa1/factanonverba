
let submitted = false;

function validateEmail () {
    let emailInputField = $('input#EmailAddress_input');
    let emailInputFieldVal = emailInputField.val();
    if (emailInputFieldVal.length == 0) {
        emailError('Please enter your email address');
        return false;
    }
    if (emailInputFieldVal.endsWith('@') || emailInputFieldVal.endsWith('.')) {
        emailError('Please enter valid email address');
        return false;
    }
    let emailParts = emailInputFieldVal.split('@');
    if (emailParts.length != 2) {
        emailError('Please enter valid email address');
        return false;
    }
    if (emailParts[1].split('.').length === 1) {
        emailError('Please enter valid email address');
        return false;
    }

    appropriateEmail();
}

const focusInputField = (selector) => {
    $(selector).focus();
}


let emailError = (message) => {
    $('div#EmailAddress_input-group').removeClass('input-group-correct');
    $('div#EmailAddress_input-group').addClass('input-group-error');
    $('span#EmailAddress_hint').addClass('text-validation-error');
    $('span#EmailAddress_hint').text(message);
    $('button#getStartedButton').prop("disabled", true);
}

let appropriateEmail = () => {
    $('div#EmailAddress_input-group').removeClass('input-group-error');
    $('div#EmailAddress_input-group').addClass('input-group-correct');
    $('span#EmailAddress_hint').removeClass('text-validation-error');
    $('span#EmailAddress_hint').text('');
    $('button#getStartedButton').prop("disabled", false);
}

let checkIfEmailExists = (event) => {
    if (submitted) return;

    event.preventDefault();
    event.stopPropagation();
    let emailAddress = $('input#EmailAddress_input').val();
    collabed.jobPortal.account.bmtAccount.checkIfEmailExists(emailAddress)
        .then((result) => {
            if (result === true) {
                emailError('Email already exists, you should Login instead.');
                $('button#getStartedButton').attr('disabled', 'true');
            }
            else {
                submitted = true;
                $('form#localUserForm').submit();
            }
        }).catch((error) => {
        // HACK: TODO, add client side errors validation
        return console.log(error);
    });
}