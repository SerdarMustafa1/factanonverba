$(function () {

});

function setInterviewDate(applicantReference) {
    let interviewDate = $("#Applicant_InterviewDate").val();
    jobPortal.jobs.job.setInterviewDate(interviewDate, applicantReference)
}

function updateApplicantRating(applicantReference) {
    let rating = $("#Applicant_Rating").val();
    jobPortal.jobs.job.updateApplicantRating(rating, applicantReference)
}

function onPromote(applicationId) {
    if (!applicationId) {
        return;
    }

    jobPortal.jobs.job.progressJobApplication(applicationId, true);
    document.getElementById('promoteBtn').setAttribute("disabled", true);
}

function onReject(applicationId) {
    if (!applicationId) {
        return;
    }

    jobPortal.jobs.job.progressJobApplication(applicationId, false);
    document.getElementById('rejectBtn').setAttribute("disabled", true);
}

function onHire(applicationId, reference) {
    if (!applicationId) {
        return;
    }

    jobPortal.jobs.job.hireApplicant(applicationId, reference);
    document.getElementById('hireBtn').setAttribute("disabled", true);
}