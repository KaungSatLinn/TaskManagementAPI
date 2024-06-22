$(document).ready(function () {

    // Base URL for your API
    const apiUrl = 'https://localhost:7063/api/tasks'; // Replace with your actual API URL

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
            loadTaskPriorities();
            loadTaskStatuses();
            $('#dueDate').datetimepicker({
                format: 'L'
            });
            
        }
        if (window.location.pathname.endsWith('edit.html')) {
            // Code for edit.html
            const urlParams = new URLSearchParams(window.location.search);
            const taskId = urlParams.get('taskId');
            if (taskId) {
                loadTaskDetails(taskId);
                loadTaskPriorities();
                loadTaskStatuses();
                editValidationForm();
            }
            $('#dueDate').datetimepicker({
                format: 'L'
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

    function loadTaskPriorities() {
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
            },
            error: function () {
                alert("Error loading Priorities!");
            }
        });
    }

    function loadTaskStatuses() {
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
                            <a href=${buildURL("EDIT", task.taskId)}>Edit</a> |
                            <a href=${buildURL("DETAILS", task.taskId)}>Details</a> |
                            <a href=${buildURL("DELETE", task.taskId)}>Delete</a>
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
                                    .search($(this).val(), {exact: true})
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
        if (action == "DETAILS") {
            return `details.html?taskId=${taskId}`;
        }
        return `#`;
    }

    // 2. Handle "Edit" button clicks on list page
    $(document).on('click', '.btn-edit', function () {
        const taskId = $(this).data('task-id');
        window.location.href = `edit.html?taskId=${taskId}`;
    });

    function loadTaskDetails(taskId) {
        $.ajax({
            url: `${apiUrl}/${taskId}`,
            type: 'GET',
            success: function (task) {
                $('#taskId').val(task.taskId);
                $('#title').val(task.title);
                $('#description').val(task.description);
                // ... set values for priority and status (you might need dropdowns)
            },
            error: function () {
                alert("Error loading task details!");
            }
        });
    }

    function saveTaskChanges() {
        const taskId = $('#taskId').val();
        const updatedTask = {
            title: $('#title').val(),
            description: $('#description').val(),
            // ... priorityId and statusId (get values from dropdowns)
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
        const createdTask = {
            title: $('#title').val(),
            description: $('#description').val(),
            // ... priorityId and statusId (get values from dropdowns)
        };

        $.ajax({
            url: `${apiUrl}`,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(createdTask),
            success: function () {
                alert("Task created successfully!");
                window.location.href = 'index.html'; // Redirect back to the list page
            },
            error: function () {
                alert("Error updating task!");
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
});



