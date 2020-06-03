declare const CurrentClubInClubMembers: number;
let ClubMemberId: number;
$(document).ready(function () {

    $('.ClubMemberToAdmin').click(function (event) {
        $('#addClubAdmin').modal('show');
        const button = $(event.target).parent();
        ClubMemberId = button.data('adminid');
        console.log(ClubAdminId);
    });
});

function AddClubAdmin() {
    const input = <HTMLInputElement>$('#AdminTypeName')[0];
    const AdminTypeName = input.value;
    if (AdminTypeName == null || AdminTypeName.length === 0) {
        alert('Введіть назву діловодства');
        return;
    }

    const input2 = <HTMLInputElement>$('#ClubAdminStartDate')[0];
    const StartDateValue = input2.value;
    const StartDate = StartDateValue + ' 00:00:00';
    if (StartDateValue == null || StartDateValue.length === 0) {
        alert('Введіть дату початку діловодства');
        return;
    }


    const input3 = <HTMLInputElement>$('#ClubAdminEndDate')[0];
    const EndDateValue = input3.value;
    let EndDate = null;
    if (EndDateValue != null && EndDateValue.length !== 0) {
        EndDate = EndDateValue + ' 00:00:00';
        var StartDateobj = new Date(StartDate);
        var EndDateobj = new Date(EndDate);
        if (StartDateobj >= EndDateobj) {
            alert('Дата кінця діловодства має бути після дати початку');
            return;
        }
    }




    const adminsData = {
        ClubMembersID: ClubMemberId,
        ClubId: CurrentClub,
        EndDate: EndDate,
        StartDate: StartDate,
        adminTypeName: AdminTypeName
    };
    console.log(adminsData);
    console.log(JSON.stringify(adminsData));

    $.ajax({
        url: '/Club/AddToClubAdministration',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(adminsData),
        timeout: 5000
    }).done((result) => {
        if (result === 1) {
            alert('Нового діловода було успішно додано');
        }
    });
}