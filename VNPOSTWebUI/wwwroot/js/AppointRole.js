'use strict'

function submitForm() {
    const id = window.location.pathname.split('/')[window.location.pathname.split('/').length - 1];
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
            'Content-Type': 'application/json',
            'RoleId': id
        }
    })
        .then(() => {
            const result = document.cookie.split("=")[0];
            const value = document.cookie.split("=")[1] === 'True';
            if (result === 'changeRoleGroupResult' && value) {
                Swal.fire({
                    title: 'Thay đổi quyền thành công',
                    icon: 'success'
                })
            }
            document.cookie = `${result}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
        });
}

window.onload = () => {
    const id = window.location.pathname.split('/')[window.location.pathname.split('/').length - 1];
    fetch('/login/LoadClaims', {
        mode: 'cors',
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RoleId': id
        }
    })
        .then(response => response.json())
        .then(data => {
            const formData = document.querySelectorAll('.hierarchy-checkbox');
            //Application
            if (data.manageApplicationRead) {
                formData[2].checked = true;
            }
            if (data.manageApplicationUpdate) {
                formData[3].checked = true;
            }
            if (data.manageApplicationUserAdd) {
                formData[4].checked = true;
            }
            if (data.manageApplicationUserUpdate) {
                formData[5].checked = true;
            }

            //User Group
            if (data.manageUserGroupAdd) {
                formData[7].checked = true;
            }
            if (data.manageUserGroupDelete) {
                formData[8].checked = true;
            }
            if (data.manageUserGroupUpdate) {
                formData[9].checked = true;
            }
            if (data.manageUserGroupDetail) {
                formData[10].checked = true;
            }
            if (data.manageUserGroupRolesAdd) {
                formData[11].checked = true;
            }
            if (data.manageUserGroupRead) {
                formData[12].checked = true;
            }

            //User
            if (data.manageUserRead) {
                formData[14].checked = true;
            }
            if (data.manageUserAdd) {
                formData[15].checked = true;
            }
            if (data.manageUserDetail) {
                formData[16].checked = true;
            }
            if (data.manageUserUpdateUserGroup) {
                formData[17].checked = true;
            }
            if (data.manageUserUpdate) {
                formData[18].checked = true;
            }
        })
};