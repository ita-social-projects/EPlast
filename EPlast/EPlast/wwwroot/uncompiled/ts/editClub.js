$(document).ready(function () {
    let mistakeClubName = $('#clubName-clubAdmins-mistake');
    let mistakeClubName2 = $('#clubName-clubAdmins-mistake2');
    let mistakeClubDescription = $('#clubDescription-clubAdmins-mistake');
    $('#Upload-photo-clubAdmin').click(function (e) {
        let input = document.getElementById("upload-file-clubAdmins");
        var files = input.files;
        let allowedFileTypes = ["image/png", "image/jpeg", "image/jpg"];
        if (allowedFileTypes.indexOf(files[0].type) < 0) {
            alert("Загружений файл має бути зображенням");
            e.preventDefault();
            e.stopPropagation();
            return;
        }
        if (files[0] != undefined && files[0].size >= 3145728) {
            alert("Фото не може займати більше ніж 3 Мб");
            e.preventDefault();
            e.stopPropagation();
            return;
        }
    });
    $('#save-editedClub').click((e) => {
        mistakeClubName2.hide();
        mistakeClubName.hide();
        mistakeClubDescription.hide();
        let input = document.getElementById("clubName-clubAdmins");
        var nameSize = input.value.length;
        if (nameSize === 0 || input.value == null) {
            mistakeClubName.show();
            e.preventDefault();
            e.stopPropagation();
            return;
        }
        if (nameSize > 50 || input.value == null) {
            mistakeClubName2.show();
            e.preventDefault();
            e.stopPropagation();
            return;
        }
        let input2 = document.getElementById("clubDescription-clubAdmins");
        var descriptionSize = input2.value.length;
        if (descriptionSize > 1024 || input2.value == null) {
            mistakeClubDescription.show();
            e.preventDefault();
            e.stopPropagation();
            return;
        }
    });
});
//# sourceMappingURL=editClub.js.map