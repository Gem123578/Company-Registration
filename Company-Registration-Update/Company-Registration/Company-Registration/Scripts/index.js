function showContent(sectionId, btn) {
    ["home", "register", "reregister"].forEach(sec => {
        document.getElementById(sec).classList.remove("active");
    });

    document.getElementById(sectionId).classList.add("active");

    document.querySelectorAll(".menu-btn").forEach(b => b.classList.remove("active"));
    btn.classList.add("active");
}

function loadSystemUsers(btn) {
    // Remove active class from sections if they exist
    ["home", "register", "reregister"].forEach(sec => {
        const el = document.getElementById(sec);
        if (el) el.classList.remove("active");
    });

    const reregisterSection = document.getElementById("reregister");
    if (reregisterSection) reregisterSection.classList.add("active");

    // Only add active to button if btn exists
    if (btn) {
        document.querySelectorAll(".menu-btn").forEach(b => b.classList.remove("active"));
        btn.classList.add("active");
    }

    // Load system users
    const container = document.getElementById("systemUserContainer");
    if (container) {
        const url = container.dataset.gridUrl;
        fetch(url)
            .then(res => res.text())
            .then(html => container.innerHTML = html);
    }
}
    function showUserModal(html) {
        document.getElementById("userModalContent").innerHTML = html;

        const modal = new bootstrap.Modal(document.getElementById("userModal"));
        modal.show();
    }


function openCreateUserModal() {
    var url = document.getElementById("systemUserContainer").dataset.createUrl;
    fetch(url)
        .then(res => res.text())
        .then(html => showUserModal(html));
}

function openEditUserModal(userId) {
    var url = document.getElementById("systemUserContainer").dataset.createUrl + "?id=" + userId;
    fetch(url)
        .then(res => res.text())
        .then(html => showUserModal(html));
}

function deleteUser(id) {

    if (!confirm("Are you sure you want to delete this user?"))
        return;

    const url = `/api/SystemUser/DeleteUser/${id}`;

    fetch(url, {
        method: 'DELETE',
        credentials: 'include' // session/cookie အတွက်
    })
        .then(res => res.json())
        .then(data => {
            alert(data.Message || "Delete finished");
            if (data.IsSuccess) {
                // user list refresh
                loadSystemUsers();
            }
        })
        .catch(err => {
            console.error("Delete Error:", err);
            alert("Delete failed");
        });
}


document.addEventListener("submit", function (e) {
    if (e.target && e.target.id === "userForm") {
        e.preventDefault(); //  prevent normal submit

        const form = e.target;
        const formData = new FormData(form);

        fetch(form.action, {
            method: "POST",
            body: formData
        })
            .then(res => res.json())
            .then(data => {
                if (data.success) {
                    // Close modal
                    const modalEl = document.getElementById("userModal");
                    const modal = bootstrap.Modal.getInstance(modalEl);
                    modal.hide();

                    //  Refresh grid
                    loadSystemUsers();

                } else {
                    alert(data.message || "Error occurred");
                }
            })
            .catch(err => console.error(err));
    }
});
