'use strict'

function submitForm() {
    const formData = document.querySelectorAll('.hierarchy-checkbox');
    const data = {
        //Application
        ManageApplicationRead: formData[2].checked,
        ManageApplicationUpdate: formData[3].checked,
        ManageApplicationUserAdd: formData[4].checked,
        ManageApplicationUserUpdate: formData[5].checked,

        //User Group
        ManageUserGroupAdd: formData[7].checked,
        ManageUserGroupDelete: formData[8].checked,
        ManageUserGroupUpdate: formData[9].checked,
        ManageUserGroupDetail: formData[10].checked,
        ManageUserGroupRolesAdd: formData[11].checked,
        ManageUserGroupRead: formData[12].checked,

        //User
        ManageUserRead: formData[14].checked,
        ManageUserAdd: formData[15].checked,
        ManageUserDetail: formData[16].checked,
        ManageUserUpdateUserGroup: formData[17].checked,
        ManageUserUpdate: formData[18].checked,
    };
    fetch('', {
        mode: 'cors',
        method: 'POST',
        body: JSON.stringify(data),
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            console.log('Sent');
        })
        .catch(error => {
            console.log('Error: ' + error);
        });
}

