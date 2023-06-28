$(function () {

});

function onJobDelete(jobRef) {
    var deleteJobModal = new abp.ModalManager('/JobListings/DeleteJobListingModal');

    deleteJobModal.onResult(function () {
        jobPortal.jobs.job.deactivateJob(jobRef);
        document.getElementById(jobRef).remove();
    });
    deleteJobModal.open({
        reference: jobRef
    });

}