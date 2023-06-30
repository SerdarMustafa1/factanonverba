$(function () {

});

function onJobDelete(jobRef, status) {
    if (status == 2 || document.getElementById('status' + jobRef).innerText == 'Deleted') {
        return;
    }

    var deleteJobModal = new abp.ModalManager('/JobListings/DeleteJobListingModal');

    deleteJobModal.onResult(function () {
        jobPortal.jobs.job.deactivateJob(jobRef);
        document.getElementById('status' + jobRef).innerText = 'Deleted';
        document.getElementById('days' + jobRef).innerText = '-';
        document.getElementById('delete' + jobRef).classList.add('link-disabled');
        document.getElementById('delete' + jobRef).removeAttribute("href");
        document.getElementById('edit' + jobRef).classList.add('link-disabled');
        document.getElementById('edit' + jobRef).removeAttribute("href");
        document.getElementById('title' + jobRef).removeAttribute("href");
        document.getElementById('title' + jobRef).style.textDecoration = "none";
        document.getElementById('title' + jobRef).style.color = "#212529";
    });
    deleteJobModal.open({
        reference: jobRef
    });

}