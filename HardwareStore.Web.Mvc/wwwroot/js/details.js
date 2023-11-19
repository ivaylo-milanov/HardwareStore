import { addToFavorite, removeFromFavorite } from "./data.js";

const likeContainer = document.querySelector('.like-container');
const likeButton = likeContainer.querySelector('.button-like');
const likeText = likeContainer.querySelector('.like-text');
const isFavorite = likeContainer.dataset.isFavorite.toLowerCase();
const productId = Number(likeContainer.dataset.productId);
const errorMessage = document.querySelector('.like-error-message');

if (isFavorite == "true") {
    likeButton.classList.add('liked');
    likeText.textContent = 'Added to favorite';
} else {
    likeText.textContent = 'Add to favorite';
}

likeButton.addEventListener('click', async () => {
    likeButton.classList.toggle('liked');

    try {
        if (likeButton.classList.contains('liked')) {
            await addToFavorite(productId);
            likeText.textContent = 'Added to favorite';
        } else {
            await removeFromFavorite(productId);
            likeText.textContent = 'Add to favorite';
        }
    } catch (e) {
        errorMessage.style.display = 'inline';
    }
});
