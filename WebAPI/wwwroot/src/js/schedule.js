

let selectedDate = new Date();
var containers = document.querySelectorAll(document.documentElement.clientWidth > 768 ? '.line_trainings .column_with_trainings_card' :
    '.day-of-week');

awaitData();


function DecreaseWeek() {
    selectedDate = new Date(selectedDate.getTime() - 7 * 24 * 60 * 60 * 1000);
}

function getCurrentWeekDate() {
    return new Date();
}

function IncreaseWeek() {
    selectedDate = new Date(selectedDate.getTime() + 7 * 24 * 60 * 60 * 1000);
}

async function UpdateByDate(changeWeek) {
    changeWeek();
    var currentDateMonday = new Date(selectedDate);
    selectedDate = new Date(currentDateMonday);
    var currentDayOfWeek = currentDateMonday.getDay();
    var daysToMonday = currentDayOfWeek === 0 ? 6 : currentDayOfWeek - 1;
    currentDateMonday.setDate(currentDateMonday.getDate() - daysToMonday);
    var currentDateSunday = new Date(currentDateMonday);
    currentDateSunday.setDate(currentDateSunday.getDate() + 6);

    var formatter = new Intl.DateTimeFormat('ru', { month: 'long', day: 'numeric' });

    var formattedDateMonday = formatter.format(currentDateMonday);
    var formattedDateSunday = formatter.format(currentDateSunday);


    var monthTextElement = document.querySelector('.month_text');
    monthTextElement.textContent = `${formattedDateMonday} - ${formattedDateSunday}`;


    if (document.documentElement.clientWidth > 768) {
        containers.forEach(function (container) {
            container.querySelector('.cards').innerHTML = '';
        });
    }
    else {
        containers.forEach(function (container) {
            container.querySelector('.row').innerHTML = '';
        });
    }


    containers.forEach(function (container) {
        if (document.documentElement.clientWidth > 768) container.querySelector('.date').textContent = currentDateMonday.getDate();
        else {
            console.log(container.querySelector('.new_days_of_week_header .date').textContent);
            container.querySelector('.new_days_of_week_header .date').textContent = currentDateMonday.getDate();
        }
        trainings.filter(training => {
            var trainingDate = new Date(training.dateTimeStart);
            return (
                trainingDate.getDate() == currentDateMonday.getDate() &&
                trainingDate.getMonth() == currentDateMonday.getMonth() &&
                IsCompatible(training)
            );
        }).forEach(training => {
            const startTime = training.dateTimeStart.split('T')[1];
            const endTime = training.dateTimeEnd.split('T')[1];

            var start = new Date(`2000-01-01 ${startTime}`);
            var end = new Date(`2000-01-01 ${endTime}`);

            const startHours = start.getHours().toString().padStart(2, '0');
            const startMinutes = start.getMinutes().toString().padStart(2, '0');

            const endHours = end.getHours().toString().padStart(2, '0');
            const endMinutes = end.getMinutes().toString().padStart(2, '0');

            const formattedTime = `${startHours}:${startMinutes} – ${endHours}:${endMinutes}`;

            var tempContainer = document.documentElement.clientWidth > 768 ? container.querySelector('.cards') : container.querySelector('.column_with_trainings_card');

            tempContainer.innerHTML += document.documentElement.clientWidth > 768 ?
                `
    <div class="schedule_card  ${training.activity.name === "Йога" ? "blue" : training.activity.name === "Фитнес" ? "green" : ""}"
    onclick='openPopup(${JSON.stringify(training)}, "${formattedTime}")'>
      <div class="card_name">${training.activity.name}</div>
      <div class="card_line ${training.activity.name === "Йога" ? "border_blue" : training.activity.name === "Фитнес" ? "border_green" : ""}"></div>
      <div class="trainer_name">${training.extendedTeacher.user.name} ${training.extendedTeacher.user.surname}</div>
      <div class="card_time">${formattedTime}</div>
    </div>` :
                `
                <div class="col-md-3 col-6">
                <div class="clearfix">
    <div class="schedule_card  ${training.activity.name === "Йога" ? "blue" : training.activity.name === "Фитнес" ? "green" : ""}"
    onclick='openPopup(${JSON.stringify(training)}, "${formattedTime}")'>
      <div class="card_name">${training.activity.name}</div>
      <div class="card_line ${training.activity.name === "Йога" ? "border_blue" : training.activity.name === "Фитнес" ? "border_green" : ""}"></div>
      <div class="trainer_name">${training.extendedTeacher.user.name} ${training.extendedTeacher.user.surname}</div>
      <div class="card_time">${formattedTime}</div>
    </div>
    </div>
    </div>`;

        });

        currentDateMonday.setDate(currentDateMonday.getDate() + 1);

    });
};

async function makeAnAppointment(trainingId) {
    var dataUser = localStorage.getItem("user");
    var userFromLS = JSON.parse(dataUser);
    try {
        const appointment = {
            TrainingId: trainingId,
            UserId: userFromLS.id
        }
        const appointmentString = JSON.stringify(appointment);
        const response = await fetch(`http://194.87.92.189:5000/api/appointments/${appointmentString}`, { method: "POST" });

        if (response.ok === false) {
            const error = await response.json();
            appendAlert('Упс! Что-то пошло не так')
            console.log(error.message);
        }
    } catch (error) {
        console.log(error);
    }
};

async function ChangeToteacherPage(teacherId) {
    let teacher = await GetTeacher(teacherId);

    localStorage.setItem("data", JSON.stringify(teacher));
    window.location.href = "trainer_page.html";
}

async function GetTeacher(teacherId) {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/teachers/extended/${teacherId}`);

        if (response.ok === true) {
            return await response.json();
        } else {
            const error = await response.json();
            console.log(error.message);
        }
    } catch (error) {
        console.log(error);
    }
};


function toggleCheckbox(option) {
    var checkbox = option.querySelector("input[type='checkbox']");
    checkbox.checked = !checkbox.checked;
    UpdateByDate(getCurrentWeekDate);
}

async function awaitData() {
    await createFilters();

    UpdateByDate(getCurrentWeekDate);
}

const alertPlaceholder = document.querySelector(".alertContainer");

const appendAlert = (message) => {
    CloseAlert();
    const wrapper = document.createElement('div');
    wrapper.innerHTML = [
        `<div class="alert alert-danger" role="alert">`,
        `   <div>${message}</div>`,
        '   <button type="button" onClick="CloseAlert()" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>',
        '</div>'
    ].join('');

    alertPlaceholder.append(wrapper);
    alertPlaceholder.scrollIntoView(wrapper);
};