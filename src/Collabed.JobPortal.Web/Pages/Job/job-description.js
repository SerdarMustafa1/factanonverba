$(function () {

});

function onExternalApply(company, title, url) {
    var externalApplicationModal = new abp.ModalManager('/Job/ExternalApplicationModal');

    externalApplicationModal.open({
        hiringCompany: company,
        jobTitle: title
    });

    window.open(url, '_blank')
}

function applyForInternal(reference) {
    if (!abp.currentUser.id) {
        location.href = "/Account/Login?returnUrl=" + window.location.href;
    }

    jobPortal.jobs.job.checkIfAlreadyApplied(reference, abp.currentUser.id)
        .then((result) => {
            if (result === true) {
                abp.message.error('You have already applied for this job');
            } else {
                location.href = "/Job/Apply/ContactDetails?jobReference=" + reference;
            }
            
        }).catch((error) => {
            return console.log(error);
        });
}