
var connection = new signalR.HubConnectionBuilder().withUrl("/connection").build();
connection.start();

document.getElementById("chats").addEventListener("click", async () => {
    document.getElementById("main_frame").src = "/chat.html";
});

document.getElementById("friends").addEventListener("click", async () => {
    document.getElementById("main_frame").src = "/friend.html";
});

document.getElementById("profile").addEventListener("click", async () => {
    document.getElementById("main_frame").src = "/profile.html";
});

document.getElementById("logOut").addEventListener("click", async () => {
    const response = await fetch("/logout", {
        method: "GET",
        headers: { "Accept": "application/json", "Content-Type": "application/json" }
    });
    if (response.ok === true) {
        window.location.href = "/";
    }
});