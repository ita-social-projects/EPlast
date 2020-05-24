function createDecisionDataTable() {
    $("#dtReadDecision").one("preInit.dt", () => {
        const button = $(`<button id="createDecisionButton" class="btn btn-sm btn-primary btn-management" data-toggle="modal" data-target="#CreateDecisionModal">Додати нове рішення</button>`);
        $("#dtReadDecision_filter label").append(button);
    });

    $("#dtReadDecision").DataTable({
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Ukrainian.json"
        },
        responsive: true,
        "autoWidth": false,
        "createdRow"(row) {
            $(row).addClass("decision-menu");
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
                "render"(data) {
                    return data.replace(/(.{16})/g, "$1</br>");
                }
            },
            null,
            null,
            {
                "render"(data) {
                    return data.replace(/(.{16})/g, "$1</br>");
                }
            },
            {
                "render"(data) {
                    return data.replace(/(.{16})/g, "$1</br>");
                }
            },
            null,
            null
        ]
    });

    $("#dtReadDecision").on("page.dt", () => {
        $("html, body").animate({
            scrollTop: 100
        }, 200);
    });
}
$(() => {
    const createDecisionForm = ["#Decision-Name", "#datepicker", "#Decision-Description", "#autocomplete_input"];
    const editDecisionForm = ["#Edit-Decision-Name", "#Edit-Decision-Description"];
    createDecisionDataTable();

    $("#datepicker").datepicker({
        dateFormat: "dd-mm-yy"
    }).datepicker("setDate", "0");

    $(".show_hide").on("click", function () {
        $(this).parent("td").children(".hidden").removeClass("hidden");
        $(this).hide();
    });
    $(".hide_show").on("click", function () {
        $(this).parent("p").addClass("hidden");
        $(this).parent("p").parent("td").children(".show_hide").show();
    });
    function ClearCreateFormData() {
        createDecisionForm.forEach(element => {
            $(element).val("");
        });
    }

    function CheckCreateFormData() {
        let bool = true;

        createDecisionForm.forEach(element => {
            if ($(element).val().toString().length === 0) {
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

    $("#CreateDecisionForm-submit").click((e) => {
        e.preventDefault();
        e.stopPropagation();
        if (!CheckCreateFormData())
            return;
        const input = document.getElementById("CreateDecisionFormFile") as HTMLInputElement;
        const files = input.files;
        if (files[0] !== undefined && files[0].size >= 10485760) {
            alert("файл за великий (більше 10 Мб)");
            return;
        }
        $("#CreateDecisionForm-submit").prop("disabled", true);
        const formData = new FormData();
        const decisionName = $("#Decision-Name").val().toString();
        const decisionOrganizationId = $("#Decision-Organization-ID option:selected").val().toString();
        const decisionTargetName = $("#autocomplete_input").val().toString();
        const decisionTargetId = $("#autocomplete_input_id_0").val().toString();
        const decisionDate = $("#datepicker").datepicker().val().toString();
        const decisionDescription = $("#Decision-Description").val().toString();
        const decisionDecisionStatusType = $("#Decision-DecisionStatusType option:selected").text();
        formData.append("file", files[0]);
        formData.append("Decision.Name", decisionName);
        formData.append("Decision.Organization.ID", decisionOrganizationId);
        formData.append("Decision.DecisionTarget.TargetName", decisionTargetName);
        formData.append("Decision.DecisionTarget.ID", decisionTargetId);
        formData.append("Decision.Date", decisionDate);
        formData.append("Decision.Description", decisionDescription);
        formData.append("Decision.DecisionStatusType", decisionDecisionStatusType);

        $.ajax({
            url: "/Documentation/SaveDecision",
            type: "POST",
            processData: false,
            contentType: false,
            enctype: "multipart/form-data",
            async: true,
            data: formData,
            success(response) {
                if (response.success) {
                    $("#CreateDecisionModal").modal("hide");
                    $("#ModalSuccess .modal-body:first p:first strong:first").html(response.text);
                    $("#ModalSuccess").modal("show");
                    let file = "";
                    if (response.decision.haveFile) {
                        file = `<a href="/Documentation/Download/${response.decision.id}?filename=${files[0].name}">додаток.${files[0].name.split('.')[1]}</a>`;
                    }
                    $("#dtReadDecision").DataTable().row.add(
                        [
                            response.decision.id,
                            response.decision.name,
                            response.decisionOrganization,
                            decisionDecisionStatusType,
                            decisionTargetName,
                            decisionDescription,
                            decisionDate,
                            file])
                        .draw();
                } else {
                    $("#CreateDecisionModal").modal("hide");
                    $("#ModalError.modal-body:first p:first strong:first").html("Не можливо додати звіт!");
                }
                ClearCreateFormData();
                $("#CreateDecisionFormFile").val("");
                $("#CreateDecisionForm-submit").prop("disabled", false);
            },
            error() {
                $("#CreateDecisionForm-submit").prop("disabled", false);
                $("#CreateDecisionModal").modal("hide");
                $("#ModalError.modal-body:first p:first strong:first").html("Не можливо додати звіт!");
            }
        });
    });
    function ClearEditFormData() {
        editDecisionForm.forEach(element => {
            $(element).val("");
        });
    }

    function checkEditFormData() {
        let bool = true;

        editDecisionForm.forEach(element => {
            if ($(element).val().toString().length === 0) {
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
    $("#EditDecisionForm-submit").click((e) => {
        e.preventDefault();
        e.stopPropagation();
        if (!checkEditFormData())
            return;
        $("#CreateDecisionForm-submit").prop("disabled", true);
        const formData = new FormData();
        const decisionId = $("#Edit-Decision-ID").val().toString();
        const decisionName = $("#Edit-Decision-Name").val().toString();
        const decisionDescription = $("#Edit-Decision-Description").val().toString();
        formData.append("Decision.ID", decisionId);
        formData.append("Decision.Name", decisionName);
        formData.append("Decision.Description", decisionDescription);

        $.ajax({
            url: "/Documentation/ChangeDecision",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success(response) {
                $("#EditDecisionForm-submit").prop('disabled', false);
                if (response.success) {
                    ClearEditFormData();
                    $("#EditDecisionModal").modal("hide");
                    $("#ModalSuccess .modal-body:first p:first strong:first").html(response.text);
                    $("#ModalSuccess").modal("show");
                    const currectRow = $(`#dtReadDecision tbody tr td:contains(${response.decision.id})`).parent();
                    currectRow.children().eq(5).text(response.decision.description);
                    currectRow.children().eq(1).text(response.decision.name);
                } else {
                    $("#EditDecisionModal").modal("hide");
                    $("#ModalError.modal-body:first p:first strong:first").html("Не можливо редагувати звіт!");
                }
            },
            error() {
                $("#EditDecisionForm-submit").prop('disabled', false);
                $("#EditDecisionModal").modal("hide");
                $("#ModalError.modal-body:first p:first strong:first").html("Не можливо редагувати звіт!");
            }
        });
    });
    $("#DeleteDecisionForm-submit").click((e) => {

        $("#DeleteDecisionForm-submit").prop('disabled', true);
        let decisionID = $("#Delete-Decision-ID").val();
        $.ajax(
            {
                url: "/Documentation/DeleteDecision",
                type: "POST",
                data: { 'id': decisionID },
                success(response) {
                    $("#DeleteDecisionForm-submit").prop('disabled', false);
                    if (response.success) {
                        $("#DeleteDecisionModal").modal("hide");
                        $("#ModalSuccess .modal-body:first p:first strong:first").html(response.text);
                        $("#ModalSuccess").modal("show");
                        let table = $("#dtReadDecision").DataTable();
                        table.rows($(`tbody tr td:contains(${decisionID})`).parent()).remove().draw();
                    }
                    else {
                        $("#EditDecisionModal").modal("hide");
                        $("#ModalError.modal-body:first p:first strong:first").html("Не вдалося видалити звіт!");
                    }
                },
                error() {
                    $("#DeleteDecisionForm-submit").prop('disabled', false);
                    $("#DeleteDecisionModal").modal("hide");
                    $("#ModalError.modal-body:first p:first strong:first").html("Не можливо редагувати звіт!");
                }
            }
        )

    })
    $.contextMenu({
        selector: ".decision-menu",

        callback: function (key) {
            const content = $(this).children().first().text();
            switch (key) {
                case "edit":
                    $.get(`/Documentation/GetDecision?id=${content}`, function (json) {
                        if (!json.success) {
                            $("#ModalError.modal-body:first p:first strong:first").html("ID рішення немає в базі!");
                            return;
                        }
                        $("#Edit-Decision-ID").val(json.decision.id);
                        $("#Edit-Decision-Name").val(json.decision.name);
                        $("#Edit-Decision-Description").text(json.decision.description);
                    });
                    $("#EditDecisionModal").modal("show");
                    break;
                case "pdf":
                    window.open(`/Documentation/CreatePDFAsync?objId=${content}`, "_blank");
                    break;
                case "delete":
                    $.get(`/Documentation/GetDecision?id=${content}`, function (json) {
                        if (!json.success) {
                            $("#ModalError.modal-body:first p:first strong:first").html("ID рішення немає в базі!");
                            return;
                        }
                        $("#Delete-Decision-ID").val(json.decision.id);
                    });
                    $("#DeleteDecisionModal").modal("show");
                    break;
            }
        },
        items: {
            "edit": { name: "Редагувати", icon: "far fa-edit" },
            "pdf": { name: "Конвертувати до PDF", icon: "far fa-file-pdf" },
            "delete": { name: "Видалити", icon: "far fa-trash-alt" }
        }
    });
});