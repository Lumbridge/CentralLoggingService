function ResolveUrl(url) {
	if (url.indexOf("~/") == 0) {
		var ap = $("#APPLICATION_PATH").val();
		url = url.replace("~", ap === "/" ? "" : ap);
	}
	return url;
}

// Simple encapsulations of basic ajax requests. If you just want a success handler then these
// are your guys, not really for use if you want a much more complicated ajax request with 
// options for done, error and context etc... If your request is complicated, it would be better to
// call $.ajax directly with your ajaxOptions rather than using one of these. 
// NOTE: Make sure to use the ResolveUrl() function if that's the case.
function AjaxGet(url, success, cache) {
	url = ResolveUrl(url);
	// also add another querystring item which identifies this as a 
	$.ajax({
		type: "GET",
		url: url,
		success: success,
		statusCode:
		{
			400: function () {
				// We've made an ajax call, but our session has expired, so we 
				// redirect to the SessionLost page
				window.location.assign(ResolveUrl("~/SessionLost"));
			},
			504: function () {
				window.location.assign(ResolveUrl("~/DbError.html"));
			}
		},
		error: function (jqXHR, textStatus, errorThrown) {
		},
		fail: function () {
		},
		cache: cache,
		headers: { 'x-ajax': 'true' }
	});
}

function AjaxLoad(url, cache) {
	url = ResolveUrl(url);
	$.ajax({
		type: "POST",
		url: url,
		statusCode:
		{
			400: function () {
				// We've made an ajax call, but our session has expired, so we 
				// redirect to the SessionLost page
				window.location.assign(ResolveUrl("~/SessionLost"));
			},
			504: function () {
				window.location.assign(ResolveUrl("~/DbError.html"));
			}
		},
		success: function () {
			window.location.href = url;
		},
		cache: cache,
		headers: { 'x-ajax': 'true' }
	});
}

function AjaxPost(url, data, success, cache) {
	url = ResolveUrl(url);
	$.ajax({
		type: "POST",
		url: url,
		data: data,
		success: success,
		cache: cache,
		error: function (xhr, ajaxOptions, thrownError) { },
		headers: { 'x-ajax': 'true' },
		statusCode:
		{
			400: function () {
				// We've made an ajax call, but our session has expired, so we 
				// redirect to the SessionLost page
				window.location.assign(ResolveUrl("~/SessionLost"));
			},
			504: function () {
				window.location.assign(ResolveUrl("~/DbError.html"));
			}
		},
	});
}

function AjaxDelete(url, data, success, cache) {
	url = ResolveUrl(url);
	$.ajax({
		type: "DELETE",
		url: url,
		data: data,
		success: success,
		cache: cache,
		headers: { 'x-ajax': 'true' },
		statusCode:
		{
			400: function () {
				// We've made an ajax call, but our session has expired, so we 
				// redirect to the SessionLost page
				window.location.assign(ResolveUrl("~/SessionLost"));
			},
			504: function () {
				window.location.assign(ResolveUrl("~/DbError.html"));
			}
		},
	});
}