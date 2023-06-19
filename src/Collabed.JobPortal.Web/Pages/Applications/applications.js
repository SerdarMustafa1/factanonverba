$(function () {

});

function onPromote(applicationId) {
    if (!applicationId) {
        return;
    }

    jobPortal.jobs.job.progressJobApplication(applicationId, true);
    document.getElementById('status' + applicationId).innerText = 'Next stage';
    document.getElementById('next' + applicationId).classList.add('link-disabled');
    document.getElementById('next' + applicationId).removeAttribute('href');
}

function onReject(applicationId) {
    if (!applicationId) {
        return;
    }

    jobPortal.jobs.job.progressJobApplication(applicationId, false);
    document.getElementById('status' + applicationId).innerText = 'Rejected';
    document.getElementById('reject' + applicationId).classList.add('link-disabled');
    document.getElementById('reject' + applicationId).removeAttribute('href');
    if (document.getElementById('next' + applicationId)) {
        document.getElementById('next' + applicationId).classList.add('link-disabled');
        document.getElementById('next' + applicationId).removeAttribute('href');
    }
}

function onHire(applicationId, reference) {
    if (!applicationId || !reference) {
        return;
    }

    let result = jobPortal.jobs.job.hireApplicant(applicationId, reference);
    if (result) {
        elementArray = document.getElementsByClassName("hire-link");
        for (var i = 0; i < elementArray.length; i++) {
            elementArray[i].classList.add('link-disabled');
            elementArray[i].removeAttribute('href');
        }
    }
    document.getElementById('status' + applicationId).innerText = 'Hired';
    document.getElementById('hire' + applicationId).classList.add('link-disabled');
    document.getElementById('hire' + applicationId).removeAttribute('href');
}

function onNotify(reference) {
    if (!reference) {
        return;
    }

    jobPortal.jobs.job.notifyApplicants(reference)
    $("#notificationSentId").show(500, function () {
        setTimeout(function () {
            $("#notificationSentId").hide(500);
        }, 3000);
    });
}

function onToggleChange(event, jobRef) {
    let acceptingApplications = event.currentTarget.checked;
    jobPortal.jobs.job.toggleJobStatus(acceptingApplications, jobRef)
        .then((result) => {
            document.getElementById('deadlineId').innerText = result.applicationDeadline;
            document.getElementById('positionsAvailableId').innerText = result.actualPositionsAvailable;
        });
}