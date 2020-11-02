
$(function() {

    $(document).on("click",
        "#LoginButton",
        function() {
            AjaxGet("~/Home/GetLoginForm",
                function(data) {
                    bootbox.dialog({
                        title: "Login",
                        message: data
                    });
                },
                false);
        });

    $(document).on("click",
        "#SubmitLogin",
        function() {
            AjaxPost("~/Base/Login",
                $("#LoginForm").serialize(),
                function (data) {
                    if (data.success) {
                        $("#ErrorMessage").addClass("d-none");
                        $("#ErrorMessage").text("");
                        window.location.href = data.redirectUrl;
                        bootbox.hideAll();
                    } else {
                        $("#ErrorMessage").text(data.message);
                        $("#ErrorMessage").removeClass("d-none");
                    }
                },
                false);
        });

});