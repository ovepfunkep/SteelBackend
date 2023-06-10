
function AddactivityToPage(activity) {
    if (!activities.some(act => act.id === activity.id)) activities.push(activity);
    const descriptions = activity.description.split('.');
    const selectedDescriptions = descriptions.slice(0, 2);
    document.getElementById("indexTrainingsSectionContainer").innerHTML += `
   
    <div class="trainings_card_wrapper_mini xl-hidden_on_index">

    <div class="trainings_card_text_wrapper">
        <div class="card_header">
            <img src="${decodeURIComponent(activity.icon)}" class="mini_icon">
            <h2 class="card_name">${activity.name}</h2>
        </div>
        <div class="card_descr">
        ${activity.description}
        </div>
        <button onclick="ChangeToActivityPage(${activity.id})" class="all_btn">Подробнее<img
                src="https://svgshare.com/i/u1G.svg" class="btn_arrow"></button>
    </div>

    <img src="${decodeURIComponent(activity.photo)}" alt="yoga" class="trainings_card_mini_img">
</div>

    <div class="trainings_card_wrapper md-hidden_on_index ">
    <div class="trainings_card_text_content">
        <div class="card_header">
            <img src="${decodeURIComponent(activity.icon)}" class="mini_icon">
            <h2 class="card_name">${activity.name}</h2>
        </div>
        <div class="card_descr">
            ${selectedDescriptions}
        </div>
        <button onclick="ChangeToActivityPage(${activity.id})" class="all_btn">Подробнее<img
                src="
                https://svgshare.com/i/u1G.svg
                " class="btn_arrow"></button>
    </div>
    <img src="${decodeURIComponent(activity.photo)}" class="trainings_card_imgs">
</div>`;

}



function AddTrainerToPage(trainer) {
    console.log(trainer)
    if (!trainers.some(train => train.id === trainer.id)) trainers.push(trainer);
    document.getElementById("trainers_section").innerHTML += `
        <div class="col-6 col-lg-3 col-md-4">
                <div class="trainers_card" onclick="ChangeToTrainerPage(${trainer.id})">
                    <div class="first_circle">
                        <div class="second_circle">
                            <img src="${decodeURIComponent(trainer.user.photo)}" class="circle_img">
                        </div>
                    </div>
                    <h2 class="trainer_name">${trainer.user && trainer.user.name} ${trainer.user && trainer.user.surname}</h2>
                    <h3 class="trainer_subtitle">${trainer.activities && trainer.activities.map(activity => activity.name).join(', ')}</h3>
                </div>
        </div>
    `;
}


function AddArticleToPage(article) {
    if (!news.some(art => art.id === article.id)) news.push(article);

    var isLast = news[news.length - 1].id === article.id;

    var wrapperClass = isLast ? "news_card_wrapper last" : "news_card_wrapper";

    document.getElementById("news_section").innerHTML += `
    <div class="col-6 col-md-4">
  
    <div class="${wrapperClass}" onclick="ChangeToNewsPage (${article.id})">
        <div class="news_card_content">
            <div class="news_card_img_wrapper">
                <img src="${decodeURIComponent(article.photo)}" class="news_card_img">
            </div>
            <div class="news_card_text_wrapper">
                <h2 class="news_card_header">${article.name}</h2>
                <h3 class="news_card_subheader">${article.description}</h3>
            </div>

        </div>
    </div>
  
</div>
    `;
}

function ChangeToTrainerPage(trainerId) {
    var trainer = trainers.find(train => train.id === trainerId);
    localStorage.setItem("data", JSON.stringify(trainer));
    window.location.href = "trainer_page.html";
}
function ChangeToActivityPage(activityId) {
    var activity = activities.find(act => act.id === activityId);
    localStorage.setItem("data", JSON.stringify(activity))
    window.location.href = "activity_page.html";
}

function ChangeToNewsPage(articleId) {
    var article = news.find(art => art.id === articleId);
    localStorage.setItem("data", JSON.stringify(article))
    window.location.href = "news_page.html";
}

