const likeButton = document.querySelector('.button-like');
likeButton.addEventListener('click', () => {
    likeButton.classList.toggle('liked');
});