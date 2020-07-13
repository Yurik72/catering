
var html_loading_element = '<div class="spinner-container"> <div>Loading</div><div class="spinner-border" role="status"> <span class="sr-only">Loading...</span> </div></div>';

function setup_listitems(options) {
    let defaultoptions = { href: '#', onloadedcb: undefined, method:"ListItems",editmethod:"EditModal",createmethod:"CreateModal"};
    if (typeof (options) == 'object') {
       // this.options = { ...defaultoptions, ...options };
        //edge troubles with es6
        this.options = defaultoptions
        this.options.href = options.href;
        if (options.onloadedcb)
            this.options.onloadedcb = options.onloadedcb;
        if (options.method)
            this.options.method = options.method;
        if (options.editmethod)
            this.options.editmethod = options.editmethod;
        if (options.createmethod)
            this.options.createmethod = options.createmethod;
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
            var url = self.options.href + '/' + self.options.createmethod;
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
                //if (!confirm("There are unsaved changes, please confirm. your input will be lost"))
               if (!confirm(get_text_res().confirm_close))
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
                    $('#modDialog').attr("_changed", false);
                    $('#modDialog').modal('hide');
                    $('#dialogContent').empty();
                    reload();
                }
                else {
                    var newBody = $('.modal-body', data);
                    $(document).find('.modal-body').replaceWith(newBody);
                    setupChangesChecker($('#modDialog'));
                }
            })
            .fail(function (xhr, status, error) {
                alert("ERROR !" + error);
             });
        });

}


function setup_search(_options) {
    let defaultoptions = { href: '#', onloadedcb: undefined, method: "ListItems",itemselector:"a" ,classname:"",multiselect:false,onitemselectedcb:undefined};
    this.options = {};
    if (typeof (_options) == 'object') {
        // this.options = { ...defaultoptions, ...options };
        //edge troubles with es6
        this.options = defaultoptions
        this.options.href = _options.href;
        if (_options.onloadedcb)
            this.options.onloadedcb = _options.onloadedcb;
        if (_options.method)
            this.options.method = _options.method;
        if (_options.multiselect)
            this.options.multiselect = _options.multiselect;
        if (_options.onitemselectedcb)
            this.options.onitemselectedcb = _options.onitemselectedcb;
        this.options.itemselector = _options.itemselector;
        this.options.classname = _options.classname;
        this.options.dialog_id = `#modDialog${this.options.classname}line`
    }
    else {

        this.options = defaultoptions;
        this.options.href = _options;
    }
    var self = this;
    function delay(callback, ms) {
        var timer = 0;
        return function () {
            var context = this, args = arguments;
            clearTimeout(timer);
            timer = setTimeout(function () {
                callback.apply(context, args);
            }, ms || 0);
        };
    };
    $(document).on('click', this.options.itemselector, function (event) {
        event.preventDefault();
        var source_item=this;
        var dlg = $(self.options.dialog_id);
        var url = self.options.href;
        dlg.find(".modal-body").html(html_loading_element);
        //$('#modDialog').modal('hide');
        dlg.modal('show');
        $.get(url, function (data) {
            dlg.find(".modal-body").html(data);

            dlg.modal('handleUpdate');
            setupSearchHandlers(dlg, source_item , function () {
                //  $('#modDialog').modal('show');

            });
        });
        $('#search-' + self.options.classname).keyup(delay(function (e) {
            dlg.find(".modal-body").html(html_loading_element);
            var url_search = url + `?SearchCriteria=${this.value}`;
            $.get(url_search, function (data) {
                dlg.find(".modal-body").html(data);

                dlg.modal('handleUpdate');
                setupSearchHandlers(dlg, source_item, function () {
                });
            });
        }));
    });
    function setupSearchHandlers(dlg, src, closecb) {
        // $(dlg).find('.add-item').click(function (e) {
        $(dlg).find('.table.search-items tr').click(function (e) {
            if (self.options.onitemselectedcb)
                self.options.onitemselectedcb(src,this, { id: $(this).attr("data-id"), name: $(this).attr("data-name")});
            if (!self.options.multiselect) {
                $(dlg).modal('hide');
                dlg.find(".modal-body").empty();
                if (closecb) {
                    closecb();
                }
            }
        });

    }
}