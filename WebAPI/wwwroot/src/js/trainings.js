async function fetchData() {
    try {
        activities = await GetActivities();
        activities.forEach(activity => AddActivityToPage(activity));
    } catch (error) {
        console.log(error);
    }
}
fetchData();



function AddActivityToPage(activity) {
    if (!activities.some(act => act.id === activity.id)) activities.push(activity);
    const descriptions = activity.description.split('.');
    const selectedDescriptions = descriptions.slice(0, 2);
    document.getElementById("activity_card_container").innerHTML += `
    <div class="container">

    <div class="trainings_card_wrapper_mini xl-hidden_on_index">

        <div class="trainings_card_text_wrapper">
            <div class="card_header">
                <img src="${decodeURIComponent(activity.icon)}" class="mini_icon">
                <h2 class="card_name">${activity.name}</h2>
            </div>
            <div class="card_descr">
                ${selectedDescriptions}
            </div>
            <button onclick="ChangeToActivityPage(${activity.id})" class="all_btn">Подробнее<img
                    src="https://svgshare.com/i/u1G.svg" class="btn_arrow"></button>
        </div>

        <img src="${decodeURIComponent(activity.photo)}" class="trainings_card_mini_img">
    </div>

    <div class="row md-hidden_on_index  first_card">
        <div class="col-md-6 offset-md-0 no_pad_r ">
            <div class="trainings_card_wrapp">
                <img src="${decodeURIComponent(activity.photo)}" class="trainings_card_img">
            </div>

        </div>
        <div class="col-md-6 offset-md-0 no_pad_l">
            <div class="card_description">
                <div class="header">
                    <img src="${decodeURIComponent(activity.icon)}" class="mini_icon">
                    <h2 class="card_name">${activity.name}</h2>
                </div>

                <div class="text">
                ${selectedDescriptions}
                </div>

                <button onclick="ChangeToActivityPage(${activity.id})" class="all_btn green_b">Подробнее<img
                        src="https://svgshare.com/i/u1G.svg" class="btn_arrow"></button>
            </div>

        </div>
    </div>
    <div class="trainings_line md-hidden_on_index"></div>
</div>
    `;
}

function ChangeToActivityPage(activityId) {
    var activity = activities.find(act => act.id === activityId);
    localStorage.setItem("data", JSON.stringify(activity))
    window.location.href = "activity_page.html";
}

async function GetActivities() {
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