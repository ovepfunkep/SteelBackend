let attendees = [];

async function fetchData() {
    try {
        attendees = await GetAttendees();
        attendees.forEach(attend => AddAttendToTable(attend));
    } catch (error) {
        console.log(error);
    }
}

fetchData();

document.getElementById("submit").addEventListener("click", () => {
    var attend =
    {
        trainingId: document.getElementById("trainingId").value,
        userId: document.getElementById("userId").value,
    }
    AddAttend(attend);
    ClearForms();
})

function SelectAttend(attend) {
    document.getElementById("id").value = attend.id;
    document.getElementById("trainingId").value = attend.trainingId;
    document.getElementById("userId").value = attend.userId;
}


document.getElementById("update").addEventListener("click", () => {
    const attend =
    {
        id: document.getElementById("id").value,
        trainingId: document.getElementById("trainingId").value,
        userId: document.getElementById("userId").value,
    }
    if (UpdateAttend(attend)) {
        ClearForms();
    }
});

function AddAttendToTable(attend) {
    if (!attendees.some(v => v.id === attend.id)) attendees.push(attend);

    document.getElementById("tableContext").innerHTML += `
    <tr id="rowattend${attend.id}">
        <td>${attend.id}</td>
        <td>${attend.trainingId}</td>
        <td>${attend.userId}</td>
        <td> 
            <button class="btn btn-warning m-2" onclick='SelectAttend(${(JSON.stringify(attend))})'>Change</button> 
            <button class="btn btn-danger m-2" onclick='DeleteAttend(${attend.id})'>Delete</button> 
        </td>
    </tr>`;
}

function ClearForms() {
    document.getElementById("id").value = 0;
    document.getElementById("trainingId").value = 1;
    document.getElementById("userId").value = "";
}

async function GetAttendees() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/attendees`);

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

async function UpdateAttend(attend) {
    try {
        const attendeestring = JSON.stringify(attend);
        const response = await fetch(`http://194.87.92.189:5000/api/attendees/${attendeestring}`, { method: "PUT" });

        if (response.ok === true) {
            const attend = await response.json();
            UpdateTableAttend(attend);
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

async function AddAttend(attend) {
    try {
        const attendeestring = encodeURIComponent(JSON.stringify(attend));
        const response = await fetch(`http://194.87.92.189:5000/api/attendees/${attendeestring}`, { method: "POST" });

        if (response.ok === true) {
            const attend = await response.json();
            AddAttendToTable(attend);
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

function UpdateTableAttend(attend) {
    let row = document.getElementById(`rowattend${attend.id}`);
    row.innerHTML = `
                <td>${attend.id}</td>
                <td>${attend.trainingId}</td>
                <td>${attend.userId}</td>
                <td> 
                    <button class="btn btn-warning m-2" onclick='SelectAttend(${(JSON.stringify(attend))})'>Change</button> 
                    <button class="btn btn-danger m-2" onclick='DeleteAttend(${attend.id})'>Delete</button> 
                </td>`
}

async function DeleteAttend(attendid) {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/attendees/${attendid}`, { method: "DELETE" });

        if (response.ok === true) {
            DeleteTableattend(attendid);
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

function DeleteTableattend(attendid) {
    attendees.splice(attendees.indexOf(attend => attend.id === attendid), 1);
    document.getElementById(`rowattend${attendid}`).remove();
}