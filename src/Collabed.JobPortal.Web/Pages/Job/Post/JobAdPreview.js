window.onload = function () {
    (function () {

    })();
}


const publishButtonOnClick = (event) => {
    event.preventDefault();
    const form = document.getElementById('previewForm');
    form.action = '';
    abp.message.success('You have successfully published a job!');
    setTimeout(function () {
        form.submit();
    }, 2000);
    
}

const backButtonOnClick = (event) => {
    event.preventDefault();
    // set action on form and then submit
    const form = document.getElementById('previewForm');
    form.action = '/Job/Post';
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