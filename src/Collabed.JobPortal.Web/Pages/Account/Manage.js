(function ($) {
    $(function () {
        var l = abp.localization.getResource("AbpAccount");
        $('#PersonalSettingsForm').submit(function (e) {
            if ($('#PersonalSettingsForm').valid()) {
                abp.notify.success(l('PersonalSettingsSaved'));
            }
        });
        $('#CompanySettingsForm').submit(function (e) {
            if ($('#CompanySettingsForm').valid()) {
                abp.notify.success('Company settings saved.');
            }
        });
        if ($('#userTypeId').val() == 'Candidate') {
            $('#PersonalSettingsForm input[type=text]').on('keyup', function () {
                validatePersonalForm()
            });
            $('#newCvId').on("change", function () {
                validatePersonalForm()
            });
        }
        if ($('#userTypeId').val() == 'Organisation') {
            $('#CompanySettingsForm input[type=text]').on('keyup', function () {
                validateCompanyForm()
                $('#newLogoId').on("change", function () {
                    validateCompanyForm()
                });
            });
        }
    });
})(jQuery);
function validatePersonalForm() {
    if ($('#PersonalSettingsForm').valid()) {
        $('#IndividualProfileSubmitId').removeAttr('disabled');
    } else {
        $('#IndividualProfileSubmitId').attr('disabled', 'true');
    }
};
function validateCompanyForm() {
    if ($('#CompanySettingsForm').valid()) {
        $('#CompanyProfileSubmitId').removeAttr('disabled');
    } else {
        $('#CompanyProfileSubmitId').attr('disabled', 'true');
    }
};
