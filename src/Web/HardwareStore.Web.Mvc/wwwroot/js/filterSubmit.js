import { sendFilterData } from './data.js';
import { getURL } from './state.js';

export async function onFormChange(ev) {
    ev.preventDefault();
    let data = getData(ev);

    let url = getURL(data);
    window.history.pushState(data, null, url);

    await sendFilterData(ev.currentTarget, data);
}

function getData(ev) {
    let data = {};

    const checkboxes = ev.currentTarget.querySelectorAll('input[type="checkbox"]:checked');
    checkboxes.forEach(checkbox => {
        const key = checkbox.name.replace('Filter.', '');
        const value = checkbox.value;

        if (!data.hasOwnProperty(key)) {
            data[key] = [];
        }

        data[key].push(value);
    });

    const orderSelect = ev.currentTarget.querySelector('#catalog-order-by');
    data.Order = orderSelect ? orderSelect.value : '1';

    return data;
}