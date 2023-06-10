let users = [];

async function fetchData() {
    try {
        users = await GetUsers();
        users.forEach(user => AddUserToTable(user));
    } catch (error) {
        console.log(error);
    }
}

fetchData();



//Добавить по кнопке
document.getElementById("submit").addEventListener("click", () => {
    const dateOfBirthValue = document.getElementById("dateOfBirth").value || '0001-01-01T';

    // Преобразуем значение в нужный формат yyyy-MM-dd
    const dateParts = dateOfBirthValue.split("T")[0].split("-");
    const year = dateParts[0];
    const month = dateParts[1];
    const day = dateParts[2];
    const formattedDate = `${year}-${month}-${day}`;
    var user = {
        roleId: document.getElementById("role_id").value || 1,
        surname: document.getElementById("surname").value || 'default',
        name: document.getElementById("name").value || 'default',
        middleName: document.getElementById("middleName").value || 'default',
        email: document.getElementById("email").value || 'default',
        phone: document.getElementById("phone").value || '70000000000',
        password: document.getElementById("password").value || 'default',
        gender: document.getElementById("gender").value || 'Мужской',
        dateOfBirth: formattedDate,
        photo: document.getElementById("photo").value || 'default',
        vkontakte: document.getElementById("vk").value || 'default',
        telegram: document.getElementById("telegram").value || 'default',
        city: document.getElementById("city").value || 'default'
    };

    AddUser(user);
    ClearForms();
});


function SelectUser(user) {
    const dateOfBirthValue = user.dateOfBirth || '0001-01-01T00:00:00';
    const formattedDate = dateOfBirthValue.split('T')[0];

    document.getElementById("ID").value = user.id;
    document.getElementById("role_id").value = user.roleId;
    document.getElementById("surname").value = user.surname;
    document.getElementById("name").value = user.name;
    document.getElementById("middleName").value = user.middleName;
    document.getElementById("email").value = user.email;
    document.getElementById("phone").value = user.phone;
    document.getElementById("password").value = user.password;
    document.getElementById("gender").value = user.gender;
    document.getElementById("dateOfBirth").value = formattedDate;
    document.getElementById("photo").value = user.photo;
    document.getElementById("vk").value = user.vkontakte;
    document.getElementById("telegram").value = user.telegram;
    document.getElementById("city").value = user.city;
}

//Обновить по кнопке
document.getElementById("update").addEventListener("click", () => {
    const dateOfBirthValue = document.getElementById("dateOfBirth").value || '01-01-0001T00:00:00';
    console.log(dateOfBirthValue)

    // Преобразуем значение в нужный формат yyyy-MM-dd
    const dateParts = dateOfBirthValue.split("T")[0].split("-");
    const year = dateParts[0];
    const month = dateParts[1];
    const day = dateParts[2];
    const formattedDate = `${year}-${month}-${day}T01:01:01`;
    const user =
    {
        id: document.getElementById("ID").value,
        roleId: document.getElementById("role_id").value,
        surname: document.getElementById("surname").value,
        name: document.getElementById("name").value,
        middleName: document.getElementById("middleName").value,
        email: document.getElementById("email").value,
        phone: document.getElementById("phone").value,
        password: document.getElementById("password").value,
        gender: document.getElementById("gender").value,
        dateOfBirth: formattedDate,
        photo: document.getElementById("photo").value,
        vkontakte: document.getElementById("vk").value,
        telegram: document.getElementById("telegram").value,
        city: document.getElementById("city").value
    }
    console.log(user)
    if (UpdateUser(user)) {
        ClearForms();
    }
});

function AddUserToTable(user) {
    if (!users.some(u => u.id === user.id)) users.push(user);

    document.getElementById("tableContext").innerHTML += `
    <tr id="rowUser${user.id}">
        <td>${user.id}</td>
        <td>${user.roleId}</td>
        <td>${user.surname}</td>
        <td>${user.name}</td>
        <td>${user.middleName}</td>
        <td>${user.email}</td>
        <td>${user.phone}</td>
        <td>${user.password}</td>
        <td>${user.gender}</td>
        <td>${user.dateOfBirth}</td>
        <td>${user.photo}</td>
        <td>${user.vkontakte}</td>
        <td>${user.telegram}</td>
        <td>${user.city}</td>
        <td> 
            <button class="btn btn-warning m-2" onclick='SelectUser(${(JSON.stringify(user))})'>Change</button> 
            <button class="btn btn-danger m-2" onclick='DeleteUser(${user.id})'>Delete</button> 
        </td>
    </tr>`;
}


//Обнулить формы
function ClearForms() {
    document.getElementById("ID").value = 0;
    document.getElementById("role_id").value = 1;
    document.getElementById("surname").value = "";
    document.getElementById("name").value = "";
    document.getElementById("middleName").value = "";
    document.getElementById("email").value = "";
    document.getElementById("phone").value = "";
    document.getElementById("password").value = "";
    document.getElementById("gender").value = "Мужской";
    document.getElementById("dateOfBirth").value = "";
    document.getElementById("photo").value = "";
    document.getElementById("vk").value = "";
    document.getElementById("telegram").value = "";
    document.getElementById("city").value = "";
}

//Получить пользователей из бд
async function GetUsers() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/users`);

        if (response.ok === true) {
            return await response.json();
        } else {
            const error = await response.json();
            console.log(error.message);
        }
    } catch (error) {
        console.log(error);
    }
}

//Обновить пользователя в БД и в таблицу
async function UpdateUser(user) {
    try {
        const userString = encodeURIComponent(JSON.stringify(user));
        const response = await fetch(`http://194.87.92.189:5000/api/users/${userString}`, { method: "PUT" });

        if (response.ok === true) {
            const user = await response.json();
            UpdateTableUser(user);
            return true;
        } else {
            const error = await response.json();
            console.log(error.message);
            return false;
        }
    } catch (error) {
        console.log(error);
        return false;
    }
}

//Добавить пользователя в БД и в таблицу
async function AddUser(user) {
    try {
        const userString = encodeURIComponent(JSON.stringify(user));
        const response = await fetch(`http://194.87.92.189:5000/api/users/${userString}`, { method: "POST" });

        if (response.ok === true) {
            const user = await response.json();

            AddUserToTable(user);

            return true;
        } else {
            const error = await response.json();
            console.log(error.message);
            return false;
        }
    } catch (error) {
        console.log(error);
        return false;
    }
}

//Обновить данные пользователя в таблице
function UpdateTableUser(user) {

    let row = document.getElementById(`rowUser${user.id}`);
    row.innerHTML = `
                <td>${user.id}</td>
                <td>${user.roleId}</td>
                <td>${user.surname}</td>
                <td>${user.name}</td>
                <td>${user.middleName}</td>
                <td>${user.email}</td>
                <td>${user.phone}</td>
                <td>${user.password}</td>
                <td>${user.gender}</td>
                <td>${user.dateOfBirth}</td>
                <td>${user.photo}</td>
                <td>${user.vkontakte}</td>
                <td>${user.telegram}</td>
                <td>${user.city}</td>
                <td> 
                    <button class="btn btn-warning m-2" onclick='SelectUser(${(JSON.stringify(user))})'>Change</button> 
                    <button class="btn btn-danger m-2" onclick='DeleteUser(${user.id})'>Delete</button> 
                </td>`
}

//Удалить пользователя из БД
async function DeleteUser(userid) {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/users/${userid}`, { method: "DELETE" });

        if (response.ok === true) {
            DeleteTableUser(userid);

            return true;
        } else {
            const error = await response.json();
            console.log(error.message);
            return false;
        }
    } catch (error) {
        console.log(error);
        return false;
    }
}

//Удалить пользователя из таблицы
function DeleteTableUser(userid) {
    users.splice(users.indexOf(user => user.id === userid), 1);
    document.getElementById(`rowUser${userid}`).remove();
}
