const subtotal = document.querySelector('.cart-subtotal');
const total = document.querySelector('.cart-total');
const cartItems = document.querySelectorAll('.cart-item');

cartItems.forEach(item => {
    const display = item.querySelector('.quantity-display');
    const priceItem = item.querySelector('.item-price');
    const totalItem = item.querySelector('.total-price');

    item.querySelector('.quantity-increase').addEventListener('click', () => {
        updateQuantity(1);
    })

    item.querySelector('.quantity-decrease').addEventListener('click', () => {
        updateQuantity(-1);
    })

    item.querySelector('.btn-remove').addEventListener('click', () => {
        item.remove();
    })

    const updatePrice = (quantity) => {
        const priceText = priceItem.textContent.substring(1);
        const price = Number(priceText);
        const totalPrice = price * quantity;

        totalItem.textContent = '$' + totalPrice.toFixed(2);
        subtotal.textContent = updatePriceValue(subtotal.textContent, price);
        total.textContent = updatePriceValue(total.textContent, price);
    }

    const updateQuantity = (quantity) => {
        const currentQuantity = display.value;
        const newQuantity = Number(currentQuantity) + quantity;

        if (newQuantity >= 1) {
            display.value = newQuantity;
            updatePrice(newQuantity);
        }
    }

    const updatePriceValue = (text, price) => {
        let curPrice = Number(text.substring(1));
        curPrice += price;

        return '$' + curPrice.toFixed(2);
    }
})