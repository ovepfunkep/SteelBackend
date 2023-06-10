let trainings = [];

async function fetchData() {
    try {
        trainings = await GetMonthlyTrainings();
    } catch (error) {
        console.log(error);
    }
}


async function GetMonthlyTrainings() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/trainings/monthly/${(new Date()).toJSON()}`);

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