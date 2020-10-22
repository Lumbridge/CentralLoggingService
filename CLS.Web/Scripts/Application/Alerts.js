$(function () {
    $(document).on("click",
        "#newAlertButton",
        function () {
            AjaxGet("~/Alerts/GetCreateUpdateAlertForm?",
                function (data) {
                    bootbox.dialog({
                        title: "Create New Alert",
                        message: data,
                        size: 'large'
                    });
                },
                false);
        });

    $(document).on("click",
        ".addLayerButton",
        function (event) {
            var $targ = $(event.target).closest("button");
            $targ.parent(".col-1").prev(".logicalOperatorsColumn").removeClass("d-none");
            AjaxGet("~/Alerts/GetSubscriptionRow",
                function (data) {
                    $("#nodeGroupContainer").append(data.view);
                },
                false);
            $targ.parent(".col-1").addClass("d-none");
            $targ.parent(".col-1").siblings(".removeLayerColumn").removeClass("d-none");
        });

    $(document).on("click",
        ".removeLayerButton",
        function (event) {
            var $target = $(event.target).closest("button");
            $target.parent(".removeLayerColumn").parent().remove();
        });

    $(document).on("change",
        ".variableNameSelect",
        function (event) {

            var $target = $(event.target);
            var dynamicInputColumn = $target.parent(".col").siblings(".dynamicInputColumn");

            var selected = $target.find("option:selected").text();

            switch (selected) {
                case "MessageSeverity":
                case "PublishingSystemName":
                case "EnvironmentType":
                case "DayOfWeek":
                    {
                        AjaxGet("~/Alerts/GetDynamicSelectList?variableName=" + selected,
                            function (data) {
                                UpdateDynamicSelect(dynamicInputColumn, data.options);
                            },
                            false);
                        break;
                    }
                case "TimeOfDay":
                    {
                        $(dynamicInputColumn).html("<input type='time' class='form-control dynamicInput node'>");
                        break;
                    }
                case "NumberOfMessages":
                case "TimeWindowMinutes":
                    {
                        $(dynamicInputColumn).html("<input type='number' class='form-control dynamicInput node'>");
                        break;
                    }
            }
        });

    $(document).on("change",
        "input, select",
        function () {
            UpdateExpressionPreview();
        });

    $(document).on("click",
        ".saveAlertButton",
        function() {
            AjaxGet("~/Alerts/SaveAlert?expression=" +
                encodeURIComponent(GetFinalExpression()) +
                "&subscriberId=" +
                $("#Subscribers").find("option:selected").val() +
                "&alertTypeId=" +
                $("#AlertTypes").find("option:selected").val(),
                function (data) {
                    if (data.success) {
                        $("#SubscriptionTableContainer").html(data.view);
                        bootbox.hideAll();
                    } else {
                        alert("error");
                    }
                },
                false);
        });

    $(document).on("click",
        ".deleteAlertButton",
        function (event) {
            var $target = $(event.target).closest("button");
            var subscriptionId = $target.data("subscriptionid");
            AjaxGet("~/Alerts/DeleteAlert?subscriptionId=" + subscriptionId,
                function (data) {
                    if (data.success) {
                        $("#SubscriptionTableContainer").html(data.view);
                        $("tr[data-id=" + subscriptionId + "]").remove();
                    } else {
                        alert("error");
                    }
                },
                false);
        });

    function UpdateExpressionPreview() {
        var expression = "";
        $(".node").each(function (index) {
            var value = $(this).find("option:selected").val();
            if (value == undefined) {
                value = "\"" + $(this).val() + "\"";
            }
            expression += value;
        });
        var endChars = expression.slice(expression.length - 2);
        if (endChars == "&&" || endChars == "||")
            expression = expression.substr(0, expression.length - 2);
        if (!expression.includes("PublishingSystem.Name"))
            $("#NoPublishingSystemWarning").removeClass("d-none");
        else
            $("#NoPublishingSystemWarning").addClass("d-none");
        if (expression.includes("NumberOfMessages") && !expression.includes("TimeWindowMinutes") ||
            !expression.includes("NumberOfMessages") && expression.includes("TimeWindowMinutes")) {
            $("#MessagesTimeframeWarning").removeClass("d-none");
        } else {
            $("#MessagesTimeframeWarning").addClass("d-none");
        }


        $(".expressionTextBox").val(expression);
    }

    function GetFinalExpression() {
        var expression = "";
        $(".node").each(function (index) {
            var value = $(this).find("option:selected").val();
            if (value == undefined) {
                value = $(this).val();
            }
            expression += value + " ";
        });
        var endChars = expression.slice(expression.length - 4);
        if (endChars == " && " || endChars == " || ")
            expression = expression.substr(0, expression.length - 4);
        return expression;
    }
    
    function UpdateDynamicSelect(target, options) {
        $(target).html("<select class='form-control dynamicInput node'></select>");
        var dynamicInput = $(target).find(".dynamicInput");
        $.each(options,
            function (index, value) {
                $(dynamicInput).append($('<option/>', {
                    value: value,
                    text: value
                }));
            });
        UpdateExpressionPreview();
    }
});