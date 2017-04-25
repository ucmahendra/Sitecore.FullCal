$(document).ready(function () {
    window.ScfullCalendar = function (calendarControlId, leftControl, centerControl, rightControl, categories, searchQuery, contextItemId) {
        $(calendarControlId).fullCalendar({
            header: {
                left: leftControl,
                center: centerControl,
                right: rightControl
            },
            defaultDate: new Date().toJSON().slice(0, 10),
            eventLimit: true,
            weekMode: 'liquid',
            events: function (start, end, timezone, callback) {
                var myObj = {
                    startDate: start.format("DD/MMMM/YYYY"),
                    endDate: end.format('DD/MMMM/YYYY'),
                    categoryList: categories,
                    Query: searchQuery,
                    ItemId: contextItemId
                };
                $.ajax({
                    type: "POST",
                    url: '/api/sitecore/Event/GetEvents',
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(myObj),
                    dataType: "json",
                    success: function (data) {
                        callback(JSON.parse(data.toString()));
                    },
                    error: function (e) {
                        console.log("Error", e);
                    }
                });

                // Add the search querystring for paging back and forth - TESTING
                if (history.pushState) {
                    var newurl = location.protocol + '//' + location.host + location.pathname + getQueryString();
                    window.history.pushState({ path: newurl }, '', newurl);
                }
            },
            eventRender: function (event, element, monthView) {

            },
            eventAfterAllRender: function (event, element, monthView) {

            }
        });

    }
});

function setEqualHeight(column) {

    var maxHeight = 0;
    //Get all the elements
    var $column = $(column);
    // $column.height('auto');
    //Loop all the column
    for (var i = 0; i < $column.length; i++) {
        var $elem = $column.eq(i);

        if ($elem.height() > maxHeight) {
            maxHeight = $elem.height();
        }
    }
    $column.height(maxHeight);
}

function ajaxcall(data) {
    var decoded = $('<div/>').html(data).text();
    $('.event-list-view-container').html(decoded);
}

function getQueryStringValue(key) {
    return unescape(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + escape(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
}

function getQueryString() {
    var itemFilters = {};
    var queryString = '';

    $('.category-checkbox').each(function (index, input) {
        if (input.type === 'text') {
            registerKeywordFilter(itemFilters, input, 'q');
        }

        if (input.type === 'checkbox') {
            getFilteringQueryString(itemFilters, input);
        }
    });

    var keys = Object.keys(itemFilters);

    for (i = 0; i < keys.length; i++) {
        queryString += '&' + keys[i] + '=' + itemFilters[keys[i]];
    }

    // Replace the first one
    queryString = queryString.replace('&', '?');
    return queryString;
}

function registerKeywordFilter(itemFilters, input) {
    var keywords = input.value;
    if (keywords !== '') {
        var name = 'q';
        var queryString = keywords.replace(/ /g, ' ');
        itemFilters[name] = queryString;
    }
}

function getFilteringQueryString(itemFilters, input) {
    if (input.checked === true) {
        var name = $(input).attr('name');
        var value = $(input).attr('value');
        var itemName = $(input).attr('itemname');

        if (itemFilters[name] === undefined) {
            itemFilters[name] = itemName;
        }
        else {
            itemFilters[name] += '-' + itemName;
        }
    }
}



