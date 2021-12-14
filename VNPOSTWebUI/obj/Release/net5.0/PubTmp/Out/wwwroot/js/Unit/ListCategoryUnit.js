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
    return "<a href='#'><i class='fas fa-info-circle'></i></a>" +
        "<a href='#'><i class='far fa-edit'></i></a>" +
        "<a href='#'><i class='fas fa-trash'></i></a>";;
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
        { title: "Thao tác", formatter: actions, hozAlign: 'left' }
    ]
});


window.onload = function () {
    const response = fetch('/Unit/GetCategoryUnitList', {
        method: 'POST',
        mode: 'cors'
    })
        .then(response => response.json())
        .then(data => {
            console.log(data);
            table.setData(data);
            document.getElementById('record').innerHTML = 'Tổng số bản ghi: #' + data.length;
        })
        .catch(error => {
            console.log(error)
        });
}

document.getElementById('category-unit-id').addEventListener('input',(e) => {
    const id = document.getElementById('category-unit-id').value;
    table.setFilter('id', 'like', id);
});

document.getElementById('category-unit-name').addEventListener('input', () => {
    const name = document.getElementById('category-unit-name').value;
    table.setFilter('name', 'like', name);
});

document.getElementById('all').onclick = () => {
    table.clearFilter('status');
};

document.getElementById('valid').onclick = () => {
    table.setFilter('status', '=', 1);
};

document.getElementById('expire').onclick = () => {
    table.setFilter('status', '=', 0);
};