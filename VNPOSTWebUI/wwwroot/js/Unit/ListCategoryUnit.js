'use strict'

const tempData = [
    {
        categoryUnitId: 'abc123',
        categoryUnitName: 'my name',
        categoryUnitStatus: 'Có hiệu lực'
    },
    {
        categoryUnitId: 'xyz456',
        categoryUnitName: 'her name',
        categoryUnitStatus: 'Hết hiệu lực'
    }
];

var actions = function (cell, formatterParams) {
    const id = cell.getData().id;
    return `<a href='/Unit/CategoryDetail/${id}'><i class='fas fa-info-circle'></i></a>` +
        `<a href='/Unit/CategoryEdit/${id}'><i class='far fa-edit'></i></a>` +
        `<a href='javascript:void(0)' onclick="deleteCategoryUnit('${id}')"><i class='fas fa-trash'></i></a>`;
};

const mutateStatus = (cell, formatterParams, onRendered) => {
    return cell.getValue() ? 'Có hiệu lực' : 'Hết hiệu lực';
};

var table = new Tabulator("#display-container", {
    pagination: true,
    paginationSize: 3,
    layout: 'fitColumns',
    columns: [
        { title: "STT", formatter: 'rownum', width: 30 },
        { title: "Mã loại đơn vị", field: "id", hozAlign: 'center' },
        { title: "Tên loại đơn vị", field: "name", hozAlign: 'left' },
        { title: "Trạng thái", field: "status", formatter: mutateStatus, hozAlign: 'left' },
        { title: "Thao tác", formatter: actions, formatterParams: { id: 1 }, hozAlign: 'left' }
    ]
});


window.onload = function () {
    const response = fetch('/Unit/GetCategoryUnitList', {
        method: 'POST',
        mode: 'cors'
    }).then(response => response.json())
       .then(data => {
           table.setData(data);
           document.getElementById('record').innerHTML = 'Tổng số bản ghi: #' + data.length;
       })
       .catch(error => {
           console.log(error)
       });
}

const updateRecordCount = () => {
    const rowCount = table.getDataCount('active');
    document.getElementById('record').innerHTML = 'Tổng số bản ghi: #' + rowCount;
}

document.getElementById('category-unit-id').addEventListener('input',(e) => {
    const id = document.getElementById('category-unit-id').value;
    table.setFilter('id', 'like', id);
    updateRecordCount();
});

document.getElementById('category-unit-name').addEventListener('input', () => {
    const name = document.getElementById('category-unit-name').value;
    table.setFilter('name', 'like', name);
    updateRecordCount();
});

document.getElementById('all').onclick = () => {
    table.clearFilter('status');
    updateRecordCount();
};

document.getElementById('valid').onclick = () => {
    table.setFilter('status', '=', 1);
    updateRecordCount();
};

document.getElementById('expire').onclick = () => {
    table.setFilter('status', '=', 0);
    updateRecordCount();
};

const deleteCategoryUnit = (id) => {
    Swal.fire({
        title: 'Xóa đơn vị?',
        text: "Loại đơn vị sẽ bị vô hiệu hóa",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Xóa',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            const row = table.searchRows('id', '=', id);
            fetch('/Unit/CategoryUnitDelete', {
                method: 'POST',
                mode: 'cors',
                headers: {
                    'Content-Type': "application/json",
                    'id': id
                }
            }).then(response => response.json())
                .then(data => {
                    if (data) {
                        Swal.fire(
                            'Đã vô hiệu hóa!',
                            'Đã vô hiệu hóa loại đơn vị',
                            'success'
                        )
                        table.updateData([{id: id, status: 0}]);
                    };
                });
        }
    });
}