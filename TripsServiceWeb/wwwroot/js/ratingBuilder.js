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

function getRatingValueFromContainer(ratingContainer) {
	const decimalSeparator = (1.1).toLocaleString().substring(1, 2);
	const nonLocalRating = $(ratingContainer).text().replace(decimalSeparator, ".");
	return parseFloat(nonLocalRating);
}

function setRatingValueContainerColor(rating, ratingContainer) {
	$(ratingContainer).css('background-color', `rgb(${255 - 51 * rating}, ${51 * rating}, 0)`);
}

$('.rating-container').each(function () {
	const ratingValueContainer = $(this).find('.rating-value')
	const starContainer = $(this).find('.star-container');
	const rating = getRatingValueFromContainer(ratingValueContainer);
	buildRating(rating, starContainer);
	setRatingValueContainerColor(rating, ratingValueContainer);
});
