

$(function () {
    $("#CardDetailModal").on('change', ':checkbox', function () { // Checkbox change event

        var Status = null;

        if ($(this).is(':checked')) {
            $(this).closest('.checkbox_group').find('.checkbox_text').addClass('checked');
            Status = "DONE";
        }
        else {
            $(this).closest('.checkbox_group').find('.checkbox_text').removeClass('checked');
            Status = "NOT DONE";
        }

        CalculateProgressBar();



        var TaskId = $(this).attr('id').replace(/[^0-9]/g, '');  // Change TaskItem Status
        var CardId = $("#CardId").val();
        var token = $("#__AjaxAntiForgeryForm input").val();

        var data = {
            __RequestVerificationToken: token,
            CardId: CardId,
            Id: TaskId,
            //Status: Status
        };

        $.ajax({
            url: "/Activities/Edit",
            type: "POST",
            data: data,
            success: function (data) {
                $("#acts").html(data);
                ReloadCards();
            }
        });

    });




    $("#CardDetailModal").on('click', '#add_act_item_txt', function () { // Add new act item link click
        $('#add_act_item_txt').hide();
        $('#add_act_item_input').show().focus();
        $("#add_act_item_btns").show();

    });



    $("#CardDetailModal").on('click', '.btn_addAct', function () { // Create new TaskItem

        var ActItemName = $("#add_act_item_input").val();
        var CardId = $("#CardId").val();
        var token = $("#__AjaxAntiForgeryForm input").val();
        var data = {
            __RequestVerificationToken: token,
            Name: ActItemName,
            CardId: CardId
        };

        $.ajax({
            url: "/Activities/Create",
            type: "POST",
            data: data,
            success: function (data) {

                // load partial view to act area div

                $('#add_act_item_input').hide();
                $("#add_act_item_btns").hide();
                $("#add_act_item_txt").show();

                $("#CardDetailModal").load("/Cards/Details/" + CardId).modal('show');
                ReloadCards();
            }
        });
    });





    $("#CardDetailModal").on('click', '.close', function () { // Hide input
        $('#add_act_item_input').val("").hide();
        $("#add_act_item_btns").hide();
        $('#add_act_item_txt').show();
    });

    //delete Activities
    $("#CardDetailModal").on('click', '.delete-actitem', function () {
        var TaskId = $(this).attr('id').replace(/[^0-9]/g, '');  // Change TaskItem Status
        var CardId = $("#CardId").val();
        var token = $("#__AjaxAntiForgeryForm input").val();

        var data = {
            __RequestVerificationToken: token,
            CardId: CardId,
            Id: TaskId
        };

        $.ajax({
            url: "/Activities/Delete",
            type: "POST",
            data: data,
            success: function (data) {
                $("#CardDetailModal").load("/Cards/Details/" + CardId).modal('show');
                ReloadCards();
            }
        });
    });

}); // end of function ready




function CalculateProgressBar() { // Calculate progress bar based on checkboxes
    var checkboxes = $(":checkbox").length;
    var calcWidth = (100 / checkboxes) * $(":checkbox:checked").length;

    $(".progress-bar").animate({
        width: calcWidth + "%"
    }, 100);


    // change color of pregress bar based on % progress
    if (calcWidth === 100) $(".progress-bar").css({ "background-color": "#61bd4f" });
    else $(".progress-bar").css({ "background-color": "rgb(0, 121, 191)" });

}