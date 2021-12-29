'use strict'

window.onload = () => {
    const result = document.cookie.split('=')[1] === 'True';
    const name = document.cookie.split('=')[0];
    if (result && name === 'resultAddRoleGroup') {
        Swal.fire({
            icon: 'success',
            title: 'Thêm thành công'
        })
    } else if (result && name === 'updateRoleGroupResult') {
        Swal.fire({
            icon: 'success',
            title: 'Sửa thành công'
        });
    } else if (!result && document.cookie !== '') {
        Swal.fire({
            icon: 'error',
            title: 'Lỗi'
        })
    }
    document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
}