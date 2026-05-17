function showContent(sectionId, btn) {
    ["home", "register", "reregister"].forEach(sec => {
        const el = document.getElementById(sec);
        if (el) el.classList.remove("active");
    });

    const sectionEl = document.getElementById(sectionId);
    if (sectionEl) sectionEl.classList.add("active");

    document.querySelectorAll(".menu-btn").forEach(b => b.classList.remove("active"));
    if (btn) btn.classList.add("active");
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
    if (!confirm("Are you sure you want to delete this user?")) return;

    // Get the URL from dataset to keep it dynamic like create/update
    const container = document.getElementById("systemUserContainer");
    if (!container) return;

    const url = container.dataset.deleteUrl + "/" + id; // dataset.deleteUrl should be set in Razor

    fetch(url, {
        method: 'POST', // matches your MVC controller
        credentials: 'include',
        headers: {
            'Accept': 'application/json'
        }
    })
        .then(res => {
            if (!res.ok) throw new Error("Server error " + res.status);
            return res.json();
        })
        .then(data => {
            if (data.success) {
                // Remove row from table
                const row = document.getElementById("row-" + id);
                if (row) row.remove();

                // Optional: show message like create/update
                const alertContainer = document.getElementById("alertContainer");
                if (alertContainer) {
                    alertContainer.innerHTML = `<div class="alert alert-success">${data.message || "User deleted successfully"}</div>`;
                    setTimeout(() => alertContainer.innerHTML = '', 3000);
                }

                // Refresh grid
                loadSystemUsers();

            } else {
                alert(data.message || "Delete failed");
            }
        })
        .catch(err => {
            console.error(err);
            alert("Delete error: " + err.message);
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

document.addEventListener("DOMContentLoaded", function () {

    var emailModal = document.getElementById('emailModal');
    if (emailModal) {
        new bootstrap.Modal(emailModal).show();
    }

    var expireModal = document.getElementById('expireModal');
    if (expireModal) {
        new bootstrap.Modal(expireModal).show();
    }
});
function resendEmail() {
    var email = '@TempData["ExpiredEmail"]';

    fetch('/CompanyApplicant/ResendConfirmation', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ email: email })
    })
        .then(res => res.json())
        .then(data => {
            alert(data.message);
        });
}