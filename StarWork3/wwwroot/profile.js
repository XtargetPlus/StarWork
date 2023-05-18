
window.onload = getProfileInfo;

async function getProfileInfo() {
    let response = await fetch(`/profile/get-my-info`, {
        method: "GET",
        headers: { "Accept": "application/json", "Content-Type": "application/json" }
    });
    if (response.ok) {
        const profileInfo = await response.json();
        document.getElementById("nick").value = profileInfo.nickName;
        document.getElementById("mobileNumber").value = profileInfo.phone;
        document.getElementById("email").value = profileInfo.email;
        document.getElementById("age").value = profileInfo.age;
        document.getElementById("description").value = profileInfo.note;
    }
}

async function updateProfileInfo(nick, age, email, phone, note) {
    let response = await fetch(`/profile/update-info`, {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            nickName: nick,
            age: age,
            email: email,
            phone: phone,
            note: note
        })
    });
    if (response.ok) {
        console.log("Все ок");
    }
}

async function updateLogin(oldLogin, newLogin) {
    let response = await fetch(`/profile/update-login`, {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            oldLogin: oldLogin,
            newLogin: newLogin
        })
    });
    if (response.ok) {
        console.log("Логин изменен");
    }
}

async function updatePassword(oldPassword, newPassword) {
    let response = await fetch(`/profile/update-password`, {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            oldPassword: oldPassword,
            newPassword: newPassword
        })
    });
    if (response.ok) {
        console.log("Логин изменен");
    }
}

document.getElementById("edit").addEventListener("click", async () => {
    openEditClose(false, true);
});

document.getElementById("saveEdit").addEventListener("click", async () => {
    const nick = document.getElementById("nick").value;
    const phone = document.getElementById("mobileNumber").value;
    const email = document.getElementById("email").value;
    const age = document.getElementById("age").value;
    const note = document.getElementById("description").value;
    await updateProfileInfo(nick, age, email, phone, note);

    openEditClose(true, false);

});

document.getElementById("cancelEdit").addEventListener("click", async () => {
    openEditClose(true, false);
    await getProfileInfo();
});

document.getElementById("editLogin").addEventListener("click", async () => {
    document.getElementById("profileContainer").hidden = true;
    document.getElementById("loginContainer").hidden = false;
});

document.getElementById("saveEditLogin").addEventListener("click", async () => {
    const oldLogin = document.getElementById("oldLogin").value;
    const newLogin = document.getElementById("newLogin").value;

    if (oldLogin != newLogin) {
        await updateLogin(oldLogin, newLogin);

        document.getElementById("profileContainer").hidden = false;
        document.getElementById("loginContainer").hidden = true;
    }
});

document.getElementById("cancelEditLogin").addEventListener("click", async () => {
    document.getElementById("profileContainer").hidden = false;
    document.getElementById("loginContainer").hidden = true;
});

document.getElementById("editPassword").addEventListener("click", async () => {
    document.getElementById("profileContainer").hidden = true;
    document.getElementById("passwordContainer").hidden = false;
});

document.getElementById("cancelEditPassword").addEventListener("click", async () => {
    document.getElementById("profileContainer").hidden = false;
    document.getElementById("passwordContainer").hidden = true;
});

document.getElementById("saveEditPassword").addEventListener("click", async () => {
    const oldPassword = document.getElementById("oldPassword").value;
    const newPassword = document.getElementById("newPassword").value;

    if (oldPassword != newPassword) {
        await updatePassword(oldPassword, newPassword);

        document.getElementById("profileContainer").hidden = false;
        document.getElementById("passwordContainer").hidden = true;
    }
});

document.getElementById("delete").addEventListener("click", async () => {

});


function openEditClose(b1, b2) {
    document.getElementById("saveEdit").hidden = b1;
    document.getElementById("cancelEdit").hidden = b1;

    document.getElementById("editLogin").hidden = b2;
    document.getElementById("edit").hidden = b2;
    document.getElementById("editPassword").hidden = b2;
    document.getElementById("delete").hidden = b2;

    document.getElementById("nick").disabled = b1;
    document.getElementById("mobileNumber").disabled = b1;
    document.getElementById("email").disabled = b1;
    document.getElementById("age").disabled = b1;
    document.getElementById("description").disabled = b1;
}