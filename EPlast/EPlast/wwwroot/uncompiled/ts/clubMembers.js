let ClubMemberId;
$(document).ready(function () {
    $('.ClubMemberToAdmin').click(function (event) {
        $('#addClubAdmin').modal('show');
        const button = $(event.target).parent();
        ClubMemberId = button.data('adminid');
        console.log(ClubAdminId);
    });
});
function AddClubAdmin() {
    const input = $('#AdminTypeName')[0];
    const AdminTypeName = input.value;
    if (AdminTypeName == null || AdminTypeName.length === 0) {
        alert('������ ����� ����������');
        return;
    }
    const input2 = $('#ClubAdminStartDate')[0];
    const StartDateValue = input2.value;
    const StartDate = StartDateValue + ' 00:00:00';
    if (StartDateValue == null || StartDateValue.length === 0) {
        alert('������ ���� ������� ����������');
        return;
    }
    const input3 = $('#ClubAdminEndDate')[0];
    const EndDateValue = input3.value;
    let EndDate = null;
    if (EndDateValue != null && EndDateValue.length !== 0) {
        EndDate = EndDateValue + ' 00:00:00';
        var StartDateobj = new Date(StartDate);
        var EndDateobj = new Date(EndDate);
        if (StartDateobj >= EndDateobj) {
            alert('���� ���� ���������� �� ���� ���� ���� �������');
            return;
        }
    }
    const adminsData = {
        adminId: ClubMemberId,
        clubIndex: CurrentClub,
        enddate: EndDate,
        startdate: StartDate,
        AdminType: AdminTypeName
    };
    console.log(adminsData);
    console.log(JSON.stringify(adminsData));
    $.ajax({
        url: '/Club/AddToClubAdministration',
        type: 'POST',
        data: JSON.stringify(adminsData),
        contentType: 'application/json; charset=utf-8',
        timeout: 5000
    }).done((result) => {
        if (result === 1) {
            alert('������ ������� ���� ������ ������');
        }
    });
}
//# sourceMappingURL=clubMembers.js.map