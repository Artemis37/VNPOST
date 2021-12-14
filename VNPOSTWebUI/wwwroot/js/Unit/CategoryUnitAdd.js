'use strict'

window.onload = function () {
    const result = document.cookie.split('=')[1] === 'True';
    const name = document.cookie.split('=')[0];
    if (result) {
        if (name === 'addResult') {
            Swal.fire({
                title: 'Thêm Thành Công',
                icon: 'success'
            });
        } else if (name === 'updateResult') {
            Swal.fire({
                title: 'Lưu Thành Công',
                icon: 'success'
            });
        }
        document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
    } else if (document.cookie.length !== 0 && !result) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!'
        });
        document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
    }
}