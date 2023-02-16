
let lengthPattern = /^.{8,}$/;
let digitPattern = /\d/;
let specialCharacterPattern = /[^a-zA-Z0-9]/;
let hasLowerCasePattern = /[a-z]/;
let hasUpperCasePattern = /[A-Z]/;

let styleValidationOfRequiredField = (propertyName, validationMessage) => {
    let inputJQueryIdentifier = 'input#' + propertyName + '_input';
    let divJQueryIdentifier = 'div#' + propertyName + '_input-group';
    let inputValue = $(inputJQueryIdentifier).val();
    if (inputValue.length == 0) {
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

let validateEmail = (showErrors) => {
    let emailInputField = $('input#EmailAddress_input');
    let emailInputFieldVal = emailInputField.val();
    if (emailInputFieldVal.length == 0) {
        if(showErrors) emailError(true);
        return false;
    }
    else {
        if (emailInputFieldVal.endsWith('@') || emailInputFieldVal.endsWith('.')) {
            if(showErrors) emailError(false);
            return false;
        }
        let emailParts = emailInputFieldVal.split('@');
        if (emailParts.length != 2) {
            if(showErrors) emailError(false);
            return false;
        }
        if (emailParts[1].split('.').length === 1) {
            if(showErrors) emailError(false);
            return false;
        }
    }

    if(showErrors) appropriateEmail();
    return true;
}

let emailError = (isEmpty) => {
    $('div#EmailAddress_input-group').addClass('input-group-error');
    $('div#EmailAddress_input-group').removeClass('input-group-correct');
    $('span#EmailAddress_hint').addClass('text-validation-error');
    if (isEmpty) {
        $('span#EmailAddress_hint').text('Please enter your email address');
    } else {
        $('span#EmailAddress_hint').text('Please enter valid email address');
    }
}

let appropriateEmail = () => {
    $('div#EmailAddress_input-group').removeClass('input-group-error');
    $('div#EmailAddress_input-group').addClass('input-group-correct');
    $('span#EmailAddress_hint').removeClass('text-validation-error');
    $('span#EmailAddress_hint').text('');
}

let validatePassword = (showErrors) => {
    let passwordAppendIconIdentifier = "i#Password_append-icon";
    let registerPasswordInputField = $("input#Password_input");
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

let validateConfirmPassword = (showErrors) => {
    let confirmPasswordInputGroupIdentifier = 'div#ConfirmPassword_input-group';
    let confirmPasswordInputFieldIdentifier = 'input#ConfirmPassword_input';
    let confirmPasswordIconIdentifier = 'i#ConfirmPassword_append-icon';
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

let isInputFieldEmpty = (propertyName) => {
    let inputFieldJQueryIdentifier = "input#" + propertyName + "_input";
    if ($(inputFieldJQueryIdentifier).val().length === 0) {
        return true;
    }
    else {
        return false;
    }
}

let validateForm = () => {
    if (validateConfirmPassword() && validateEmail(false) && validatePassword(false) &&
        !isInputFieldEmpty('UserName') && !isInputFieldEmpty('FirstName') &&
        !isInputFieldEmpty('LastName') && $("input#GDPRConsent_input").is(":checked")) {
        $("button#RegisterButton").attr('disabled', false);
    }
    else {
        $("button#RegisterButton").attr('disabled', true);
    }
}

let styleHintPar = (isPasswordValid) => {
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

let styleCasePar = (isCaseValid) => {
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

let styleLengthPar = (isLengthValid) => {
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

let styleNonAlphabeticalPar = (containsNonAlphabeticalCharacters) => {
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

