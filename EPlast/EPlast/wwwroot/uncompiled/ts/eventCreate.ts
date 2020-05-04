$(document).ready(function () {
    $('#block button').click(function () {
        $('#name').val(this.id);
    });
    
    $("#txtFromDate").datepicker({
        dateFormat: "dd-mm-yy",
        changeMonth: true,
        changeYear: true,
        yearRange: 'c-0:c+nn',
        minDate: 'd',
        onSelect: function (selected) {
            $("#txtToDate").datepicker("option", "minDate", selected)
        }
    });

    $("#txtToDate").datepicker({
        dateFormat: "dd-mm-yy",
        changeMonth: true,
        changeYear: true,
        yearRange: 'c-0:c+nn',
        minDate: 'd',
        onSelect: function (selected) {
            $("#txtFromDate").datepicker("option", "maxDate", selected)
        }
    });

    $("#visitedEvent").click(function () {
        $("#visitedModal").modal('show');
    });

    $("#planedEvent").click(function () {
        $("#planedModal").modal('show');
    });

    $("#createdEvent").click(function () {
        $("#createdModal").modal('show');
    });

});