(function (window, document, trousers, undefined) {

    var self = {

        google: window.google,

        displayResponse: function (response, targetElement) {
            var data = self.google.visualization.arrayToDataTable(response.Data);
            var options = response.Options;

            var divResult = $(targetElement);
            divResult.html('<div id="chart"></div>');
            var googleChart = new self.google.visualization.AreaChart($("#chart")[0]);
            googleChart.draw(data, options);
        },

    };

    trousers.registerPlugin(self, "ChartResponse");
})(window, document, window.trousers);
