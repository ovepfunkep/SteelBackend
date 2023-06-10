document.addEventListener('DOMContentLoaded', () => {
    const popupButton = document.getElementById('account_icon_add_info');
    const popup = document.querySelector('.popup_add_info_wrapper');

    popupButton.addEventListener('click', (event) => {
        event.stopPropagation();

        popup.classList.toggle('popup_show');
    });

    document.addEventListener('click', (event) => {
        const isClickInsidePopup = popup.contains(event.target);

        if (!isClickInsidePopup) {
            popup.classList.remove('popup_show');
        }
    });
});