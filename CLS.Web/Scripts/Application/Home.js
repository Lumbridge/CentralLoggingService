$(function() {
    $("time.timeago").timeago();

    var $chart = $("#messagesOverTimeChart");
    new Chart($chart,
        {
            "type": "line",
            "data": {
                "labels": $chart.data("labels"),
                "datasets": [
                    {
                        "label": "# of Messages",
                        "data": $chart.data("values"),
                        "fill": false,
                        "borderColor": "rgb(75, 192, 192)",
                        "lineTension": 0.1
                    }
                ]
            },
            "options": {
                title: {
                    display: true,
                    text: '# of Messages Logged over the Last 7 Days'
                }
            }
        });
});