const form = document.querySelector('form');
const select = document.querySelector('select');
const initialUrl = 'https://localhost:7213';

form.addEventListener('change', onFormChange);

const defaultData = {
    "Order": "Default"
};

setInitialData();

async function onFormChange(ev) {
    ev.preventDefault();
    let data = getData();

    let url = window.location.pathname + buildQueryString(data);
    window.history.pushState(data, null, url);



    await request(data);
}

async function request(data) {
    let subUrl = form.dataset.url;
    let container = form.dataset.container;

    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }

    try {
        const response = await fetch(initialUrl + subUrl, options);

        const html = await response.text();

        document.querySelector(container).innerHTML = html;
    } catch (e) {
        console.error(e);
    }
}

function buildQueryString(data) {
    const searchParams = new URLSearchParams();

    for (let [key, value] of Object.entries(data)) {
        if (Array.isArray(value)) {
            searchParams.set(key, value.join(','))
        } else {
            searchParams.set(key, value);
        }
    }

    const queryString = searchParams.toString();
    return queryString ? '?' + queryString : '';
}

function getData() {
    let data = {};

    const checkboxes = form.querySelectorAll('input[type="checkbox"]:checked');
    checkboxes.forEach(checkbox => {
        const key = checkbox.name.replace('Filter.', '');
        const value = checkbox.value;

        if (!data.hasOwnProperty(key)) {
            data[key] = [];
        }

        data[key].push(value);
    });

    const order = select.value;
    data["Order"] = order;

    return data;
}

window.onpopstate = async (ev) => {
    let data = ev.state;

    if (data === null) {
        data = getSavedData();
    }

    returnFilterState(data);

    await request(data);
};

function returnFilterState(data) {
    const distinctNames = [];
    const checkboxes = document.querySelectorAll('input[type=checkbox]');

    checkboxes.forEach((checkbox) => {
        if (!distinctNames.includes(checkbox.name)) {
            distinctNames.push(checkbox.name);
        }
    });

    select.value = data["Order"];

    for (const key of distinctNames) {
        var nameCheckboxes = Array.from(checkboxes).filter(ch => ch.name === key);

        if (!data.hasOwnProperty(key)) {
            nameCheckboxes.forEach(function (checkbox) {
                checkbox.checked = false;
            });
        } else {
            nameCheckboxes.forEach(function (checkbox) {
                checkbox.checked = data[key].includes(checkbox.value);
            });
        }
    }
}

function setInitialData() {
    const savedData = localStorage.getItem('filterData');
    const initialData = savedData ? JSON.parse(savedData) : {};

    let queryString = window.location.pathname + buildQueryString(initialData);
    history.replaceState(null, null, initialUrl + queryString);

    if (Object.keys(savedData).length === 0) {
        localStorage.setItem('filterData', JSON.stringify(initialData));
    }
}

function getSavedData() {
    const savedData = localStorage.getItem('filterData');
    return savedData ? JSON.parse(savedData) : defaultData;
}
