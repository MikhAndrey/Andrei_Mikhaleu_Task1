$('.delete-btn').on('click', function (e) {
	e.preventDefault();
	var url = $(this).attr('href');
	fetch(url, {
		method: 'DELETE'
	})
		.then(response => {
			if (response.ok) {
				location.reload();
			} else {
				alert("An error occured while deleting a trip");
			}
		})
		.catch(error => {
			console.error('Error:', error);
		});
});
