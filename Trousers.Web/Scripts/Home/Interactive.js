﻿
(function(window, document, trousers, undefined) {

    var self = {
        _sequenceNumber: 0,
        _searchTimeout: null,

        onInput: function(e) {
            self.queueSearch();
        },

        queueSearch: function () {
            window.clearTimeout(self._searchTimeout);
            self._searchTimeout = window.setTimeout(self.doSearch, 400);
        },

        doSearch: function() {
            $("#result").html("imma thinkin");
            var data = $("#form").serialize();

            $.ajax({
                type: 'POST',
                url: "/Home/Search",
                data: data,
                success: self.onSuccess,
                error: self.onError,
            });
        },

        onSuccess: function (response, textStatus, jqXHR) {
            $("#expr").focus();
            if (response.IsJson) {
                if (response.SequenceNumber > self._sequenceNumber) {
                    self._sequenceNumber = response.SequenceNumber;
                    trousers.handleResponse(response, $("#result"));
                }
                return;
            }

            $("#result").html(response);
        },

        onError: function (response, textStatus, jqXHR) {
            $("#expr").focus();
            $("result").text(response);
        },
    };


    $(document).ready(function() {

        $("#expr")
            .bind("input", self.onInput)
            .focus();

        $("span.searchAction").live("click", function(e) {
            $("#searchAction").val($(this).text());
            self.queueSearch();
        });

        self.queueSearch();
    });

})(window, document, window.trousers);