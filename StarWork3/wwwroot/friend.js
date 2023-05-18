
let connection = new signalR.HubConnectionBuilder().withUrl("/connection").build();
connection.start();

connection.on("CheckedConnections", function (users) {
    if (document.getElementById("friendsContainer").hidden == false) {
        for (let key in users) {
            buildFriends(key, users[key]);
        }
    }
    else if (document.getElementById("addFriendContainer").hidden == false) {
        for (let key in users) {
            buildUsers(key, users[key]);
        }
    }
    else if (document.getElementById("applicationsContainer").hidden == false) {
        for (let key in users) {
            buildApplications(key, users[key]);
        }
    }
});

async function getAllUsers() {
    let response = await fetch(`/friends/get-users`, {
        method: "GET",
        headers: { "Accept": "application/json", "Content-Type": "application/json" }
    });
    if (response.ok) {
        const users = await response.json();
        connection.invoke("CheckConnections", users);
        return true;
    }
    else {
        return false;
    }
}

async function getAllFriends() {
    let response = await fetch(`/friends/get-friends`, {
        method: "GET",
        headers: { "Accept": "application/json", "Content-Type": "application/json" }
    });
    if (response.ok) {
        const users = await response.json();
        connection.invoke("CheckConnections", users);
        return true;
    }
    else {
        return false;
    }
}

async function getUserProfile(nick) {
    let response = await fetch(`/profile/get-info?nick=${nick}`, {
        method: "GET",
        headers: { "Accept": "application/json", "Content-Type": "application/json" }
    });
    if (response.ok) {
        const profileInfo = await response.json();
        document.getElementById("main").hidden = true;
        document.getElementById("profile").hidden = false;
        document.getElementById("nick").value = profileInfo.nickName;
        document.getElementById("email").value = profileInfo.email;
        document.getElementById("age").value = profileInfo.age;
        document.getElementById("description").value = profileInfo.note;
    }
}

async function addFriend(nick) {
    let response = await fetch(`/friends/add`, {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: '"' + nick + '"'
    });
    if (response.ok) {
        document.getElementById(nick).remove();
    }
}

async function getAllApplications() {
    let response = await fetch(`/friends/get-applications`, {
        method: "GET",
        headers: { "Accept": "application/json", "Content-Type": "application/json" }
    });
    if (response.ok) {
        const users = await response.json();
        connection.invoke("CheckConnections", users);
        return true;
    }
    else {
        return false;
    }
}

async function acceptApplication(nick) {
    let response = await fetch(`/friends/accept`, {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: '"' + nick + '"'
    });
    if (response.ok) {
        document.getElementById(nick).remove();
    }
}

async function cancelApplication(nick) {
    let response = await fetch(`/friends/cancel`, {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: '"' + nick + '"'
    });
    if (response.ok) {
        document.getElementById(nick).remove();
    }
}

async function getChat(nick) {

}

async function deleteFriend(nick) {
    let response = await fetch(`/friends/delete`, {
        method: "DELETE",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: '"' + nick + '"'
    });
    if (response.ok) {
        document.getElementById(nick).remove();
    }
}

document.getElementById("myFriends").addEventListener("click", async () => {
    clearBlock("usersTable");
    clearBlock("applicationsTable");
    clearBlock("friendsTable");
    if (await getAllFriends() == true) {
        document.getElementById("applicationsContainer").hidden = true;
        document.getElementById("addFriendContainer").hidden = true;
        document.getElementById("friendsContainer").hidden = false;
    }
});

document.getElementById("addFriends").addEventListener("click", async () => {
    clearBlock("usersTable");
    clearBlock("applicationsTable");
    clearBlock("friendsTable");
    if (await getAllUsers() == true) {
        document.getElementById("friendsContainer").hidden = true;
        document.getElementById("applicationsContainer").hidden = true;
        document.getElementById("addFriendContainer").hidden = false;
    }
});

document.getElementById("applications").addEventListener("click", async () => {
    clearBlock("usersTable");
    clearBlock("applicationsTable");
    clearBlock("friendsTable");
    if (await getAllApplications() == true) {
        document.getElementById("friendsContainer").hidden = true;
        document.getElementById("addFriendContainer").hidden = true;
        document.getElementById("applicationsContainer").hidden = false;
    }
});

document.getElementById("closeProfile").addEventListener("click", async () => {
    document.getElementById("main").hidden = false;
    document.getElementById("profile").hidden = true;
});

function buildUsers(nick, status) {
    let tr = createTr(nick);
    tr = addTdNick(tr, nick);
    tr = addTdStatus(tr, status);
    tr = addTdProfile(tr, nick);
    tr = addTdAdd(tr, nick);

    document.getElementById("usersTable").appendChild(tr);
}

function buildApplications(nick, status) {
    let tr = createTr(nick);
    tr = addTdNick(tr, nick);
    tr = addTdStatus(tr, status);
    tr = addTdProfile(tr, nick);
    tr = addTdAccept(tr, nick);
    tr = addTdCancel(tr, nick);

    document.getElementById("applicationsTable").appendChild(tr);
}

function buildFriends(nick, status) {
    let tr = createTr(nick);
    tr = addTdNick(tr, nick);
    tr = addTdStatus(tr, status);
    tr = addTdProfile(tr, nick);
    tr = addTdChat(tr, nick);
    tr = addTdDelete(tr, nick);

    document.getElementById("friendsTable").appendChild(tr);
}

function createTr(nick) {
    let tr = document.createElement("tr");
    tr.id = nick;
    return tr;
}

function addTdNick(tr, nick) {
    let tdNick = document.createElement("td");
    tdNick.innerText = nick;
    tr.appendChild(tdNick);
    return tr;
}

function addTdStatus(tr, status) {
    let tdStatus = document.createElement("td");
    tdStatus.innerText = status ? "В сети" : "Не в сети";
    tr.appendChild(tdStatus);
    return tr;
}

function addTdProfile(tr, nick) {
    let tdProfile = document.createElement("td");
    let profileButton = document.createElement("button");
    profileButton.classList.add("btn");
    profileButton.classList.add("btn-primary");
    profileButton.type = "button";
    profileButton.textContent = "Профиль";
    profileButton.addEventListener("click", async () => {
        await getUserProfile(nick);
    });
    tdProfile.appendChild(profileButton);
    tr.appendChild(tdProfile);
    return tr;
}

function addTdAdd(tr, nick) {
    let tdAdd = document.createElement("td");
    let addButton = document.createElement("button");
    addButton.classList.add("btn");
    addButton.classList.add("btn-primary");
    addButton.type = "button";
    addButton.textContent = "Добавить в друзья";
    addButton.addEventListener("click", async () => {
        addFriend(nick);
    });
    tdAdd.appendChild(addButton);
    tr.appendChild(tdAdd);
    return tr;
}

function addTdAccept(tr, nick) {
    let tdAccept = document.createElement("td");
    let addButton = document.createElement("button");
    addButton.classList.add("btn");
    addButton.classList.add("btn-success");
    addButton.type = "button";
    addButton.textContent = "Принять";
    addButton.addEventListener("click", async () => {
        acceptApplication(nick);
    });
    tdAccept.appendChild(addButton);
    tr.appendChild(tdAccept);
    return tr;
}

function addTdCancel(tr, nick) {
    let tdCancel = document.createElement("td");
    let addButton = document.createElement("button");
    addButton.classList.add("btn");
    addButton.classList.add("btn-danger");
    addButton.type = "button";
    addButton.textContent = "Отклонить";
    addButton.addEventListener("click", async () => {
        cancelApplication(nick);
    });
    tdCancel.appendChild(addButton);
    tr.appendChild(tdCancel);
    return tr;
}

function addTdChat(tr, nick) {
    let tdCancel = document.createElement("td");
    let addButton = document.createElement("button");
    addButton.classList.add("btn");
    addButton.classList.add("btn-primary");
    addButton.type = "button";
    addButton.textContent = "Чат";
    addButton.addEventListener("click", async () => {
        getChat(nick);
    });
    tdCancel.appendChild(addButton);
    tr.appendChild(tdCancel);
    return tr;
}

function addTdDelete(tr, nick) {
    let tdCancel = document.createElement("td");
    let addButton = document.createElement("button");
    addButton.classList.add("btn");
    addButton.classList.add("btn-danger");
    addButton.type = "button";
    addButton.textContent = "Удалить";
    addButton.addEventListener("click", async () => {
        deleteFriend(nick);
    });
    tdCancel.appendChild(addButton);
    tr.appendChild(tdCancel);
    return tr;
}

function clearBlock(name) {
    let tbody = document.getElementById(name);
    while (tbody.firstChild) {
        tbody.removeChild(tbody.firstChild);
    }
}
