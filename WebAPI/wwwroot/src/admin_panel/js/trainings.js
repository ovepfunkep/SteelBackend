/* = gettrainings() */;
let trainings = [];

async function fetchData() {
    try {
        trainings = await GetTrainings();
        trainings.forEach(training => AddTrainingToTable(training));
    } catch (error) {
        console.log(error);
    }
}
fetchData();



document.getElementById("submit").addEventListener("click", () => {
    var training = {
        activityId: document.getElementById("activityId").value,
        teacherId: document.getElementById("teacherId").value,
        dateTimeStart: document.getElementById("dateTimeStart").value,
        totalSeats: document.getElementById("totalSeats").value,
    }
    AddTraining(training);
    ClearForms();
})

function SelectTraining(training) {
    document.getElementById("id").value = training.id;
    document.getElementById("activityId").value = training.activityId;
    document.getElementById("teacherId").value = training.teacherId;
    document.getElementById("dateTimeStart").value = training.dateTimeStart;
    document.getElementById("totalSeats").value = training.totalSeats;
}

document.getElementById("update").addEventListener("click", () => {
    const training = {
        id: document.getElementById("id").value,
        activityId: document.getElementById("activityId").value,
        teacherId: document.getElementById("teacherId").value,
        dateTimeStart: document.getElementById("dateTimeStart").value,
        totalSeats: document.getElementById("totalSeats").value,
    }
    if (UpdateTraining(training)) {
        ClearForms();
    }
});

function AddTrainingToTable(training) {
    if (!trainings.some(trai => trai.id === training.id)) trainings.push(training);
    document.getElementById("tableContext").innerHTML += `
    <tr id="rowtraining${training.id}">
        <td>${training.id}</td>
        <td>${training.activityId}</td>
        <td>${training.teacherId}</td>
        <td>${training.dateTimeStart}</td>
        <td>${training.totalSeats}</td>
        <td> 
            <button class="btn btn-warning m-2" onclick='SelectTraining(${(JSON.stringify(training))})'>Change</button> 
            <button class="btn btn-danger m-2" onclick='Deletetraining(${training.id})'>Delete</button> 
        </td>
    </tr>`;
}

function ClearForms() {
    document.getElementById("id").value = 0;
    document.getElementById("activityId").value = 1;
    document.getElementById("teacherId").value = 1;
    document.getElementById("dateTimeStart").value = "";
    document.getElementById("totalSeats").value = "";
}

async function GetTrainings() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/trainings`);

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

async function UpdateTraining(training) {
    try {
        const trainingstring = JSON.stringify(training);
        const response = await fetch(`http://194.87.92.189:5000/api/trainings/${trainingstring}`, { method: "PUT" });

        if (response.ok === true) {
            const training = await response.json();
            UpdateTableTraining(training);
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


async function AddTraining(training) {
    try {
        const trainingstring = encodeURIComponent(JSON.stringify(training));
        const response = await fetch(`http://194.87.92.189:5000/api/trainings/${trainingstring}`, { method: "POST" });

        if (response.ok === true) {
            const training = await response.json();

            AddTrainingToTable(training);

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


function UpdateTableTraining(training) {
    let row = document.getElementById(`rowtraining${training.id}`);
    row.innerHTML = `
                <td>${training.id}</td>
                <td>${training.activityId}</td>
                <td>${training.teacherId}</td>
                <td>${training.dateTimeStart}</td>
                <td>${training.totalSeats}</td>
                <td> 
                    <button class="btn btn-warning m-2" onclick='SelectTraining(${(JSON.stringify(training))})'>Change</button> 
                    <button class="btn btn-danger m-2" onclick='DeleteTraining(${training.id})'>Delete</button> 
                </td>`
}


async function DeleteTraining(trainingid) {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/trainings/${trainingid}`, { method: "DELETE" });

        if (response.ok === true) {
            DeleteTableTraining(trainingid);
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

function DeleteTableTraining(trainingid) {
    trainings.splice(trainings.indexOf(training => training.id === trainingid), 1);
    document.getElementById(`rowtraining${trainingid}`).remove();
}