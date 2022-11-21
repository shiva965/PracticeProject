// CommonJS
const jQuery = require('jquery');
require('jquery.easing')(jQuery);

// AMD
define(['jquery', 'jquery.easing'], function (jQuery, easing) {
	easing(jQuery);
});
