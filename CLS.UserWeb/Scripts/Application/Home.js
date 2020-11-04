$(function() {
    $("time.timeago").timeago();

    var $chart = $("#messagesOverTimeChart");

    var data = {
        labels: $chart.data("labels"),
        datasets: [{
            label: "# of Messages",
            backgroundColor: "rgba(179, 252, 255, 0.2)",
            borderColor: "rgb(75, 192, 192)",
            borderWidth: 2,
            hoverBackgroundColor: "rgba(255,99,132,0.4)",
            hoverBorderColor: "rgba(255,99,132,1)",
            data: $chart.data("values")
        }]
    };

    var options = {
        maintainAspectRatio: false,
        title: {
            display: true,
            text: '# of Messages Logged over the Last 7 Days'
        },
        scales: {
            yAxes: [{
                stacked: true,
                gridLines: {
                    display: true,
                    color: "rgba(255,99,132,0.2)"
                }
            }],
            xAxes: [{
                gridLines: {
                    display: false
                }
            }]
        }
    };

    Chart.Line('messagesOverTimeChart',
        {
            options: options,
            data: data
        });
});