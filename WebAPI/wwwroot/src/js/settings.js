var dataUser = localStorage.getItem("user");
var user = JSON.parse(dataUser);

const dateOfBirthValue = user.dateOfBirth || '0001-01-01T00:00:00';
const formattedDate = dateOfBirthValue.split('T')[0];

var accountName = document.querySelector(".account_name");
accountName.textContent = user.name + ' ' + user.surname;
var photoButton = document.querySelector(".photo_button");
photoButton.style.border = "2px solid #787878";
photoButton.style.backgroundImage = `url(${user.photo})`;

document.getElementById("name").value = user.name;
document.getElementById("surname").value = user.surname;
document.getElementById("date_of_birth").value = formattedDate;
document.getElementById("gender").value = user.gender;
document.getElementById("email").value = user.email;
document.getElementById("password").value = user.password;
document.getElementById("phone").value = user.phone;
document.getElementById("town").value = user.city;
document.getElementById("vk").value = user.vkontakte;
document.getElementById("telegram").value = user.telegram;

async function UpdateUserClick() {
    const dateOfBirthValue = document.getElementById("date_of_birth").value;

    const dateParts = dateOfBirthValue.split("T")[0].split("-");
    const year = dateParts[0];
    const month = dateParts[1];
    const day = dateParts[2];
    const formattedDate = `${year}-${month}-${day}T01:01:01`;
    const user1 =
    {
        surname: document.getElementById("surname").value,
        name: document.getElementById("name").value,
        email: document.getElementById("email").value,
        phone: document.getElementById("phone").value,
        password: document.getElementById("password").value,
        gender: document.getElementById("gender").value,
        dateOfBirth: formattedDate,
        vkontakte: document.getElementById("vk").value,
        telegram: document.getElementById("telegram").value,
        city: document.getElementById("town").value
    }
    await UpdateUser(user1);


}



document.getElementById("update").addEventListener("click", () => {
    UpdateUser(
        {
            id: user.id,
            surname: document.getElementById("surname").value,
            name: document.getElementById("name").value,
            email: document.getElementById("email").value,
            phone: document.getElementById("phone").value,
            password: document.getElementById("password").value,
            gender: document.getElementById("gender").value,
            dateOfBirth: '0001-01-01',
            vkontakte: document.getElementById("vk").value,
            telegram: document.getElementById("telegram").value,
            city: document.getElementById("town").value
        });
});

async function UpdateUser(user1) {
    try {
        const userString = encodeURIComponent(JSON.stringify(user1));
        const response = await fetch(`http://194.87.92.189:5000/api/users/${userString}`, { method: "PUT" });

        if (response.ok === true) {
            localStorage.setItem("user", JSON.stringify(await response.json()))
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