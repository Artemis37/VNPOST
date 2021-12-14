'use strict'

window.onload = () => {
    const cookie = document.cookie.split('=');
    const name = cookie[0];
    const result = cookie[1] === 'True';
    if (name === 'unitAddResult' && result) {
        Swal.fire({
            title: 'Thêm đơn vị thành công',
            icon: 'success',
            confirmButtonText: 'Đồng ý'
        })
            .then(result => {
                console.log(result);
            });
    } else if (name === 'unitUpdateResult' && result) {
        Swal.fire({
            title: 'Sửa đơn vị thành công',
            icon: 'success',
            confirmButtonText: 'Đồng ý'
        })
    } else if (document.cookie !== '' && result === false) {
        Swal.fire({
            title: 'Thao tác đơn vị thất bại',
            text: 'Kiểm tra lại thông tin đơn vị',
            icon: 'error',
        });
    }
    document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
}