
//DrownDownt en cascada entre teams y lague y generico, lo puedo invocar desde cualquier lugar:

$(document).ready(function () {
    $("#LeagueId").change(function () {
        $("#TeamId").empty();
        $.ajax({
            type: 'POST',     
            url: Url,//lo dejo asi, para quesea dinamico,//url: '@Url.Action("GetTeams")', aqui seria desde la misma vista
            dataType: 'json',
            data: { lagueId: $("#LeagueId").val() },//aqui paso el parametro, de lo seleccionado del combobox de leagueId
            success: function (team) {
                $.each(team, function (i, team) {
                    $("#TeamId").append('<option value="'
                        + team.TeamId + '">'
                        + team.Name + '</option>');
                });
            },
            error: function (ex) {
                alert('Failed to retrieve Teams.' + ex);
            }
        });
        return false;
    })
});