document.querySelectorAll('.product-card').forEach(function (card) {
    card.addEventListener('click', function (e) {
        e.preventDefault();
        var detailsLink = e.target.querySelector('#details-link').href;
        window.location.href = detailsLink;
    });
});