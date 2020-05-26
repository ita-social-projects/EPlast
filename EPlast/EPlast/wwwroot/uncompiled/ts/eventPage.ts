$(document).ready(function () {

    $('#dtParticipants').DataTable({
        'language': {
            'url': "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Ukrainian.json"
        }
    });

    $('.dataTables_length').addClass('bs-select');
    $('img').on("error", function () {
        $(this).attr('src', "https://images.pexels.com/photos/459225/pexels-photo-459225.jpeg");
    });

    let participantId: string | number | string[];
    let eventId: string | number | string[];
    let activeElement: HTMLElement;
    const participantStatus = {
        Approved: 'Учасник',
        Undetermined: 'Розглядається',
        Rejected: 'Відмовлено'
    }

    $("#emptyGallery").click(function () {
        $("#emptyGalleryModal").modal('show');
    });

    $('#deleteIcon').click(function () {
        eventId = $('#eventId').val();
        activeElement = this;
        $("#myModal").modal('show');
    });

    $('#subscribeIcon').click(function () {
        eventId = $('#eventId').val();
        activeElement = this;
        $("#modalSubscribe").modal('show');
    });

    $('#unsubscribeIcon').click(function () {
        eventId = $('#eventId').val();
        activeElement = this;
        $("#modalUnSubscribe").modal('show');
    });


    $("#delete").click(function () {
        if ($(this).parents('div.container').hasClass('event-page-wrapper')) {
            $.ajax({
                type: "POST",
                url: "/Action/DeleteEvent",
                data: { id: eventId },
                success: function () {
                    $("#myModal").modal('hide');
                    window.location.replace("/Action/GetAction");
                },
                error: function () {
                    $("#myModal").modal('hide');
                    $("#deleteResultFail").modal('show');
                },
            });
        }
    });

    $("#unsubscribe").click(function () {
        if ($(this).parents('div.container').hasClass('event-page-wrapper')) {
            $.ajax({
                type: "POST",
                url: "/Action/UnSubscribeOnEvent",
                data: { id: eventId },
                success: function () {
                    $("#modalUnSubscribe").modal('hide');
                    $('#participantBlock').hide();
                    $('#userClockBlock').hide();
                    $('#unsubscribeBlock').hide();
                    $('#subscribeBlock').show();
                    $('#myTable').load(document.URL + ' #myTable');
                    $("#modalUnSubscribeSuccess").modal('show');
                },
                error: function (response: JQueryXHR) {
                    if (response.status !== 409) {
                        $("#modalUnSubscribe").modal('hide');
                        $("#FAIL").modal('show');
                    }
                    else {
                        $("#modalUnSubscribe").modal('hide');
                        $("#conflictModal").modal('show');
                    }
                },
            });
        }
    });

    $("#subscribe").click(function () {
        if ($(this).parents('div.container').hasClass('event-page-wrapper')) {
            $.ajax({
                type: "POST",
                url: "/Action/SubscribeOnEvent",
                data: { id: eventId },
                success: function () {
                    $("#modalSubscribe").modal('hide');
                    $('#participantBlock').hide();
                    $('#userClockBlock').show();
                    $('#unsubscribeBlock').show();
                    $('#subscribeBlock').hide();
                    $('#myTable').load(document.URL + ' #myTable');
                    $("#modalSubscribeSuccess").modal('show');
                },
                error: function (response: JQueryXHR) {
                    if (response.status !== 409) {
                        $("#modalSubscribe").modal('hide');
                        $("#FAIL").modal('show');
                    }
                    else {
                        $("#modalSubscribe").modal('hide');
                        $("#conflictModal").modal('show');
                    }
                },
            });
        }
    });

    $(".approved").click(function () {
        participantId = $(this).parents("tr").children("td.event-invisible").children("input[type=hidden]").val();
        activeElement = this;
        $.ajax({
            type: "GET",
            url: "/Action/ApproveParticipant",
            data: { id: participantId },
            cache: false,
            success: function () {
                $(activeElement).parents("tr").children("td:nth-child(3)").html(participantStatus.Approved);
            },
            error: function () {
                $("#resultOfStatusChanging").html(`Не вдалося змінити статус даного користувача на <b>'${participantStatus.Approved}'</b>.`);
                $("#statusModal").modal('show');
            },
        });
    });

    $(".undetermined").click(function () {
        participantId = $(this).parents("tr").children("td.event-invisible").children("input[type=hidden]").val();
        activeElement = this;
        $.ajax({
            type: "GET",
            url: "/Action/UndetermineParticipant",
            data: { id: participantId },
            cache: false,
            success: function () {
                $(activeElement).parents("tr").children("td:nth-child(3)").html(participantStatus.Undetermined);
            },
            error: function () {
                $("#resultOfStatusChanging").html(`Не вдалося змінити статус даного користувача на <b>'${participantStatus.Undetermined}'</b>.`);
                $("#statusModal").modal('show');
            },
        });
    });

    $(".rejected").click(function () {
        participantId = $(this).parents("tr").children("td.event-invisible").children("input[type=hidden]").val();
        activeElement = this;
        $.ajax({
            type: "GET",
            url: "/Action/RejectParticipant",
            data: { id: participantId },
            cache: false,
            success: function () {
                $(activeElement).parents("tr").children("td:nth-child(3)").html(participantStatus.Rejected);
            },
            error: function () {
                $("#resultOfStatusChanging").html(`Не вдалося змінити статус даного користувача на <b>'${participantStatus.Rejected}'</b>.`);
                $("#statusModal").modal('show');
            },
        });
    });

    $('[data-toggle="tooltip"]').tooltip();

    $("#uploadRes").click(function () {
        location.reload(true);
    });

    $("#editGallery").click(function () {
        $("#deletePicture").show();
        $("#fullCarousel").hide();
        $("#addPicture").hide();
        $(this).hide();
    });

    $("#backBut").click(function () {
        $('#carouselBlock').load(document.URL + ' #carouselBlock');
        $("#deletePicture").hide();
        $("#fullCarousel").show();
        $("#addPicture").show();
        $("#editGallery").show();
    });

    $("a.picture-remove").click(function(){
        let pictureToDelete = $(this).parents("div.picture-deleting").children('input[type="hidden"]').val();
        let elementToDelete = $(this).parents("div.picture-deleting").get(0);
        deletePicture(pictureToDelete,elementToDelete);
    });

    function deletePicture(pictureToDelete: string | number | string[], elementToDelete: HTMLElement) {
        $(elementToDelete).hide();
        $.ajax({
            type: "POST",
            url: "/Action/DeletePicture",
            data: { id: pictureToDelete },
            success: () => {
                $(elementToDelete).remove();
            },
            error: () => {
                $(elementToDelete).show();
                $("#FAIL").modal("show");
            },
        });
    }
});