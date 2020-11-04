
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
                        alert("error");
                    }
                },
                false);
        });
});