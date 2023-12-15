window.onload = function () {
    (function () {

    })();
}


const updateButtonOnClick = (event) => {
    event.preventDefault();
    const form = document.getElementById('previewForm');
    form.action = '';
    abp.message.success('You have successfully updated a job!');
    setTimeout(function () {
        form.submit();
    }, 2000);
    
}

const backButtonOnClick = (event) => {
    event.preventDefault();
    // set action on form and then submit
    const form = document.getElementById('previewForm');
    let id = document.getElementById("jobReferenceHiddenInput").value;
    console.log("id = " + id);
    form.action = '/Job/Edit?reference='+id;
    form.submit();
}

const saveButtonOnClick = (event) => {
    event.preventDefault();
    alert('saved for later');

    // SUBMIT FORM
}

const shareButtonOnClick = (event) => {
    event.preventDefault();
    alert('shared with someone');

    // SUBMIT FORM
}

const applyNowOnClick = (event) => {
    event.preventDefault();
    alert('submitted!');
}