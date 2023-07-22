import { login } from "./user.js";

var form = document.querySelector('.login');

form.addEventListener('submit', onFormSubmit);

async function onFormSubmit(event) {
    event.preventDefault();

    var formData = new FormData(event.target);

    var data = Object.fromEntries(formData.entries());

    await login(data);
}