//const { functions } = require("lodash");

$(document).ready(function () {
    $('#loadNewsGroup option').click(function (e) {
        e.preventDefault();
        var majorGroupId = $('#loadNewsGroup option:selected').val();

        console.log(majorGroupId);

        $.ajax({
            url: '/manage/loadNewsGroup',
            type: 'POST',
            dataType: 'JSON',
            data: {
                id: majorGroupId
            }
        }).done(function (result) {
            console.log(result);
            //Clear all previous value
            $('#newsGroup')
                .find('option')
                .remove();

            $('#newsGroup').append($('<option>', {
                value: '',
                text: 'Choose one...'
            }));

            const obj = JSON.parse(result);
            var display = "";
            for (var key in obj) {
                if (obj.hasOwnProperty(key)) {
                    $('#newsGroup').append($('<option>', {
                        value: key,
                        text: obj[key]
                    }));
                    //$('#newsGroup').append(new Option(obj[key],key));
                }
            }
        });
    });
});
