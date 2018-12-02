/* ------------------------------------------------------------------------------
*
*  # Basic map
*
*  Specific JS code additions for maps_google_basic.html page
*
*  Version: 1.0
*  Latest update: Aug 1, 2015
*
* ---------------------------------------------------------------------------- */

$(function() {

	// Map settings
	function initialize() {

		// Optinos
		var mapOptions = {
			zoom    : 5,
			center  : new google.maps.LatLng(-0.789275, 113.92132700000002)
		};

		// Apply options
		map = new google.maps.Map($('.map-basic')[0], mapOptions);

		var marker = new google.maps.Marker({
		    position    : new google.maps.LatLng(-6.193456200000001, 106.85028790000001),
		    map         : map,
		    title       : '172.168.0.22'
		});
		google.maps.event.addListener(marker, 'click', (function (marker) {
		    return function () {
		        jQuery('#modal-trigger').click();
		        console.log(marker);
		    }
		})(marker));

		marker = new google.maps.Marker({
		    position: new google.maps.LatLng(-6.2845276, 106.80013959999997),
		    map: map,
		    title: '172.168.0.23'
		});
		google.maps.event.addListener(marker, 'click', (function (marker) {
		    return function () {
		        jQuery('#modal-trigger').click();
		    }
		})(marker));

		marker = new google.maps.Marker({
		    position: new google.maps.LatLng(-6.183459, 106.7647475),
		    map: map,
		    title: '172.168.0.24'
		});

		google.maps.event.addListener(marker, 'click', (function (marker) {
		    return function () {
		        jQuery('#modal-trigger').click();
		    }
		})(marker));
	}

	// Load map
	google.maps.event.addDomListener(window, 'load', initialize);

});
