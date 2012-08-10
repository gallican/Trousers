
(function(window, document, trousers, undefined) {

    var self = {
        onInput: function(e) {
            self.doSearch();
        },

        doSearch: function() {
            $("#result").html("imma thinkin");
            var data = $("#form").serialize();
            $.post("/Home/Search", data, self.onSuccess);
        },

        onSuccess: function(response) {
            $("#expr").focus();
            trousers.handleResponse(response, $("#result"));
        },
    };


    $(document).ready(function() {

        $("#expr")
            .bind("input", self.onInput)
            .focus();

        $("span.searchAction").live("click", function(e) {
            $("#searchAction").val($(this).text());
            self.doSearch();
        });

        self.doSearch();
    });

})(window, document, window.trousers);