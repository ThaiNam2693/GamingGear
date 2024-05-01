const body = document.querySelector("body"),
modeToggle = body.querySelector(".mode-toggle");
sidebar = body.querySelector("nav");
sidebartoggle = body.querySelector(".sidebar-toggle");

sidebartoggle.addEventListener("click", () =>{
    sidebar.classList.toggle("close");
})