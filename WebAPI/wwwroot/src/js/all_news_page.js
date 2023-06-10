let news = [];


async function fetchData() {
    try {
        news = await GetNews();
        console.log(news)
        news.forEach(article => AddArticleToPage(article));
    } catch (error) {
        console.log(error);
    }
}
fetchData();

function AddArticleToPage(article) {
    if (!news.some(art => art.id === article.id)) news.push(article);

    document.getElementById("news_cards_section").innerHTML += `
    <div class="col-md-6 calendar_cards_section">
    <div class="promotions_wrapper" onclick="ChangeToArticlePage(${article.id})">
        <div class="promotions_img_wrapper">
            <img src="${article.photo}" class="promotions_img">
        </div>
        <div class="promotions_text_wrapper">
            <h2 class="promotions_header">${article.name}</h2>
            <h3 class="promotions_subheader">${article.description}</h3>
        </div>
    </div>
</div>
    `
}

function ChangeToArticlePage(articleId) {
    var article = news.find(art => art.id === articleId);
    localStorage.setItem("data", JSON.stringify(article))
    window.location.href = "news_page.html";
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