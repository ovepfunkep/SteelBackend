window.addEventListener('DOMContentLoaded', () => {
    const menu = document.querySelector('.menu'),
        menuItem = document.querySelectorAll('.menu_item'),
        hamburger = document.querySelector('.hamburger_menu'),
        searchIcon = document.querySelector('.searchIcon'),
        searchBox = document.querySelector('.search_box'),
        searchInput = document.querySelector('.search_input');

    const closeMenu = () => {
        menu.classList.remove('menu_active');
    };

    const handleClickOutsideMenu = (event) => {
        const isClickInsideMenu = menu.contains(event.target);
        const isClickOnHamburger = hamburger.contains(event.target);
        if (!isClickInsideMenu && !isClickOnHamburger) {
            closeMenu();
        }
    };

    hamburger.addEventListener('click', () => {
        menu.classList.toggle('menu_active');
    });

    searchIcon.addEventListener('click', () => {
        searchBox.classList.toggle('search_box_active');
        searchInput.focus(); // Добавьте эту строку
    });

    menuItem.forEach(item => {
        item.addEventListener('click', () => {
            closeMenu();
        });
    });

    document.addEventListener('click', handleClickOutsideMenu);
});
