let teachers = [];
var data = localStorage.getItem("data");
var activity = JSON.parse(data);
AwaitData();

function AddteacherToPage(teacher) {
    console.log(teacher)
    if (!teachers.some(teach => teach.id === teacher.id)) teachers.push(teacher);
    document.getElementById("trainer_on_activity_page").innerHTML += `
        <div class="col-6 col-md-4 col-lg-3">
            <div class="trainers_card"  onclick="ChangeToteacherPage (${teacher.id})">
                <div class="first_circle">
                    <div class="second_circle">
                        <img src="${decodeURIComponent(teacher.user.photo)}" class="circle_img">
                    </div>
                </div>
                <h2 class="trainer_name">${teacher.user && teacher.user.name}</h2>
            </div>
         </div>
    `;
}


function ChangeToteacherPage(teacherId) {
    var teacher = teachers.find(teach => teach.id === teacherId);
    localStorage.setItem("data", JSON.stringify(teacher));
    window.location.href = "trainer_page.html";
}

async function GetTeachers() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/teachers/extended/activity/${activity.id}`);

        if (response.ok === true) {
            teachers = await response.json();
        } else {
            const error = await response.json();
            console.log(error.message);
        }
    } catch (error) {
        console.log(error);
    }
}

async function AwaitData() {
    await GetTeachers();

    teachers.forEach(teacher => AddteacherToPage(teacher));

    document.getElementById("activity_page_title").innerText = activity.name;
    document.getElementById("training_name").innerText = activity.name;
    document.getElementById("training_descr").innerText = activity.description;
    document.getElementById("banner").setAttribute("src", decodeURIComponent(activity.photo));
    document.getElementById("training_name").innerText = activity.name;
}
