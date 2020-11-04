
$(function() {

    $(document).on("change",
        "input",
        function () {
            UpdateValidation();
        });

    $(document).on("click",
        "#saveSubscriberButton",
        function () {
            if (UpdateValidation()) {
                AjaxPost("~/Register/SaveUser",
                    $("#CreateUpdateSubscriberForm").serialize(),
                    function (data) {
                        if (data.success) {
                            $("#serverValidationMessage").addClass("d-none");
                            $("#serverValidationMessage").text("");
                            window.location.href = data.redirectUrl;
                        } else {
                            $("#serverValidationMessage").text(data.message);
                            $("#serverValidationMessage").removeClass("d-none");
                        }
                    },
                    false);
            } else {
                alert("Form has invalid fields.");
            }
        });

    $(document).on("click",
        ".deleteSubscriberButton",
        function(event) {
            var $target = $(event.target).closest("button");
            var subscriberId = $target.data("subscriberid");
            AjaxGet("~/Subscribers/DeleteSubscriber?subscriberId=" + subscriberId,
                function(data) {
                    if (data.success) {
                        $("#SubscriberTableContainer").html(data.view);
                        $("tr[data-id=" + subscriberId + "]").remove();
                    } else {
                        alert("error");
                    }
                },
                false);
        });

    function UpdateValidation() {
        var valid = true;
        if ($("#Password").val() != $("#ConfirmPassword").val()) {
            valid = false;
            $("#PasswordMismatchError").removeClass("d-none");
        } else {
            $("#PasswordMismatchError").addClass("d-none");
        }
        if ($("#Email").val() != $("#ConfirmEmail").val()) {
            valid = false;
            $("#EmailMismatchError").removeClass("d-none");
        } else {
            $("#EmailMismatchError").addClass("d-none");
        }
        if ($("#Password").val() == "" || $("#ConfirmPassword").val() == "") {
            valid = false;
        }
        if ($("#Email").val() == "" || $("#ConfirmEmail").val() == "") {
            valid = false;
        }
        return valid;
    }
});