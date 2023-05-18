async function getUser(login, password) {
    const response = await fetch("/login/get-user", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            login: login,
            password: password
        })
    });
    if (response.ok === true) {
        console.log(response.statusText);
        window.location.href = "/";
    }
}
async function redirectToRegistration() {
    console.log("переадресация на страницу регистрации");
    window.location.href = "/registr";
}

document.getElementById("logIn").addEventListener("click", async () => {
    const login = document.getElementById("login").value;
    const password = document.getElementById("password").value;

    let error = false;

    error = checkTextToNull(login, "alertLogin") == true && error == false ? true : error == true ? error : false;
    error = checkTextToNull(password, "alertPassword") == true && error == false ? true : error == true ? error : false;

    error = checkTextToSpaces(login, "alertLogin", "логин") == true && error == false ? true : error == true ? error : false;
    error = checkTextToSpaces(password, "alertPassword", "пароль") == true && error == false ? true : error == true ? error : false;

    if (error == false) {
        await getUser(login, password);
    }
});
document.getElementById("registration").addEventListener("click", redirectToRegistration);

function checkTextToNull(text, id) {
    if (text == "") {
        document.getElementById(id).hidden = false;
        return true;
    }
    document.getElementById(id).hidden = true;
    return false;
}

function checkTextToSpaces(text, id, textError) {
    if (text.includes(" ")) {
        document.getElementById(id).innerHTML = `${textError.charAt(0).toUpperCase()}${textError.slice(1)} не должен содержать пробелы!`;
        document.getElementById(id).hidden = false;
        return true;
    }
    document.getElementById(id).innerHTML = `Введите ${textError}`;
    return false;
}