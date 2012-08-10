
(function(window, document, trousers, undefined) {

    var self = {
        displayResponse: function(result, targetElement) {

            // populate the results container
            var detailsDisplay = (result.WorkItems.length === 1) ? "table-row" : "none"; // we'll display details by default if there's only one match...

            var html = "";
            html += '<table style="display: block;">';
            html += "<tbody>";

            html += self.BuildHeaderRow(result.DisplayFields);
            for (var k in result.WorkItems) {
                var oddOrEven = (k % 2 === 0) ? "even" : "odd";
                var workItem = result.WorkItems[k];

                // first pass: display all the visible-by-default fields
                html += self.BuildMainRow(result.DisplayFields, workItem, oddOrEven);

                // second pass: display all the other fields
                html += self.BuildDetailRow(result.DisplayFields, result.LongFields, detailsDisplay, workItem, oddOrEven);
            }
            html += "</tbody>";
            html += "</table>";

            var divResult = $(targetElement);
            divResult.html(html);
        },

        onMainRowClick: function(event) {
            ToggleDetailsDisplay($(event.target).closest(".mainRow"));
            event.stopPropagation();
            return false;
        },

        ToggleDetailsDisplay: function(jMainRow) {
            var jDetailRow = jMainRow.next(".detailRow");
            jDetailRow.toggle();
        },

        BuildHeaderRow: function(displayFields) {

            var tr = "<tr>";
            for (var wiHeaderKey in displayFields) {
                var wiHeaderValue = displayFields[wiHeaderKey];
                tr += "<th>" + wiHeaderValue + "</th>";
            }
            tr += "</tr>";

            return tr;
        },

        BuildMainRow: function(displayFields, workItem, oddOrEven) {
            var tr = '<tr class="mainRow ' + oddOrEven + '" WorkItemId="' + workItem.ID + '">';

            for (var l = 0; l < displayFields.length; l++) {
                var wiK = displayFields[l];
                var m = workItem.Keys.indexOf(wiK);
                var wiV = self.nullCoalesce(workItem.Values[m], '');
                tr += '<td style="vertical-align: top;">' + wiV + '</td>';
            }

            tr += "</tr>";
            return tr;
        },

        nullCoalesce: function(value, defaultValue) {
            if (typeof(value) === 'undefined') return defaultValue;
            if (value === null) return defaultValue;
            return value;
        },

        BuildDetailRow: function(displayFields, longFields, detailsDisplay, workItem, oddOrEven) {
            // begin containing cell and row
            var tr = '<tr class="detailRow ' + oddOrEven + '" style="display: ' + detailsDisplay + ';">';
            tr += '<td colspan=' + displayFields.length + '>';

            // begin containing table
            tr += '<table>';
            tr += '<tr>';

            tr += self.BuildDetailCell(displayFields, longFields, workItem);
            tr += self.BuildAttachmentsCell(workItem);

            // end containing table
            tr += '</tr>';
            tr += '</table>';

            // end containing cell and row
            tr += '</td>';
            tr += '</tr>';

            return tr;
        },

        BuildDetailCell: function(displayFields, longFields, workItem) {

            var td = '<td>';
            td += '<table style="display: block; margin: 10px; border: 1px solid #cccccc;">';

            // second pass
            td += '<tr>';
            td += '<td>';
            for (var l = 0; l < workItem.Keys.length; l++) {
                var wiK = workItem.Keys[l];
                if ((displayFields.indexOf(wiK) >= 0) || (longFields.indexOf(wiK) >= 0)) {
                    // second pass - ignore these
                } else {
                    var oddOrEven = l % 2 === 0 ? 'even' : 'odd';
                    var wiV = workItem.Values[l];
                    td += '<div class="DetailItem ' + oddOrEven + '">';
                    td += '<p style="font-weight: bold;">' + wiK + '</p>';
                    td += '<p>' + wiV + '</p>';
                    td += '</div>';
                }
            }
            td += '</td>';
            td += '</tr>';

            // third pass: display all the rows that were marked as "long"
            td += '<tr>';
            td += '<td>';

            for (var l = 0; l < workItem.Keys.length; l++) {
                var wiK = workItem.Keys[l];
                if (longFields.indexOf(wiK) >= 0) {
                    var wiV = workItem.Values[l];

                    td += '<p style="font-weight: bold;">' + wiK + '</p>';
                    td += '<p>' + wiV + '</p>';
                }
            }
            td += '</td>';
            td += '</tr>';

            td += '</table>';
            td += '</td>';

            return td;
        },

        BuildAttachmentsCell: function(workItem) {
            var td = '<td style="width: 320px;">';
            td += '<div class="Attachments" Populated="false"></div>';
            td += '</td>';
            return td;
        },
    };

    trousers.registerPlugin(self, "SearchResponse");

    $(document).ready(function() {

        $(".mainRow").live("click", self.onMainRowClick);
    });


})(window, document, window.trousers);