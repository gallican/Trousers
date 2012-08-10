
(function(window, document, undefined) {

    var self = {
        _plugins: { },

        registerPlugin: function(plugin, name) {
            self._plugins[name] = plugin;
        },

        handleResponse: function(response, resultsElement) {
            var plugin = self._plugins[response.ResponseType];
            if (plugin) plugin.displayResponse(response, resultsElement);
        },
    };

    window.trousers = self;

})(window, document);