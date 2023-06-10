// Получите родительский элемент, в котором будет содержаться список тренеров
var filterContent = document.getElementById('filter1');

// Получите родительский элемент, в котором будет содержаться список тренировок
var filterContent2 = document.getElementById('filter2');
console.log(trainings)

function toggleFilter(filterId) {
    var filter = document.getElementById(filterId);
    filter.classList.toggle('show');
}


function IsCompatible(training) {
    var trainerName = `${training.extendedTeacher.user.name} ${training.extendedTeacher.user.surname}`;
    var selectedTrainers = getSelectedFilters('filter1');
    var selectedActivities = getSelectedFilters('filter2');
    return selectedTrainers.includes(trainerName) && selectedActivities.includes(training.activity.name);
}

function getSelectedFilters(filterName) {
    var checkboxes = document.querySelectorAll(`input[name="${filterName}_option"]:checked`);
    var selectedFilters = [];
    checkboxes.forEach(function (checkbox) {
        selectedFilters.push(checkbox.value);
    });
    return selectedFilters;
}

async function createFilters() {
    await fetchData();

    // Пройдитесь по массиву trainings и получите уникальные имена и фамилии тренеров
    var uniqueTrainers = [...new Set(trainings.map(training => `${training.extendedTeacher.user.name} ${training.extendedTeacher.user.surname}`))];

    // Создайте и добавьте элементы списка тренеров в родительский элемент
    uniqueTrainers.forEach(trainer => {
        var filterOption = document.createElement('div');
        filterOption.className = 'filter_option';
        filterOption.onclick = function () {
            toggleCheckbox(this);
        };

        var checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.name = 'filter1_option';
        checkbox.value = trainer;
        checkbox.checked = true;

        var label = document.createElement('label');
        label.textContent = trainer;

        filterOption.appendChild(checkbox);
        filterOption.appendChild(label);
        filterContent.appendChild(filterOption);
    });

    // Пройдитесь по массиву trainings и получите уникальные названия тренировок
    var uniqueActivities = [...new Set(trainings.map(training => training.activity.name))];
    console.log(uniqueActivities)

    // Создайте и добавьте элементы списка тренировок в родительский элемент
    uniqueActivities.forEach(activityName => {
        var filterOption = document.createElement('div');
        filterOption.className = 'filter_option';
        filterOption.onclick = function () {
            toggleCheckbox(this);
        };

        var checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.name = 'filter2_option';
        checkbox.value = activityName;
        checkbox.checked = true;

        var label = document.createElement('label');
        label.textContent = activityName;

        filterOption.appendChild(checkbox);
        filterOption.appendChild(label);
        filterContent2.appendChild(filterOption);
    });
}