import { decreaseItemQuantity, increaseItemQuantity, removeItemFromCart, updateItemQuantity } from "./data.js";

const subtotal = document.querySelector('.cart-subtotal');
const total = document.querySelector('.cart-total');
const cartItems = document.querySelectorAll('.cart-item');

cartItems.forEach(item => {
    const cartProductQuantity = item.querySelector('.cart-product-quantity');
    const display = cartProductQuantity.querySelector('.quantity-display');
    const priceItem = item.querySelector('.item-price');
    const totalItem = item.querySelector('.total-price');
    const productId = Number(cartProductQuantity.dataset.productId);
    const productQuantity = Number(cartProductQuantity.dataset.productQuantity);
    const productName = item.querySelector('.cart-product-name').textContent;

    const getQuantity = () => {
        let quantity = Number(display.value);

        if (quantity < 0) {
            quantity = previousQuantity;
            display.value = previousQuantity;
        }

        return quantity;
    }

    let previousQuantity = getQuantity();

    const getPrice = () => {
        const priceText = priceItem.textContent.substring(1);
        const price = Number(priceText);

        return price;
    }

    const price = getPrice();

    const increaseQuantityHandler = async () => {
        updateQuantity(1);

        const currentQuantity = getQuantity();
        if (currentQuantity > productQuantity) {
            display.value = previousQuantity;
            alert(`Only ${productQuantity} "${productName}" left in stock.`);
        }
        else {
            updateTotalAndSubtotal(price);
            previousQuantity = getQuantity();
            await increaseItemQuantity(productId);
        }
    }

    item.querySelector('.quantity-increase').addEventListener('click', increaseQuantityHandler);

    const decreaseQuantityHandler = async () => {
        const quantity = getQuantity();
        if (quantity == 1) {
            return;
        }

        updateQuantity(-1);
        updateTotalAndSubtotal(-price);
        previousQuantity = getQuantity();

        await decreaseItemQuantity(productId);
    }

    item.querySelector('.quantity-decrease').addEventListener('click', decreaseQuantityHandler);

    const removeItemHandler = async () => {
        item.remove();
        const removePrice = multiplyPriceAndQuantity(getQuantity());
        updateTotalAndSubtotal(-removePrice);

        await removeItemFromCart(productId);

        location.reload();
    }

    item.querySelector('.btn-remove').addEventListener('click', removeItemHandler);

    const inputHandler = async (ev) => {
        if (ev.keyCode === 13) {
            let quantity = getQuantity();

            if (quantity > productQuantity) {
                display.value = previousQuantity;
                alert(`Only 3 "${productName}" left in stock.`);
                return;
            }

            const difference = quantity - previousQuantity;

            if (difference == 0) {
                return;
            }

            updateTotalAndSubtotal(difference * price);
            previousQuantity = quantity;

            if (quantity == 0) {
                item.remove();
                await removeItemFromCart(productId);
                location.reload();
                return;
            } 

            await updateItemQuantity(productId);
        }
    }

    display.addEventListener('keypress', inputHandler);

    const updatePrice = (quantity) => {
        const totalPrice = multiplyPriceAndQuantity(quantity);

        totalItem.textContent = '$' + totalPrice.toFixed(2);
    }

    const updateQuantity = (quantity) => {
        const newQuantity = getQuantity() + quantity;
        display.value = newQuantity;

        if (newQuantity > productQuantity) {
            return;
        }

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

    const multiplyPriceAndQuantity = (quantity) => {
        const curPrice = getPrice();
        return curPrice * quantity;
    }
})