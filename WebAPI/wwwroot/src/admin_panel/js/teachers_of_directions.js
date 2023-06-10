let activityTeachers = [];

async function fetchData() {
    try {
        activityTeachers = await GetActivityTeachers();
        activityTeachers.forEach(activityTeacher => AddActivityTeacherToTable(activityTeacher));
    } catch (error) {
        console.log(error);
    }
}

fetchData();

document.getElementById("submit").addEventListener("click", () => {
    var activityTeacher =
    {
        activityId: document.getElementById("activityId").value,
        teacherId: document.getElementById("teacherId").value,
    }
    AddActivityTeacher(activityTeacher);
    ClearForms();
})

function SelectActivityTeacher(activityTeacher) {
    document.getElementById("id").value = activityTeacher.id;
    document.getElementById("activityId").value = activityTeacher.activityId;
    document.getElementById("teacherId").value = activityTeacher.teacherId;
}


document.getElementById("update").addEventListener("click", () => {
    const activityTeacher =
    {
        id: document.getElementById("id").value,
        activityId: document.getElementById("activityId").value,
        teacherId: document.getElementById("teacherId").value,
    }
    if (UpdateActivityTeacher(activityTeacher)) {
        ClearForms();
    }
});

function AddActivityTeacherToTable(activityTeacher) {
    if (!activityTeachers.some(tea => tea.id == activityTeacher.id)) activityTeachers.push(activityTeacher);

    document.getElementById("tableContext").innerHTML += `
    <tr id="rowactivityTeacher${activityTeacher.id}">
        <td>${activityTeacher.id}</td>
        <td>${activityTeacher.activityId}</td>
        <td>${activityTeacher.teacherId}</td>
        <td> 
            <button class="btn btn-warning m-2" onclick='SelectActivityTeacher(${(JSON.stringify(activityTeacher))})'>Change</button> 
            <button class="btn btn-danger m-2" onclick='DeleteActivityTeacher(${activityTeacher.id})'>Delete</button> 
        </td>
    </tr>`;
}

function ClearForms() {
    document.getElementById("id").value = 0;
    document.getElementById("activityId").value = "";
    document.getElementById("teacherId").value = 1;
}

async function GetActivityTeachers() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/activitiesTeachers`);

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


async function UpdateActivityTeacher(activityTeacher) {
    try {
        const activityTeacherString = encodeURIComponent(JSON.stringify(activityTeacher));
        const response = await fetch(`http://194.87.92.189:5000/api/activitiesTeachers/${activityTeacherString}`, { method: "PUT" });

        if (response.ok === true) {
            const activityTeacher = await response.json();
            UpdateTableActivityTeacher(activityTeacher);
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


async function AddActivityTeacher(activityTeacher) {
    try {
        const activityTeacherString = encodeURIComponent(JSON.stringify(activityTeacher));
        const response = await fetch(`http://194.87.92.189:5000/api/activitiesTeachers/${activityTeacherString}`, { method: "POST" });

        if (response.ok === true) {
            const result = await response.json();
            console.log(result);
            AddActivityTeacherToTable(result);
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


function UpdateTableActivityTeacher(activityTeacher) {
    let row = document.getElementById(`rowactivityTeacher${activityTeacher.id}`);
    row.innerHTML = `
                <td>${activityTeacher.id}</td>
                <td>${activityTeacher.activityId}</td>
                <td>${activityTeacher.teacherId}</td>
                <td> 
                    <button class="btn btn-warning m-2" onclick='SelectActivityTeacher(${(JSON.stringify(activityTeacher))})'>Change</button> 
                    <button class="btn btn-danger m-2" onclick='DeleteActivityTeacher(${activityTeacher.id})'>Delete</button> 
                </td>`
}


async function DeleteActivityTeacher(activityTeacherid) {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/activitiesTeachers/${activityTeacherid}`, { method: "DELETE" });

        if (response.ok === true) {
            DeleteTableactivityTeacher(activityTeacherid);
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


function DeleteTableactivityTeacher(activityTeacherid) {
    activityTeachers.splice(activityTeachers.indexOf(activityTeacher => activityTeacher.id === activityTeacherid), 1);
    document.getElementById(`rowactivityTeacher${activityTeacherid}`).remove();
}