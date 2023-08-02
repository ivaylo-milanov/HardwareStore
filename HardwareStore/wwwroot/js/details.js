const likeContainer = document.querySelector('.like-container');
const likeButton = likeContainer.querySelector('.button-like');
const likeText = likeContainer.querySelector('.like-text');
const isFavorite = likeContainer.dataset.isFavorite.toLowerCase();

if (isFavorite == "true") {
    likeButton.classList.add('liked');
    likeText.textContent = 'Added to favorite';
} else {
    likeText.textContent = 'Add to favorite';
}

likeButton.addEventListener('click', () => {
    likeButton.classList.toggle('liked');
});
