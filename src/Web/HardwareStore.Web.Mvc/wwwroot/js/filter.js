import * as local from './utils.js';
import { getURL, returnFilterState } from './state.js';
import { onFormChange } from './filterSubmit.js';
import { sendFilterData } from './data.js';

const form = document.querySelector('.page-layout');
const initialUrl = local.getInitialURL();
const defaultData = { "Order": "1" };

function setInitialData() {
    returnFilterState(defaultData);

    replaceState(defaultData);
}

function replaceState(data) {
    let queryString = getURL(data);
    history.replaceState(data, null, initialUrl + queryString);
}

async function onPopState(ev) {
    let data = ev.state;

    returnFilterState(data || defaultData);
    replaceState(data || defaultData);

    await sendFilterData(form, data || defaultData);
}

window.addEventListener('popstate', onPopState);
window.addEventListener('DOMContentLoaded', setInitialData);
form.addEventListener('change', onFormChange);