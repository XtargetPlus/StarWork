async function addUser(fullName, age, phone, email, login, password) {
    const response = await fetch("/registr/add-user", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            fullName: fullName,
            age: age,
            mobileNumber: phone,
            email: email,
            login: login,
            password: password
        })
    });
    if (response.ok === true) {
        console.log(response.statusText);
        window.location.href = "/login";
    }
}

document.getElementById("logIn").addEventListener("click", async () => {
    window.location.href = "/login";
})

document.getElementById("registration").addEventListener("click", async () => {
    const login = document.getElementById("login").value;
    const password = document.getElementById("password").value;
    const checkPassword = document.getElementById("checkPassword").value;
    const nickName = document.getElementById("nickName").value;
    const phone = document.getElementById("phone").value;
    const email = document.getElementById("email").value;
    const age = document.getElementById("age").value;

    let error = false;

    error = checkTextToNull(login, "alertLogin");
    error = checkTextToNull(email, "alertEmail") == true && error == false ? true : error == true ? error : false;
    error = checkTextToNull(password, "alertPassword") == true && error == false ? true : error == true ? error: false;
    error = checkTextToNull(checkPassword, "alertCheckPassword") == true && error == false ? true : error == true ? error : false;
    error = checkTextToNull(nickName, "alertNickName") == true && error == false ? true : error == true ? error : false;
    error = checkTextToNull(phone, "alertPhone") == true && error == false ? true : error == true ? error : false;
    error = checkTextToNull(age, "alertAge") == true && error == false ? true : error == true ? error : false;

    error = checkTextToSpaces(login, "alertLogin", "логин") == true && error == false ? true : error == true ? error : false;
    error = checkTextToSpaces(password, "alertPassword", "пароль") == true && error == false ? true : error == true ? error : false;
    error = checkTextToSpaces(email, "alertEmail", "почта") == true && error == false ? true : error == true ? error : false;

    if (password != checkPassword) {
        error = true;
        document.getElementById("alertCheckPassword").hidden = false;
        document.getElementById("alertCheckPassword").innerHTML = "Пароли не совпадают!";
    } else if (document.getElementById("alertCheckPassword").innerHTML == "Пароли не совпадают") {
        document.getElementById("alertCheckPassword").innerHTML = "Введите пароль еще раз!";
    }

    if (error == false) {
        await addUser(nickName, age, phone, email, login, password);
    }
});

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
    if (textError == "почта")
        document.getElementById(id).innerHTML = `Введите почту`;
    else
        document.getElementById(id).innerHTML = `Введите ${textError}`;
    return false;
}