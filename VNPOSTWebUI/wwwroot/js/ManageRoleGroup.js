'use strict'

var actions = (cell, formatterParams) => {
    const id = cell.getData().id;
    return `<a href='/Login/ViewRoleGroup/${id}'><i class='fas fa-info-circle'></i></a>` +
        `<a href='/Login/UpdateRoleGroup/${id}'><i class='far fa-edit'></i></a>` +
        `<a href='javascript:void(0)' onclick="deleteCategoryUnit('${id}')"><i class='fas fa-trash'></i></a>` + 
        `<a href='/Login/AppointRole/${id}'><i class='fas fa-list-alt'></i></a>`;
};

var table = new Tabulator("#display-container", {
    pagination: true,
    paginationSize: 3,
    layout: 'fitColumns',
    columns: [
        { title: "Tên nhóm", field: "name", hozAlign: 'left' },
        { title: "Phạm vi ứng dụng", formatter: () => { return 'MCAS' }, hozAlign: 'left' },
        { title: "Thao tác", formatter: actions, hozAlign: 'left' }
    ]
});

window.onload = () => {
    fetch('/login/loadRoles', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
    })
        .then(response => response.json())
        .then(data => {
            table.setData(data);
            document.getElementById('record-number').innerHTML = 'Tổng số bản ghi #' + data.length;
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

document.getElementById('users-group-name').addEventListener('input', (e) => {
    table.setFilter('name', 'like', e.target.value);
    updateRecordCount();
})

const updateRecordCount = () => {
    const rowCount = table.getDataCount('active');
    document.getElementById('record-number').innerHTML = 'Tổng số bản ghi: #' + rowCount;
}

const deleteCategoryUnit = (id) => {
    Swal.fire({
        title: 'Xóa nhóm người dùng?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#fcb71e',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Xóa'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch('/login/DeleteRoleGroup', {
                method: 'POST',
                headers: {
                    "Content-Type": "application/json",
                    'Id': id
                }
            })
                .then(response => response.json())
                .then(data => {
                    if (data) {
                        Swal.fire({
                            title: 'Đã xóa!',
                            icon: 'success'
                        })
                        table.getRow(id).delete();
                    }
                })
                .catch(error => {
                    console.log(error);
                })
        }
    });
}