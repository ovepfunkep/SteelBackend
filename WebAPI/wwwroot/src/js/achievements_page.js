var dataUser = localStorage.getItem("user");
var user = JSON.parse(dataUser);

let UserAchievements = [];
let AllAchievements = [];

async function fetchData() {
    try {
        UserAchievements = await GetAchievementsByUser();
        AllAchievements = await GetAllAchievements();
        AllAchievements = AllAchievements.filter(AA =>
            !UserAchievements.some(UA =>
                UA.id == AA.id
            )
        )
        UserAchievements.forEach(UserAchievement => AddUserAchievmentToPage(UserAchievement));
        AllAchievements.forEach(AllAchievement => AddAllAchievementsToPage(AllAchievement));
    } catch (error) {
        console.log(error);
    }
}
fetchData();

function AddUserAchievmentToPage(UserAchievement) {
    document.getElementById("acepted_achievements").innerHTML += `
    <div class="col-md-6">
    <div class="big_achievment_card_wrapper mt">
        <img src="${decodeURIComponent(UserAchievement.photo)}" class="achievment_img">
        <div class="text_wrapp">
            <div class="big_achievment_card_header">${UserAchievement.name}</div>
            <div class="big_achievment_card_subheader">${UserAchievement.description}</div>
        </div>
    </div>

</div>

    `
}

function AddAllAchievementsToPage(AllAchievement) {
    document.getElementById("not_acepted_achievements").innerHTML += `
    <div class="col-md-6">
    <div class="big_achievment_card_wrapper disabled mt">
        <img src="${decodeURIComponent(AllAchievement.photo)}" class="achievment_img">
        <div class="text_wrapp">
            <div class="big_achievment_card_header disabled_text">${AllAchievement.name}</div>
            <div class="big_achievment_card_subheader disabled">${AllAchievement.description}</div>
        </div>
    </div>
</div>
    `
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

async function GetAllAchievements() {
    try {
        const response = await fetch("http://194.87.92.189:5000/api/achievements");

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