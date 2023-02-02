
let lengthPattern = /^.{8,}$/;
let digitPattern = /\d/;
let specialCharacterPattern = /[^a-zA-Z0-9]/;
let hasLowerCasePattern = /[a-z]/;
let hasUpperCasePattern = /[A-Z]/;

validatePassword = () => {
    let registerPasswordInputField = $("input#registerPasswordInputField");
    let typedInPassword = registerPasswordInputField.val();

    let isCaseOk = hasLowerCasePattern.test(typedInPassword) && hasUpperCasePattern.test(typedInPassword);
    let isLengthOk = lengthPattern.test(typedInPassword);
    let containsDigit = digitPattern.test(typedInPassword);
    let containsSpecialChar = specialCharacterPattern.test(typedInPassword);
    let isValid = isCaseOk && isLengthOk && containsDigit && containsSpecialChar;

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