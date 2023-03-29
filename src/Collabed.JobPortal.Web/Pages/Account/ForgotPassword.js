let submitted = false;
let togglePasswordButton = () => {
    let input = $('input#Email_input').val();
    if (input.includes('@') && input.split('@')[1].includes('.')) {
        $('button#resetPasswordButton').attr('disabled', false);
        appropriateEmail();
    }
    else {
        $('button#resetPasswordButton').attr('disabled', true);
    }
}

const focusInputField = (selector) => {
    $(selector).focus();
}

let checkIfEmailExists = (event) => {
    if (submitted) return;
    console.log('checkIfEmailExists');
    event.preventDefault();
    event.stopPropagation();
    let emailAddress = $('input#Email_input').val();
    console.log('emailAddress = ', emailAddress);
    collabed.jobPortal.account.bmtAccount.checkIfEmailExists(emailAddress)
        .then((result) => {
            if (result === true) {
                submitted = true;
                $('form#resetPasswordForm').submit();
            }
            else { 
                emailError("Such an email don't match our records");
                $('button#resetPasswordButton').attr('disabled', true);
            }
        }).catch((error) => {
            console.log('caught an error');
            // HACK: TODO, add client side errors validation
            return console.log(error);
        });
}

let emailError = (message) => {
    $('div#Email_input-group').removeClass('input-group-correct');
    $('div#Email_input-group').addClass('input-group-error');
    $('span#Email_hint').addClass('text-validation-error');
    $('span#Email_hint').text(message);
    $('button#resetPasswordButton').prop("disabled", true);
}

let appropriateEmail = () => {
    $('div#Email_input-group').removeClass('input-group-error');
    $('div#Email_input-group').addClass('input-group-correct');
    $('span#Email_hint').removeClass('text-validation-error');
    $('span#Email_hint').text('');
    $('button#resetPasswordButton').prop("disabled", false);
}