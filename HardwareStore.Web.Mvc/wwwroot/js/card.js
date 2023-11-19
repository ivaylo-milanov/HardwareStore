document.addEventListener('DOMContentLoaded', function () {
    var cards = document.querySelectorAll('.product-card');

    cards.forEach(function (card) {
        card.addEventListener('click', function (e) {
            if (!e.target.classList.contains('product-button')) {
                var hiddenLink = this.querySelector('.hiddenLink');
                if (hiddenLink) {
                    hiddenLink.click();
                }
            }
        });
    });
});