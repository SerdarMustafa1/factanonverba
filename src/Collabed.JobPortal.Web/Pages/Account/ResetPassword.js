
let lengthPattern = /^.{8,}$/;
let digitPattern = /\d/;
let specialCharacterPattern = /[^a-zA-Z0-9]/;
let hasLowerCasePattern = /[a-z]/;
let hasUpperCasePattern = /[A-Z]/;

let validateForm = () => {
    if (validateConfirmPassword(false) && validatePassword(false)) {
        $("button#RegisterButton").attr('disabled', false);
    }
    else {
        $("button#RegisterButton").attr('disabled', true);
    }
}

let validatePassword = (showErrors) => {
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
    else {
        return false;
    }
}

let validateConfirmPassword = (showErrors) => {
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

let confirmPasswordError = (message) => {
    $('div#ConfirmPassword_input-group').addClass('input-group-error');
    $('div#ConfirmPassword_input-group').removeClass('input-group-correct');
    $('span#ConfirmPassword_hint').addClass('text-validation-error');
    $('span#ConfirmPassword_hint').text(message);
}

let appropriateConfirmPassword = (message) => {
    $('div#ConfirmPassword_input-group').removeClass('input-group-error');
    $('div#ConfirmPassword_input-group').addClass('input-group-correct');
    $('span#ConfirmPassword_hint').removeClass('text-validation-error');
    $('span#ConfirmPassword_hint').text(message);
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
    if (isCaseValid) {
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