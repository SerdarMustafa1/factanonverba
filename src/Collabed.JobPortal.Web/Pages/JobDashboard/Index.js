$(function () {
    var createModal = new abp.ModalManager(abp.appPath + 'Jobs/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Jobs/EditModal');
    var jobService = jobPortal.jobs.job;

    var dataTable = $('#JobsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(jobService.getList),
            columnDefs: [
                {
                    title: 'Actions',
                    rowAction: {
                        items:
                            [
                                {
                                    text: 'Edit',
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: 'Delete',
                                    confirmMessage: function (data) {
                                        return "Are you sure to delete the job '" + data.record.title + "'?";
                                    },
                                    action: function (data) {
                                        bookService
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.info("Successfully deleted!");
                                                dataTable.ajax.reload();
                                            });
                                    }
                                }
                            ]
                    }
                },
                {
                    title: 'Title',
                    data: "title"
                },
                {
                    title: 'Description',
                    data: "description"
                }
            ]
        })
    );

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewJobButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
