import * as local from './utils.js';
import { getURL, returnFilterState } from './state.js';
import { onFormChange } from './filterSubmit.js';

const form = document.querySelector('.filter form');
const initialUrl = local.getInitialURL();
const defaultData = { "Order": "Default" };

function setInitialData() {
    let initialData = null;

    if (performance.navigation.type == 0) {
        local.removeData('filterData');
    } else {
        const savedData = local.getData('filterData');
        initialData = savedData;
        returnFilterState(initialData || defaultData);
    }

    replaceState(initialData || defaultData);

    if (initialData === null) {
        local.setData('filterData', defaultData);
    }
}

function replaceState(data) {
    let queryString = getURL(data);
    history.replaceState(data, null, initialUrl + queryString);
}

async function onPopState(ev) {
    let data = ev.state;

    returnFilterState(data || defaultData);
    replaceState(data || defaultData);

    await sendFilterData(ev);
}

window.addEventListener('popstate', onPopState);
window.addEventListener('DOMContentLoaded', setInitialData);
form.addEventListener('change', onFormChange);