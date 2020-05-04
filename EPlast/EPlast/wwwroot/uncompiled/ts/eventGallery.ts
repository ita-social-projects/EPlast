function uploadFiles(inputId: string) {
        $("#addPicturesLabel").hide();
        $("#spinnerGal").show();
        let input: HTMLInputElement = <HTMLInputElement>document.getElementById(inputId);
        var files = input.files;
        var formData = new FormData();
        formData.append("ID", <string>$('#eventId').val());
        for (var i = 0; i != files.length; i++) {
            formData.append("files", files[i]);
        }
        $("#filesBadge").html(files.length.toString());
        $("#carouselBlock").addClass("progress-cursor");
        $("#files").addClass("progress-cursor");
        $.ajax(
            {
                url: "/Action/FillEventGallery",
                data: formData,
                processData: false,
                contentType: false,
                type: "POST",
                success: function () {
                    $("#addPicturesLabel").show();
                    $("#spinnerGal").hide();
                    $("#carouselBlock").removeClass("progress-cursor");
                    $("#uploadModal").modal('show');
                    $("#filesBadge").html('0');
                },
                error: function () {
                    $("#addPicturesLabel").show();
                    $("#spinnerGal").hide();
                    $("#carouselBlock").removeClass("progress-cursor");
                    $("#files").removeClass("progress-cursor");
                    $("#filesBadge").html('0');
                    $("#FAIL").modal('show');
                },
            });
    }
