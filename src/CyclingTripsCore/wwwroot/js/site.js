// Write your Javascript code.

(function () {

    var $userNavAndWrapper = $("#userNav, #wrapper");

    $("#sidebarToggle").on("click", function () {
        $userNavAndWrapper.toggleClass("hide-sidebar");
        var $i = $(this).children("i");
        if ($userNavAndWrapper.hasClass("hide-sidebar")) {
            $i.toggleClass("fa-caret-left, fa-caret-right")
        }
        else {
            $i.toggleClass("fa-caret-left, fa-caret-right")
        }
    });


})();

