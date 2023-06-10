let trainers = [];

async function fetchData() {
    try {
        trainers = await GetTrainers();
        trainers.forEach(trainer => AddTrainerToPage(trainer));
    } catch (error) {
        console.log(error);
    }
}
fetchData();


function AddTrainerToPage(trainer) {
    if (!trainers.some(train => train.id === trainer.id)) trainers.push(trainer);
    const color = trainer.activities[0].name === "Йога" ? "blue_border" : trainer.activities[0].name === "Фитнес" ? "green_border" : "";
    document.getElementById("trainers_section").innerHTML += `
    <div class="col-12 col-md-6">
                    <div class="content_box upper  ${color}">
                        <img src="${decodeURIComponent(trainer.user.photo)}" class="trainer_photo">
                        <div class="mini_box">
                            <h2 class="trainer_names">${trainer.user.name}</h2>
                            <div class="trainer_descr">${trainer.activities && trainer.activities.map(activity => activity.name).join(', ') + ', опыт работы ' + trainer.experience}</div>
                            <button onclick="ChangeToTrainerPage(${trainer.id})"
                                class="all_btn green_b">Подробнее<img src="https://svgshare.com/i/u1G.svg"
                                    class="btn_arrow"></button>
                        </div>
                    </div>
                </div>
    `
};

function ChangeToTrainerPage(trainerId) {
    var trainer = trainers.find(train => train.id === trainerId);
    localStorage.setItem("data", JSON.stringify(trainer));
    window.location.href = "trainer_page.html";
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