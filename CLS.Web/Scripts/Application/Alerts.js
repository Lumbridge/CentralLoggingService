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

    function UpdateExpressionPreview() {
        var expression = "";
        $(".node").each(function (index) {
            expression += $(this).find("option:selected").val();
        });
        var endChars = expression.slice(expression.length - 2);
        if (endChars == "&&" || endChars == "||")
            expression = expression.substr(0, expression.length - 2);
        $(".expressionTextBox").val(expression);
    }

    $(document).on("change",
        "input, select",
        function () {
            UpdateExpressionPreview();
        });

    function UpdateDynamicSelect(target, options) {
        $(target).html("<select class='form-control dynamicInput node'></select>");
        var dynamicInput = $(target).find(".dynamicInput");
        $.each(options,
            function (index, value) {
                $(dynamicInput).append($('<option/>', {
                    value: "\"" + value + "\"",
                    text: value
                }));
            });
        UpdateExpressionPreview();
    }
});