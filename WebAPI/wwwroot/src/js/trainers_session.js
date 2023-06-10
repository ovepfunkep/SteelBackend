var dataTraining = localStorage.getItem("training");
var training = JSON.parse(dataTraining);
console.log(training)
let appointments = [];
let checkedAppointments = [];

async function fetchData() {
    try {
        await GetAppointmentExtendedByTrainerId();
        console.log(appointments)
        // Добавьте дату сессии только один раз перед циклом
        document.getElementById("trainer_session_section").innerHTML += `
            <h3 class="date_of_session mt-5">Дата: ${formatDate(training.dateTimeStart)}</h3>
        `;
        appointments.forEach(appointment => AddAppointmentToPage(appointment));
    } catch (error) {
        console.log(error);
    }
}
fetchData();

function AddAppointmentToPage(appointment) {
    document.getElementById("trainer_session_section").innerHTML += `
        <form class="attendance_list mt-5">
            <div id="appointment${appointment.id}" class="col-md-12 mb-3 present_list">
                <input class="checkbox" type="checkbox" onchange='AddCheckedAppointment(${(JSON.stringify(appointment))})' name="present_people">
                <label>${appointment.user.name} ${appointment.user.surname}</label>
            </div>
        </form>
    `;
}

function AddCheckedAppointment(appointment) {
    checkbox = document.getElementById(`appointment${appointment.id}`).querySelector(".checkbox");
    if (checkbox.checked) {
        checkedAppointments.push(appointment)
        console.log(checkedAppointments)
    }
    else {
        checkedAppointments.splice(checkedAppointments.indexOf(a => a.id == appointment.id), 1)
        console.log(checkedAppointments)
    }
}

document.querySelector(".banner_btn").addEventListener("click", () => {
    console.log(checkedAppointments)
    checkedAppointments.forEach(appointment => AddAttend(appointment.training.id, appointment.user.id))
});

function formatDate(dateTime) {
    const date = new Date(dateTime);
    const day = date.getDate().toString().padStart(2, '0'); // Получите день и добавьте ведущий ноль, если необходимо
    const month = (date.getMonth() + 1).toString().padStart(2, '0'); // Получите месяц и добавьте ведущий ноль, если необходимо
    const year = date.getFullYear().toString(); // Получите год

    return `${day}.${month}.${year}`;
}

async function GetAppointmentExtendedByTrainerId() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/appointments/extended/training/${training.id}`);

        if (response.ok === true) {
            appointments = await response.json();
        } else {
            const error = await response.json();
            console.log(error.message);
        }
    } catch (error) {
        console.log(error);
    }
}

async function AddAttend(trainingId, userId) {
    try {
        const attend = {
            TrainingId: trainingId,
            UserId: userId
        }
        const attendString = JSON.stringify(attend);
        const response = await fetch(`http://194.87.92.189:5000/api/attendees/${attendString}`, { method: "POST" });

        if (response.ok === true) {
            const data = await response.json();
            console.log(data);
        } else {
            const error = await response.json();
            console.log(error.message);
        }
    } catch (error) {
        console.log(error);
    }
}
