$(document).ready(function () {
    $('#CurrentPositionsTable tr').click(function () {
        var buttons = [$('#endPosition'), $('#deleteCurrentPosition')];
        setRowSelected($(this), buttons);
    })

    $('#PositionsTable tr').click(function () {
        var buttons = [$('#deletePosition')];
        setRowSelected($(this), buttons);
    })

    $('#endPosition').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        var tr = $('#CurrentPositionsTable tr.row-selected:first');
        var positionId = tr.find(':first').html();
        $.ajax({
            url: '/Account/EndPosition',
            type: 'GET',
            cache: false,
            data: { id: positionId },
            success: function (message) {
                tr.click();
                tr.remove();
                var currentDate = new Date();
                var day = currentDate.getDate();
                var month = currentDate.getMonth() + 1;
                var year = currentDate.getFullYear();
                var strDate = (day < 10 ? '0' : '') + day + '.'
                    + (month < 10 ? '0' : '') + month + '.'
                    + year;
                tr.find('td:last').append(' - ' + strDate);
                tr.bind("click", function () {
                    var buttons = [$('#deletePosition')];
                    setRowSelected(tr, buttons);
                });
                $('#PositionsTable tbody').append(tr);
                showModalMessage($('#ModalSuccess'), message);
            },
            error: function (response) {
                if (response.status === 404) {
                    showModalMessage($('#ModalError'), response.responseText);
                }
                else {
                    var strURL = '/Error/HandleError?code=' + response.status;
                    window.open(strURL, '_self');
                }
            }
        });
    })

    $('#deleteCurrentPosition').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        var tr = $('#CurrentPositionsTable tr.row-selected:first');
        var positionId = tr.find(':first').html();
        $.ajax({
            url: '/Account/DeletePosition',
            type: 'GET',
            cache: false,
            data: { id: positionId },
            success: function (message) {
                tr.click();
                tr.remove();
                showModalMessage($('#ModalSuccess'), message);
            },
            error: function (response) {
                if (response.status === 404) {
                    showModalMessage($('#ModalError'), response.responseText);
                }
                else {
                    var strURL = '/Error/HandleError?code=' + response.status;
                    window.open(strURL, '_self');
                }
            }
        });
    })

    $('#deletePosition').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        var tr = $('#PositionsTable tr.row-selected:first');
        var positionId = tr.find(':first').html();
        $.ajax({
            url: '/Account/DeletePosition',
            type: 'GET',
            cache: false,
            data: { id: positionId },
            success: function (message) {
                tr.click();
                tr.remove();
                showModalMessage($('#ModalSuccess'), message);
            },
            error: function (response) {
                if (response.status === 404) {
                    showModalMessage($('#ModalError'), response.responseText);
                }
                else {
                    var strURL = '/Error/HandleError?code=' + response.status;
                    window.open(strURL, '_self');
                }
            }
        });
    });

    $("#upload-file").change(function(e){
        let input: HTMLInputElement = <HTMLInputElement>document.getElementById("upload-file");

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
    $("#TheVYO").click(function () {
        window.open(`/Account/CreatePDFAsync?userId=${$("#userId").val()}`, "_blank");
    });
})

function setRowSelected(tr: JQuery<HTMLElement>, disabledElements: JQuery<HTMLElement>[]): void {
    setDisabled(disabledElements, true);
    var selected = $(tr).hasClass('row-selected');
    $(tr).parent().find('tr').removeClass('row-selected');
    if (!selected) {
        $(tr).addClass('row-selected');
        setDisabled(disabledElements, false);
    }
}