
$(function() {
    $(document).on("click",
        ".deletePublishingSystemButton",
        function (event) {
            var $target = $(event.target).closest("button");
            var publishingSystemId = $target.data("publishingSystemId");
            AjaxGet("~/PublishingSystems/DeletePublishingSystem?publishingSystemId=" + publishingSystemId,
                function (data) {
                    if (data.success) {
                        $("#PublishingSystemTableContainer").html(data.view);
                        $("tr[data-id=" + publishingSystemId + "]").remove();
                    } else {
                        DisplayError(data.message);
                    }
                },
                false);
        });

    $(document).on("click",
        "#newOwnerButton",
        function (event) {
            var publishingSystemName = $(event.target).data("publishingsystemname");
            AjaxGet("~/PublishingSystems/GetCreateUpdatePublishingSystemOwnerForm?publishingSystemName=" + publishingSystemName,
                function (data) {
                    if (data == undefined) {
                        DisplayError("An unknown error occurred.");
                    } else {
                        bootbox.dialog({
                            title: "Add Publishing System Owner",
                            message: data,
                            size: 'large'
                        });
                    }
                },
                false);
        });

    $(document).on("click",
        "#addPublishingSystemOwnerButton",
        function (event) {
            var $target = $(event.target).closest("button");
            var publishingSystemName = $target.data("publishingsystemname");
            var username = $("#username").val();
            AjaxGet("~/PublishingSystems/AddPublishingSystemOwner?publishingSystemName=" +
                publishingSystemName +
                "&username=" +
                username,
                function(data) {
                    if (data.success) {
                        $("#PublishingSystemOwnersTableContainer").html(data.view);
                        bootbox.hideAll();
                    } else {
                        DisplayError(data.message);
                    }
                },
                false);
        });

    $(document).on("click",
        ".removePublishingSystemOwnerButton",
        function (event) {
            var $target = $(event.target).closest("button");
            var publishingSystemName = $target.data("publishingsystemname");
            var username = $target.data("username");
            AjaxGet("~/PublishingSystems/RemovePublishingSystemOwner?publishingSystemName=" +
                publishingSystemName +
                "&username=" +
                username,
                function (data) {
                    if (data.success) {
                        $("#PublishingSystemOwnersTableContainer").html(data.view);
                        bootbox.hideAll();
                    } else {
                        DisplayError(data.message);
                    }
                },
                false);
        });
});