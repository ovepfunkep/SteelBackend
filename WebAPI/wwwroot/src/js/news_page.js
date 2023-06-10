var data = localStorage.getItem("data");
var entity = JSON.parse(data);

document.getElementById("news_page_title").innerText = entity.name;
document.getElementById("news_page_header").innerText = entity.name;
document.getElementById("news_page_img").setAttribute("src", decodeURIComponent(entity.photo));
document.getElementById("news_main_text").innerText = entity.text;
