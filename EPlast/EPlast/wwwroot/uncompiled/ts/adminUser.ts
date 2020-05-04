$(document).ready(function () {
    jQuery(function ($) {
        $.datepicker.regional['ua'] = {
            closeText: 'Закрити',
            prevText: '&#x3c;Поп',
            nextText: 'Наст&#x3e;',
            currentText: 'Сьогодні',
            monthNames: ['Січень', 'Лютий', 'Березень', 'Квітень', 'Травень', 'Червень',
                'Липень', 'Серпень', 'Вересень', 'Жовтень', 'Листопад', 'Грудень'],
            monthNamesShort: ['Січ', 'Лют', 'Бер', 'Квіт', 'Трав', 'Черв',
                'Лип', 'Серп', 'Вер', 'Жовт', 'Лист', 'Груд'],
            dayNames: ['неділя', 'понеділок', 'вівторок', 'среда', 'четвер', 'п\'ятниця', 'субота'],
            dayNamesShort: ['нд', 'пн', 'вт', 'ср', 'чт', 'пт', 'сб'],
            dayNamesMin: ['Нд', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
            weekHeader: 'Ти',
            dateFormat: 'dd-mm-yy',
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        };
        $.datepicker.setDefaults($.datepicker.regional['ua']);
    });

    $("#phoneNumber").mask("+38(999)-999-99-99", { placeholder: "+38(___)-___-__-__" });

    $("#datepickerBirthday").datepicker({
        dateFormat: "dd-mm-yy",
        changeMonth: true,
        changeYear: true,
        yearRange: '-100y:c+nn',
        maxDate: '-1d'
    });

    $("#datepickerBirthday").datepicker("setDate", $("#dtOfBrthd").val());

    $.contextMenu({
        className: "admin-edit",
        selector: '.context-menu-one',

        callback: function (key) {
            $.get("/Admin/" + key + "?userid=" + $(this).data("id"), function (data) {
                $('#dialogContent').html(data);
                $('#modDialog').modal('show');
            });
        },
        items: loadMe()
    });

    $("input#regionsAndAdmins").each(function () {
        $(this).change(function () {
            $.get("/Admin/GetAdmins/" + "?cityId=" + $('option[value="' + $(this).val() + '"]').data('value'), function (data) {
                $('#getAdmins').empty();
                $('#getAdmins').append(data);
            });
        });
    });

    $("#dtUsersTable").DataTable({
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Ukrainian.json"
        },
        responsive: true,
        columnDefs: [
            { type: 'string', targets: 0 },
            { type: 'string', targets: 1 },
        ]
    });

    $('#dtUsersTable').on('page.dt', function () {
        $('html, body').animate({
            scrollTop: 100
        }, 200);
    });
});
function loadMe() {
    if ($("#role").val() == "Admin") {
        return {
            "Edit": { name: "Змінити", icon: "fas fa-align-justify" },
            "Delete": { name: "Видалити", icon: "fas fa-trash-alt delete" }
        }
    }
    return {
        "Edit": { name: "Права доступу", icon: "fas fa-align-justify" },
    }
}