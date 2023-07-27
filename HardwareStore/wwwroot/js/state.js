import * as local from './utils.js';

const select = document.querySelector('.order select');

export function getURL(data) {
    return window.location.pathname + buildQueryString(data);
}

export function returnFilterState(data) {
    const distinctNames = [];
    const checkboxes = document.querySelectorAll('input[type=checkbox]');

    checkboxes.forEach((checkbox) => {
        if (!distinctNames.includes(checkbox.name)) {
            distinctNames.push(checkbox.name);
        }
    });

    select.value = data.Order;

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

    local.setData('filterData', data);
}

function buildQueryString(data) {
    const searchParams = new URLSearchParams();

    var arrayEntries = Object.entries(data).filter(p => Array.isArray(p.value));

    if (arrayEntries.length > 0) {
        arrayEntries.forEach(([key, value]) => searchParams.set(key, value.join(',')));
    }

    var [orderKey, orderValue] = Object.entries(data).filter(p => !Array.isArray(p.value))[0];

    searchParams.set(orderKey, orderValue);

    const queryString = searchParams.toString();
    return queryString ? '?' + queryString : '';
}