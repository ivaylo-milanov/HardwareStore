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

export async function decreaseItemQuantity(productId) {
    await request.post(endpoint.decreaseQuantity, JSON.stringify(productId));
}

export async function increaseItemQuantity(productId) {
    await request.post(endpoint.increaseQuantity, JSON.stringify(productId));
}

export async function updateItemQuantity(productId) {
    await request.post(endpoint.updateQuantity, JSON.stringify(productId));
}

export async function removeItemFromCart(productId) {
    await request.post(endpoint.removeItem, JSON.stringify(productId));
}