import * as request from "./api.js";

export async function sendFilterData(ev, data) {
    const path = ev.currentTarget.dataset.url;
    const html = await request.post(path, data);

    const container = ev.currentTarget.dataset.container;
    document.querySelector(container).innerHTML = html;
}

export async function decreaseItemQuantity(productId) {
    const path = '/Cart/DecreaseItemQuantity';
    const body = {
        productId: productId
    }

    await request.post(path, body);
}