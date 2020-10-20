$(function() {

    $(document).on("click",
        "#newSubscriberButton",
        function() {
            AjaxGet("~/Subscribers/GetCreateUpdateSubscriberForm",
                function(data) {
                    bootbox.dialog({
                        title: "Create New Subscriber",
                        message: data,
                        size: 'large'
                    });
                },
                false);
        });

    $(document).on("click",
        "#saveSubscriberButton",
        function() {
            AjaxPost("~/Subscribers/SaveSubscriber",
                $("#CreateUpdateSubscriberForm").serialize(),
                function(data) {
                    $("#SubscriberTableContainer").html(data.view);
                    bootbox.hideAll();
                },
                false);
        });

    $(document).on("click",
        ".deleteSubscriberButton",
        function (event) {
            var $target = $(event.target).closest("button");
            var subscriberId = $target.data("subscriberid");
            AjaxGet("~/Subscribers/DeleteSubscriber?subscriberId=" + subscriberId,
                function (data) {
                    if (data.success) {
                        $("#SubscriberTableContainer").html(data.view);
                        $("tr[data-id=" + subscriberId + "]").remove();
                    } else {
                        alert("error");
                    }
                },
                false);
        });
});