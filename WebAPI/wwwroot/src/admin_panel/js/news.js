let news = [];


async function fetchData() {
    try {
        news = await GetNews();
        news.forEach(article => AddArticleToTable(article));
    } catch (error) {
        console.log(error);
    }
}
fetchData();

document.getElementById("submit").addEventListener("click", () => {
    var decodedPhoto = decodeURIComponent(document.getElementById("photo").value)
    var article =
    {
        name: document.getElementById("name").value,
        description: document.getElementById("description").value,
        text: document.getElementById("text").value,
        photo: decodedPhoto
    };
    AddArticle(article);
    ClearForms();
})


function SelectArticle(article) {
    document.getElementById("id").value = article.id;
    document.getElementById("name").value = article.name;
    document.getElementById("description").value = article.description;
    document.getElementById("text").value = article.text;
    document.getElementById("photo").value = decodeURIComponent(article.photo);
}

document.getElementById("update").addEventListener("click", () => {
    var decodedPhoto = decodeURIComponent(document.getElementById("photo").value)
    const article =
    {
        id: document.getElementById("id").value,
        name: document.getElementById("name").value,
        description: document.getElementById("description").value,
        text: document.getElementById("text").value,
        photo: decodedPhoto
    };
    if (UpdateArticle(article)) {
        ClearForms();
    }
});


function AddArticleToTable(article) {

    if (!news.some(art => art.id === article.id)) news.push(article);

    document.getElementById("tableContext").innerHTML += `
    <tr id="rowarticle${article.id}">
        <td>${article.id}</td>
        <td>${article.name}</td>
        <td>${article.description}</td>
        <td>${article.text}</td>
        <td>${decodeURIComponent(article.photo)}</td>
        <td> 
            <button class="btn btn-warning m-2" onclick='SelectArticle(${(JSON.stringify(article))})'>Change</button> 
            <button class="btn btn-danger m-2" onclick='DeleteArticle(${article.id})'>Delete</button> 
        </td>
    </tr>`;
}

function ClearForms() {
    document.getElementById("id").value = 0;
    document.getElementById("name").value = 1;
    document.getElementById("description").value = "";
    document.getElementById("text").value = "";
    document.getElementById("photo").value = "";
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


async function UpdateArticle(article) {
    try {
        const articleString = encodeURIComponent(JSON.stringify(article));
        const response = await fetch(`http://194.87.92.189:5000/api/news/${articleString}`, { method: "PUT" });

        if (response.ok === true) {
            const article = await response.json();
            UpdateTableArticle(article);
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


async function AddArticle(article) {
    try {
        const articleString = encodeURIComponent(JSON.stringify(article));
        const response = await fetch(`http://194.87.92.189:5000/api/news/${articleString}`, { method: "POST" });

        if (response.ok === true) {
            const article = await response.json();

            AddArticleToTable(article);

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


function UpdateTableArticle(article) {
    let row = document.getElementById(`rowarticle${article.id}`);
    row.innerHTML = `
                <td>${article.id}</td>
                <td>${article.name}</td>
                <td>${article.description}</td>
                <td>${article.text}</td>
                <td>${decodeURIComponent(article.photo)}</td>
                <td> 
                    <button class="btn btn-warning m-2" onclick='SelectArticle(${(JSON.stringify(article))})'>Change</button> 
                    <button class="btn btn-danger m-2" onclick='DeleteArticle(${article.id})'>Delete</button> 
                </td>`
}


async function DeleteArticle(articleid) {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/news/${articleid}`, { method: "DELETE" });

        if (response.ok === true) {
            DeleteTableArticle(articleid);
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

function DeleteTableArticle(articleid) {
    news.splice(news.indexOf(article => article.id === articleid), 1);
    document.getElementById(`rowarticle${articleid}`).remove();
}