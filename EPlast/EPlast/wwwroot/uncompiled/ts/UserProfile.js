$(document).ready(function () {
    $("#upload-file").change(function (e) {
        let input = document.getElementById("upload-file");
        var files = input.files;
        if (files[0] != undefined && files[0].size >= 3145728) {
            alert("Фото не може займати більше ніж 3 Мб");
            $("#upload-file").val("");
            e.preventDefault();
            e.stopPropagation();
            return;
        }
        else {
            $("#image").attr("src", URL.createObjectURL(input.files[0])).show();
        }
    });
});
//# sourceMappingURL=userProfile.js.map