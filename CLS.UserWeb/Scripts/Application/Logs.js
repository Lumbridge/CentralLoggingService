$(function () {
    $("time.timeago").timeago();

    $(document).on("click",
        ".modalLink",
        function (event) {
            var $targ = $(event.target);
            var text = $targ.prop("title");
            bootbox.dialog({
                title: "",
                message: text,
                size: 'large'
            });
        });
});