$('#dropZone').filedrop({
    url: '@Url.Action("UploadFiles")',
    paramname: 'files',
    maxFiles: 5,
    dragOver: function () {
        $('#dropZone').css('background', 'blue');
    },
    dragLeave: function () {
        $('#dropZone').css('background', 'gray');
    },
    drop: function () {
        $('#dropZone').css('background', 'gray');
    },
    afterAll: function () {
        $('#dropZone').html('The file(s) have been uploaded successfully!');
    },
    uploadFinished: function (i, file, response, time) {
        $('#uploadResult').append('<li>' + file.name + '</li>');
    }
});