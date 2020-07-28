﻿(function () {

    var allNavItems = $(".main-nav li");
    var allTabs = $(".tab");

    allNavItems.on("click", function (e) {

        e.preventDefault();

        allNavItems.removeClass("active");
        allTabs.removeClass("active");

        // this = list item
        $(this).addClass("active");

        var selector = $(this).find("a").attr("href");
        $(selector).addClass("active");

    });

    // ServiceWorker is a progressive technology. Ignore unsupported browsers
    if ('serviceWorker' in navigator) {
        console.log('CLIENT: service worker registration in progress.');
        navigator.serviceWorker.register('./js/service-worker.js').then(function () {
            console.log('CLIENT: service worker registration complete.');
        }).catch ((error) => {
            // регистрация прошла неудачно
            console.log('Registration failed with ' + error);
        });

       //     function () {
       //     console.log('CLIENT: service worker registration failure.');
       // });
    } else {
        console.log('CLIENT: service worker is not supported.');
    }

})();