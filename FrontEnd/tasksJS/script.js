$(document).ready(function () {

    const apiUrl = 'https://localhost:7063/api/tasks';
    var editPriorityId = 0;
    var editStatusId = 0;

    // Load content into the specified div
    loadPage();
    function loadPage() {
        // Page-specific scripts here... 
        if (window.location.pathname.endsWith('index.html')) {
            // Code for index.html
            loadTasks();
        }
        if (window.location.pathname.endsWith('create.html')) {
            createValidationForm();
            loadTaskPriorities("CREATE");
            loadTaskStatuses("CREATE");
            $('#dueDate').datetimepicker({
                format: 'YYYY-MM-DD',
                date: new Date()
            });

        }
        if (window.location.pathname.endsWith('edit.html')) {
            // Code for edit.html
            const urlParams = new URLSearchParams(window.location.search);
            const taskId = urlParams.get('taskId');
            if (taskId) {
                loadTaskDetails(taskId);
                loadTaskPriorities("EDIT");
                loadTaskStatuses("EDIT");
                editValidationForm();
            }
            $('#dueDate').datetimepicker({
                format: 'YYYY-MM-DD'
            });
        }
        if (window.location.pathname.endsWith('detail.html')) {
            const urlParams = new URLSearchParams(window.location.search);
            const taskId = urlParams.get('taskId');
            if (taskId) {
                loadTaskDetails(taskId);
                loadTaskPriorities("EDIT");
                loadTaskStatuses("EDIT");
            }
            $('#dueDate').datetimepicker({
                format: 'YYYY-MM-DD'
            });
        }

        $('.index-link').each(function () {
            const url = `index.html`;
            $(this).attr('href', url);
        });
        $('.create-link').each(function () {
            $(this).attr('href', `create.html`);
        });
    }

    function loadTaskPriorities(action) {
        $.ajax({
            url: `${apiUrl}/priorities`,
            type: 'GET',
            success: function (priorities) {
                $('#prioritySelect2').select2({
                    theme: 'bootstrap4',
                    placeholder: "Select Priority",
                    allowClear: true,
                    data: $.map(priorities, function (item) {
                        return {
                            text: item.priorityName,
                            id: item.priorityId
                        }
                    })
                });
                if (action == "EDIT") {
                    $('#prioritySelect2').val(editPriorityId).trigger('change');
                }
            },
            error: function () {
                alert("Error loading Priorities!");
            }
        });
    }

    function loadTaskStatuses(action) {
        $.ajax({
            url: `${apiUrl}/statuses`,
            type: 'GET',
            success: function (statuses) {
                $('#statusSelect2').select2({
                    theme: 'bootstrap4',
                    placeholder: "Select Status",
                    allowClear: true,
                    data: $.map(statuses, function (item) {
                        return {
                            text: item.statusName,
                            id: item.statusId
                        }
                    })
                });
                if (action == "EDIT") {
                    $('#statusSelect2').val(editStatusId).trigger('change');
                }
            },
            error: function () {
                alert("Error loading Statuses!");
            }
        });
    }

    // 1. Load tasks from API on list page (index.html)
    function loadTasks() {
        $.ajax({
            url: apiUrl,
            type: 'GET',
            success: function (tasks) {
                const taskList = $('#taskList');
                taskList.empty();
                tasks.forEach(task => {
                    const row = `<tr>
                        <td>${task.taskId}</td>
                        <td>${task.title}</td>
                        <td>${task.description}</td>
                        <td>${task.priorityName}</td>
                        <td>${task.dueDate}</td>
                        <td>${task.statusName}</td>
                        <td>
                            <button type="button" class="btn btn-secondary dropdown-toggle" data-toggle="dropdown">
                                Action
                            </button>
                            <div class="dropdown-menu">
                                <a href=${buildURL("EDIT", task.taskId)} class="dropdown-item">Edit</a>
                                <a href=${buildURL("DETAIL", task.taskId)} class="dropdown-item">Detail</a>
                                <button type="button" class="dropdown-item btn-delete" data-taskId=${task.taskId} data-toggle="modal" data-target="#modal-default">
                                    Delete</button>
                            </div>
                        </td>
                    </tr>`;
                    taskList.append(row);
                });
                assignDataTable();
            },
            error: function () {
                alert("Error loading tasks!");
            }
        });
    }

    function assignDataTable() {
        $("#taskTable").DataTable({
            responsive: true,
            lengthChange: false,
            autoWidth: false,
            buttons: ["copy", "csv", "excel", "pdf", "print", "colvis"],
            initComplete: function () {
                this.api()
                    .columns([3, 5])
                    .every(function () {
                        var column = this;

                        // Create select element and listener
                        var select = $('<select><option value="">All</option></select>')
                            .appendTo($(column.footer()).empty())
                            .on('change', function () {
                                column
                                    .search($(this).val(), { exact: true })
                                    .draw();
                            });

                        // Add list of options
                        column
                            .data()
                            .unique()
                            .sort()
                            .each(function (d, j) {
                                select.append(
                                    '<option value="' + d + '">' + d + '</option>'
                                );
                            });
                    });
            }
        }).buttons().container().appendTo('#taskTable_wrapper .col-md-6:eq(0)');
    }

    function buildURL(action, taskId) {
        if (action == "INDEX") {
            return `index.html`;
        }
        if (action == "CREATE") {
            return `create.html`;
        }
        if (action == "EDIT") {
            return `edit.html?taskId=${taskId}`;
        }
        if (action == "DETAIL") {
            return `detail.html?taskId=${taskId}`;
        }
        return `#`;
    }
    
    function loadTaskDetails(taskId) {
        $.ajax({
            url: `${apiUrl}/${taskId}`,
            type: 'GET',
            success: function (task) {
                $('#taskId').val(task.taskId);
                $('#title').val(task.title);
                $('#description').val(task.description);
                editPriorityId = task.priorityId;
                editStatusId = task.statusId;
                $('#prioritySelect2').val(task.priorityId).trigger('change');
                $('#statusSelect2').val(task.statusId).trigger('change');
                $('#dueDate').datetimepicker("date", task.dueDate);
            },
            error: function () {
                alert("Error loading task details!");
            }
        });
    }

    function saveTaskChanges() {
        const taskId = $('#taskId').val();
        let priorityId = $('#prioritySelect2').val();
        if (priorityId == '') {
            priorityId = null;
        }
        let statusId = $('#statusSelect2').val();
        if (statusId == '') {
            statusId = null;
        }
        const updatedTask = {
            title: $('#title').val(),
            description: $('#description').val(),
            priorityId: priorityId,
            statusId: statusId,
            dueDate: $('#dueDate').datetimepicker("viewDate").format("YYYY-MM-DD")
        };

        $.ajax({
            url: `${apiUrl}/${taskId}`,
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(updatedTask),
            success: function () {
                alert("Task updated successfully!");
                window.location.href = 'index.html'; // Redirect back to the list page
            },
            error: function () {
                alert("Error updating task!");
            }
        });
    }

    function editValidationForm() {
        $('.edit-validate-form').validate({
            rules: {
                title: {
                    required: true,
                    maxlength: 255
                }
            },
            messages: {
                title: {
                    required: "Please enter task title",
                    maxlength: "Your title should only be 255 characters long"
                }
            },
            submitHandler: function (form) {
                // This function is executed when the form is valid
                saveTaskChanges();
            },
            errorElement: 'span',
            errorPlacement: function (error, element) {
                error.addClass('invalid-feedback');
                element.closest('.form-group').append(error);
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass('is-invalid');
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass('is-invalid');
            }
        });
    }

    function saveTask() {
        let priorityId = $('#prioritySelect2').val();
        if (priorityId == '') {
            priorityId = null;
        }
        let statusId = $('#statusSelect2').val();
        if (statusId == '') {
            statusId = null;
        }
        const createdTask = {
            title: $('#title').val(),
            description: $('#description').val(),
            priorityId: priorityId,
            statusId: statusId,
            dueDate: $('#dueDate').datetimepicker("viewDate").format("YYYY-MM-DD")
        };

        $.ajax({
            url: `${apiUrl}/create`,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(createdTask),
            success: function () {
                alert("Task created successfully!");
                window.location.href = 'index.html'; // Redirect back to the list page
            },
            error: function () {
                alert("Error creating task!");
            }
        });
    }

    function createValidationForm() {
        $('.create-validate-form').validate({
            rules: {
                title: {
                    required: true,
                    maxlength: 255
                }
            },
            messages: {
                title: {
                    required: "Please enter task title",
                    maxlength: "Your title should only be 255 characters long"
                }
            },
            submitHandler: function (form) {
                saveTask();
            },
            errorElement: 'span',
            errorPlacement: function (error, element) {
                error.addClass('invalid-feedback');
                element.closest('.form-group').append(error);
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass('is-invalid');
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass('is-invalid');
            }
        });
    }

    $(document).on('click', '.btn-delete', function() {
        const taskId = $(this).data('taskid');
        $('#taskId').val(taskId);
    });

    $(document).on('click', '#btnDeleteConfirm', function() {
        const taskId = $('#taskId').val();
        $('#modal-default').modal('hide');
        $.ajax({
            url: `${apiUrl}/delete/${taskId}`,
            type: 'POST',
            contentType: 'application/json',
            success: function () {
                alert("Task deleted successfully!");
                window.location.href = 'index.html';
            },
            error: function () {
                alert("Error deleting task!");
            }
        });
    });
});



