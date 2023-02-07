
let lengthPattern = /^.{8,}$/;
let digitPattern = /\d/;
let specialCharacterPattern = /[^a-zA-Z0-9]/;
let hasLowerCasePattern = /[a-z]/;
let hasUpperCasePattern = /[A-Z]/;

let validateRequiredField = (propertyName, validationMessage) => {
    let inputJQueryIdentifier = 'input#' + propertyName + '_input';
    let divJQueryIdentifier = 'div#' + propertyName + '_input-group';
    let iconJQueryIdentifier = 'i#' + propertyName + '_append-icon';
    let inputValue = $(inputJQueryIdentifier).val();
    if (inputValue.length == 0) {
        $(divJQueryIdentifier).addClass('input-group-error');
        $(iconJQueryIdentifier).removeClass('bi bi-question-circle');
        $(iconJQueryIdentifier).addClass('bi bi-exclamation-circle');
        $('span#' + propertyName + '_validation-message').text(validationMessage);
    } else {
        $(divJQueryIdentifier).removeClass('input-group-error');
        $(iconJQueryIdentifier).removeClass('bi bi-exclamation-circle');
        $(iconJQueryIdentifier).addClass('bi bi-question-circle');
        $('span#' + propertyName + '_validation-message').text('');
    }
}

let validateEmail = () => {
    let emailInputField = $('input#EmailAddress_input');
    let emailInputFieldVal = emailInputField.val();
    if (emailInputFieldVal.length == 0) {
        emailError(true);
        return false;
    }
    else {
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
    }

    appropriateEmail();
}

let emailError = (isEmpty) => {
    $('div#EmailAddress_input-group').addClass('input-group-error');
    $('i#EmailAddress_append-icon').removeClass('bi bi-question-circle');
    $('i#EmailAddress_append-icon').addClass('bi bi-exclamation-circle');
    if (isEmpty) {
        $('span#EmailAddress_validation-message').text('Please type your email address');
    } else {
        $('span#EmailAddress_validation-message').text('Please type valid email address');
    }
}

let appropriateEmail = () => {
    $('div#EmailAddress_input-group').removeClass('input-group-error');
    $('i#EmailAddress_append-icon').removeClass('bi bi-exclamation-circle');
    $('i#EmailAddress_append-icon').addClass('bi bi-question-circle');
    $('span#EmailAddress_validation-message').text('');
}

let validatePassword = () => {
    let registerPasswordInputField = $("input#Password_input");
    let typedInPassword = registerPasswordInputField.val();

    let isCaseOk = hasLowerCasePattern.test(typedInPassword) && hasUpperCasePattern.test(typedInPassword);
    let isLengthOk = lengthPattern.test(typedInPassword);
    let containsDigit = digitPattern.test(typedInPassword);
    let containsSpecialChar = specialCharacterPattern.test(typedInPassword);
    let isValid = isCaseOk && isLengthOk && containsDigit && containsSpecialChar;
    if(!isValid){
        $('div#Password_input-group').addClass('input-group-error');
    }
    else{
        $('div#Password_input-group').removeClass('input-group-error');
    }
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
}

let validateConfirmPassword = () => {
    let confirmPasswordInputGroupIdentifier = 'div#ConfirmPassword_input-group';
    let confirmPasswordInputFieldIdentifier = 'input#ConfirmPassword_input';
    let password = $("input#Password_input").val();
    if ($(confirmPasswordInputFieldIdentifier).val() === password) {
        $(confirmPasswordInputGroupIdentifier).removeClass('input-group-error');
        $('i#ConfirmPassword_append-icon').removeClass('bi bi-exclamation-circle');
        $('i#ConfirmPassword_append-icon').addClass('bi bi-question-circle');
        $('span#ConfirmPassword_validation-message').text('');

    } else {
        $(confirmPasswordInputGroupIdentifier).addClass('input-group-error');
        $('i#ConfirmPassword_append-icon').removeClass('bi bi-question-circle');
        $('i#ConfirmPassword_append-icon').addClass('bi bi-exclamation-circle');
        $('span#ConfirmPassword_validation-message').text("Your passwords don't match");
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

