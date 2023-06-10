let activities = [];


async function fetchData() {
    try {
        activities = await GetActivities();
        activities.forEach(activity => AddActivityToTable(activity));
    } catch (error) {
        console.log(error);
    }
}
fetchData();


document.getElementById("submit").addEventListener("click", () => {
    var activity =
    {
        name: document.getElementById("name").value,
        description: document.getElementById("description").value,
        photo: document.getElementById("photo").value,
        icon: document.getElementById("icon").value,
        lastsInMinutes: document.getElementById("lastsInMinutes").value,
    };
    AddActivity(activity);
    ClearForms();
})

function SelectActivity(activity) {
    document.getElementById("id").value = activity.id;
    document.getElementById("name").value = activity.name;
    document.getElementById("description").value = activity.description;
    document.getElementById("photo").value = activity.photo;
    document.getElementById("icon").value = activity.icon;
    document.getElementById("lastsInMinutes").value = activity.lastsInMinutes;
}

document.getElementById("update").addEventListener("click", () => {
    const activity =
    {
        id: document.getElementById("id").value,
        name: document.getElementById("name").value,
        description: document.getElementById("description").value,
        photo: document.getElementById("photo").value,
        icon: document.getElementById("icon").value,
        lastsInMinutes: document.getElementById("lastsInMinutes").value,
    };
    if (UpdateActivity(activity)) {
        ClearForms();
    }
});

function AddActivityToTable(activity) {
    if (!activities.some(act => act.id === activity.id)) activities.push(activity);

    document.getElementById("tableContext").innerHTML += `
    <tr id="rowactivity${activity.id}">
        <td>${activity.id}</td>
        <td>${activity.name}</td>
        <td>${activity.description}</td>
        <td>${activity.photo}</td>
        <td>${activity.icon}</td>
        <td>${activity.lastsInMinutes}</td>
        <td> 
            <button class="btn btn-warning m-2" onclick='SelectActivity(${(JSON.stringify(activity))})'>Change</button> 
            <button class="btn btn-danger m-2" onclick='DeleteActivity(${activity.id})'>Delete</button> 
        </td>
    </tr>`;
}

function ClearForms() {
    document.getElementById("id").value = 0;
    document.getElementById("name").value = 1;
    document.getElementById("description").value = "";
    document.getElementById("photo").value = "";
    document.getElementById("icon").value = "";
    document.getElementById("lastsInMinutes").value = "";
}


async function GetActivities() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/activities`);

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


async function UpdateActivity(activity) {
    try {
        const activitiestring = JSON.stringify(activity);
        const response = await fetch(`http://194.87.92.189:5000/api/activities/${activitiestring}`, { method: "PUT" });

        if (response.ok === true) {
            const activity = await response.json();
            UpdateTableActivity(activity);
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


async function AddActivity(activity) {
    try {
        const activitiestring = encodeURIComponent(JSON.stringify(activity));
        const response = await fetch(`http://194.87.92.189:5000/api/activities/${activitiestring}`, { method: "POST" });

        if (response.ok === true) {
            const activity = await response.json();

            AddActivityToTable(activity);

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


function UpdateTableActivity(activity) {
    let row = document.getElementById(`rowactivity${activity.id}`);
    row.innerHTML = `
                <td>${activity.id}</td>
                <td>${activity.name}</td>
                <td>${activity.description}</td>
                <td>${activity.photo}</td>
                <td>${activity.icon}</td>
                <td>${activity.lastsInMinutes}</td>
                <td> 
                    <button class="btn btn-warning m-2" onclick='SelectActivity(${(JSON.stringify(activity))})'>Change</button> 
                    <button class="btn btn-danger m-2" onclick='DeleteActivity(${activity.id})'>Delete</button> 
                </td>`
}


async function DeleteActivity(activityid) {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/activities/${activityid}`, { method: "DELETE" });

        if (response.ok === true) {
            DeleteTableActivity(activityid);
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


function DeleteTableActivity(activityid) {
    activities.splice(activities.indexOf(activity => activity.id == activityid), 1);
    document.getElementById(`rowactivity${activityid}`).remove();
}