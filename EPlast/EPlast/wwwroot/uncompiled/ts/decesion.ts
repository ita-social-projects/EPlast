$(document).ready(function () {
    var createDecisionForm = ["#Decesion-Name", "#datepicker", "#Decesion-Description", "#autocomplete_input"];
    var editDecisionForm = ["#Edit-Decesion-Name", "#Edit-Decesion-Description"];
    createDecesionDataTable();

    $("#datepicker").datepicker({
        dateFormat: "dd-mm-yy"
    }).datepicker("setDate", "0");

    $(".show_hide").on('click', function () {
        $(this).parent("td").children(".hidden").removeClass("hidden");
        $(this).hide();
    });
    $(".hide_show").on('click', function () {
        $(this).parent("p").addClass("hidden");
        $(this).parent("p").parent("td").children(".show_hide").show();
    });
    function ClearCreateFormData() {
        createDecisionForm.forEach(function (element) {
            $(element).val("");
        });
    }

    function CheckCreateFormData() {
        var bool = true;

        createDecisionForm.forEach(function (element) {
            if ($(element).val().toString().length == 0) {
                console.log($(element).val().toString().replace(" ", "").length);
                $(element).parent("div").children(".field-validation-valid").text("Це поле має бути заповнене.");
                bool = false;
            } else
                $(element).parent("div").children(".field-validation-valid").text("");
        });
        if (!bool)
            return false;
        return true;
    }

    $("#CreateDecesionForm-submit").click((e) => {
        e.preventDefault();
        e.stopPropagation();
        if (!CheckCreateFormData())
            return;
        let input: HTMLInputElement = <HTMLInputElement>document.getElementById("CreateDecesionFormFile");
        var files = input.files;
        if (files[0] != undefined && files[0].size >= 10485760) {
            alert("файл за великий (більше 10 Мб)");
            return;
        }
        $("#CreateDecesionForm-submit").prop('disabled', true);
        var formData = new FormData();
        var decesionName = $("#Decesion-Name").val().toString();
        var decesionOrganizationId = $("#Decesion-Organization-ID option:selected").val().toString();
        var decesionTargetName = $("#autocomplete_input").val().toString();
        var decesionTargetId = $("#autocomplete_input_id_0").val().toString();
        var decesionDate = $("#datepicker").datepicker().val().toString();
        var decesionDescription = $("#Decesion-Description").val().toString()
        var decesionDecesionStatusType = $("#Decesion-DecesionStatusType option:selected").text();
        formData.append("file", files[0]);
        formData.append("Decesion.Name", decesionName);
        formData.append("Decesion.Organization.ID", decesionOrganizationId);
        formData.append("Decesion.DecesionTarget.TargetName", decesionTargetName);
        formData.append("Decesion.DecesionTarget.ID", decesionTargetId);
        formData.append("Decesion.Date", decesionDate);
        formData.append("Decesion.Description", decesionDescription);
        formData.append("Decesion.DecesionStatusType", decesionDecesionStatusType);

        $.ajax({
            url: "/Documentation/SaveDecesionAsync",
            type: "POST",
            processData: false,
            contentType: false,
            enctype: "multipart/form-data",
            async: true,
            data: formData,
            success(response) {
                if (response.success) {
                    $("#CreateDecesionModal").modal("hide");
                    $("#ModalSuccess .modal-body:first p:first strong:first").html(response.text);
                    $("#ModalSuccess").modal("show");
                    var file = "";
                    if (response.decesion.haveFile) {
                        file = `<a href="/Documentation/Download/${response.decesion.id}?filename=${files[0].name}">додаток.${files[0].name.split('.')[1]}</a>`
                    }
                    $("#dtReadDecesion").DataTable().row.add(
                        [
                            response.decesion.id,
                            response.decesion.name,
                            response.decesionOrganization,
                            decesionDecesionStatusType,
                            decesionTargetName,
                            decesionDescription,
                            decesionDate,
                            file])
                        .draw();
                } else {
                    $("#CreateDecesionModal").modal("hide");
                    $("#ModalError.modal-body:first p:first strong:first").html("Не можливо додати звіт!");
                }
                ClearCreateFormData();
                $("#CreateDecesionFormFile").val("");
                $("#CreateDecesionForm-submit").prop('disabled', false);
            },
            error() {
                $("#CreateDecesionForm-submit").prop('disabled', false);
                $("#CreateDecesionModal").modal("hide");
                $("#ModalError.modal-body:first p:first strong:first").html("Не можливо додати звіт!");
            }
        });
    });
    function ClearEditFormData() {
        editDecisionForm.forEach(function (element) {
            $(element).val("");
        });
    }

    function ChecEditFormData() {
        var bool = true;

        editDecisionForm.forEach(function (element) {
            if ($(element).val().toString().length == 0) {
                console.log($(element).val().toString().length);
                $(element).parent("div").children(".field-validation-valid").text("Це поле має бути заповнене.");
                bool = false;
            } else
                $(element).parent("div").children(".field-validation-valid").text("");
        });
        if (!bool)
            return false;
        return true;
    }
    $("#EditDecesionForm-submit").click((e) => {
        e.preventDefault();
        e.stopPropagation();
        if (!ChecEditFormData())
            return;
        $("#CreateDecesionForm-submit").prop('disabled', true);
        let formData = new FormData();
        let decesionID = $("#Edit-Decesion-ID").val().toString();
        let decesionName = $("#Edit-Decesion-Name").val().toString();
        let decesionDescription = $("#Edit-Decesion-Description").val().toString()
        formData.append("Decesion.ID", decesionID);
        formData.append("Decesion.Name", decesionName);
        formData.append("Decesion.Description", decesionDescription);

        $.ajax({
            url: "/Documentation/ChangeDecesion",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success(response) {
                $("#EditDecesionForm-submit").prop('disabled', false);
                if (response.success) {
                    ClearEditFormData();
                    $("#EditDecesionModal").modal("hide");
                    $("#ModalSuccess .modal-body:first p:first strong:first").html(response.text);
                    $("#ModalSuccess").modal("show");
                    let currectRow = $(`#dtReadDecesion tbody tr td:contains(${response.decesion.id})`).parent();
                    currectRow.children().eq(5).text(response.decesion.description);
                    currectRow.children().eq(1).text(response.decesion.name);
                } else {
                    $("#EditDecesionModal").modal("hide");
                    $("#ModalError.modal-body:first p:first strong:first").html("Не можливо редагувати звіт!");
                }
            },
            error() {
                $("#EditDecesionForm-submit").prop('disabled', false);
                $("#EditDecesionModal").modal("hide");
                $("#ModalError.modal-body:first p:first strong:first").html("Не можливо редагувати звіт!");
            }
        });
    });

    $.contextMenu({
        selector: '.decesion-menu',

        callback: function (key) {
            const content = $(this).children().first().text();
            switch (key) {
                case "edit":
                    $.get(`/Documentation/GetDecesion?id=${content}`, function (json) {
                        if (!json.success) {
                            $("#ModalError.modal-body:first p:first strong:first").html("ID рішення немає в базі!");
                            return;
                        }
                        $("#Edit-Decesion-ID").val(json.decesion.id)
                        $("#Edit-Decesion-Name").val(json.decesion.name)
                        $("#Edit-Decesion-Description").text(json.decesion.description)
                    });
                    $("#EditDecesionModal").modal("show");
                    break
                case "pdf":
                    window.open(`/Documentation/CreatePDFAsync?objId=${content}`, "_blank");
                    break
            }
        },
        items: {
            "edit": { name: "Редагувати", icon: "far fa-edit" },
            "pdf": { name: "Конвертувати до PDF", icon: "far fa-file-pdf" }
        }
    });
});

function createDecesionDataTable() {
    $("#dtReadDecesion").one("preInit.dt", function () {
        var button = $(`<button id="createDecesionButton" class="btn btn-sm btn-primary btn-management" data-toggle="modal" data-target="#CreateDecesionModal">Додати нове рішення</button>`);
        $("#dtReadDecesion_filter label").append(button);
    });

    $("#dtReadDecesion").DataTable({
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Ukrainian.json"
        },
        responsive: true,
        "autoWidth": false,
        "createdRow": function (row, data, dataIndex) {
            $(row).addClass("decesion-menu");
        },
        "columnDefs": [
            { "width": "10px", "targets": 0 },
            { "width": "1%", "targets": 1 },
            { "width": "2%", "targets": 2 },
            { "width": "7%", "targets": 3 },
            { "width": "10%", "targets": 4 },
            { "width": "8%", "targets": 6 },
            { "width": "4%", "targets": 7 }
        ],
        columns: [
            null,
            {
                "render": function (data, type, row) {
                    return data.replace(/(.{16})/g, "$1</br>")
                }
            },
            null,
            null,
            {
                "render": function (data, type, row) {
                    return data.replace(/(.{16})/g, "$1</br>")
                }
            },
            {
                "render": function (data, type, row) {
                    return data.replace(/(.{16})/g, "$1</br>")
                }
            },
            null,
            null
        ]
    });

    $('#dtReadDecesion').on('page.dt', function () {
        $('html, body').animate({
            scrollTop: 100
        }, 200);
    });
}