let reviews = [];

async function fetchData() {
    try {
        reviews = await GetReviews();
        reviews.forEach(review => AddReviewToTable(review));
    } catch (error) {
        console.log(error);
    }
}

fetchData();

document.getElementById("submit").addEventListener("click", () => {
    var review =
    {
        teacherId: document.getElementById("teacherId").value,
        userId: document.getElementById("userId").value,
        text: document.getElementById("text").value,
        rate: document.getElementById("rate").value,
    }
    AddReview(review);
    ClearForms();
})

function SelectReview(review) {
    document.getElementById("id").value = review.id;
    document.getElementById("teacherId").value = review.teacherId;
    document.getElementById("userId").value = review.userId;
    document.getElementById("text").value = review.text;
    document.getElementById("rate").value = review.rate;
}

document.getElementById("update").addEventListener("click", () => {
    const review =
    {
        id: document.getElementById("id").value,
        teacherId: document.getElementById("teacherId").value,
        userId: document.getElementById("userId").value,
        text: document.getElementById("text").value,
        rate: document.getElementById("rate").value,
    }
    if (UpdateReview(review)) {
        ClearForms();
    }
});

function AddReviewToTable(review) {
    if (!reviews.some(rew => rew.id === review.id)) reviews.push(review);

    document.getElementById("tableContext").innerHTML += `
    <tr id="rowreview${review.id}">
        <td>${review.id}</td>
        <td>${review.teacherId}</td>
        <td>${review.userId}</td>
        <td>${review.text}</td>
        <td>${review.rate}</td>
        <td> 
            <button class="btn btn-warning m-2" onclick='SelectReview(${(JSON.stringify(review))})'>Change</button> 
            <button class="btn btn-danger m-2" onclick='DeleteReview(${review.id})'>Delete</button> 
        </td>
    </tr>`;
}

function ClearForms() {
    document.getElementById("id").value = 0;
    document.getElementById("teacherId").value = 1;
    document.getElementById("userId").value = "";
    document.getElementById("text").value = "";
    document.getElementById("rate").value = "";
}

async function GetReviews() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/reviews`);

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

async function UpdateReview(review) {
    try {
        const reviewString = JSON.stringify(review);
        const response = await fetch(`http://194.87.92.189:5000/api/reviews/${reviewString}`, { method: "PUT" });

        if (response.ok === true) {
            const review = await response.json();
            UpdateTableReview(review);
            return true;
        } else {
            const error = await response.json();
            console.log(error.message);
            return false;
        }
    } catch (error) {
        console.log(error);
        return false;
    }
}

async function AddReview(review) {
    try {
        const reviewString = encodeURIComponent(JSON.stringify(review));
        const response = await fetch(`http://194.87.92.189:5000/api/reviews/${reviewString}`, { method: "POST" });

        if (response.ok === true) {
            const review = await response.json();
            AddReviewToTable(review);
            return true;
        } else {
            const error = await response.json();
            console.log(error.message);
            return false;
        }
    } catch (error) {
        console.log(error);
        return false;
    }
}

function UpdateTableReview(review) {
    let row = document.getElementById(`rowreview${review.id}`);
    row.innerHTML = `
                <td>${review.id}</td>
                <td>${review.teacherId}</td>
                <td>${review.userId}</td>
                <td>${review.text}</td>
                <td>${review.rate}</td>
                <td> 
                    <button class="btn btn-warning m-2" onclick='SelectReview(${(JSON.stringify(review))})'>Change</button> 
                    <button class="btn btn-danger m-2" onclick='DeleteReview(${review.id})'>Delete</button> 
                </td>`
}

async function DeleteReview(reviewid) {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/reviews/${reviewid}`, { method: "DELETE" });

        if (response.ok === true) {
            DeleteTableReview(reviewid);
            return true;
        } else {
            const error = await response.json();
            console.log(error.message);
            return false;
        }
    } catch (error) {
        console.log(error);
        return false;
    }
}

function DeleteTableReview(reviewid) {
    reviews.splice(reviews.indexOf(review => review.id === reviewid), 1);
    document.getElementById(`rowreview${reviewid}`).remove();
}