'use strict'

const callAJAX = () => {
    const url = '/Unit/GetCategoryUnitList';
    const response = fetch(url, {
        method: 'POST',
        mode: 'cors',
    });
    response.then(data => {
        console.log('success');
    }).catch((error) => {
        console.log('failed');
    });
}