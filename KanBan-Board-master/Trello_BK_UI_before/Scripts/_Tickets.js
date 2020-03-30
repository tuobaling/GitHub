$(document).ready(function() {
    $(document).on('click', '.delete-ticket', function () {
        var ticketId = $(this).closest('.ui-state-default').find('.ticket').attr('id');
        var boardId = $(document).find('#BoardId').val();
        var token = $('#__AjaxAntiForgeryForm input').val();
        //alert(ticketId + " " + boardId);

        if (ticketId != null) {
            var data = {
                __RequestVerificationToken: token,
                ticketId: ticketId
            };

            $.ajax({
                url: "/Tickets/Delete",
                type: "POST",
                data: data,
                success: function (data) {
                    var domain = window.location.host;
                    window.location.replace("http://" + domain + "/Boards/Details/" + boardId);
                }

            });

        } else {
            alert("There was some error in deleting this ticket. Try reload the page and do this action again.");
        }
    });

    // add a card link
    $(document).on('click', '.addTicket', function () {

        var htmlCode = '<div class="ticket new"><textarea name="add_ticket_input" placeholder="Enter note..."></textarea></div>';

        $(this).closest('.ticket').find('.ticketContainer').hide();
        $(this).closest('.ticket').find('.add_ticket').append(htmlCode);
        $(this).closest('.ticket').find('.ticket_footer').show();
    });


    //Close button in footer
    $(document).on('click', '.ticket.close', function () {
        $(".ticket.new").remove();
        $(".ticket_footer").hide();
        $('.ticketContainer').show();
    });

    $(document).on('click', '.btn_addTicket', function () { // click on add new card with class new - creates ticket first
        
        var boardId = $(document).find('#BoardId').val();
        var token = $('#__AjaxAntiForgeryForm input').val();
        var name = $('textarea').val().replace(/\s+/g, ' ').trim();

        var data = {
            __RequestVerificationToken: token,
            Name: name,
            BoardId: boardId
        };

        $.ajax({
            url: "/Tickets/Create",
            data: data,
            type: "POST",
            success: function(data) {
                var domain = window.location.host;
                window.location.replace("http://" + domain + "/Boards/Details/" + boardId);
            }
        });

    });
});