(function ($) {
    $(function () {
        var l = abp.localization.getResource("AbpAccount");

        $('#ChangePasswordForm').submit(function (e) {
            e.preventDefault();

            var input = $('#ChangePasswordForm').serializeFormToObject();

            if (input.currentPassword && input.currentPassword == ''){
                return;
            }
            
            if(input.currentPassword == input.password) {
                abp.message.error('New Password must be different to Current Password');
                return;
            }
            
            collabed.jobPortal.account.userProfile.changePassword(input).then(function (result) {
                $("#passwordChangedAlertId").show(500, function () {
                    setTimeout(function () {
                        $("#passwordChangedAlertId").hide(500);
                    }, 3000);
                });
                //abp.notify.success('Password has been changed successfully!');
                //abp.message.success(l('PasswordChanged'));
                abp.event.trigger('passwordChanged');
                $('#ChangePasswordForm').trigger("reset");
                resetPasswordValidationMarkers();
            });
        });
    });
})(jQuery);
const lengthPattern = /^.{8,}$/;
const digitPattern = /\d/;
const specialCharacterPattern = /[^a-zA-Z0-9]/;
const hasLowerCasePattern = /[a-z]/;
const hasUpperCasePattern = /[A-Z]/;
const reWhiteSpace = new RegExp("/\s/g");
let submitted = false;

function onMyAccountDelete() {
    var deleteAccountModal = new abp.ModalManager('/Account/Components/ProfileManagementGroup/Password/DeleteMyAccountModal');

    deleteAccountModal.onResult(function () {
        window.location.href = "/Account/Logout";
    });
    deleteAccountModal.open();

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
        if (registerPasswordInputField.val() && registerPasswordInputField.val().length > 0) {
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

const validateCurrentPassword = () => {
    let currentPasswordInputGroupIdentifier = 'div#CurrentPassword_input-group';
    let currentPasswordInputFieldIdentifier = 'input#CurrentPassword_input';
    let currentPasswordIconIdentifier = 'i#CurrentPassword_append-icon';
    if(!$(currentPasswordInputFieldIdentifier).is(':visible')) {
        return true;
    }
    if ($(currentPasswordInputFieldIdentifier).val() && $(currentPasswordInputFieldIdentifier).val() != '') {
        $(currentPasswordInputGroupIdentifier).removeClass('input-group-error');
        $(currentPasswordInputGroupIdentifier).addClass('input-group-correct');
        $(currentPasswordIconIdentifier).attr('hidden', true);
        $(currentPasswordIconIdentifier).removeClass('text-validation-error');
        $('span#CurrentPassword_hint').text('');
        $('span#CurrentPassword_hint').removeClass('text-validation-error');
        return true;
    } else {
        $(currentPasswordInputGroupIdentifier).removeClass('input-group-correct');
        $(currentPasswordInputGroupIdentifier).addClass('input-group-error');
        $(currentPasswordIconIdentifier).attr('hidden', false);
        $(currentPasswordIconIdentifier).addClass('text-validation-error');
        $('span#CurrentPassword_hint').text("Current password is required");
        $('span#CurrentPassword_hint').addClass('text-validation-error');
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
    if (validateConfirmPassword() && validatePassword(false) &&
        validateCurrentPassword()) {
        $("button#ChangePasswordBtn").attr('disabled', false); // enable button
        return true;
    }
    else {
        $("button#ChangePasswordBtn").attr('disabled', true); // disable button
        return false;
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

const resetPasswordValidationMarkers = () => {
    $("#nonAlphanumericIcon").attr("style", "color:#475467;");
    $("#nonAlphanumericParagraph").attr("style", "color:#475467;");
    $("#lengthIcon").attr("style", "color:#475467;");
    $("#lengthHint").attr("style", "color:#475467;");
    $("#caseIcon").attr("style", "color:#475467;");
    $("#caseHint").attr("style", "color:#475467;");
    $("button#ChangePasswordBtn").attr('disabled', true);
}

let showOrHideVisibilityButton = (propertyName) => {
    if ($('input#' + propertyName + '_input').val().length > 0) {
        $('i#' + propertyName + '_visibility-icon').attr('hidden', false);
        $('i#' + propertyName + '_visibility-icon').css('cursor', 'pointer');
    }
    else {
        $('i#' + propertyName + '_visibility-icon').css('cursor', 'default');
        $('i#' + propertyName + '_visibility-icon').attr('hidden', true);
    }
}

let togglePasswordVisibility = (propertyName) => {
    let iconId = propertyName + '_visibility-icon';
    let inputId = propertyName + '_input';
    let isVisible = $('i#' + iconId).hasClass("bi bi-eye-slash");
    if (isVisible) {
        $('input#' + inputId).attr('type', 'password');
        $('i#' + iconId).removeClass("bi bi-eye-slash");
        $('i#' + iconId).addClass("bi bi-eye");
    }
    else {
        $('input#' + inputId).attr('type', 'text');
        $('i#' + iconId).removeClass("bi bi-eye");
        $('i#' + iconId).addClass("bi bi-eye-slash");
    }

}