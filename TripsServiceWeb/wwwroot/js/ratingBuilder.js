function buildRating(rating, container) {
	const intRating = Math.floor(rating);
	const fracRating = rating - intRating;
	const stars = container.find(".star-filled");

	for (let i = 0; i < stars.length; i++) {
		if (i < intRating) {
			stars.eq(i).css("width", "100%");
		}
		else if (i === intRating) {
			stars.eq(i).css("width", `${fracRating * 100}%`);
		}
		else {
			stars.eq(i).css("width", "0%");
		}
	}
}

$('.rating-container').each(function () {
	const ratingValueContainer = $(this).find('.rating-value')
	const starContainer = $(this).find('.star-container');
	const decimalSeparator = (1.1).toLocaleString().substring(1, 2);
	const nonLocalRating = $(ratingValueContainer).text().replace(decimalSeparator, ".");
	const rating = parseFloat(nonLocalRating);
	buildRating(rating, starContainer);
	$(ratingValueContainer).css('background-color', `rgb(${255 - 51 * rating}, ${51 * rating}, 0)`);
});
