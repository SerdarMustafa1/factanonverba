$(document).ready(function () {
    $('#CategoriesSelected').multiselect({
        templates: {
            button: '<button type="button" class="multiselect dropdown-toggle btn btn-primary" data-bs-toggle="dropdown" aria-expanded="false"><span class="multiselect-selected-text"></span></button>',
        },
        nonSelectedText: 'Select category',
        numberDisplayed: 2
    });
    $("#CategoriesSelected").attr('name', 'Category');
    document.getElementsByTagName("html")[0].style.visibility = "visible";
});

const clearSearchTerm = () => {
    $('#keywordInput').val('');
}

const onPostSearch = () => {
    $('#currentPageHiddenInput').val('1');
}