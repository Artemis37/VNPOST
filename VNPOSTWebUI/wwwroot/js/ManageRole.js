"use strict";

const moveItem = (event) => {
    const movedItem = event.target;
    let assigned = false;
    document
        .getElementById("available-role")
        .childNodes.forEach((item, index) => {
            if (item === movedItem) {
                assigned = true;
            }
        });

    if (!assigned) {
        document.getElementById("available-role").appendChild(movedItem);
    } else {
        document.getElementById("chosen-role").appendChild(movedItem);
    }
};

const appendButton = (name, ParentId) => {
    const buttonEl = document.createElement("button");
    const textEl = document.createTextNode(name);
    buttonEl.appendChild(textEl);
    buttonEl.classList.add("list-group-item", "list-group-item-action");
    buttonEl.onclick = moveItem;
    document.getElementById(ParentId).appendChild(buttonEl);
};

window.onload = () => {
    const requestOptions = {
        method: "POST",
        mode: "cors",
        redirect: "follow",
    };
    let availableRoles;
    let assignedRoles;
    let id =
        window.location.pathname.split("/")[
        window.location.pathname.split("/").length - 1
        ];
    //load available roles
    const availableRolesPromise = fetch(
        "https://localhost:44358/login/loadRoles",
        requestOptions
    )
        .then((response) => {
            return response.json();
        })
        .catch((error) => console.log("error", error));

    //load roles assigned to user
    requestOptions.headers = {
        id: id,
    };
    const assignedRolesPromise = fetch(
        "https://localhost:44358/login/loadAppointedRoles",
        requestOptions
    )
        .then((response) => response.json())
        .then((data) => {
            return data;
        })
        .catch((error) => {
            console.log(error);
        });

    availableRolesPromise.then((r1) => {
        assignedRolesPromise.then((r2) => {
            //remove duplicate roles
            r2.forEach((assigned, assignedIndex) => {
                r1.forEach((available, availableIndex) => {
                    if (available.id === assigned.id) {
                        r1.splice(availableIndex, 1);
                    }
                });
            });

            //append roles child
            r1.forEach((item, index) => {
                appendButton(item.name, "available-role");
            });

            r2.forEach((item, index) => {
                appendButton(item.name, "chosen-role");
            });
        });
    });
};

const getRoleList = () => {
    const availableRoles = [];
    const assignedRoles = [];
    let id =
        window.location.pathname.split("/")[
        window.location.pathname.split("/").length - 1
        ];

    document.querySelectorAll("#available-role button").forEach((item, index) => {
        availableRoles.push({ name: item.innerText });
    });

    document.querySelectorAll("#chosen-role button").forEach((item, index) => {
        assignedRoles.push({ name: item.innerText });
    });

    const data = {
        Id: id,
        AssignedRoles: assignedRoles,
        AvailableRoles: availableRoles,
    };

    fetch("https://localhost:44358/login/updateRoleForUser", {
        method: "POST",
        mode: "cors",
        body: JSON.stringify(data),
        redirect: "follow",
        headers: {
            "Content-Type": "application/json",
        },
    })
        .then((response) => response.json())
        .then((data) => {
            if (data) {
                Swal.fire({
                    title: 'Cập nhật quyền thành công',
                    icon: 'success'
                })
            }
        });
};
