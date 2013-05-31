; (function () {

    var app = {
        init: function () {
            this.timer.setup(); // Set up the timer

            $('#showEmailForm').on('click', function (e) { // When the "Email" button is clicked...
                e.preventDefault(); // Stop the page from jumping to the top
                /*$('.actions a, .actions span').fadeOut(500, function () { // Fade out our links & "OR" seperator
                    $('.actions form').fadeIn(500); // Fade in our email form
                });*/
                $('.actions, .initial-content').fadeOut(500, function () {
                     $('.subscribe').fadeIn(500); // Fade in thank you content
                });
            });

            $('#emailForm').on('submit', function (e) { // When the email form is submitted
                e.preventDefault();
                var $this = $(this),
					email = $this.find('[name="email"]').val(),
                    name = $this.find('[name="name"]').val(),
					password = $this.find('[name="password"]').val();
                $('.subscribe p.loading').css("visibility","visible");
                $.post("/api/users/create", { Nombre: name, Email: email, Password: password })
                .done(function (data) {

                    $('.subscribe').fadeOut(700, function () { // Fade out our links & "OR" seperator
                        $('.thanks').fadeIn(500); // Fade in thank you content
                    });
                });

            });
        },
        timer: {
            setup: function () {
                $('.phone-countdown').countdown("2013/06/25", function (event) { // Be sure to change "2013/12/25" to your launch date!
                    var $this = $(this);
                    switch (event.type) {
                        case "seconds":
                        case "minutes":
                        case "hours":
                        case "days":
                        case "weeks":
                        case "daysLeft":
                            $this.find('.countdown-' + event.type).html(event.value);
                            break;
                        case "finished":
                            $this.hide();
                            break;
                    }
                });
            }
        },
        phone: {
            /* Animations for the phone */
            init: function () {
                $('.logo').delay(1000).animate({
                    top: 78
                }, 1000);

                $('.phone-countdown').delay(1500).fadeIn(1000, function () {
                    if ($('body').width() > 979) {
                        $('.phone').animate({
                            right: '29%'
                        }, 750);
                    }

                    $('.main-content').delay(450).animate({
                        opacity: 1
                    }, 500);
                });
            }
        },
        domReady: function () {
            //Check for confirm
            var match = location.hash.match(new RegExp("confirm" + '=([^&]*)'));
            if (match != null) {
                var confirm = match[1];
                $('.initial-content').fadeOut();
                $('.actions').fadeOut();
                $('.confirmation-content').fadeIn(100);
                $.get("api/users/confirm?id=" + confirm).done(function (data) {
                    $('.before-activating, .confirmation-content p.loading').css("display", "none");
                    $('.account-activated').css("display", "block");
                });
            }
            //Check for privacy
            var match = location.hash.match(new RegExp("privacy"));
            if (match != null) {
                $('.initial-content').fadeOut();
                $('.actions').fadeOut();
                $('.title').fadeOut();
                $('.privacy').fadeIn();
            }
            //Check for conditions
            var match = location.hash.match(new RegExp("conditions"));
            if (match != null) {
                $('.initial-content').fadeOut();
                $('.actions').fadeOut();
                $('.title').fadeOut();
                $('.conditions').fadeIn();
            }
        },
        windowLoad: function () {
            $('.phone').fadeIn(500, app.phone.init); // When the window has loaded, fade in the phone...
        }
    };

    app.init();
    $(function () {
        app.domReady();

        $(window).load(app.windowLoad);
    });

})(jQuery)