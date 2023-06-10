const deviceString = window.innerWidth < 768 ? '.hidden_on_laptop' : '';
const form = document.querySelector(`${deviceString} .registration_data`);

async function GetUser(event) {
    event.preventDefault();
    try {
        const phoneNumber = form.elements["phone"].value.replace(/\D/g, '');
        const password = form.elements["password"].value.trim();

        // Проверка на пустые значения
        if (phoneNumber.length === 0 || password.length === 0) {
            appendAlert('Пожалуйста, заполните все поля.');
            return;
        }

        const user = {
            Phone: phoneNumber,
            Password: password,
        }
        console.log(user);
        const userString = encodeURIComponent(JSON.stringify(user));
        const response = await fetch(`http://194.87.92.189:5000/api/users/${userString}`);

        if (response.ok === true) {
            const user = await response.json();
            localStorage.setItem("user", JSON.stringify(user));
            if (user.roleId === 3) {
                openPopup();
            }
            else {
                window.location.href = "index.html";
            }

        } else {
            const error = await response.json();
            appendAlert('Что-то пошло не так');

            console.log(error.message);
        }
    } catch (error) {
        console.log(error);
    }
}



const alertPlaceholder = document.querySelector(".alertContainer");

const appendAlert = (message) => {
    CloseAlert();
    const wrapper = document.createElement('div');
    wrapper.innerHTML = [
        `<div class="alert alert-danger" role="alert">`,
        `   <div>${message}</div>`,
        '   <button type="button" onClick="CloseAlert()" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>',
        '</div>'
    ].join('');

    alertPlaceholder.append(wrapper);
    alertPlaceholder.scrollIntoView(wrapper);
};

function CloseAlert() {
    alertPlaceholder.innerHTML = "";
}

const phoneInput = document.querySelector(`${deviceString} .phone`);
const phoneError = document.querySelector(`${deviceString} .phoneError`);

phoneInput.addEventListener('input', function () {
    formatPhoneNumber();
    validatePhoneNumber();
});

// Установка значения по умолчанию при загрузке страницы
phoneInput.value = '+7';

function formatPhoneNumber() {
    const phoneNumber = phoneInput.value.replace(/\D/g, ''); // Удалить все нецифровые символы
    const formattedPhoneNumber = phoneNumber.replace(/^7(\d{3})(\d{3})(\d{2})(\d{2})$/, '+7 $1 $2 $3 $4');
    phoneInput.value = formattedPhoneNumber;
}

function validatePhoneNumber() {
    const phoneNumber = phoneInput.value.replace(/\D/g, ''); // Удалить все нецифровые символы

    if (phoneNumber.length === 0) {
        phoneInput.value = '+7'; // Если поле пустое, восстановить "+7"
    } else if (phoneNumber.length === 11 && phoneNumber.startsWith('7')) {
        phoneInput.classList.remove('error');
        phoneError.textContent = '';
    } else {
        phoneInput.classList.add('error');
        phoneError.textContent = 'Пожалуйста, введите корректный номер телефона.';
        phoneError.classList.add('error-message');
    }
}


const passwordInput = document.querySelector(`${deviceString} .password`);
const passwordError = document.querySelector(`${deviceString} .passwordError`);

passwordInput.addEventListener('input', function () {
    validatePassword();
});

function validatePassword() {
    const password = passwordInput.value.trim();

    if (password !== '') {
        passwordInput.classList.remove('error');
        passwordError.textContent = '';
    } else {
        passwordInput.classList.add('error');
        passwordError.textContent = 'Пожалуйста, введите пароль.';
        passwordError.classList.add('error-message');
    }
}

function openPopup() {
    const popup = document.getElementById('confirmingPopup');
    popup.style.display = 'block';
    var overlay = document.createElement("div");
    overlay.className = "overlay";
    document.body.appendChild(overlay);
    overlay.addEventListener("click", closePopup);
    document.addEventListener("keydown", handleKeyPress);
}

function closePopup() {
    const popup = document.getElementById('confirmingPopup');
    popup.style.display = 'none';
    var overlay = document.querySelector(".overlay");
    document.body.removeChild(overlay);
    overlay.removeEventListener("click", closePopup);
    document.removeEventListener("keydown", handleKeyPress);
}
function handleKeyPress(event) {
    if (event.keyCode === 27) {
        closePopup();
    }
}
function redirectToAdminPanel() {
    document.location = 'admin_panel/admin_panel_index.html';
}

function redirectToUserPage() {
    document.location = 'index.html';
}