﻿
function setup_listitems(options) {
    let defaultoptions = { href: '#', onloadedcb: undefined };
    if (typeof (options) == 'object') {
       // this.options = { ...defaultoptions, ...options };
        //edge troubles with es6
        this.options = defaultoptions
        this.options.href = options.href;
        if (options.onloadedcb)
            this.options.onloadedcb = options.onloadedcb;
    }
    else {

        this.options = defaultoptions;
        this.options.href = options;
    }
   
    var self = this; 
        var reload = function (href) {
            if (!href)
                href = self.options.href+'/ListItems?';
            else
                href += "&";
            $('#table-content').load(href + 'searchcriteria=' + $('#search-val').val());

        }
        $.ajaxSetup({ cache: false });
       
        reload();
        $('#search-btn').click(function (e) {
            reload();
        });
        $('#custom-search-input').keydown((event) => {
            if (event.which == 13) {
                event.preventDefault();
                reload();
            }
        });
        $('#create-btn').click(function (e) {
            e.preventDefault();
            var url = self.options.href+'/CreateModal';
            $.get(url, function (data) {
                $('#dialogContent').html(data);
                $('#modDialog').modal('show');
                if (self.options.onloadedcb)
                    self.options.onloadedcb();
            });
        });
        $(document).on("click", "a.ahead", function (e) {
            e.preventDefault();
            reload(this.href);
        });
        $(document).on("click", "a.apagebottom", function (e) {
            e.preventDefault();
            reload(this.href);
        });
        $(document).on("click", "a.catitem", function (e) {
            e.preventDefault();
            $.get(this.href, function (data) {

                $('#dialogContent').html(data);
                $('#modDialog').modal('show');
                if (self.options.onloadedcb)
                    self.options.onloadedcb();
            });
        });

        $(document).on('click', '[data-save="modal"]', function (event) {
            event.preventDefault();

            var form = $(this).parents('.modal-body').find('form');
            var actionUrl = form.attr('action');
            var dataToSend = form.serialize();

            $.post(actionUrl, dataToSend).done(function (data) {
                var isValid = false;

                if (data && data.res && data.res == "OK")
                    isValid = true;

                if (isValid) {
                    $('#modDialog').modal('hide');
                    $('#dialogContent').empty();
                    reload();
                }
                else {
                    var newBody = $('.modal-body', data);
                    $(document).find('.modal-body').replaceWith(newBody);
                }
            });
        });

}


function listSearchExamplesScript() {

    var value = $("#SearchFieldId").val();

    $.ajax({
        type: 'GET',
        url: '/Pizzas/AjaxSearchList',
        data: { searchString: value }
    })
        .done(function (result) {
            $("#SuggestOutput").html(result);
            $("#PizzaSummaryId").remove();
        })

        .fail(function (xhr, status, error) {
            $("#SuggestOutput").text("No matches where found.");
        });
}
