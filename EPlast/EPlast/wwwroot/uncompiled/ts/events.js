$(document).ready(function () {
    let status = 0;
    let value;
    let elementToDelete;
    let activeEvent;
    $('[data-toggle="tooltip"]').tooltip();
    $("#conflictBut").click(function () {
        location.reload(true);
    });
    $("div.single-card").mouseleave(function () {
        if ($(this).find("div.events-unsubscribe").is(":visible")) {
            $(this).find("div.events-unsubscribe").hide();
            $(this).find("div.events-part").show();
        }
    });
    $('a.delete-card').click(function () {
        value = $(this).parents("div.single-card").children('input[type="hidden"]').val();
        elementToDelete = this;
        $("#myModal").modal('show');
    });
    $('a.subscribe').click(function () {
        value = $(this).parents("div.single-card").children('input[type="hidden"]').val();
        activeEvent = this;
        $("#modalSubscribe").modal('show');
    });
    $('a.unsubscribe').click(function () {
        value = $(this).parents("div.single-card").children('input[type="hidden"]').val();
        activeEvent = this;
        $("#modalUnSubscribe").modal('show');
    });
    $("#delete").click(function () {
        if ($(this).parents('div.container').hasClass('events-page-wrapper')) {
            $.ajax({
                type: "POST",
                url: "/Action/DeleteEvent",
                data: { id: value },
                success: function () {
                    $("#myModal").modal('hide');
                    $("#deleteResultSuccess").modal('show');
                    $(elementToDelete).parents("div.single-card").remove();
                },
                error: function () {
                    $("#myModal").modal('hide');
                    $("#deleteResultFail").modal('show');
                },
            });
        }
    });
    $("#subscribe").click(function () {
        if ($(this).parents('div.container').hasClass('events-page-wrapper')) {
            $.ajax({
                type: "POST",
                url: "/Action/SubscribeOnEvent",
                data: { id: value },
                success: function () {
                    $("#modalSubscribe").modal('hide');
                    $(activeEvent).parents("div.events-operations").children("div.events-part").hide();
                    $(activeEvent).parents("div.events-operations").children("div.events-pen").hide();
                    $(activeEvent).parents("div.events-operations").children("div.events-participants").show();
                    $("#modalSubscribeSuccess").modal('show');
                },
                error: function (response) {
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
    $("#unsubscribe").click(function () {
        if ($(this).parents('div.container').hasClass('events-page-wrapper')) {
            $.ajax({
                type: "POST",
                url: "/Action/UnSubscribeOnEvent",
                data: { id: value },
                success: function () {
                    $("#modalUnSubscribe").modal('hide');
                    $(activeEvent).parents("div.events-operations").children("div.events-unsubscribe").hide();
                    $(activeEvent).parents("div.events-operations").children("div.events-part").hide();
                    $(activeEvent).parents("div.events-operations").children("div.events-participants").hide();
                    $(activeEvent).parents("div.events-operations").children("div.events-pen").show();
                    $("#modalUnSubscribeSuccess").modal('show');
                },
                error: function (response) {
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
    $("div.events-part").mouseenter(function () {
        status = 1;
        $(this).hide();
        $(this).parents("div").first().children("div.events-unsubscribe").show();
    });
    $("div.events-participants").mouseenter(function () {
        status = 0;
        $(this).hide();
        $(this).parents("div").first().children("div.events-unsubscribe").show();
    });
    $("div.events-unsubscribe").mouseleave(function () {
        $(this).hide();
        if (!$(this).parents("div").first().children("div.events-pen").is(":visible")) {
            if (status === 0) {
                $(this).parents("div").first().children("div.events-participants").show();
            }
            else {
                $(this).parents("div").first().children("div.events-part").show();
            }
        }
    });
});
//# sourceMappingURL=events.js.map