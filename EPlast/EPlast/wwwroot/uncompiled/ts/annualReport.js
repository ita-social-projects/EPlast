$('#view-annual-reports-form').ready(function () {
    var indexAnnualReportId = 0;
    var indexCityId = 1;
    var indexCityName = 2;
    var indexDate = 5;
    var indexAnnualReportStatus = 6;
    $('#AnnualReportsTable').DataTable({
        'language': {
            'url': "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Ukrainian.json"
        }
    });
    const AnnualReportStatus = {
        Unconfirmed: 'Непідтверджений',
        Confirmed: 'Підтверджений',
        Saved: 'Збережений'
    };
    const AnnualReportAction = {
        Confirm: 'підтвердити',
        Delete: 'видалити',
        Cancel: 'скасувати'
    };
    $.contextMenu({
        selector: '.unconfirmed-ar-menu',
        callback: function (key) {
            var annualReportId = $(this).find('td').eq(indexAnnualReportId).html();
            switch (key) {
                case 'review':
                    viewAnnualReport(annualReportId);
                    break;
                case 'edit':
                    var strURL = '/Documentation/EditAnnualReportAsync?id=' + annualReportId;
                    window.open(strURL, '_self');
                    break;
                case 'confirm':
                    $('#Yes').bind('click', { row: this, annualReportId: annualReportId }, function (event) {
                        confirmAnnualReport(event.data.row, event.data.annualReportId);
                    });
                    showYesNoModal(this, AnnualReportAction.Confirm.toString());
                    break;
                case 'delete':
                    $('#Yes').bind('click', { row: this, annualReportId: annualReportId }, function (event) {
                        deleteAnnualReport(event.data.row, event.data.annualReportId);
                    });
                    showYesNoModal(this, AnnualReportAction.Delete.toString());
                    break;
            }
        },
        items: {
            'review': { name: 'Переглянути', icon: 'fas fa-search' },
            'edit': { name: 'Редагувати', icon: 'fas fa-edit' },
            'confirm': { name: 'Підтвердити', icon: 'far fa-check-circle' },
            'delete': { name: 'Видалити', icon: 'fas fa-trash-alt' },
        },
    });
    $.contextMenu({
        selector: '.confirmed-ar-menu',
        callback: function (key) {
            var annualReportId = $(this).find('td').eq(indexAnnualReportId).html();
            switch (key) {
                case 'review':
                    viewAnnualReport(annualReportId);
                    break;
                case 'cancel':
                    $('#Yes').bind('click', { row: this, annualReportId: annualReportId }, function (event) {
                        cancelAnnualReport(event.data.row, event.data.annualReportId);
                    });
                    showYesNoModal(this, AnnualReportAction.Cancel.toString());
                    break;
            }
        },
        items: {
            'review': { name: 'Переглянути', icon: 'fas fa-search' },
            'cancel': { name: 'Скасувати', icon: 'far fa-times-circle' }
        },
    });
    $.contextMenu({
        selector: '.saved-ar-menu',
        callback: function (key) {
            var annualReportId = $(this).find('td').eq(indexAnnualReportId).html();
            switch (key) {
                case 'review':
                    viewAnnualReport(annualReportId);
                    break;
            }
        },
        items: {
            'review': { name: 'Переглянути', icon: 'fas fa-search' }
        },
    });
    function viewAnnualReport(annualReportId) {
        $.ajax({
            url: '/Documentation/GetAnnualReportAsync',
            type: 'GET',
            cache: false,
            data: { id: annualReportId },
            success: function (result) {
                $('#ModalContent').html(result);
                $('#AnnualReportModal').modal('show');
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
    }
    function confirmAnnualReport(row, annualReportId) {
        $('#Yes').modal('hide');
        $('#Yes').unbind();
        $.ajax({
            url: '/Documentation/ConfirmAnnualReportAsync',
            type: 'GET',
            cache: false,
            data: { id: annualReportId },
            success: function (message) {
                var rows = $('#AnnualReportsTable tbody:first tr');
                var cityId = row.find('td').eq(indexCityId).html();
                var selectedRows = $(rows).filter(function () {
                    return $(this).find('td').eq(indexCityId).html() == cityId
                        && $(this).find('td').eq(indexAnnualReportStatus).html() == AnnualReportStatus.Confirmed;
                });
                selectedRows.find('td').eq(indexAnnualReportStatus).html(AnnualReportStatus.Saved);
                selectedRows.removeClass();
                selectedRows.addClass('saved-ar-menu');
                $(row).find('td').eq(indexAnnualReportStatus).html(AnnualReportStatus.Confirmed);
                $(row).removeClass();
                $(row).addClass('confirmed-ar-menu');
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
    }
    function cancelAnnualReport(row, annualReportId) {
        $('#Yes').modal('hide');
        $('#Yes').unbind();
        $.ajax({
            url: '/Documentation/CancelAnnualReportAsync',
            type: 'GET',
            cache: false,
            data: { id: annualReportId },
            success: function (message) {
                $(row).find('td').eq(indexAnnualReportStatus).html(AnnualReportStatus.Unconfirmed);
                $(row).removeClass();
                $(row).addClass('unconfirmed-ar-menu');
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
    }
    function deleteAnnualReport(row, annualReportId) {
        $('#Yes').modal('hide');
        $('#Yes').unbind();
        $.ajax({
            url: '/Documentation/DeleteAnnualReportAsync',
            type: 'GET',
            cache: false,
            data: { id: annualReportId },
            success: function (message) {
                $(row).remove();
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
    }
    $('#CreateAnnualReportLikeAdmin').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        var cityId = $('#CitiesList option').filter(':selected').val();
        var strURL = '/Documentation/CreateAnnualReportLikeAdminAsync?cityId=' + cityId;
        window.open(strURL, '_self');
    });
    function showYesNoModal(row, actionStr) {
        var cityName = $(row).find('td').eq(indexCityName).html();
        var date = $(row).find('td').eq(indexDate).html();
        var year = date.split('-').pop();
        showModalMessage($('#YesNoModal'), 'Ви дійсно хочете ' + actionStr + ' річний звіт станиці ' + cityName +
            ' за ' + year + ' рік?');
    }
});
$('#annual-report-form').ready(function () {
    var hasChanges = false;
    if ($('#ModalSuccess .modal-body:first p:first strong:first').contents().length != 0) {
        $('#ModalSuccess').modal('show');
    }
    else {
        if ($('#ModalError .modal-body:first p:first strong:first').contents().length != 0) {
            $('#ModalError').modal('show');
        }
    }
    $('select').change(function () {
        hasChanges = true;
    });
    $('input').change(function () {
        hasChanges = true;
    });
    $('textarea').change(function () {
        hasChanges = true;
    });
    $('#CreateAnnualReport').click(function (e) {
        if (!hasChanges) {
            e.preventDefault();
            e.stopPropagation();
            $('#Yes').bind('click', createEmptyAnnualReport);
            showModalMessage($('#YesNoModal'), 'Ви не внесли жодних даних! Чи дійсно створити такий звіт?');
        }
    });
    function createEmptyAnnualReport() {
        $('#Yes').unbind();
        hasChanges = true;
        $('#CreateAnnualReport').click();
    }
});
function setDisabled(elements, disabled) {
    for (let el of elements) {
        el.prop('disabled', disabled);
        if (disabled) {
            el.addClass('disabled');
        }
        else {
            el.removeClass('disabled');
        }
    }
}
function showModalMessage(modalWindow, message) {
    modalWindow.find('.modal-body:first p:first strong:first').html(message);
    modalWindow.modal('show');
}
//# sourceMappingURL=annualReport.js.map