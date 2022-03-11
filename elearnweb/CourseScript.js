last_lesson_clicked = -1;
var p_tag = document.createElement("script");
p_tag.src = "https://www.youtube.com/iframe_api"
var firstscript = document.getElementsByTagName("script")[0];
firstscript.parentElement.insertBefore(p_tag, firstscript);

var player;
var last_lesson_clicked;

function colorLesson(item) {
    var percent = $(item).attr("percent");
    $(item).css("background", "linear-gradient(90deg, #80f276 " + percent + "%, white 0%)");
}

function onYouTubeIframeAPIReady() {
    player = new YT.Player('player', {
        height: 350,
        width: (0.95 * $("#player").parent().width()) | 0,
        event: {
            'onReady': ready()
        }
    });
}
function ready() {
    console.log("gottem");
}

function commentPanel(target) {
    $("div#comment-panel").children("textarea").val("");
    $("div#comment-panel").insertAfter($(target).parent());
    $("div#comment-panel").slideDown(250);

    var id = $(target).parent().parent().attr("commentID");

    if (id == null || id === null || id == "")
        id = "-1";

    $("div#comment-panel").attr("target-comment", id);
}

function collapse(sender) {
    $(sender).parent().parent().siblings("ul.children").slideToggle("fast");

    if ($(sender).text().startsWith("Show"))
        $(sender).text($(sender).text().replace("Show", "Hide"));
    else
        $(sender).text($(sender).text().replace("Hide", "Show"));

}

function newComment(target) {
    if (target.val() != null && target.val() != "") {
        $.ajax({
            url: "/CoursePage.aspx",
            type: "POST",
            success: function (data) {
                if (data == "ok") {
                    $("#comments").html(data);
                }
                else if (data.startsWith("login")) {
                    window.location.replace(data.split(';')[1]);
                }
            }
             ,
            data: {
                ajax: 1,
                lesson: last_lesson_clicked,
                command: "comment",
                content: $(target).val(),
                parent: $(target).parent().attr("target-comment")
            }
        });

    }
    $(target).parent().hide();
}

// Once every 5 seconds, save lesson progress
window.setInterval(function () {
    if (last_lesson_clicked != -1 && player.getPlayerState() == 1) {
        var newPerc = player.getMediaReferenceTime() / player.getDuration();
        $.ajax({
            url: "/CoursePage.aspx",
            type: "POST",
            success: function (data) {
                if (data.startsWith("ok")) {
                    var select = $("a[lessonid=" + last_lesson_clicked.toString() + "]");
                    select.attr("percent", parseInt(newPerc * 100));
                    colorLesson(select);
                    console.log("Saved");
                }
                else if (data.startsWith("login")) {
                    window.location.replace(data.split(';')[1]);
                }
            },
            data: {
                ajax: 1,
                lesson: last_lesson_clicked,
                percent: newPerc,
                command: "save_progress"
            }
        });
    }
}, 10000);

$(document).ready(function () {

    var lesItems = document.getElementsByClassName("lesson-list-item list-group-item");
    for (i = 0; i < lesItems.length; i++) {
        colorLesson(lesItems[i]);
    }
    $("div#comment-panel").hide();
    //var last_lesson_clicked = document.getElementsByClassName("lesson-list-item")[0].getAttribute("lessonID")

    function loadVideoById(id) {
        if (player === null)

            player.loadVideoById(id);
    }

    $(".lesson-list-item").click(function (event) {

        if ($(event.target).attr("lessonID") != last_lesson_clicked) {
            last_lesson_clicked = $(event.target).attr("lessonID");
            loadLesson(last_lesson_clicked, event.target);

        }

    });


    function loadLesson(lessonId, target) {
        $.ajax({
            url: "/CoursePage.aspx",
            type: "POST",
            success: function (data) {
                if (data.startsWith("ok")) {
                    $("#comments").html(data.split(';')[1]);
                    $(".children").hide();
                    player.loadVideoById($(target).attr("videoID"));
                    var perc = (parseInt($(target).attr("percent")) / 100);

                    var interval = setInterval(function () {
                        if (!(player.getDuration() == 0 || player.getDuration() === undefined)) {
                            player.seekTo(player.getDuration() * perc);
                            clearInterval(interval);
                        }
                    }, 100);
                }
                else if (data.startsWith("login")) {
                    window.location.replace(data.split(';')[1]);
                }
            }
            ,
            data: {
                ajax: 1,
                lesson: lessonId,
                command: "load"
            }
        });
    }





});