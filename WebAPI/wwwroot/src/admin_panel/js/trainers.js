let trainers = [];

async function fetchData() {
    try {
        trainers = await GetTrainers();
        trainers.forEach(trainer => AddTrainerToTable(trainer));
    } catch (error) {
        console.log(error);
    }
}
fetchData();

document.getElementById("submit").addEventListener("click", () => {
    var trainer = {
        userId: document.getElementById("userId").value || 11,
        experience: document.getElementById("experience").value || 1,
        description: document.getElementById("description").value || 'default'
    }
    AddTrainer(trainer);
    ClearForms();
})

function SelectTrainer(trainer) {
    document.getElementById("id").value = trainer.id;
    document.getElementById("userId").value = trainer.userId;
    document.getElementById("experience").value = trainer.experience;
    document.getElementById("description").value = trainer.description;
}

document.getElementById("update").addEventListener("click", () => {
    const trainer = {
        id: document.getElementById("id").value,
        userID: document.getElementById("userId").value,
        experience: document.getElementById("experience").value,
        description: document.getElementById("description").value,
    }
    if (UpdateTrainer(trainer)) {
        ClearForms();
    }
});

function AddTrainerToTable(trainer) {
    if (!trainers.some(tra => tra.id === trainer.id)) trainers.push(trainer);

    document.getElementById("tableContext").innerHTML += `
    <tr id="rowtrainer${trainer.id}">
        <td>${trainer.id}</td>
        <td>${trainer.userId}</td>
        <td>${trainer.experience}</td>
        <td>${trainer.description}</td>
        <td> 
            <button class="btn btn-warning m-2" onclick='SelectTrainer(${(JSON.stringify(trainer))})'>Change</button> 
            <button class="btn btn-danger m-2" onclick='DeleteTrainer(${trainer.id})'>Delete</button> 
        </td>
    </tr>`;
}

function ClearForms() {
    document.getElementById("id").value = 0;
    document.getElementById("userId").value = 1;
    document.getElementById("experience").value = "";
    document.getElementById("description").value = "";
}

async function GetTrainers() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/teachers`);

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

async function UpdateTrainer(trainer) {
    try {
        console.log(trainer);
        const trainerString = JSON.stringify(trainer);
        const response = await fetch(`http://194.87.92.189:5000/api/teachers/${trainerString}`, { method: "PUT" });

        if (response.ok === true) {
            const trainer = await response.json();
            UpdateTableTrainer(trainer);
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

async function AddTrainer(trainer) {
    try {
        const trainerString = encodeURIComponent(JSON.stringify(trainer));
        const response = await fetch(`http://194.87.92.189:5000/api/teachers/${trainerString}`, { method: "POST" });

        if (response.ok === true) {
            const trainer = await response.json();

            AddTrainerToTable(trainer);

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


function UpdateTableTrainer(trainer) {
    let row = document.getElementById(`rowtrainer${trainer.id}`);
    row.innerHTML = `
                <td>${trainer.id}</td>
                <td>${trainer.userId}</td>
                <td>${trainer.experience}</td>
                <td>${trainer.description}</td>
                <td> 
                    <button class="btn btn-warning m-2" onclick='SelectTrainer(${(JSON.stringify(trainer))})'>Change</button> 
                    <button class="btn btn-danger m-2" onclick='DeleteTrainer(${trainer.id})'>Delete</button> 
                </td>`
}


async function DeleteTrainer(trainerid) {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/teachers/${trainerid}`, { method: "DELETE" });

        if (response.ok === true) {
            DeleteTableTrainer(trainerid);
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


function DeleteTableTrainer(trainerid) {
    trainers.splice(trainers.indexOf(trainer => trainer.id === trainerid), 1);
    document.getElementById(`rowtrainer${trainerid}`).remove();
}