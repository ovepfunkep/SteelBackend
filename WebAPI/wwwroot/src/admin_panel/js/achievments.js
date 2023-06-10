let achievments = [];


async function fetchData() {
    try {
        achievments = await GetAchievments();
        achievments.forEach(achievment => AddAchievmentToTable(achievment));
    } catch (error) {
        console.log(error);
    }
}
fetchData();

document.getElementById("submit").addEventListener("click", () => {
    var achievment =
    {
        name: document.getElementById("name").value,
        description: document.getElementById("description").value,
        photo: document.getElementById("photo").value
    }
    AddAchievment(achievment);
    ClearForms();
})

function SelectAchievment(achievment) {
    document.getElementById("id").value = achievment.id;
    document.getElementById("name").value = achievment.name;
    document.getElementById("description").value = achievment.description;
    document.getElementById("photo").value = achievment.photo;
}

document.getElementById("update").addEventListener("click", () => {
    const achievment =
    {
        id: document.getElementById("id").value,
        name: document.getElementById("name").value,
        description: document.getElementById("description").value,
        photo: document.getElementById("photo").value
    }
    if (UpdateAchievment(achievment)) {
        ClearForms();
    }
});

function AddAchievmentToTable(achievment) {
    if (!achievments.some(ach => ach.id === achievment.id)) achievments.push(achievment);
    document.getElementById("tableContext").innerHTML += `
    <tr id="rowachievment${achievment.id}">
        <td>${achievment.id}</td>
        <td>${achievment.name}</td>
        <td>${achievment.description}</td>
        <td>${achievment.photo}</td>
        <td> 
            <button class="btn btn-warning m-2" onclick='SelectAchievment(${(JSON.stringify(achievment))})'>Change</button> 
            <button class="btn btn-danger m-2" onclick='DeleteAchievment(${achievment.id})'>Delete</button> 
        </td>
    </tr>`;
}

function ClearForms() {
    document.getElementById("id").value = 0;
    document.getElementById("name").value = 1;
    document.getElementById("description").value = "";
    document.getElementById("photo").value = "";
}


async function GetAchievments() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/achievements`);

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


async function UpdateAchievment(achievment) {
    try {
        const achievmentString = JSON.stringify(achievment);
        const response = await fetch(`http://194.87.92.189:5000/api/achievements/${achievmentString}`, { method: "PUT" });

        if (response.ok === true) {
            const achievment = await response.json();
            UpdateTableAchievment(achievment);
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


async function AddAchievment(achievment) {
    try {
        const achievmentString = encodeURIComponent(JSON.stringify(achievment));
        const response = await fetch(`http://194.87.92.189:5000/api/achievements/${achievmentString}`, { method: "POST" });

        if (response.ok === true) {
            const achievment = await response.json();

            AddAchievmentToTable(achievment);

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


function UpdateTableAchievment(achievment) {
    let row = document.getElementById(`rowachievment${achievment.id}`);
    row.innerHTML = `
                <td>${achievment.id}</td>
                <td>${achievment.name}</td>
                <td>${achievment.description}</td>
                <td>${achievment.photo}</td>
                <td> 
                    <button class="btn btn-warning m-2" onclick='SelectAchievment(${(JSON.stringify(achievment))})'>Change</button> 
                    <button class="btn btn-danger m-2" onclick='DeleteAchievment(${achievment.id})'>Delete</button> 
                </td>`
}


async function DeleteAchievment(achievmentid) {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/achievements/${achievmentid}`, { method: "DELETE" });

        if (response.ok === true) {
            DeleteTableAchievment(achievmentid);
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


function DeleteTableAchievment(achievmentid) {
    achievments.splice(achievments.indexOf(achievment => achievment.id === achievmentid), 1);
    document.getElementById(`rowachievment${achievmentid}`).remove();
}