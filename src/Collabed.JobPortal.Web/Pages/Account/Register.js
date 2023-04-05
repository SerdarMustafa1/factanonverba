const lengthPattern = /^.{8,}$/;
const digitPattern = /\d/;
const specialCharacterPattern = /[^a-zA-Z0-9]/;
const hasLowerCasePattern = /[a-z]/;
const hasUpperCasePattern = /[A-Z]/;
const reWhiteSpace = new RegExp("/\s/g");
let submitted = false;


const selectUserType = (userType) => {
    $('input#userTypeHiddenInput').val(userType);
    $('div#registerStepOne').attr('hidden', true);
    $('div#registerStepTwo').attr('hidden', false);

    if (userType === 0) {
        $('div#OrganisationName_container').attr('hidden', true);
    } else {
        $('div#PersonalNameContainer').attr('hidden', true);
    }
}

const styleValidationOfRequiredField = (propertyName, validationMessage) => {
    let inputJQueryIdentifier = 'input#' + propertyName + '_input';
    let divJQueryIdentifier = 'div#' + propertyName + '_input-group';
    let inputValue = $(inputJQueryIdentifier).val();
    if (propertyName === 'UserName') {
        $(divJQueryIdentifier).removeClass('input-group-correct');
        $(divJQueryIdentifier).addClass('input-group-error');
        let inputVal = $('input#UserName_input').val();
        if (inputVal.length > 0 && inputVal.indexOf(' ') >= 0) {
            $('span#' + propertyName + '_hint').text('Spaces are not allowed for user name');
            $('span#' + propertyName + '_hint').addClass('text-validation-error');
            $(divJQueryIdentifier).removeClass('input-group-correct');
            $(divJQueryIdentifier).addClass('input-group-error');
        } else if (inputValue.length == 0) {
            $(divJQueryIdentifier).removeClass('input-group-correct');
            $(divJQueryIdentifier).addClass('input-group-error');
            $('span#' + propertyName + '_hint').text(validationMessage);
            $('span#' + propertyName + '_hint').addClass('text-validation-error');
        } else {
            $(divJQueryIdentifier).removeClass('input-group-error');
            $(divJQueryIdentifier).addClass('input-group-correct');
            $('span#' + propertyName + '_hint').text('');
            $('span#' + propertyName + '_hint').removeClass('text-validation-error');
        }
    }
    else if (inputValue.length == 0) {
        $(divJQueryIdentifier).removeClass('input-group-correct');
        $(divJQueryIdentifier).addClass('input-group-error');
        $('span#' + propertyName + '_hint').text(validationMessage);
        $('span#' + propertyName + '_hint').addClass('text-validation-error');
    }
    else {
        $(divJQueryIdentifier).removeClass('input-group-error');
        $(divJQueryIdentifier).addClass('input-group-correct');
        $('span#' + propertyName + '_hint').text('');
        $('span#' + propertyName + '_hint').removeClass('text-validation-error');
    }
}

const validateEmail  = (showErrors) => {
    let emailInputField = $('input#EmailAddress_input');
    let emailInputFieldVal = emailInputField.val();
    if (emailInputFieldVal.length == 0) {
        if (showErrors) emailError('Please enter your email address');
        return false;
    }
    else {
        if (emailInputFieldVal.endsWith('@') || emailInputFieldVal.endsWith('.')) {
            if (showErrors) emailError('Please enter valid email address');
            return false;
        }
        let emailParts = emailInputFieldVal.split('@');
        if (emailParts.length != 2) {
            if (showErrors) emailError('Please enter valid email address');
            return false;
        }
        if (emailParts[1].split('.').length === 1) {
            if (showErrors) emailError('Please enter valid email address');
            return false;
        }
    }

    if(showErrors) appropriateEmail();
    return true;
}

const emailError = (message) => {
    $('div#EmailAddress_input-group').addClass('input-group-error');
    $('div#EmailAddress_input-group').removeClass('input-group-correct');
    $('span#EmailAddress_hint').addClass('text-validation-error');
    $('span#EmailAddress_hint').text(message);
}

const appropriateEmail = () => {
    $('div#EmailAddress_input-group').removeClass('input-group-error');
    $('div#EmailAddress_input-group').addClass('input-group-correct');
    $('span#EmailAddress_hint').removeClass('text-validation-error');
    $('span#EmailAddress_hint').text('');
}

const focusInputField = (selector) => {
    $(selector).focus();
}

const validatePassword = (showErrors) => {
    let passwordAppendIconIdentifier = "i#Password_append-icon";
    let registerPasswordInputField = $("input#Password_input");
    if (!registerPasswordInputField.is(':visible')) {
        return true; // invisible treated as valid for the sake of external users
    }
    let typedInPassword = registerPasswordInputField.val();

    let isCaseOk = hasLowerCasePattern.test(typedInPassword) && hasUpperCasePattern.test(typedInPassword);
    let isLengthOk = lengthPattern.test(typedInPassword);
    let containsDigit = digitPattern.test(typedInPassword);
    let containsSpecialChar = specialCharacterPattern.test(typedInPassword);
    let isValid = isCaseOk && isLengthOk && containsDigit && containsSpecialChar;
    if (showErrors) {
        if (registerPasswordInputField.val().length > 0) {
            styleHintPar(isValid);
            styleCasePar(isCaseOk);
            styleLengthPar(isLengthOk);
            styleNonAlphabeticalPar(containsDigit && containsSpecialChar);
        }
        else {
            styleLengthPar(false);
            styleNonAlphabeticalPar(false);
            styleCasePar(false);
            styleHintPar(false);
        }
        if (isValid) {
            $('div#Password_input-group').removeClass('input-group-error');
            $('div#Password_input-group').addClass('input-group-correct');
            $('span#Password_hint').removeClass('text-validation-error');
            $(passwordAppendIconIdentifier).attr('hidden', true);
            $(passwordAppendIconIdentifier).removeClass('text-validation-error');
        }
        else {
            $('div#Password_input-group').removeClass('input-group-correct');
            $('div#Password_input-group').addClass('input-group-error');
            $('span#Password_hint').addClass('text-validation-error');
            $(passwordAppendIconIdentifier).attr('hidden', false);
            $(passwordAppendIconIdentifier).addClass('text-validation-error');
        }
    }
    if (isValid) {
        return true;
    }
    else{
        return false;
    }
}

const validateConfirmPassword = (showErrors) => {
    let confirmPasswordInputGroupIdentifier = 'div#ConfirmPassword_input-group';
    let confirmPasswordInputFieldIdentifier = 'input#ConfirmPassword_input';
    let confirmPasswordIconIdentifier = 'i#ConfirmPassword_append-icon';
    if (!$(confirmPasswordInputFieldIdentifier).is(':visible')) {
        return true;
    }
    let password = $("input#Password_input").val();
    if ($(confirmPasswordInputFieldIdentifier).val() === password) {
        if (showErrors) {
            $(confirmPasswordInputGroupIdentifier).removeClass('input-group-error');
            $(confirmPasswordInputGroupIdentifier).addClass('input-group-correct');
            $(confirmPasswordIconIdentifier).attr('hidden', true);
            $(confirmPasswordIconIdentifier).removeClass('text-validation-error');
            $('span#ConfirmPassword_hint').text('');
            $('span#ConfirmPassword_hint').removeClass('text-validation-error');
        }
        return true;

    } else {
        if (showErrors) {
            $(confirmPasswordInputGroupIdentifier).removeClass('input-group-correct');
            $(confirmPasswordInputGroupIdentifier).addClass('input-group-error');
            $(confirmPasswordIconIdentifier).attr('hidden', false);
            $(confirmPasswordIconIdentifier).addClass('text-validation-error');
            $('span#ConfirmPassword_hint').text("Your passwords don't match");
            $('span#ConfirmPassword_hint').addClass('text-validation-error');
        }
        return false;
    }
}

const isInputFieldEmpty = (propertyName) => {
    let inputFieldJQueryIdentifier = "input#" + propertyName + "_input";
    if (!$(inputFieldJQueryIdentifier).is(':visible')) {
        // if field is not visible - will simply treat is as valid
        return false;
    }
    else if ($(inputFieldJQueryIdentifier).val().length === 0) {
        return true;
    }
    else {
        return false;
    }
}

const isUserNameValid = () => {
    let inputVal = $('input#UserName_input').val();
    if (inputVal.length === 0) {
        return false;
    } else if (inputVal.indexOf(' ') >= 0) {
        return false;
    }
    return true;
}

const validateForm = () => {
    if (validateConfirmPassword() && validateEmail(false) && validatePassword(false) &&
        !isInputFieldEmpty('FirstName') &&
        !isInputFieldEmpty('LastName') && !isInputFieldEmpty('OrganisationName') &&
        $("input#GDPRConsent_input").is(":checked")) {
        $("button#RegisterButton").attr('disabled', false); // enable button
    }
    else {
        $("button#RegisterButton").attr('disabled', true); // disable button
    }
}

const styleHintPar = (isPasswordValid) => {
    if (isPasswordValid === true) {
        $("#hintParagraph").attr("style", "color:dimgray;");
    }
    else if (typeof isPasswordValid != "boolean") {
        throw new Error("Unacceptable property passed to styleHintPar: " + isPasswordValid);
    }
    else {
        $("#hintParagraph").attr("style", "color:red;");
    }
}

const styleCasePar = (isCaseValid) => {
    if(isCaseValid) {
        $("#caseIcon").attr("class", "bi bi-check2-circle");
        $("#caseIcon").attr("style", "color:green;");
        $("#caseHint").attr("style", "color:green;");
    }
    else {
        $("#caseIcon").attr("class", "bi bi-x-circle");
        $("#caseIcon").attr("style", "color:red;");
        $("#caseHint").attr("style", "color:red;");
    }
}

const styleLengthPar = (isLengthValid) => {
    if (isLengthValid) {
        $("#lengthIcon").attr("class", "bi bi-check2-circle");
        $("#lengthIcon").attr("style", "color:green;");
        $("#lengthHint").attr("style", "color:green;");
    }
    else {
        $("#lengthIcon").attr("class", "bi bi-x-circle");
        $("#lengthIcon").attr("style", "color:red;");
        $("#lengthHint").attr("style", "color:red;");
    }
}

const styleNonAlphabeticalPar = (containsNonAlphabeticalCharacters) => {
    if (containsNonAlphabeticalCharacters) {
        $("#nonAlphanumericIcon").attr("class", "bi bi-check2-circle");
        $("#nonAlphanumericIcon").attr("style", "color:green;");
        $("#nonAlphanumericParagraph").attr("style", "color:green;");
    }
    else {
        $("#nonAlphanumericIcon").attr("class", "bi bi-x-circle");
        $("#nonAlphanumericIcon").attr("style", "color:red;");
        $("#nonAlphanumericParagraph").attr("style", "color:red;");
    }
}

const checkIfEmailExists = (event) => {
    if (submitted) return;

    event.preventDefault();
    event.stopPropagation();
    let emailAddress = $('input#EmailAddress_input').val();
    collabed.jobPortal.account.bmtAccount.checkIfEmailExists(emailAddress)
        .then((result) => {
            if (result === true) {
                emailError('Email already exists, you should Login instead.');
                $('button#RegisterButton').attr('disabled', true);
            }
            else { // have to check is this really sugmitting the form or what, - has to be clicked twice to work propperly (maybe its some async or smth?)
                submitted = true;
                $('form#registerForm').submit();
            }
        }).catch((error) => {
            // HACK: TODO, add client side errors validation
            return console.log(error);
        });
}