
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