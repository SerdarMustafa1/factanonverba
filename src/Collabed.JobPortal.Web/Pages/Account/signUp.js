
let validateEmail = () => {
    let emailInputField = $('input#EmailAddress_input');
    let emailInputFieldVal = emailInputField.val();

    if (emailInputFieldVal.endsWith('@') || emailInputFieldVal.endsWith('.')) {
        emailError(false);
        return false;
    }
    let emailParts = emailInputFieldVal.split('@');
    if (emailParts.length != 2) {
        emailError(false);
        return false;
    }
    if (emailParts[1].split('.').length === 1) {
        emailError(false);
        return false;
    }

    appropriateEmail();
}


let emailError = (isEmpty) => {
    $('div#EmailAddress_input-group').removeClass('input-group-correct');
    $('div#EmailAddress_input-group').addClass('input-group-error');
    $('span#EmailAddress_hint').addClass('text-validation-error');
    if (isEmpty) {
        $('span#EmailAddress_hint').text('Please enter your email address');
    } else {
        $('span#EmailAddress_hint').text('Please enter valid email address');
    }
    $('button#getStartedButton').prop("disabled", true);
}

let appropriateEmail = () => {
    $('div#EmailAddress_input-group').removeClass('input-group-error');
    $('div#EmailAddress_input-group').addClass('input-group-correct');
    $('span#EmailAddress_hint').removeClass('text-validation-error');
    $('span#EmailAddress_hint').text('');
    $('button#getStartedButton').prop("disabled", false);
}