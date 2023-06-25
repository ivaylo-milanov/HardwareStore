let form = document.querySelector('form');
let allInputs = document.querySelectorAll('input[value=All]');
let order = document.querySelector('select');
let categories = document.querySelectorAll('.filter-category');
let url = 'https://localhost:7213';
let defaultData;

setInitialData();

form.addEventListener('change', onFormChange);

async function onFormChange(ev) {
    ev.preventDefault();
    let data = getData();

    let url = buildQueryString(data);
    window.history.pushState(data, null, url);

    await sendData(data);
}

async function sendData(data) {
    let requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    };

    let subUrl = form.dataset.url;
    let container = form.dataset.container;

    try {
        let response = await fetch(url + subUrl, requestOptions);
        let html = await response.text();

        document.querySelector(container).innerHTML = html;

    } catch (e) {
        console.error(e);
    }
}

function buildQueryString(data) {
    var currentUrl = new URL(window.location.href);

    for (let [key, value] of Object.entries(data)) {
        currentUrl.searchParams.set(key, value);
    }

    return currentUrl.href;
}

function getData() {
    let formData = new FormData(form);
    let data = {};

    formData.forEach(function (value, key) {
        if (data[key]) {
            data[key].push(value);
        } else {
            data[key] = [value];
        }
    });

    return data;
}

window.onpopstate = onPopState;

function getDefaultData() {
    let data = {};

    allInputs.forEach(function (input) {
        data[input.name] = input.value;
    });

    data["Order"] = ["Default"];

    return data;
}

function onPopState(ev) {
    let data = ev.state

    if (!data) {
        data = defaultData;
    }

    returnFilterState(data);

    (async function () {
        await sendData(data);
    })();
}

function returnFilterState(data) {
    for (const [key, values] of Object.entries(data)) {
        if (key == "Order") {
            order.value = values[0];
        } else {
            var checkboxes = document.querySelectorAll(`input[name="${key}"]`);
            checkboxes.forEach(function (checkbox) {
                checkbox.checked = values.includes(checkbox.value);
            });
        }
    }
}

function setInitialData() {
    defaultData = getDefaultData();
    let url = buildQueryString(defaultData);
    history.replaceState(null, null, url);

    allInputs.forEach(input => {
        input.checked = true;
        input.className = 'all';
    });
}

document.addEventListener('DOMContentLoaded', onLoad);

function onLoad() {
    categories.forEach(function (category) {
        category.addEventListener('change', function (event) {
            let categoryInputs = event.target.closest('.filter-category');
            let allCheckbox = categoryInputs.querySelector('.all');
            let otherCheckboxes = categoryInputs.querySelectorAll('input:not(.all)');

            let checkedCount = 0;
            otherCheckboxes.forEach(function (checkbox) {
                if (checkbox.checked) {
                    checkedCount++;
                }
            });

            if (checkedCount > 0) {
                allCheckbox.checked = false;
            } else {
                allCheckbox.checked = true;
            }
        });
    });

    allInputs.forEach(function (allCheckbox) {
        allCheckbox.addEventListener('change', function (event) {
            let categoryInputs = event.target.closest('.filter-category');
            let otherCheckboxes = categoryInputs.querySelectorAll('input:not(.all)');

            if (event.target.checked) {
                otherCheckboxes.forEach(function (checkbox) {
                    checkbox.checked = false;
                });
            }
        });
    });
}