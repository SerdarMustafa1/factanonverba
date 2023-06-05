$(function () {

});

function onClientDelete(userId, companyId, companyName) {
    var deleteClientModal = new abp.ModalManager('/Clients/Modals/DeleteClientModal');

    deleteClientModal.onResult(function () {
        document.getElementById(companyId).remove();
    });
    deleteClientModal.open({
        userId: userId,
        companyId: companyId,
        companyName: companyName
    });

}

function onClientEdit(userId, jobPostingPermission, permissionId) {
    var editClientModal = new abp.ModalManager('/Clients/Modals/EditPermissionsModal');

    editClientModal.onResult(function () {
        if ($('#JobPostingPermission').prop('checked')) {
            document.getElementById(userId).innerText = "Post a Job / ON";
            $('#' + permissionId).val("True");
        } else {
            document.getElementById(userId).innerText = "Post a Job / OFF";
            $('#' + permissionId).val("False");
        }
    });
    editClientModal.open({
        userId: userId,
        jobPostingPermission: $('#' + permissionId).val()
    });

}