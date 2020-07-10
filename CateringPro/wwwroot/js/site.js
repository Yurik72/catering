
function setup_listitems(options) {
    let defaultoptions = { href: '#', onloadedcb: undefined, method:"ListItems"};
    if (typeof (options) == 'object') {
       // this.options = { ...defaultoptions, ...options };
        //edge troubles with es6
        this.options = defaultoptions
        this.options.href = options.href;
        if (options.onloadedcb)
            this.options.onloadedcb = options.onloadedcb;
        if (options.method)
            this.options.method = options.method;
    }
    else {

        this.options = defaultoptions;
        this.options.href = options;
    }
   
    var self = this; 
        var reload = function (href) {
            if (!href)
                href = self.options.href + `/${self.options.method}?`;
            else {
                href += "&";
                href = href.replace("ListItems", self.options.method);
                
            }
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
    function setupChangesChecker(dlg) {
        $(dlg).attr("_changed", false);
        $(dlg).find("input,textarea").change(function () {
            $(dlg).attr("_changed", true);
        });
        $(dlg).on('hide.bs.modal', function (e) {
           if ($(dlg).attr("_changed")=="true") {
                if (!confirm("There are unsaved changes, please confirm. your input will be lost"))
                   e.preventDefault();
               $(dlg).attr("_changed", false);
           }
        }); 

    }
     $(document).on("click", "a.cmd-edit", function (e) {
            e.preventDefault();
            $.get(this.href, function (data) {

                $('#dialogContent').html(data);
                $('#modDialog').attr("data-backdrop", false);
                $('#modDialog').css("background-color", "rgba(117, 117, 117, 0.5)");
                $('#modDialog').modal('show');
                if (self.options.onloadedcb)
                    self.options.onloadedcb();
                setupChangesChecker($('#modDialog'));
            });
        });
    $(document).on("click", "a.cmd-delete", function (e) {
        e.preventDefault();
        $.get(this.href, function (data) {

            $('#dialogContent').html(data);
            $('#modDialog').modal('show');
            //if (self.options.onloadedcb)
           //     self.options.onloadedcb();
        }).fail(function (xhr, status, error) {
            alert("ERROR !" + error);
        });
    });
    $(document).on('click', '[data-action="modal"]', function (event) {
            event.preventDefault();

            var form = $(this).parents('.modal-body').find('form');
            var actionUrl = $(event.target).attr('action');
            var dataToSend = form.serialize();

            $.post(actionUrl, dataToSend).done(function (data) {
                var isValid = false;
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
            })
            .fail(function (xhr, status, error) {
                alert("ERROR !" + error);
             });
        });

}


