
var userAlreadyRegistered = false;

async function openPopup(training, formattedTime) {
    var dataUser = localStorage.getItem("user");
    var userFromLS = JSON.parse(dataUser);
    if (training.extendedTeacher.user.id === userFromLS.id) {
        redirectToTrainersSession(JSON.stringify(training))
    }
    else {
        await GetAppointment(training.id, userFromLS.id);
        var popup = document.getElementById("popup");

        document.getElementById('popup')
            .querySelector('.trainer_popup_card_wrapper')
            .setAttribute("onclick", `ChangeToteacherPage(${training.extendedTeacher.id})`);

        var overlay = document.createElement("div");
        overlay.className = "overlay";
        document.body.appendChild(overlay);
        popup.classList.add("popup_visible");
        overlay.addEventListener("click", closePopup);
        document.addEventListener("keydown", handleKeyPress);
        var popupHeaderIcon = document.querySelector('.trainer_popup_header_icon');
        var popupHeaderText = document.querySelector('.trainer_popup_header_text');
        var trainerCardPhoto = document.querySelector('.trainer_popup_card_photo');
        var trainerCardHeaderText = document.querySelector('.trainer_popup_card_text_header');
        var timePopupHeader = document.getElementById('training_time');
        var placesPopupHeader = document.getElementById('training_seats');
        popupHeaderIcon.setAttribute("src", training.activity.icon);
        popupHeaderText.textContent = training.activity.name;
        trainerCardPhoto.setAttribute("src", training.extendedTeacher.user.photo);
        trainerCardHeaderText.textContent = `${training.extendedTeacher.user.name} ${training.extendedTeacher.user.surname}`;
        timePopupHeader.textContent = formattedTime;
        placesPopupHeader.textContent = training.leftSeats;

        var button = document.getElementById("signupButton");
        console.log(training)
        console.log(userFromLS)
        console.log(userAlreadyRegistered)
        if (userAlreadyRegistered) {
            button.textContent = "Вы уже записаны";
            button.classList.add("banner_btn_disabled")
            button.disabled = true;
        }
        else {
            button.textContent = "Записаться";
            button.classList.remove("banner_btn_disabled")
            button.disabled = false;
            button.onclick = function () { makeAnAppointment(training.id) };
        }
    }


}

function closePopup() {
    var popup = document.getElementById("popup");
    var overlay = document.querySelector(".overlay");
    document.body.removeChild(overlay);
    popup.classList.remove("popup_visible");
    overlay.removeEventListener("click", closePopup);
    document.removeEventListener("keydown", handleKeyPress);
}

function handleKeyPress(event) {
    if (event.keyCode === 27) {
        closePopup();
    }
}
function openConfirmingPopup() {
    var confirmingPopup = document.getElementById("confirming_popup");
    var overlay = document.createElement("div");
    overlay.className = "overlay";
    document.body.appendChild(overlay);
    confirmingPopup.classList.add("confirming_popup_visible");
    overlay.addEventListener("click", closeConfirmingPopup);
    document.addEventListener("keydown", handleKeyPress);
}

function closeConfirmingPopup() {
    var confirmingPopup = document.getElementById("confirming_popup");
    var overlay = document.querySelector(".overlay");
    document.body.removeChild(overlay);
    confirmingPopup.classList.remove("confirming_popup_visible");
    overlay.removeEventListener("click", closeConfirmingPopup);
    document.removeEventListener("keydown", handleKeyPress);
}
document.getElementById("signupButton").addEventListener("click", function (event) {
    if (!userAlreadyRegistered) {
        event.preventDefault();
        closePopup();
        openConfirmingPopup();
    }
});

document.getElementById("closeButton").addEventListener("click", function (event) {
    closeConfirmingPopup();
});


function redirectToTrainersSession(training) {
    localStorage.setItem('training', training);
    document.location = 'trainers_session.html';
}

async function GetAppointment(trainingId, userId) {
    try {
        const appointment = {
            TrainingId: trainingId,
            UserId: userId
        }
        const appointmentString = JSON.stringify(appointment);
        const response = await fetch(`http://194.87.92.189:5000/api/appointments/${appointmentString}`);

        if (response.ok === true) {
            userAlreadyRegistered = (await response.json()) != null;
        } else {
            const error = await response.json();
            console.log(error.message);
        }
    } catch (error) {
        console.log(error);
    }
}