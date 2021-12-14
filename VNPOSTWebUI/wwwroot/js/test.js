'use strict'

$(document).ready(function () {
    $('#call').click(() => {
        Swal.fire(
            'The Internet?',
            'That thing is still around?',
            'question'
        )
    });
});