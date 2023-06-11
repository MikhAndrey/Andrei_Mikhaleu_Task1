let files = [];

$('#imageUpload').on('change', function () {
    const imagesList = $('#imagesList').addClass('row');
    const loadedFiles = Array.from(this.files);
    loadedFiles.forEach(function (file) {
        if (file.type.startsWith('image/')) {
            const reader = new FileReader();
            reader.onload = function (event) {
                const imgContainer = $('<div>').addClass('col-4 mb-3').append(
                    $('<img>').addClass('img-fluid d-block').attr({
                        'src': event.target.result,
                        'style': 'object-fit: cover; height: 250px',
                    }),
                    $('<button>').text('Delete').addClass('btn btn-danger mt-2').on('click', function () {
                        const index = files.indexOf(file);
                        if (index >= 0) {
                            files.splice(index, 1);
                            imgContainer.remove();
                        }
                    })
                );
                imagesList.append(imgContainer);
            };
            reader.readAsDataURL(file);
            files.push(file);
        }
    });
    $(this).val('');
});

$('#main-form').on('submit', function (event) {
    event.preventDefault();
    if ($(this).valid()) {
        const formData = new FormData(this);
        for (const file of files) {
            formData.append('images', file, file.name);
        }
        $.ajax({
            url: this.action,
            type: "POST",
            data: formData,
            success: function (response) {
                if (response.ok) {
                    window.location.href = response.url;
                } else {
                    alert("An error occured while sending data to server");
                }
            }
        });
    }
});