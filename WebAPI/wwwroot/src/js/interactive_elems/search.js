let activities = [];
let trainers = [];
let news = [];
let currentSelection = [];

async function fetchData() {
    try {
        activities = await Getactivities();
        trainers = await GetTrainers();
        news = await GetNews();
        const firstFourTrainers = trainers.slice(0, 4);
        const firstFourActivities = activities.slice(0, 4);
        const firstThreeNews = news.slice(0, 3);
        firstFourTrainers.forEach(trainer => AddTrainerToPage(trainer));
        firstFourActivities.forEach(activity => AddactivityToPage(activity));
        firstThreeNews.forEach(article => AddArticleToPage(article));
    } catch (error) {
        console.log(error);
    }
}

fetchData();

function LoadSearchData(filterWord) {
    let container = document.querySelector('.results');
    container.innerHTML = '';

    activities.filter(activity => activity.name.includes(filterWord)).forEach(activity => {
        const descriptions = activity.description.split('.');
        const selectedDescriptions = descriptions.slice(0, 2);
        if (container.querySelectorAll('.result_cont').length == 4) return;
        container.innerHTML += `
        <div class="result_cont" onclick="ChangeToActivityPage(${activity.id})">
        <img src="${decodeURIComponent(activity.photo)}" class="result_img">
        <div class="result_textContent">
            <h6 class="result_header">${activity.name}</h6>
            <h6 class="result_Subheader">${selectedDescriptions}</h6>
        </div>

    </div>
 `
    })

    trainers.filter(trainer => trainer.user.name.includes(filterWord) || trainer.user.surname.includes(filterWord)).forEach(trainer => {
        if (container.querySelectorAll('.result_cont').length == 4) return;
        container.innerHTML += `
        <div class="result_cont" onclick="ChangeToTrainerPage(${trainer.id})">
        <img src="${decodeURIComponent(trainer.user.photo)}" class="result_img">
        <div class="result_textContent">
            <h6 class="result_header">${trainer.user.name} ${trainer.user.surname}</h6>
            <h6 class="result_Subheader">${trainer.description}</h6>
        </div>

    </div>
 `
    })

    news.filter(article => article.name.includes(filterWord)).forEach(article => {
        if (container.querySelectorAll('.result_cont').length == 4) return;
        container.innerHTML += `
        <div class="result_cont" onclick="ChangeToNewsPage (${article.id})">
        <img src="${decodeURIComponent(article.photo)}" class="result_img">
        <div class="result_textContent">
            <h6 class="result_header">${article.name}</h6>
            <h6 class="result_Subheader">${article.description}</h6>
        </div>

    </div>
 `
    })
}

async function Getactivities() {
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

async function GetTrainers() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/teachers/extended`);

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

async function GetNews() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/news`);

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