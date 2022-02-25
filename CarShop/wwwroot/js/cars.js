let dataTable;

$(document).ready(() => {
    loadDataTable();
});

const loadDataTable = () => {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Car/GetAll"
        },
        "columns": [
            { "data": "brand", "width": "10%" },
            { "data": "model", "width": "10%" },
            { "data": "price", "width": "40%" },
            { "data": "productionDate", "width": "10%" },
            { "data": "registrationDate", "width": "10%" },
            { "data": "vin", "width": "10%" },
            { "data": "registration", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Car/Upsert?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a onClick=Delete('/Admin/Car/Delete/${data}')
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
					</div>
                        `
                },
                "width": "15%"
            }
        ]
    });
}


const Delete = (url) => {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: (data) => {
                    if (data.success) {
                        location.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    });
}