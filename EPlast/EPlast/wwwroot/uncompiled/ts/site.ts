$(document).ready(function () {
    $('.dataTables_length').addClass('bs-select');
});
/*js from LoginAndRegister*/

function registerClick() {
    $(".switcher-text").addClass("register-active").removeClass("login-active");
}

function loginClick() {
    $(".switcher-text").addClass("login-active").removeClass("register-active");
}

$(".register-text").click(registerClick);
$(".login-text").click(loginClick);

$("input#autocomplete_input").each(function (index) {
    $(this).change(function () {
        if ($(this).val() == "") {
            $("#autocomplete_input_id_" + index).val(null);
        }
        else {
            $("#autocomplete_input_id_" + index).val($('option[value="' + $(this).val() + '"]').data('value'));
        }
    });
});
