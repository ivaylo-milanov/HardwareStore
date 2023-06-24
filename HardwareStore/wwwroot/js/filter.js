document.querySelector('form').addEventListener('change', onFormChange);

async function onFormChange(ev) {
    ev.preventDefault();
    let data = getData();

    let url = buildQueryString(data);
    window.history.pushState(data, null, url);
    await sendData(data, document.querySelector('form'));
}

async function sendData(data, form) {
    let url = 'https://localhost:7213';

    let requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    };

    url += form.dataset.url;
    let container = form.dataset.container;

    try {
        let response = await fetch(url, requestOptions);
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
    let formData = new FormData(document.querySelector('form'));
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

function onPopState(ev) {
    let data = ev.state;

    if (!data) {
        data = {
            Manufacturer: ['All'],
            Price: ['All'],
            Sensitivity: ['All'],
            Sensor: ['All'],
            Connectivity: ['All'],
            Interface: ['All'],
            Color: ['All'],
            NumberOfKeys: ['All'],
            Order: ['Default']
        };
    }

    returnFilterState(data);

    (async function () {
        await sendData(data, document.querySelector('form'));
    })();
}

function returnFilterState(data) {
    for (const [key, values] of Object.entries(data)) {
        var checkboxes = document.querySelectorAll(`input[name="${key}"]`);
        checkboxes.forEach(function (checkbox) {
            checkbox.checked = values.includes(checkbox.value);
        });
    }
}

$(document).ready(function () {
    document.title = 'Mouses - HardwareStore';
    if (!history.state) {
        let data = getData();
        let url = buildQueryString(data);
        history.replaceState(null, null, url);
    }

    $('.filter-category').on('change', ':input', function () {
        var categoryInputs = $(this).closest('.filter-category');
        var allCheckbox = categoryInputs.find('.all');
        var otherCheckboxes = categoryInputs.find(':input').not(allCheckbox);

        if (otherCheckboxes.is(':checked')) {
            allCheckbox.prop('checked', false);
        } else {
            allCheckbox.prop('checked', true);
        }
    });

    $('.all').on('change', function () {
        var categoryInputs = $(this).closest('.filter-category');
        var otherCheckboxes = categoryInputs.find(':input').not('.all');

        if ($(this).is(':checked')) {
            otherCheckboxes.prop('checked', false);
        }
    });
});