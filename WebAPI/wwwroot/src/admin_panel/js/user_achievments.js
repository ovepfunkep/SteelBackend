let usersAchievements = [];

async function fetchData() {
    try {
        usersAchievements = await GetUsersAchievements();
        usersAchievements.forEach(userAchievement => AddUserAchievementToTable(userAchievement));
    } catch (error) {
        console.log(error);
    }
}

fetchData();

document.getElementById("submit").addEventListener("click", () => {
    var userAchievement =
    {
        userId: document.getElementById("userId").value,
        achievementId: document.getElementById("achievementId").value,
    }
    AddUserAchievement(userAchievement);
    ClearForms();
})

function SelectUserAchievement(userAchievement) {
    document.getElementById("id").value = userAchievement.id;
    document.getElementById("userId").value = userAchievement.userId;
    document.getElementById("achievementId").value = userAchievement.achievementId;
}


document.getElementById("update").addEventListener("click", () => {
    const userAchievement =
    {
        id: document.getElementById("id").value,
        userId: document.getElementById("userId").value,
        achievementId: document.getElementById("achievementId").value,
    }
    if (UpdateUserAchievement(userAchievement)) {
        ClearForms();
    }
});

function AddUserAchievementToTable(userAchievement) {
    if (!usersAchievements.some(who => who.id === userAchievement.id)) usersAchievements.push(userAchievement);

    document.getElementById("tableContext").innerHTML += `
    <tr id="rowuserAchievement${userAchievement.id}">
        <td>${userAchievement.id}</td>
        <td>${userAchievement.userId}</td>
        <td>${userAchievement.achievementId}</td>
        <td> 
            <button class="btn btn-warning m-2" onclick='SelectUserAchievement(${(JSON.stringify(userAchievement))})'>Change</button> 
            <button class="btn btn-danger m-2" onclick='DeleteUserAchievement(${userAchievement.id})'>Delete</button> 
        </td>
    </tr>`;
}

function ClearForms() {
    document.getElementById("id").value = 0;
    document.getElementById("userId").value = "";
    document.getElementById("achievementId").value = 1;
}


async function GetUsersAchievements() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/usersAchievements`);

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

async function UpdateUserAchievement(userAchievement) {
    try {
        const userAchievementString = JSON.stringify(userAchievement);
        const response = await fetch(`http://194.87.92.189:5000/api/usersAchievements/${userAchievementString}`, { method: "PUT" });

        if (response.ok === true) {
            const userAchievement = await response.json();
            UpdateTableUserAchievement(userAchievement);
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

async function AddUserAchievement(userAchievement) {
    try {
        const userAchievementString = encodeURIComponent(JSON.stringify(userAchievement));
        const response = await fetch(`http://194.87.92.189:5000/api/usersAchievements/${userAchievementString}`, { method: "POST" });

        if (response.ok === true) {
            const userAchievement = await response.json();
            AddUserAchievementToTable(userAchievement);
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

function UpdateTableUserAchievement(userAchievement) {
    let row = document.getElementById(`rowuserAchievement${userAchievement.id}`);
    row.innerHTML = `
                <td>${userAchievement.id}</td>
                <td>${userAchievement.userId}</td>
                <td>${userAchievement.achievementId}</td>
                <td> 
                    <button class="btn btn-warning m-2" onclick='SelectUserAchievement(${(JSON.stringify(userAchievement))})'>Change</button> 
                    <button class="btn btn-danger m-2" onclick='DeleteUserAchievement(${userAchievement.id})'>Delete</button> 
                </td>`
}


async function DeleteUserAchievement(userAchievementid) {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/usersAchievements/${userAchievementid}`, { method: "DELETE" });

        if (response.ok === true) {
            DeleteTableUserAchievement(userAchievementid);
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

function DeleteTableUserAchievement(userAchievementid) {
    usersAchievements.splice(usersAchievements.indexOf(userAchievement => userAchievement.id === userAchievementid), 1);
    document.getElementById(`rowuserAchievement${userAchievementid}`).remove();
}