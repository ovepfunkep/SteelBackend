
var dataUser = localStorage.getItem("user");
var user = JSON.parse(dataUser);

const birthDate = new Date(user.dateOfBirth);
const currentDate = new Date();
const age = calculateAge(birthDate, currentDate);
const ageText = getAgeText(age);
const formattedPhoneNumber = formatPhoneNumber(user.phone);

let trainings = [];
let achievements = [];

async function fetchData() {
    try {
        trainings = await GetUpcomingUserTrainingsExtended();
        achievements = await GetAchievementsByUser();
        console.log(trainings)
        console.log(achievements)
        trainings.forEach(training => AddTrainingToPage(training));
        achievements.forEach(achievement => AddAchievmentToPage(achievement));
    } catch (error) {
        console.log(error);
    }
}

async function AddUserToPage(user) {
    document.getElementById("account_header_info").innerHTML += `
        <div class="container">
            <div class="photo_wrapper">
                <img src="${user.photo}" class="account_photo">
        </div>
        <div class="account_content">
            <div class="account_name">${user.name} ${user.surname}</div>
            <div class="account_age">${age} ${ageText}</div>
            <div class="account_descr">
                <img src="icons/pin.svg" alt="pin" class="account_icon">
                <div class="account_text">${user.city}</div>
                <img src="icons/vk_logo.svg" class="account_icon sec">
                <div class="account_text">${user.vkontakte}</div>
                <img src="icons/telega.svg" class="account_icon sec">
                <div class="account_text">${user.telegram}</div>
                <img src="icons/info.svg"  id="account_icon_add_info"
                class="account_icon sec xl-hidden">
                </div>
                </div>
                <button onclick="document.location='settings.html'" class="gear_btn"><img src="icons/gear.svg"
                class="gear"></button>
            </div>
`

    document.getElementById("additional_info").innerHTML += `
        <div class="info_col">
            <img src="icons/phone.svg" alt="pin" class="info_icon">
            <div class="info_text">${formattedPhoneNumber}</div>
        </div>
        <div class="info_col down">
            <img src="icons/convert.svg" alt="pin" class="info_icon">
            <div class="info_text">${user.email}</div>
        </div>
`

    document.getElementById("popup_add_info").innerHTML += `
<div class="popup_row_inf">
                <img src="icons/phone.svg" alt="phone" class="popup_icon">
                <div class="popup_text">${formattedPhoneNumber}</div>
            </div>
            <div class="popup_row_inf">
                <img src="icons/convert.svg" alt="phone" class="popup_icon">
                <div class="popup_text">${user.email}</div>
            </div>
`

    await fetchData();
}


function AddTrainingToPage(training) {

    const activityDate = new Date(training.dateTimeStart.split('T')[0]);
    const activityTimeStart = training.dateTimeStart.split('T')[1].substring(0, 5);
    const activityTimeEnd = training.dateTimeEnd.split('T')[1].substring(0, 5);
    const weekdays = ['ВС', 'ПН', 'ВТ', 'СР', 'ЧТ', 'ПТ', 'СБ'];
    const dayOfWeek = weekdays[activityDate.getDay()];
    const dayOfMonth = activityDate.getDate();

    document.getElementById("calendar_cards_section").innerHTML += `

    <div class="calendar_card marg  ${training.activity.name === "Йога" ? "blue_card" : training.activity.name === "Фитнес" ? "green_card" : ""}"">

        <div class="date_card ${training.activity.name === "Йога" ? "blue_date_card" : training.activity.name === "Фитнес" ? "green_date_card" : ""}""">
            <div class="date_card_text">
            ${dayOfMonth}<br>${dayOfWeek}
            </div>
        </div>

        <div class="card_info">
            <div class="name_line">
                <div class="activity_name">${training.activity.name}</div>
                <svg class="activity_icon" viewBox="0 0 42 42"
                    xmlns="http://www.w3.org/2000/svg">
                    <path
                        d="M8.75 35L17.3425 34.083L22.113 24.9165L24.0223 15.75L15.4315 17.584L18.2945 21.2502M25.9315 26.7505H29.75V35M28 12.25C28.9283 12.25 29.8185 11.8813 30.4749 11.2249C31.1313 10.5685 31.5 9.67826 31.5 8.75C31.5 7.82174 31.1313 6.9315 30.4749 6.27513C29.8185 5.61875 28.9283 5.25 28 5.25C27.0717 5.25 26.1815 5.61875 25.5251 6.27513C24.8688 6.9315 24.5 7.82174 24.5 8.75C24.5 9.67826 24.8688 10.5685 25.5251 11.2249C26.1815 11.8813 27.0717 12.25 28 12.25Z" />
                </svg>
            </div>
            <div class="name_line">
                <div class="trainer_name">Тренер: ${training.extendedTeacher.user.name} ${training.extendedTeacher.user.surname}</div>
                <div class="trainer_name">${activityTimeStart} - ${activityTimeEnd}</div>
            </div>
        </div>
    </div>
`
}

function AddAchievmentToPage(achievement) {
    document.getElementById("achievment_block").innerHTML += `
    <div class="card_wrapper">
    <div class="achievment_card">
        <img src="${achievement.photo}" class="achievment_img">
    </div>
    <div class="achievment_card_name">${achievement.name}</div>
</div>

    `
}

function calculateAge(birthDate, currentDate) {
    const diffInMilliseconds = currentDate - birthDate;
    const millisecondsInYear = 1000 * 60 * 60 * 24 * 365.25; // Среднее количество миллисекунд в году
    const age = Math.floor(diffInMilliseconds / millisecondsInYear);
    return age;
}

function calculateAge(birthDate, currentDate) {
    const diffInMilliseconds = currentDate - birthDate;
    const millisecondsInYear = 1000 * 60 * 60 * 24 * 365.25; // Среднее количество миллисекунд в году
    const age = Math.floor(diffInMilliseconds / millisecondsInYear);
    return age;
}

function getAgeText(age) {
    if (age % 10 === 1 && age % 100 !== 11) {
        return "год";
    } else if (
        (age % 10 >= 2 && age % 10 <= 4) &&
        !(age % 100 >= 12 && age % 100 <= 14)
    ) {
        return "года";
    } else {
        return "лет";
    }
}

function formatPhoneNumber(phoneNumber) {
    const countryCode = phoneNumber.substring(0, 1); // Получаем код страны (первая цифра)
    const areaCode = phoneNumber.substring(1, 4); // Получаем код региона (три цифры)
    const firstPart = phoneNumber.substring(4, 7); // Получаем первую часть номера (три цифры)
    const secondPart = phoneNumber.substring(7, 9); // Получаем вторую часть номера (две цифры)
    const thirdPart = phoneNumber.substring(9, 11); // Получаем третью часть номера (две цифры)

    return `+${countryCode} (${areaCode}) ${firstPart}-${secondPart}-${thirdPart}`;
}

async function GetUpcomingUserTrainingsExtended() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/trainings/users/${user.id}?isUpcoming=true`);

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

async function GetAchievementsByUser() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/achievements/users/${user.id}`);

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

AddUserToPage(user)