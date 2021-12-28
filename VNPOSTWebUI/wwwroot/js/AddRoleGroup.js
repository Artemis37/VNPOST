'use strict'

window.onload = () => {
    const addResult = document.cookie.split('=')[1] === 'True';
    const name = document.cookie.split('=')[0];
    if (addResult) {
        Swal.fire({
            icon: 'success',
            title: 'Thêm thành công'
        })
    } else if (!addResult && document.cookie !== '') {
        Swal.fire({
            icon: 'error',
            title: 'Thêm thất bại',
            text: 'Đã tồn tại nhóm'
        })
    }
    document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
}