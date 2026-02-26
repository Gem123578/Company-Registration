function showRegister() {
    hideAll();
    document.getElementById("register").style.display = "block";
}

function showReregister() {
    hideAll();
    document.getElementById("reregister").style.display = "block";
}

function hideAll() {
    document.getElementById("home").style.display = "none";
    document.getElementById("register").style.display = "none";
    document.getElementById("reregister").style.display = "none";
}