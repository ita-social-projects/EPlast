declare const CurrentClub: number;
let ClubAdminId: number;
$(document).ready(function () {
    $('#dtClubAdmins').DataTable({
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Ukrainian.json"
        }
    });
    $('.changeEnddatebutton').click(function (event) {
        $('#changeEnddate').modal('show');
        const button = $(event.target).parent();
        ClubAdminId = button.data('adminid');
        console.log(ClubAdminId);
    });
});

function ChooseEndDate() {
    const input = <HTMLInputElement>$('#txtToEndDate')[0];
    const date = input.value;
    const longDate = date + ' 00:00:00';

    const adminsData = {
        adminId: ClubAdminId,
        clubIndex: CurrentClub,
        enddate: longDate,
    };

    $.ajax({
        url: '/Club/AddEndDate',
        type: 'POST',
        data: JSON.stringify(adminsData),
        contentType: 'application/json; charset=utf-8',
        timeout: 5000
    }).done((result) => {
        if (result === 1) {
            $('a[data-adminid="' + ClubAdminId + '"]').parent().siblings('.ClubAdminEndDate')[0].innerHTML = date;
        }
    });
}