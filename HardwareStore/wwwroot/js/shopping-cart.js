import { decreaseItemQuantity, increaseItemQuantity, removeItemFromCart } from "./data.js";

const subtotal = document.querySelector('.cart-subtotal');
const total = document.querySelector('.cart-total');
const cartItems = document.querySelectorAll('.cart-item');

cartItems.forEach(item => {
    const display = item.querySelector('.quantity-display');
    const priceItem = item.querySelector('.item-price');
    const totalItem = item.querySelector('.total-price');

    const getPrice = () => {
        const priceText = priceItem.textContent.substring(1);
        const price = Number(priceText);

        return price;
    }

    const price = getPrice();

    const increaseQuantityHandler = async (ev) => {
        updateQuantity(1);
        updateTotalAndSubtotal(price);

        const productId = Number(ev.target.parentNode.dataset.productId);
        await increaseItemQuantity(productId);
    }

    item.querySelector('.quantity-increase').addEventListener('click', increaseQuantityHandler);

    const decreaseQuantityHandler = async (ev) => {
        const quantity = getQuantity();
        if (quantity == 1) {
            return;
        }

        updateQuantity(-1);
        updateTotalAndSubtotal(-price);

        const productId = Number(ev.target.parentNode.dataset.productId);
        await decreaseItemQuantity(productId);
    }

    item.querySelector('.quantity-decrease').addEventListener('click', decreaseQuantityHandler);

    const removeItemHandler = async (ev) => {
        item.remove();
        const removePrice = multiplyPriceAndQuantity(getQuantity());
        updateTotalAndSubtotal(-removePrice);

        const productId = Number(ev.target.parentNode.dataset.productId);
        await removeItemFromCart(productId);
    }

    item.querySelector('.btn-remove').addEventListener('click', removeItemHandler);

    const updatePrice = (quantity) => {
        const totalPrice = multiplyPriceAndQuantity(quantity);

        totalItem.textContent = '$' + totalPrice.toFixed(2);
    }

    const updateQuantity = (quantity) => {
        const newQuantity = getQuantity() + quantity;

        display.value = newQuantity;
        updatePrice(newQuantity);
    }

    const updatePriceValue = (text, itemPrice) => {
        let curPrice = Number(text.substring(1));
        curPrice += itemPrice;

        return '$' + curPrice.toFixed(2);
    }

    const updateTotalAndSubtotal = (amount) => {
        subtotal.textContent = updatePriceValue(subtotal.textContent, amount);
        total.textContent = updatePriceValue(total.textContent, amount);
    }

    const getQuantity = () => {
        return Number(display.value);
    }

    const multiplyPriceAndQuantity = (quantity) => {
        const curPrice = getPrice();
        return curPrice * quantity;
    }
})