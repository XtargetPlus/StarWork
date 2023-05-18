
let connection = new signalR.HubConnectionBuilder().withUrl("/connection").build();
connection.start();

window.onload = getAllChats;

let friendsId = []
let lastActiveChatId = 0;

async function getAllFriends() {
    let response = await fetch(`/friends/get-friends-with-id`, {
        method: "GET",
        headers: { "Accept": "application/json", "Content-Type": "application/json" }
    });
    if (response.ok) {
        const users = await response.json();
        for (let number in users) {
            buildFriends(users[number]);
        }
        return true;
    }
    else {
        return false;
    }
}

async function getAllChats() {
    let response = await fetch(`/chats/get-all`, {
        method: "GET",
        headers: { "Accept": "application/json", "Content-Type": "application/json" }
    });
    if (response.ok) {
        const chats = await response.json();
        for (let number in chats) {
            buildChatInLists(chats[number].id,
                chats[number].title,
                chats[number].lastMessage == null ? "" : chats[number].lastMessage,
                chats[number].postedTime == null ? "" : chats[number].postedTime);
        }
    }
}

async function createChat(chatName, chatNote) {
    let response = await fetch(`/chats/add`, {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            title: chatName,
            note: chatNote,
            usersId: friendsId
        })
    });
    if (response.ok) {
        buildChatInLists(await response.json(), document.getElementById("creatingChatName").value, "", "");
        document.getElementById("creatingChatName").value = "";
        document.getElementById("creatingChatNote").value = "";
        clearBlock("friendsTable");
        document.getElementById("createChatContainer").hidden = true;
        document.getElementById("chatInfo").hidden = true;
    }
}

async function openChat(chatId) {
    let response = await fetch(`/chats/get-all-messages?chatId=${chatId}`, {
        method: "GET",
        headers: { "Accept": "application/json", "Content-Type": "application/json" }
    });
    if (response.ok) {
        clearBlock("messages");
        document.getElementById("chatContainer").hidden = false;
        document.getElementById("createChatContainer").hidden = true;
        document.getElementById("chatInfo").hidden = true;
        buildChat(chatId, document.getElementById(`${chatId}12`).innerHTML, await response.json());
    }
}

async function getMoreChatInfo(chatId) {
    let response = await fetch(`/chats/get-info?chatId=${chatId}`, {
        method: "GET",
        headers: { "Accept": "application/json", "Content-Type": "application/json" }
    });
    if (response.ok) {
        clearBlock("infoUsersInChatTable");
        clearBlock("chatInfoButtons");
        document.getElementById("chatInfo").hidden = false;
        document.getElementById("chatContainer").hidden = true;
        document.getElementById("profile").hidden = true;
        buildChatInfo(await response.json());
    }
}

async function getUserProfile(chatId, nick) {
    let response = await fetch(`/profile/get-info?nick=${nick}`, {
        method: "GET",
        headers: { "Accept": "application/json", "Content-Type": "application/json" }
    });
    if (response.ok) {
        document.getElementById("backToChat").value = chatId;

        document.getElementById("profile").hidden = false;
        document.getElementById("chatInfo").hidden = true;

        const profileInfo = await response.json();
        document.getElementById("nick").value = profileInfo.nickName;
        document.getElementById("email").value = profileInfo.email;
        document.getElementById("age").value = profileInfo.age;
        document.getElementById("description").value = profileInfo.note;
    }
}

async function deleteUserFromChat(userId, chatId) {
    let response = await fetch(`/chats/expel-user`, {
        method: "DELETE",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            userId: userId,
            chatId: chatId
        })
    });
    if (response.ok) {
        try {
            var tr = document.getElementById(userId);
            document.getElementById("infoUsersInChatTable").removeChild(tr);
        }
        catch {
            document.getElementById("chatInfo").hidden = true;
            document.getElementById("chatContainer").hidden = true;
            document.getElementById("profile").hidden = true;

            await getAllChats();
        }
    }
}

async function updateChatInfo(chatId, chatName, note) {
    let response = await fetch(`/chats/update-info`, {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            chatId: chatId,
            chatName: chatName,
            note: note
        })
    });
    if (response.ok) {
        document.getElementById("infoChatName").disabled = true;
        document.getElementById("infoChatNote").disabled = true;

        document.getElementById("editChatInfo").hidden = false;
        document.getElementById("closeChatInfo").hidden = false;
        document.getElementById("exitFromChat").hidden = false;

        document.getElementById("updateChatInfo").hidden = true;
        document.getElementById("cancelEditChatInfo").hidden = true;

        document.getElementById(`${chatId}12`).innerText = chatName;
    }
}

async function sendMessage(chatId, message) {
    let response = await fetch(`/chats/send-message`, {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            chatId: chatId,
            message: message
        })
    });
    if (response.ok) {
        document.getElementById("textMessage").value = "";

        let messageInfo = await response.json();
        let messageDiv = addDiv(["row", "p-r-10"]);
        let messageInfoDiv = addDiv(["d-flex", "justify-content-end"]);
        let userP = addPWithStyle("margin: 0px", ["h5"], `${messageInfo.nickName}&nbsp;`);
        let postingP = addPWithStyle("margin: 0px", [], messageInfo.dateTime);
        let messageP = document.createElement("p");
        messageP.classList.add("text-end");
        messageP.innerText = messageInfo.text;
        messageP.id = `message${messageInfo.messageId}`;

        messageInfoDiv.appendChild(userP);
        messageInfoDiv.appendChild(postingP);
        messageDiv.appendChild(messageInfoDiv);
        messageDiv.appendChild(messageP);

        document.getElementById(`${chatId}31`).innerText = messageInfo.text;
        document.getElementById(`${chatId}32`).innerText = messageInfo.dateTime;
        document.getElementById("messages").appendChild(messageDiv);
    }
}

document.getElementById("createChatButton").addEventListener("click", async () => {
    if (lastActiveChatId != 0) {
        let lastEventA = document.getElementById(lastActiveChatId);
        lastEventA.firstChild.style = "";
    }
    document.getElementById("chatContainer").hidden = true;
    document.getElementById("chatInfo").hidden = true;
    document.getElementById("createChatContainer").hidden = false;
    clearBlock("friendsTable");
    await getAllFriends();
});

document.getElementById("creatingChat").addEventListener("click", async () => {
    let creatingChatName = document.getElementById("creatingChatName").value;
    let creatingChatNote = document.getElementById("creatingChatNote").value;
    await createChat(creatingChatName, creatingChatNote);
});

document.getElementById("closeProfile").addEventListener("click", async () => {
    await getMoreChatInfo(document.getElementById("backToChat").value);
});

function buildFriends(user) {
    let tr = createTr(user.nickName);
    tr = addTdNick(tr, user.nickName);
    tr = addTdCheckbox(tr, user.id)

    document.getElementById("friendsTable").appendChild(tr);
}

function buildChatInLists(chatId, chatName, lastMessage, time) {
    let a = createA(chatId);
    let divFather = addDiv(["row", "rounded-2", "p-t-15", "p-b-15"]);
    let divChildFirst = addDiv(["m-l-10", "col-8", "text-dark"]);
    let divChildSecond = addDiv(["col-auto", "d-flex", "align-items-end", "text-dark"]);
    let pChatName = addPWithStyleAndId("margin: 0px;", ["d-flex", "align-items-center", "h5"], chatName, `${chatId}12`);
    let pLastMessage = addPWithStyle("margin: 0px;", ["d-flex", "align-items-center"], lastMessage);
    pLastMessage.id = `${chatId}31`;
    let pTime = addPWithStyle("margin: 0px;", ["d-flex", "align-items-center"], time);
    pTime.id = `${chatId}32`;

    let divBrFather = addDiv(["row"]);
    divBrFather.id = `${chatId}21`;
    let divBrChildFirst = addDiv(["col-3"]);
    let divBrChildSecond = addDiv(["col-6", "border", "border-dark", "rounded-2", "bg-white"]);

    divBrFather.appendChild(divBrChildFirst);
    divBrFather.appendChild(divBrChildSecond);

    divChildFirst.appendChild(pChatName);
    divChildFirst.appendChild(pLastMessage);
    divChildSecond.appendChild(pTime);
    divFather.appendChild(divChildFirst);
    divFather.appendChild(divChildSecond);
    a.appendChild(divFather);

    document.getElementById("chats").appendChild(a);
    document.getElementById("chats").appendChild(divBrFather);
}

function buildChat(chatId, chatName, messages) {
    clearBlock("chatMoreInfoButton");
    clearBlock("chatSendMessageButton");
    document.getElementById("chatName").innerHTML = chatName;
    let chatInfoButton = createChatInfoButton(chatId);
    document.getElementById("chatMoreInfoButton").appendChild(chatInfoButton);
    for (let number in messages) {
        let messageDiv = addDiv(["row", messages[number].isHost == true ? "p-r-10" : "p-l-10"]);
        let messageInfoDiv = addDiv(["d-flex", messages[number].isHost == true ? "justify-content-end" : "justify-content-start"]);
        let userP = addPWithStyle("margin: 0px", ["h5"], `${messages[number].nickName}&nbsp;`);
        let postingP = addPWithStyle("margin: 0px", [], messages[number].postingTime);
        let messageP = document.createElement("p");
        if (messages[number].isHost == true)
            messageP.classList.add("text-end");
        messageP.innerText = messages[number].message;

        messageInfoDiv.appendChild(userP);
        messageInfoDiv.appendChild(postingP);
        messageDiv.appendChild(messageInfoDiv);
        messageDiv.appendChild(messageP);

        document.getElementById("messages").appendChild(messageDiv);
    }
    let chatSendMessageButton = createChatSendMessageButton(chatId);
    document.getElementById("chatSendMessageButton").appendChild(chatSendMessageButton);
}

function buildChatInfo(chatInfo) {
    if (document.getElementById("trChatInfo").childElementCount == 3) {
        document.getElementById("trChatInfo").removeChild(document.getElementById("trChatInfo").lastChild);
    }

    document.getElementById("infoChatName").value = chatInfo.chatName;
    document.getElementById("infoChatNote").value = chatInfo.note;

    let editChatInfoButton = document.createElement("button");
    editChatInfoButton.id = "editChatInfo";
    editChatInfoButton.innerHTML = "Редактировать";
    editChatInfoButton.classList.add("m-l-15");
    editChatInfoButton.classList.add("col-3");
    editChatInfoButton.classList.add("btn");
    editChatInfoButton.classList.add("btn-primary");
    editChatInfoButton.addEventListener("click", async () => {
        document.getElementById("infoChatName").disabled = false;
        document.getElementById("infoChatNote").disabled = false;

        document.getElementById("editChatInfo").hidden = true;
        document.getElementById("exitFromChat").hidden = true;
        document.getElementById("closeChatInfo").hidden = true;

        document.getElementById("updateChatInfo").hidden = false;
        document.getElementById("cancelEditChatInfo").hidden = false;
    });

    let cancelEditChatInfoButton = document.createElement("button");
    cancelEditChatInfoButton.id = "cancelEditChatInfo";
    cancelEditChatInfoButton.innerHTML = "Отмена";
    cancelEditChatInfoButton.hidden = true;
    cancelEditChatInfoButton.classList.add("m-l-30");
    cancelEditChatInfoButton.classList.add("col-3");
    cancelEditChatInfoButton.classList.add("btn");
    cancelEditChatInfoButton.classList.add("btn-danger");
    cancelEditChatInfoButton.addEventListener("click", async () => {
        document.getElementById("infoChatName").disabled = true;
        document.getElementById("infoChatNote").disabled = true;

        document.getElementById("editChatInfo").hidden = false;
        document.getElementById("closeChatInfo").hidden = false;
        document.getElementById("exitFromChat").hidden = false;

        document.getElementById("updateChatInfo").hidden = true;
        document.getElementById("cancelEditChatInfo").hidden = true;

        await getMoreChatInfo(chatInfo.chatId);
    });

    let closeChatInfoButton = document.createElement("button");
    closeChatInfoButton.id = "closeChatInfo";
    closeChatInfoButton.innerHTML = "Закрыть";
    closeChatInfoButton.classList.add("m-l-30");
    closeChatInfoButton.classList.add("col-3");
    closeChatInfoButton.classList.add("btn");
    closeChatInfoButton.classList.add("btn-primary");
    closeChatInfoButton.addEventListener("click", async () => {
        await openChat(chatInfo.chatId);
    });

    let exitFromChatButton = document.createElement("button");
    exitFromChatButton.id = "exitFromChat";
    exitFromChatButton.innerHTML = "Выйти";
    exitFromChatButton.classList.add("m-l-30");
    exitFromChatButton.classList.add("col-3");
    exitFromChatButton.classList.add("btn");
    exitFromChatButton.classList.add("btn-danger");
    exitFromChatButton.addEventListener("click", async () => {
        await deleteUserFromChat(chatInfo.userId, chatInfo.chatId);

        if (chatInfo.chatId == lastActiveChatId)
            lastActiveChatId = 0;

        document.getElementById("chatInfo").hidden = true;
        document.getElementById("chatContainer").hidden = true;
        document.getElementById("profile").hidden = true;
        let a = document.getElementById(chatInfo.chatId);
        let br = document.getElementById(`${chatInfo.chatId}21`);
        document.getElementById("chats").removeChild(a);
        document.getElementById("chats").removeChild(br);
    });

    let updateChatInfoButton = document.createElement("button");
    updateChatInfoButton.id = "updateChatInfo";
    updateChatInfoButton.innerHTML = "Подтвердить";
    updateChatInfoButton.hidden = true;
    updateChatInfoButton.classList.add("m-l-15");
    updateChatInfoButton.classList.add("col-3");
    updateChatInfoButton.classList.add("btn");
    updateChatInfoButton.classList.add("btn-primary");
    updateChatInfoButton.addEventListener("click", async () => {
        chatName = document.getElementById("infoChatName").value;
        note = document.getElementById("infoChatNote").value;

        await updateChatInfo(chatInfo.chatId, chatName, note);
        await getMoreChatInfo(chatInfo.chatId);
    });

    let isAdmin = false;
    for (let number in chatInfo.usersInChat) {
        if (chatInfo.userId == chatInfo.usersInChat[number].id && chatInfo.usersInChat[number].role == 2) {
            isAdmin = true;
        }
    }
    if (isAdmin) {
        let th = document.createElement("th");
        th.classList.add("col-2");
        document.getElementById("trChatInfo").appendChild(th);

        document.getElementById("chatInfoButtons").appendChild(editChatInfoButton);
        document.getElementById("chatInfoButtons").appendChild(updateChatInfoButton);
        document.getElementById("chatInfoButtons").appendChild(cancelEditChatInfoButton);
    }
    document.getElementById("chatInfoButtons").appendChild(closeChatInfoButton);
    document.getElementById("chatInfoButtons").appendChild(exitFromChatButton);

    for (let number in chatInfo.usersInChat) {
        let tr = createTr(chatInfo.usersInChat[number].id);
        tr = addTdNick(tr, chatInfo.usersInChat[number].nickName);
        tr = addTdProfile(tr, chatInfo.chatId, chatInfo.usersInChat[number].nickName);
        if (isAdmin && chatInfo.usersInChat[number].id != chatInfo.userId) {
            tr = addTdDelete(tr, chatInfo.usersInChat[number].id, chatInfo.chatId);
        }
        document.getElementById("infoUsersInChatTable").appendChild(tr);
    }
}

function createTr(value) {
    let tr = document.createElement("tr");
    tr.id = value;
    return tr;
}

function createA(chatId) {
    let a = document.createElement("a");
    a.id = chatId;
    a.href = "#";
    a.addEventListener("click", async () => {
        let eventA = document.getElementById(chatId);
        eventA.firstChild.style = "background-color: #eee;";
        if (lastActiveChatId != 0 && lastActiveChatId != chatId) {
            let lastEventA = document.getElementById(lastActiveChatId);
            lastEventA.firstChild.style = "";
        }
        lastActiveChatId = chatId;
        await openChat(chatId);
    });
    return a;
}

function createChatInfoButton(id) {
    let button = document.createElement("button");
    button.classList.add("btn");
    button.classList.add("btn-primary");
    button.type = "button";
    button.innerText = "Подробнее";
    button.addEventListener("click", async () => {
        await getMoreChatInfo(id);
    });
    return button;
}

function createChatSendMessageButton(chatId) {
    let button = document.createElement("button");
    button.classList.add("btn");
    button.classList.add("btn-primary");
    button.type = "button";
    button.innerText = "Отправить";
    button.addEventListener("click", async () => {
        let message = document.getElementById("textMessage").value;
        if (message !== '' || message.trim() !== '')
            await sendMessage(chatId, message.trim());
    });
    return button;
}

function addDivWithStyle(style, classes) {
    let div = document.createElement("div");
    div.style = style;
    for (let number in classes) {
        div.classList.add(classes[number]);
    }
    return div;
}

function addDiv(classes) {
    let div = document.createElement("div");
    for (let number in classes) {
        div.classList.add(classes[number]);
    }
    return div;
}

function addPWithStyle(style, classes, text) {
    let p = document.createElement("p");
    p.style = style;
    for (let number in classes) {
        p.classList.add(classes[number]);
    }
    p.innerHTML = text;
    return p;
}

function addPWithStyleAndId(style, classes, text, id) {
    let p = document.createElement("p");
    p.id = id;
    p.style = style;
    for (let number in classes) {
        p.classList.add(classes[number]);
    }
    p.innerText = text;
    return p;
}

function addTdNick(tr, nick) {
    let tdNick = document.createElement("td");
    tdNick.innerText = nick;
    tr.appendChild(tdNick);
    return tr;
}

function addTdCheckbox(tr, id) {
    let tdCheckbox = document.createElement("td");
    let inputCheckbox = document.createElement("input");
    inputCheckbox.type = "checkbox";
    inputCheckbox.classList.add("form-check-input");
    inputCheckbox.addEventListener("click", async () => {
        let repit = false;
        let newFriendsId = [];
        for (let number in friendsId)
            if (friendsId[number] != id)
                newFriendsId.push(friendsId[number]);
            else
                repit = true;
        friendsId = newFriendsId;
        if (repit === true)
            return;
        friendsId.push(id);
    });
    tdCheckbox.appendChild(inputCheckbox);
    tr.appendChild(tdCheckbox);
    return tr;
}

function addTdProfile(tr, chatId, nick) {
    let tdProfile = document.createElement("td");
    let profileButton = document.createElement("button");
    profileButton.classList.add("btn");
    profileButton.classList.add("btn-primary");
    profileButton.type = "button";
    profileButton.textContent = "Профиль";
    profileButton.addEventListener("click", async () => {
        await getUserProfile(chatId, nick);
    });
    tdProfile.appendChild(profileButton);
    tr.appendChild(tdProfile);
    return tr;
}

function addTdDelete(tr, userId, chatId) {
    let tdDelete = document.createElement("td");
    let deleteButton = document.createElement("button");
    deleteButton.classList.add("btn");
    deleteButton.classList.add("btn-danger");
    deleteButton.type = "button";
    deleteButton.textContent = "Удалить";
    deleteButton.addEventListener("click", async () => {
        await deleteUserFromChat(userId, chatId);
    });
    tdDelete.appendChild(deleteButton);
    tr.appendChild(tdDelete);
    return tr; 
}

function clearBlock(name) {
    let tbody = document.getElementById(name);
    while (tbody.firstChild) {
        tbody.removeChild(tbody.firstChild);
    }
}