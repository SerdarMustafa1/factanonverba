$(function () {

});

function onJobDelete(jobRef, status) {
    if (status == 2 || document.getElementById('status' + jobRef).innerText == 'Deleted') {
        return;
    }

    var deleteJobModal = new abp.ModalManager('/JobListingsFull/DeleteJobListingModal');

    deleteJobModal.onResult(function () {
        jobPortal.jobs.job.deactivateJob(jobRef);
        document.getElementById('status' + jobRef).innerText = 'Deleted';
        document.getElementById('days' + jobRef).innerText = '-';
        document.getElementById('delete' + jobRef).classList.add('link-disabled');
        document.getElementById('delete' + jobRef).removeAttribute("href");
        document.getElementById('edit' + jobRef).classList.add('link-disabled');
        document.getElementById('edit' + jobRef).removeAttribute("href");
    });
    deleteJobModal.open({
        reference: jobRef
    });

}