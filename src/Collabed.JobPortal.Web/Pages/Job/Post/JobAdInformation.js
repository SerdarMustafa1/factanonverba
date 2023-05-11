$(function () {

    if ($(window).width() < 900) {
        $('ul.nav').addClass('nav-justified');
    }

    var toolbarOptions = [
        ['bold', 'italic', 'underline'],        // toggled buttons
        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        [{ 'align': [] }],
        ['video']
    ];

    quill = new Quill('#quill-editor', {
        modules: {
            toolbar: toolbarOptions
        },
        theme: 'snow'
    });
    $('#brand-logo').attr("src", "/images/logo/BMT_Service_Drk Blue_symbol.png");

    let today = new Date();
    today = today.toISOString().split('T')[0];
    flatpickr("#StartDate_input", { minDate: today });
    flatpickr("#ApplicationDeadline_input", { minDate: today });

    const delta = quill.clipboard.convert($('#jobDescriptionHiddenInput').val());
    quill.setContents(delta);
    assignQuillToInput();

});
let quill = undefined;

let quillOnKeyUp = () => {
    assignQuillToInput();
    $('#quill-editor-form-group').removeClass('red-border');
    $('#descriptionErrorMessage').text('');
}

let assignQuillToInput = () => {
    $('#jobDescriptionHiddenInput').val(quill.root.innerHTML);
}

let onClickPublishButton = (event) => {
    event.preventDefault();
    assignQuillToInput();
    if (isFormValid()) {
        $('#jobPostForm').submit();
    }
}

let isFormValid = () => {  
    let jobTitleValid = ($('#jobTitleInput').val()).length > 0;
    let jobSubDescValid = ($('#subDescriptionInput').val()).length > 0;
    let jobDescValid = quill.getText().trim().length > 0;
    let jobCatValid = ($('#JobCategoryId').val()).length > 0;
    let employmentTypeValid = ($('#EmploymentTypeId').val()).length > 0;
    let contractTypeValid = ($('#ContractTypeId').val()).length > 0;
    let jobLocationTypeValid = ($('#JobLocationTypeId').val()).length > 0;
    //let startDateValid = !isNaN(Date.parse($('#StartDate_input').val()));

    if (jobTitleValid && jobSubDescValid && jobDescValid &&
        jobCatValid && employmentTypeValid && contractTypeValid &&
        jobLocationTypeValid) {
        // form is valid
        return true;
    }
    else {
        validateInformationTab();
        validateRequirementsTab();
        // form is invalid
        return false;
    }
}

//function tabClick(event) {
//    event.preventDefault();
//    event.stopPropagation();
//    let activeTab = $('.nav-tabs .active').html();
//    console.log(activeTab);
//    console.log('event', event);
//    let isInformation = activeTab.includes('Information');
//    let isRequirements = activeTab.includes('Requirements');
//    if (isInformation) validateInformationTab()
//    if (isRequirements) validateRequirementsTab();
    
//}

let markTabs = () => {
    var inputs = $('.tab-content').find('input.input-validation-error');
    var selects = $('tab-content').find('select.input-validation-error');
    var textAreas = $('tab-content').find('textarea.input-validation-error');

    for (let i = 0; i < inputs.length; i++) {
        let field = inputs[i];
        if (field.name === 'JobTitle') {
            $('#tabLink1').addClass('tab-has-error');
            $('#icon1').addClass('icon-error');
        }
        if (field.name === 'JobDescription') {
            $('#tabLink1').addClass('tab-has-error');
            $('#icon1').addClass('icon-error');
        }
    }

    for (let i = 0; i < selects.length; i++) {
        let field = selects[i];
        if (field.name === 'JobCategoryId') {
            $('#tabLink1').addClass('tab-has-error');
            $('#icon1').addClass('icon-error');
        }
        if (field.name === 'EmploymentTypeId') {
            $('#tabLink2').addClass('tab-has-error');
            $('#icon2').addClass('icon-error');
        }
        if (field.name === 'ContractTypeId') {
            $('#tabLink2').addClass('tab-has-error');
            $('#icon2').addClass('icon-error');
        }
        if (field.name === 'JobLocationTypeId') {
            $('#tabLink2').addClass('tab-has-error');
            $('#icon2').addClass('icon-error');
        }
    }

    for (let i = 0; i < textAreas.length; i++) {
        let field = textAreas[i];
        if (field.name === 'SubDescription') {
            $('#tabLink1').addClass('tab-has-error');
            $('#icon1').addClass('icon-error');
        }
    }

    if (inputs.length > 0 || selects.length > 0 || textAreas.length > 0) {
        return false;
    }
    else {
        return true;
    }
}

let showCalendar = () => {
    $('#DatePicker_input').focus();
}

function nextPage(event) {
    event.preventDefault();
    let activeTab = $('.nav-tabs .active').html();
    let isInformation = activeTab.includes('Information');
    let isRequirements = activeTab.includes('Requirements');

    if (isInformation) {
        let isValid = validateInformationTab();
        if (!isValid) return;
        markTabAsValid('1');
    }
    if (isRequirements) {
        let isValid = validateRequirementsTab();
        if (!isValid) return;
        markTabAsValid('2');
        // validate EmploymentType, ContractType, JobLocation, and StartDate fields
    }

    const nextTabLinkEl = $('.nav-tabs .active').closest('li').next('li').find('a')[0];
    const nextTab = new bootstrap.Tab(nextTabLinkEl);
    nextTab.show();
    window.scrollTo(0, 0);
}

function validateInformationTab() {
    let isJobTitleValid = $('#jobTitleInput').val().length > 0;
    let isSubDescriptionValid = $('#subDescriptionInput').val().length > 0;
    let isDescriptionValid = isQuillValid();
    let isCategoryValid = $('#JobCategoryId').val().length > 0;

    if (!isJobTitleValid) {
        $('#jobTitleInput').addClass('input-validation-error');
        $('#jobTitleErrorMessage').text('Please enter the job title');
    }
    else {
        $('#jobTitleInput').removeClass('input-validation-error');
        $('#jobTitleErrorMessage').text('');
    }
    if (!isSubDescriptionValid) {
        $('#subDescriptionInput').addClass('input-validation-error');
        $('#subDescriptionErrorMessage').text('Please provide a sub-description');
    }
    else {
        $('#subDescriptionInput').removeClass('input-validation-error');
        $('#subDescriptionErrorMessage').text('');
    }
    if (!isDescriptionValid) {
        $('#quill-editor-form-group').addClass('red-border');
        $('#descriptionErrorMessage').text('Please enter a job descripiton');
    }
    else {
        $('#quill-editor-form-group').removeClass('red-border');
        $('#descriptionErrorMessage').text('');
    }
    if (!isCategoryValid) {
        $('#JobCategoryId').addClass('input-validation-error');
        $('#jobCategoryErrorMessage').text('Please select a category from the dropdown');
    }
    else {
        $('#JobCategoryId').removeClass('input-validation-error');
        $('#jobCategoryErrorMessage').text('');
    }
    if (isJobTitleValid && isSubDescriptionValid && isDescriptionValid && isCategoryValid) {
        markTabAsValid('1');
        return true;
    }
    else {
        markTabAsInvalid('1');
        return false;
    }
}

const isQuillValid = () => {
    if (quill.root.innerHTML.replace(/<(.|\n)*?>/g, '').trim().length === 0 &&
        !quill.root.innerHTML.includes("<img") && !quill.root.innerHTML.includes("<iframe")) {
        return false;
    }
    else {
        return true;
    }
}

function validateRequirementsTab() {
    let isEmploymentTypeValid = $('#EmploymentTypeId').val().length > 0;
    let isContractTypeValid = $('#ContractTypeId').val().length > 0;
    let isJobLocationValid = $('#JobLocationTypeId').val().length > 0;
    //let isStartDateValid = $('#StartDate_input').val().length > 0;

        if (!isEmploymentTypeValid) {
            $('#EmploymentTypeId').addClass('input-validation-error');
            $('#EmploymentTypeErrorMessage').text('Please select an employment type');
        }
        else {
            $('#EmploymentTypeId').removeClass('input-validation-error');
            $('#EmploymentTypeErrorMessage').text('');
        }
        if (!isContractTypeValid) {
            $('#ContractTypeId').addClass('input-validation-error');
            $('#ContractTypeIdErrorMessage').text('Please select a contract type');
        }
        else {
            $('#ContractTypeId').removeClass('input-validation-error');
            $('#ContractTypeIdErrorMessage').text('');
        }
        if (!isJobLocationValid) {
            $('#JobLocationTypeId').addClass('input-validation-error');
            $('#JobLocationTypeErrorMessage').text('Please select a job location type from the dropdown');
        }
        else {
            $('#JobLocationTypeId').removeClass('input-validation-error');
            $('#JobLocationTypeErrorMessage').text('');
        }
        //if (!isStartDateValid) {
        //    $('#StartDate_input').addClass('input-validation-error');
        //    $('#StartDateErrorMessage').text('Please select a starting date');
        //}
        //else {
        //    $('#StartDate_input').removeClass('input-validation-error');
        //    $('#StartDateErrorMessage').text('');
        //}
    if (isEmploymentTypeValid && isContractTypeValid && isJobLocationValid) {
        markTabAsValid('2');
        return true;
    }
    else {
        markTabAsInvalid('2');
        return false;
    }
}

function markTabAsValid(tabNo) {
    $('#tabLink' + tabNo).removeClass('tab-has-error');
    $('#icon' + tabNo).removeClass();
    $('#icon' + tabNo).addClass('bi bi-' + tabNo + '-circle-fill');
}
function markTabAsInvalid(tabNo) {
    $('#tabLink' + tabNo).addClass('tab-has-error');
    $('#icon' + tabNo).removeClass();
    $('#icon' + tabNo).addClass('icon-error');
    $('#icon' + tabNo).addClass('bi bi-exclamation-circle');
}

$(window).resize(function () {
    if ($(window).width() < 900) {
        $('ul.nav').addClass('nav-justified');
    } else {
        $('ul.nav').removeClass('nav-justified');
    }
});