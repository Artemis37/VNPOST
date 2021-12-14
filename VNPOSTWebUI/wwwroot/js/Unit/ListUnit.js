'use strict'

const action = (cell, formatterParams) => {
    const id = cell.getData().id;
    //console.log(cell.getData()._children !== undefined);
    if (cell.getData()._children !== undefined) {//categoryUnit
        return `<a href='/Unit/CategoryDetail/${id}'><i class='fas fa-info-circle'></i></a>` + 
               `<a href='/Unit/CategoryUnitAdd'><i class="fas fa-plus-circle"></i></a>` + 
               `<a href='/Unit/CategoryEdit/${id}'><i class='far fa-edit'></i></a>` + 
               `<a href='javascript:void(0)' onclick="deleteCategoryUnit('${id}')"><i class='fas fa-trash'></i></a>`;
    } else { //unit
        return `<a href='/Unit/UnitDetail/${id}'><i class='fas fa-info-circle'></i></a>` +
               `<a href='/Unit/UnitAdd'><i class="fas fa-plus-circle"></i></a>` +
               `<a href='/Unit/UnitEdit/${id}'><i class='far fa-edit'></i></a>` +
               `<a href='javascript:void(0)' onclick="deleteUnit('${id}')"><i class='fas fa-trash'></i></a>`;
    }
}

const stripedRow = (row) => {
    if (!row.getData().status) {
        row.getElement().style.color = '#d63031';
    }
    if (row.getData()._children !== undefined) {
        row.getElement().style.backgroundColor = '#ecf0f1';
    }
}

var table = new Tabulator("#table-container", {
    height: "311px",
    layout: 'fitColumns',
    rowFormatter: stripedRow,
    dataTree: true,
    dataTreeStartExpanded: true,
    headerVisible: false,
    columns: [
        { field: "name", responsive: 0, width: 630 }, //never hide this column
        { formatter: action, hozAlign: 'center' }
    ],
});

const loadData = () => {
    fetch('/Unit/GetDataForListUnit', {
        method: 'POST',
        mode: 'cors',
    })
        .then(response => response.json())
        .then(data => {
            table.setData(data);
        });
}

window.onload = () => {
    loadData();
}

const deleteCategoryUnit = (id) => {
    Swal.fire({
        title: 'Xóa loại đơn vị?',
        text: "Loại đơn vị sẽ bị vô hiệu hóa",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Xóa',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch('/Unit/CategoryUnitDelete', {
                method: 'POST',
                mode: 'cors',
                headers: {
                    'Content-Type': "application/json",
                    'id': id
                }
            })
            .then(response => response.json())
            .then(data => {
                if (data) {
                    Swal.fire(
                        'Đã xóa!',
                        'Đã xóa bản ghi',
                        'success'
                    )
                        .then(result => {
                            if (result.isConfirmed || result.isDismissed) {
                                location.reload();
                            }
                        });
                };
            });
        }
    });
}

const deleteUnit = (id) => {
    Swal.fire({
        title: 'Xóa đơn vị?',
        text: "Đơn vị sẽ bị vô hiệu hóa",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Xóa',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch('/Unit/UnitDelete', {
                method: 'POST',
                mode: 'cors',
                headers: {
                    'Content-Type': "application/json",
                    'id': id
                }
            })
                .then(response => response.json())
                .then(data => {
                    if (data) {
                        Swal.fire(
                            'Đã xóa!',
                            'Đã xóa bản ghi',
                            'success'
                        ).then(result => {
                            if (result.isConfirmed || result.isDismissed) {
                                location.reload();
                            }
                        })
                    };
                });
        }
    });
}