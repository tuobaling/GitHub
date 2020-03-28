//編輯器擴充項目-Edit Helper
//說明：只針對單一、須切換編輯的項目
//      以<div class="edithelper"></div>包住
//      主要都在.edithelper內操作

$(function () {
    var edithelper = $(document);

    edithelper.on('click', '.edithelper .labelfield', function () {
        var htmlCode = '<div contenteditable="true" class="editfield">' +
                            '<input type="text" class="form-control" id="edit_control" placeholder="input text..." value="' + $(this).text() + '" />' +
                            ' <div id="edit_btns" class="col-xs-2 padding_left_right_0 " style="margin-top: 5px;float: inherit;">' +
                                '<input type="button" class="edit_comfirm btn btn-success" value="Edit" />' +
                                '<button type="button" class="close">&times;</button>' +
                            '</div>' +
                        '</div>';

        $(this).hide();
        $(this).parent().append(htmlCode);
    });

    edithelper.on('click', '.edithelper .editfield .close', function () {
        $(this).closest('.edithelper').find('.labelfield').show();
        $(this).closest('.edithelper').find('.editfield').remove();
    });

    edithelper.on('click', '.edithelper .editfield .edit_comfirm', function () {
        var eventhandler = $(this).closest('.edithelper').find('.edithelper_event');

        switch (eventhandler.val()) {
            case 'boradedit':
                BoardEdit();
                break;

            case 'ticketedit':
                var ticketid = $(this).closest('.ui-state-default').find('.ticket').attr('id');
                TicketEdit(ticketid);
                break;

            case "cardedit":
                CardEdit();
                break;

            default:
                alert('Nobody sucks!');
        }
    });

    function BoardEdit() {
        var BoardId = $("#BoardId").val();
        var inputValue = $('.editfield .edit_comfirm').parent().parent().find('#edit_control').val();
        var token = $("#__AjaxAntiForgeryForm input").val();
        var data = {
            __RequestVerificationToken: token,
            Id: BoardId,
            Name: inputValue
        };

        $.ajax({
            url: "/Boards/Edit",
            type: "POST",
            data: data,
            success: function (data) {
                var domain = window.location.host;
                window.location.replace("http://" + domain + "/Boards/Details/" + BoardId);
            }
        });
    }

    function TicketEdit(ticketid) {
        var BoardId = $("#BoardId").val();
        var TicketId = ticketid;
        var inputValue = $('.editfield .edit_comfirm').parent().parent().find('#edit_control').val();
        var token = $("#__AjaxAntiForgeryForm input").val();
        var data = {
            __RequestVerificationToken: token,
            Id: TicketId,
            Name: inputValue,
            BoardId: BoardId
        };

        $.ajax({
            url: "/Tickets/Edit",
            type: "POST",
            data: data,
            success: function (data) {
                var domain = window.location.host;
                window.location.replace("http://" + domain + "/Boards/Details/" + BoardId);
            }
        });
    }

    function CardEdit() {
        var CardId = $("#CardId").val();
        var token = $("#__AjaxAntiForgeryForm input").val();
        var inputValue = $('.editfield .edit_comfirm').parent().parent().find('#edit_control').val();

        var data = {
            __RequestVerificationToken: token,
            Id: CardId,
            Name: inputValue
        };

        $.ajax({
            url: "/Cards/Edit",
            type: "POST",
            data: data,
            success: function (data) {
                ReloadCards();
                $("#CardDetailModal").html("").load("/Cards/Details/" + CardId).modal('show');
            }
        }).done(function () { // reattach sortable event to dynamically created elements
            AttachCardSortable();
        });
    }
});