let appointments = [];

async function fetchData() {
    try {
        appointments = await GetAppointments();
        appointments.forEach(appointment => AddAppointmentToTable(appointment));
    } catch (error) {
        console.log(error);
    }
}

fetchData();

document.getElementById("submit").addEventListener("click", () => {
    var appointment =
    {
        trainingId: document.getElementById("trainingId").value,
        userId: document.getElementById("userId").value,
    }
    AddAppointment(appointment);
    ClearForms();
})

function SelectAppointment(appointment) {
    document.getElementById("id").value = appointment.id;
    document.getElementById("trainingId").value = appointment.trainingId;
    document.getElementById("userId").value = appointment.userId;
}

document.getElementById("update").addEventListener("click", () => {
    const appointment =
    {
        id: document.getElementById("id").value,
        trainingId: document.getElementById("trainingId").value,
        userId: document.getElementById("userId").value,
    }
    if (UpdateAppointment(appointment)) {
        ClearForms();
    }
});

function AddAppointmentToTable(appointment) {
    if (!appointments.some(rec => rec.id === appointment.id)) appointments.push(appointment);

    document.getElementById("tableContext").innerHTML += `
    <tr id="rowappointment${appointment.id}">
        <td>${appointment.id}</td>
        <td>${appointment.trainingId}</td>
        <td>${appointment.userId}</td>
        <td> 
            <button class="btn btn-warning m-2" onclick='SelectAppointment(${(JSON.stringify(appointment))})'>Change</button> 
            <button class="btn btn-danger m-2" onclick='DeleteAppointment(${appointment.id})'>Delete</button> 
        </td>
    </tr>`;
}

function ClearForms() {
    document.getElementById("id").value = 0;
    document.getElementById("trainingId").value = 1;
    document.getElementById("userId").value = "";
}

async function GetAppointments() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/appointments`);

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

async function UpdateAppointment(appointment) {
    try {
        const appointmentString = JSON.stringify(appointment);
        const response = await fetch(`http://194.87.92.189:5000/api/appointments/${appointmentString}`, { method: "PUT" });

        if (response.ok === true) {
            const appointment = await response.json();
            UpdateTableAppointment(appointment);
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

async function AddAppointment(appointment) {
    try {
        const appointmentString = encodeURIComponent(JSON.stringify(appointment));
        const response = await fetch(`http://194.87.92.189:5000/api/appointments/${appointmentString}`, { method: "POST" });

        if (response.ok === true) {
            const appointment = await response.json();

            AddAppointmentToTable(appointment);

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

function UpdateTableAppointment(appointment) {
    let row = document.getElementById(`rowappointment${appointment.id}`);
    row.innerHTML = `
                <td>${appointment.id}</td>
                <td>${appointment.trainingId}</td>
                <td>${appointment.userId}</td>
                <td> 
                    <button class="btn btn-warning m-2" onclick='SelectAppointment(${(JSON.stringify(appointment))})'>Change</button> 
                    <button class="btn btn-danger m-2" onclick='DeleteAppointment(${appointment.id})'>Delete</button> 
                </td>`
}

async function DeleteAppointment(appointmentid) {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/appointments/${appointmentid}`, { method: "DELETE" });

        if (response.ok === true) {
            DeleteTableappointment(appointmentid);
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

function DeleteTableappointment(appointmentid) {
    appointments.splice(appointments.indexOf(appointment => appointment.id === appointmentid), 1);
    document.getElementById(`rowappointment${appointmentid}`).remove();
}