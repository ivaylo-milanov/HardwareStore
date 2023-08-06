import * as request from "./api.js";

const endpoint = {
    decreaseQuantity: '/Cart/DecreaseItemQuantity',
    removeItem: '/Cart/RemoveFromShoppingCart',
    increaseQuantity: '/Cart/IncreaseItemQuantity',
    updateQuantity: '/Cart/UpdateItemQuantity'
}

export async function sendFilterData(form, data) {
    const path = form.dataset.url;
    const html = await request.post(path, JSON.stringify(data));

    document.querySelector('#products').innerHTML = html;
}