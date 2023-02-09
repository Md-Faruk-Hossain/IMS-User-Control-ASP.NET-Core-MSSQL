
var floatingMessage = {
    messageTemplate: null,
    init: function () {
        let template = $('<div class="floating-message"></div>');
        let msgIcon = $('<i class="fa fa-info-circle"></i>');
        let msgBody = $('<div class="msg-body"></div>');
        let msgTitle = $('<h3></h3>');
        let msgText = $('<p></p>');
        let dismissButton = $('<a class="fa fa-times" id="btnCloseFloatMessage"></a>');

        msgTitle.appendTo(msgBody);
        msgText.appendTo(msgBody);
        msgIcon.appendTo(template);
        msgBody.appendTo(template);
        dismissButton.appendTo(template);

        let msgContainer = $('<div id="floating-message-container"></div>');
        msgContainer.appendTo($('body'));

        this.messageTemplate = template;
    },
    Show: function (title, text) {

        let msg = this.messageTemplate.clone();

        msg.find('h3').text(title);
        msg.find('p').text(text);
        msg.find('i').addClass('fa fa-info-circle');
        msg.addClass('error');
        msg.hide();
        msg.appendTo($('#floating-message-container'));

        setTimeout(function () {

            msg.slideDown('slow');
            setTimeout(function () {
                msg.slideUp(function () {
                    msg.remove();
                });
            }, 5000);

        }, 500);
    }
};
floatingMessage.init();

//floatingMessage.Show('Oops something went wrong', 'DEEP was unable to do bla bla');
$(document).on('click', '#btnCloseFloatMessage', function (e) {
    e.preventDefault();
    var msg = $(this).closest(".floating-message");

    msg.fadeOut(500, function () {
        $(this).remove();
    });

});