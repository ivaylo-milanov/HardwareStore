import { removeFromFavorite } from "./data.js";

document.querySelectorAll('.remove-favorite').forEach(removeButton => {
    removeButton.addEventListener('click', async (ev) => {
        const productId = ev.target.dataset.productId;

        await removeFromFavorite(productId);

        location.reload();
    })
})