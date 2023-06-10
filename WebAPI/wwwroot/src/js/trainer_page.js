let reviews = [];
var reviewFormAdded = false;
var selectedRating = null;
var data = localStorage.getItem("data");
var teacher = JSON.parse(data);
var reviewSectionHeaderAdded = false;
var showAllBtnClicked = false;
AwaitData()

function AddReviewToPage(review) {
    if (!reviews.some(rew => rew.id === review.id)) reviews.push(review);

    var reviewSection = document.getElementById("review_section");

    if (!reviewSectionHeaderAdded) {
        reviewSection.innerHTML += `
            <div class="reviews_section_header">
                <div class="row_inf">
                    <div class="header_marker">
                        <img src="icons/marker_train.png" class="marker">
                        <span class="marker_text new_m">Отзывы</span>
                    </div>
                    <button onclick="ShowAllReviews()" class="all_btn" id="show_all_btn">Смотреть все<img src="icons/back_arrow_subheader2.svg" class="btn_arrow"></button>
                </div>
            </div>
        `;

        reviewSectionHeaderAdded = true;
    }

    var reviewSectionContent = '';
    if (!showAllBtnClicked) {
        reviewSectionContent += `
            <div class="review_section">
                <div class="reviews_wrapper">
                    <div class="header_line">
                        <div class="review_line">
                            <div class="review_img_wrapper">
                                <img src="${review.user.photo}" class="review_img">
                            </div>
                            <div class="reviewers_name">${review.user.name}</div>
                        </div>
                        <div class="raiting">
                            <img src="icons/star_filled.svg" class="raiting_img">
                            <div class="raiting_text">${review.rate}</div>
                        </div>
                    </div>
                    <div class="review_text">
                        ${review.text}
                    </div>
                </div>
            </div>
        `;
    }

    reviewSection.innerHTML += reviewSectionContent;
}

function ShowAllReviews() {
    var reviewSection = document.getElementById("review_section");
    var reviewSectionHeader = reviewSection.querySelector(".reviews_section_header");

    reviewSection.innerHTML = ""; // Очистить секцию отзывов

    reviewSection.appendChild(reviewSectionHeader); // Добавить заголовок обратно

    // Расчет средней оценки
    var totalrate = reviews.reduce((sum, review) => sum + review.rate, 0);
    var averagerate = totalrate / reviews.length;

    // Генерация кода рейтинга
    var ratingSectionContent = `
        <div class="raiting_box">
            <h2 class="raiting_header">Рейтинг</h2>
            <div class="raiting_line"></div>
            <div class="raiting_row">
                <div class="raiting_number">${averagerate.toFixed(1)}</div>
    `;

    // Генерация звездочек в зависимости от значения рейтинга
    for (var i = 1; i <= 5; i++) {
        if (i <= averagerate) {
            ratingSectionContent += `<img src="icons/star_filled.svg" class="raiting_star">`;
        } else {
            ratingSectionContent += `<img src="icons/star.svg" class="raiting_star">`;
        }
    }

    ratingSectionContent += `
            </div>
            <button onclick="ShowReviewForm()" class="banner_btn">Написать отзыв</button>
        </div>
    `;

    reviewSection.innerHTML += ratingSectionContent;

    reviews.forEach(review => {
        var reviewSectionContent = `
            <div class="review_section">
                <div class="reviews_wrapper">
                    <div class="header_line">
                        <div class="review_line">
                            <div class="review_img_wrapper">
                                <img src="${review.user.photo}" class="review_img">
                            </div>
                            <div class="reviewers_name">${review.user.name}</div>
                        </div>
                        <div class="raiting">
                            <img src="icons/star_filled.svg" class="raiting_img">
                            <div class="raiting_text">${review.rate}</div>
                        </div>
                    </div>
                    <div class="review_text">
                        ${review.text}
                    </div>
                </div>
            </div>
        `;

        reviewSection.innerHTML += reviewSectionContent;
    });

    var showAllBtn = document.getElementById("show_all_btn");
    showAllBtn.disabled = true; // Отключить кнопку "Смотреть все"

    showAllBtnClicked = true;
}

function ShowReviewForm() {
    if (reviewFormAdded) {
        return; // Если форма отзыва уже добавлена, прекратить выполнение функции
    }

    var reviewSection = document.getElementById("review_section");

    var reviewForm = document.createElement("div");
    reviewForm.innerHTML = `

        <h2 class="review_rate">Оцените работу тренера</h2>
        <hr>
        <div class="rating_stars">
            <span class="star" data-rating="1"><img src="icons/star.svg" class="raiting_star"></span>
            <span class="star" data-rating="2"><img src="icons/star.svg" class="raiting_star"></span>
            <span class="star" data-rating="3"><img src="icons/star.svg" class="raiting_star"></span>
            <span class="star" data-rating="4"><img src="icons/star.svg" class="raiting_star"></span>
            <span class="star" data-rating="5"><img src="icons/star.svg" class="raiting_star"></span>
        </div>

        <div class="review_section">
            <div class="review_form">
                <textarea class="review_textarea" placeholder="Напишите ваш отзыв..."></textarea>
                <button onclick="PostReview()" class="review_submit_btn"><img src="icons/submit.svg" class="review_submit_btn_icon"></button>
            </div>
            <h6 class="warning">*При оставлении нового отзыва повторно, первый отзыв автоматически удаляется </h6>
        </div>
    `;

    reviewSection.appendChild(reviewForm);

    // Прокрутка страницы к форме отзыва
    reviewForm.scrollIntoView();

    // Установка флага, что форма отзыва уже добавлена
    reviewFormAdded = true;

    // Добавление обработчиков событий для звездочек
    var stars = document.getElementsByClassName("star");
    for (var i = 0; i < stars.length; i++) {
        stars[i].addEventListener("mouseover", fillStars);
        stars[i].addEventListener("mouseout", resetStars);
        stars[i].addEventListener("click", selectRating);
    }
}

function fillStars() {
    var rating = parseInt(this.getAttribute("data-rating"));

    var stars = this.parentNode.getElementsByClassName("raiting_star");
    for (var i = 0; i < stars.length; i++) {
        if (i < rating) {
            stars[i].setAttribute("src", "icons/star_filled.svg");
        } else {
            stars[i].setAttribute("src", "icons/star.svg");
        }
    }
}

function resetStars() {
    if (selectedRating !== null) {
        return; // Если рейтинг уже выбран, прекратить выполнение функции
    }

    var stars = this.parentNode.getElementsByClassName("raiting_star");
    for (var i = 0; i < stars.length; i++) {
        stars[i].setAttribute("src", "icons/star.svg");
    }
}

function selectRating() {
    var rating = parseInt(this.getAttribute("data-rating"));

    if (selectedRating !== null) {
        // Если рейтинг уже выбран, удалить выбранную оценку справа от звезд
        var selectedRatingText = document.querySelector(".selected_rating");
        selectedRatingText.parentNode.removeChild(selectedRatingText);
    }

    // Заполнить звезды до выбранного рейтинга
    var stars = this.parentNode.getElementsByClassName("raiting_star");
    for (var i = 0; i < stars.length; i++) {
        if (i < rating) {
            stars[i].setAttribute("src", "icons/star_filled.svg");
        } else {
            stars[i].setAttribute("src", "icons/star.svg");
        }
    }

    // Показать выбранный рейтинг справа от звезд
    var ratingText = document.createElement("div");
    ratingText.classList.add("selected_rating");
    ratingText.textContent = rating;
    this.parentNode.appendChild(ratingText);

    selectedRating = rating;
}

async function PostReview(review) {
    var textarea = document.querySelector(".review_textarea");
    var reviewText = textarea.value.trim();

    if (!reviewText) {
        // Если отзыв пустой, вывести сообщение об ошибке
        alert("Пожалуйста, напишите ваш отзыв");
        return;
    }

    if (selectedRating === null) {
        // Если рейтинг не выбран, вывести сообщение об ошибке
        alert("Пожалуйста, выберите оценку");
        return;
    }

    try {
        const reviewString = encodeURIComponent(JSON.stringify({
            TeacherId: teacher.id,
            //UserId: localStorage.Get("userId"),
            Text: reviewText,
            Rate: selectedRating
        }))
        const response = await fetch(`http://194.87.92.189:5000/api/reviews/${reviewString}`, { method: "POST" });



        if (response.ok === true) {
            const data = await response.json();
            console.log(data);

            // Добавить отзыв на страницу
            AddReviewToPage(review);

            // Сбросить форму отзыва
            textarea.value = "";
            selectedRating = null;
            var selectedRatingText = document.querySelector(".selected_rating");
            selectedRatingText.parentNode.removeChild(selectedRatingText);

        } else {
            const error = await response.json();
            console.log(error.message);
            alert("Ошибка при отправке отзыва");
        }
    } catch (error) {
        console.log(error);
        alert("Ошибка при отправке отзыва");
    }
}

async function GetReviews() {
    try {
        const response = await fetch(`http://194.87.92.189:5000/api/reviews/teacher/${teacher.id}`);

        if (response.ok === true) {
            let responseData = await response.json();
            reviews = Array.isArray(responseData) ? responseData : [responseData];
        } else {
            const error = await response.json();
            console.log(error.message);
        }
    } catch (error) {
        console.log(error);
    }
};

async function AwaitData() {
    await GetReviews()
    var lastTwoReviews = reviews.slice(-2);
    lastTwoReviews.forEach(review => AddReviewToPage(review));

    document.getElementById("trainer_page_title").innerText = teacher.user.name + ' ' + teacher.user.surname;
    document.getElementById("account_photo").setAttribute("src", teacher.user.photo);
    document.getElementById("account_name").innerText = teacher.user.name + ' ' + teacher.user.surname;
    document.getElementById("account_add_info").innerText = teacher.activities && teacher.activities.map(activity => activity.name).join(', ') + ', опыт работы ' + teacher.experience;
    document.getElementById("city").innerText = teacher.user.city;
    document.getElementById("vk").innerText = teacher.user.vkontakte;
    document.getElementById("telega").innerText = teacher.user.telegram;
    document.getElementById("phone").innerText = teacher.user.phone;
    document.getElementById("phone_small").innerText = teacher.user.phone;
    document.getElementById("email").innerText = teacher.user.email;
    document.getElementById("email_small").innerText = teacher.user.email;
    document.getElementById("additional_info").innerText = teacher.description;
}