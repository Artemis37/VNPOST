"use strict";

$(document).ready(function () {
    //indicator show on hover
    $("#carouselImageIndicators").mouseenter(function () {
        $(".carousel-indicators").css({ "visibility": "visible", "opacity": "1" });
        $(".carousel-control-hover").css({ "visibility": "visible", "opacity": "1" });
    });
    $("#carouselImageIndicators").mouseleave(function () {
        $(".carousel-indicators").css({ "visibility": "hidden", "opacity": "0" });
        $(".carousel-control-hover").css({ "visibility": "hidden", "opacity": "0" });
    });

    //carousel news
    $(".first-five-news-item:nth-child(1)").css("background-color", "#fcb71e");
    let i = 1;
    setInterval(() => {
        $(`.first-five-news-item:nth-child(${i})`).css("background-color", "inherit");
        i++;
        if (i > 5) i = 1;
        $(`.first-five-news-item:nth-child(${i})`).css("background-color", "#fcb71e");
        if (i === 4) {
            $("#preview-news div img").attr("src", `/Images/News/${i}.png`);
        } else {
            $("#preview-news div img").attr("src", `/Images/News/${i}.jpg`);
        }
    }, 10000);
});