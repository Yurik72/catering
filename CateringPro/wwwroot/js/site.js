﻿
var html_loading_element = '<div class="spinner-container"> <div>Loading</div><div class="spinner-border" role="status"> <span class="sr-only">Loading...</span> </div></div>';

$(function () {
    
    setup_change_company();
    setup_changechield();
});
function gethtmlloading() {
    return html_loading_element;
}
function setup_changechield() {
    $("#changechield").click(function (e) {
        var url = '/Account/UserChilds';
        var self = this;
        $.get(url, function (data) {
            var dialog = $(data);
            $("body").append(dialog);// $('#moddialogyesno');
            // var dialog = $('#moddialogyesno').dialog();
            dialog.modal('show');
            dialog.on('hide.bs.modal', function (e) {
                dialog.empty();

            });
            dialog.find(".role-item").click(function (e) {
                e.preventDefault();
                if (!$(this).hasClass('active')) {
                    dialog.find(".role-item").removeClass('active');
                    $(this).addClass('active')
                }
            });
            dialog.find("#selectchild").click(function (e) {
                e.preventDefault();
                var userid = dialog.find(".role-item.active").attr("data-id");
                var token = dialog.find("[name='__RequestVerificationToken'").val();
                
                $.ajax({
                    type: "POST",
                    data: { UserId: userid, __RequestVerificationToken: token },

                    url: "/Account/ChangeUserChild",
                    success: function (data) {
                        dialog.modal('hide');
                        var getUrl = window.location;
                        var baseUrl = getUrl.protocol + "//" + getUrl.host;
                        window.location.href = baseUrl;
                    }
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    dialog_error(errorThrown);
                });

            });
           
        })
            .fail(function (xhr, status, error) {

            });
    });
}
function setup_change_company() {
    var ul_select_company = '<ul class="nav navbar-nav navbar-right show"> <a id="selectcompanyloaded" class="nav-link dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">.....</a><li class="nav-item dropdown"><div class="dropdown-menu show" id="companylist"></div></li></ul>';
    $("#selectcompany").click(function (e) {
        e.preventDefault();
        var url = '/Account/UserOtherCompanies';
        var self = this;
        $.get(url, function (data) {
            var ul = $(ul_select_company);
            if ($.isArray(data) && data.length > 0) {
                data.map(function (el) {
                    ul.find("#companylist").append(`<a class="setcompany dropdown-item" data-id="${el.id}" href="#">${el.name}</a>`)
                });
                ul.find("#selectcompanyloaded").text($(self).text());
                $(self).before(ul);
                $(self).empty();
                setupcompanychanges(ul);
            }

        });

    });
    function setupcompanychanges(menu) {
        menu.find(".setcompany").click(function (e) {
            e.preventDefault();
            var url = '/Account/SetCompanyId?CompanyId=")' + $(this).attr("data-id");
            $.get(url, function (data) {
                //var x = data;
            }).done(function () {
                //alert("second success");
                location.reload();
            })
                .fail(function () {
                    //alert("error");
                });
        });

    }
}
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
       self.reload = reload;
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
               e.preventDefault();
               dialog_yes_no($.text_resource.confirm_close,
                   function () { //yes
                       $(dlg).attr("_changed", false);
                       $('#modDialog').modal('hide');
                   },
                   function () { //no
                   });
               return;
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
    return self;
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

function dialog_yes_no(message, yesCallback, noCallback) {

    var dlg_html = '   <div id="moddialogyesno" class="modal" tabindex="-1" role="dialog">  ' +
        '       <div class="modal-dialog modal-sm modal-alert" role="document">  ' +
        '           <div class="modal-content">  ' +
        '               <div class="modal-header">  ' +
        '                   <h5 class="modal-title"><i class="fa fa-exclamation-circle fa-3" style="color:red" aria-hidden="true"></i>  ' + $.text_resource.confirm_title+'</h5>  ' +
        '                   <button type="button" class="close" data-dismiss="modal" aria-label="Close">  ' +
        '                       <span aria-hidden="true">X</span>  ' +
        '                   </button>  ' +
        '               </div>  ' +
        '               <div class="modal-body">  ' +
        '               <p>' + message+'</p>'+
        '               </div>  ' +
        '               <div class="modal-footer">  ' +
        '     ' +
        '                   <button id="btnyes" type="button" class="btn btn-primary" >' + $.text_resource.yes+'</button>  ' +
        '                   <button id="btnno" type="button" class="btn btn-secondary" >' + $.text_resource.no +'</button>  ' +
        '               </div>  ' +
        '           </div>  ' +
        '       </div>  ' +
        '  </div>  ';
   // $("body").append(dlg_html);
    var dialog = $(dlg_html);
    $("body").append(dialog);// $('#moddialogyesno');
   // var dialog = $('#moddialogyesno').dialog();
    dialog.modal('show');
    dialog.find('#btnyes').click(function () {
        dialog.modal('hide');
        //dialog.empty();
        if (yesCallback)
             yesCallback();
    });

    dialog.find('#btnno').click(function () {
        dialog.modal('hide');
        //dialog.empty();
        if (noCallback)
            noCallback();
    });
    dialog.on('hide.bs.modal', function (e) {
        dialog.empty();
       
    });

}

function dialog_error(message) {

    var dlg_html = '   <div id="moddialogerr" class="modal" tabindex="-1" role="dialog">  ' +
        '       <div class="modal-dialog modal-lg modal-alert" role="document">  ' +
        '           <div class="modal-content">  ' +
        '               <div class="modal-header">  ' +
        '                   <h5 class="modal-title"><i class="fa fa-exclamation-circle fa-3" style="color:red" aria-hidden="true"></i>  ' + $.text_resource.error + '</h5>  ' +
        '                   <button type="button" class="close" data-dismiss="modal" aria-label="Close">  ' +
        '                       <span aria-hidden="true">X</span>  ' +
        '                   </button>  ' +
        '               </div>  ' +
        '               <div class="modal-body">  ' +
        '               <p>' + message + '</p>' +
        '               </div>  ' +
        '               <div class="modal-footer">  ' +
        '     ' +
        '                   <button id="btnyes" type="button" class="btn btn-primary" >' + $.text_resource.yes + '</button>  ' +
        '               </div>  ' +
        '           </div>  ' +
        '       </div>  ' +
        '  </div>  ';
    // $("body").append(dlg_html);
    var dialog = $(dlg_html);
    $("body").append(dialog);// $('#moddialogyesno');
    // var dialog = $('#moddialogyesno').dialog();
    dialog.modal('show');
    dialog.find('#btnyes').click(function () {
        dialog.modal('hide');

    });


    dialog.on('hide.bs.modal', function (e) {
        dialog.empty();

    });
}