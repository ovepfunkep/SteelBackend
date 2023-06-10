window.addEventListener('DOMContentLoaded', () => {
    const footerContainer = document.getElementById('footerContainer');
    const xhr = new XMLHttpRequest();

    xhr.open('GET', 'blocks/footer.html', true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            footerContainer.innerHTML = xhr.responseText;
        }
    };
    xhr.send();
});