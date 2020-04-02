window.MenuCtrl = {
    BtnBack: function () {
        location.href = appPath + "Manager/MenuManager";
    },

    BtnSave: function (id) {
        var param = {
            MenuID: id,
            MenuName: $('#txtMenuName').val(),
            FatherID: $('#txtFatherId').val(),
            UrlRedirect: $('#txtUrlRedirect').val(),
            Status: $("#cbxIsActive").is(":checked") ? 1 : 0
        }
        Utils.Loading();
        $.ajax({
            type: 'POST',
            url: appPath + 'Manager/MenuInsertUpdate',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: true,
            success: function (data) {
                Utils.UnLoading();
                if (data.ResponseCode >= 0) {
                    $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
                    $(".alert-success").show("slow");
                    $(".alert-danger").hide("slow");
                    setTimeout(function () { $(".alert-success").hide("slow"); location.href = appPath + "Manager/MenuManager"; }, 2000);
                }
                else {
                    $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
                    $("#ErrorMessage").text(data.Msg);
                    $(".alert-danger").show("slow");
                    $(".alert-success").hide("slow");
                    setTimeout(function () { $(".alert-danger").hide("slow"); }, 2000);
                }
            }
        });
    }
}

window.ArtCtrl = {
    GetList: function (hot, status, page) {
        Utils.Loading();
        $.ajax({
            type: 'GET',
            url: appPath + 'Manager/ArticleGetList',
            data: {
                title: $("#txtTitle").val(),
                menuId: $("#txtMenu").val(),
                hot: hot,
                status: status,
                fromdate: $("#txtFromDate").val(),
                todate: $("#txtToDate").val(),
                page: page == null ? 1 : page
            },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (data) {
                Utils.UnLoading();
                $("#ArticlePartial").html(data);
            }
        });
    },

    BtnBack: function () {
        window.location.href = appPath + "Manager/ArticleManager";
    },

    EditView: function () {
        //if ($("#cbxIsHot").is(":checked")) {
        //    $("#img_hot").css("display", "block");
        //};

        $("#cbxIsHot").change(function () {
            if ($("#cbxIsHot").is(":checked")) {
                $("#img_hot").css("display", "block");
            }
            else {
                $("#img_hot").css("display", "none");
            }
        });

        $("#txtMenuId").change(function () {
            var html = '<label for="txtTitle" class="control-label col-md-1">Link Redirect: </label>';
            html += '<div class="col-md-8"><div class="input-icon right">';
            html += '<input type="text" id="txtMetaData" name="txtMetaData" value="" class="form-control" placeholder="Link redirect của bài viết Ủng hộ trực tuyến" /></div></div>';

            var html2 = '<label for="txtTitle" class="control-label col-md-1">Link Youtube: </label>';
            html2 += '<div class="col-md-8"><div class="input-icon right">';
            html2 += '<input type="text" id="txtMetaData" name="txtMetaData" value="" class="form-control" placeholder="https://www.youtube.com/watch?v=c25WRUwrTIQ" /></div></div>';
            var check = $("#txtMenuId").val();
            if (check >= 9 && check <= 11) {
                $("#IsArticle").css("display", "none");
            }
            else {
                $("#IsArticle").css("display", "block");
            }

            if (check == 13 || check == 11) {

                if (check == 13) {
                    $(".meta_data_uhtt").html(html);
                }
                else {
                    $(".meta_data_uhtt").html(html2);
                }
                $(".meta_data_uhtt").css("display", "block");
            }
            else {
                $(".meta_data_uhtt").css("display", "none");
            }
        });

        //$("button.close").click(function () {
        //    $(this).parent().css("display", "none");
        //});

        $('#txtPublicTime').datetimepicker({
            //pickTime: false
            format: 'DD/MM/YYYY HH:mm:ss'
        });

        //$("#txtOrderSort").keypress(function (e) {
        //    //if the letter is not digit then display error and don't type anything
        //    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        //        //display error message
        //        $("#errmsg").html("Chỉ Nhập Được Số").show().fadeOut(1000);
        //        return false;
        //    }
        //});

        var editor = CKEDITOR.instances['txtContent'];
        if (editor) {
            CKEDITOR.remove(editor);
        }
        CKEDITOR.replace('txtContent', {
            //filebrowserBrowseUrl: appPath + 'ckfinder/ckfinder.html',
            //filebrowserImageBrowseUrl: appPath + 'ckfinder/ckfinder.html',
            //filebrowserUploadUrl: appPath + 'ckfinder/ckfinder.html',
            //filebrowserImageUploadUrl: appPath + 'ckfinder/ckfinder.html',
            //disallowedContent: 'img{width,height}',
            uiColor: '#77C0F1',
            extraPlugins: 'youtube,video', //colordialog,dialogadvtab,tableresize,
            //enterMode: CKEDITOR.ENTER_BR,
            //allowedContent: true,
            height: 600,
            width: 980
        });

        var editor2 = CKEDITOR.instances['txtBottomDes'];
        if (editor2) {
            CKEDITOR.remove(editor2);
        }
        CKEDITOR.replace('txtBottomDes', {
            uiColor: '#77C0F1',
            height: 150,
            width: 980
        });
    },

    ArtDetail: function (id) {
        var fromdate = $("#txtFromDate").val();
        var todate = $("#txtToDate").val();

        window.location.href = appPath + "Manager/ArticleDetail?ID=" + id;
    },

    BtnSave: function (id) {

        if ($("#txtDescription").val() == "") {
            $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
            $("#ErrorMessage").text("Bạn chưa nhập mô tả");
            $(".alert-danger").show("slow");
            $(".alert-success").hide("slow");
            setTimeout(function () { $(".alert-danger").hide("slow"); }, 2000);
            return;
        }

        if ($("#txtPublicTime").val() == "") {
            $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
            $("#ErrorMessage").text("Bạn chưa nhập thời gian Publish");
            $(".alert-danger").show("slow");
            $(".alert-success").hide("slow");
            setTimeout(function () { $(".alert-danger").hide("slow"); }, 2000);
            return;
        }
        var fromDateTimeArr = $("#txtPublicTime").val().split(' ');
        var fromDateArr = fromDateTimeArr[0].split('/');
        var startTime = fromDateArr[2] + "-" + fromDateArr[1] + "-" + fromDateArr[0] + " " + fromDateTimeArr[1];

        var param = {
            ArticleID: id,
            Title: $('#txtTitle').val(),
            MenuID: $('#txtMenuId').val(),
            Description: $("#txtDescription").val(),
            Detail: CKEDITOR.instances['txtContent'].getData(),
            Image: $('#txtImage1').val(),
            ImageDetail: $('#txtImage2').val(),
            ImageHot: $("#txtImage3").val(),
            BottomDesc: CKEDITOR.instances['txtBottomDes'].getData(),
            PublishDate: startTime,
            isHot: $("#cbxIsHot").is(":checked") ? 1 : 0,
            MetaData: $("#txtMetaData").val(),
            Tags: '',
            Author: $("#txtAuthor").val()
        }
        Utils.Loading();
        $.ajax({
            type: 'POST',
            url: appPath + 'Manager/ArticleInUp',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: true,
            success: function (data) {
                Utils.UnLoading();
                if (data.ResponseCode > 0) {
                    $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
                    $(".alert-success").show("slow");
                    $(".alert-danger").hide("slow");
                    console.log("MenuId = " + data.MenuId);
                    setTimeout(function () {
                        $(".alert-success").hide("slow");
                        if (data.MenuId == 10) {
                            location.href = appPath + "Manager/ImgManager?ArtId=" + data.ResponseCode;
                        }
                        else {
                            location.href = appPath + "Manager/ArticleManager";
                        }
                    }, 2000);
                }
                else {
                    $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
                    $("#ErrorMessage").text(data.Description);
                    $(".alert-danger").show("slow");
                    $(".alert-success").hide("slow");
                    setTimeout(function () { $(".alert-danger").hide("slow"); }, 2000);
                }
            }
        });
    },

    UpdateStatus: function (id, status, CurStatus) {
        CurStatus == null ? CurStatus = 0 : CurStatus;
        var text = "";
        if (status == 2) {
            text = "Bạn có chắc duyệt bài viết này";
        }
        else if (status == 3 && CurStatus == 4) {
            text = "Bạn có chắc chắn khôi phục bài viết này";
        }
        else if (status == 3 && CurStatus != 4) {
            text = "Bạn có chắc hạ bài viết này";
        }
        else if (status == 4) {
            text = "Bạn có chắc chắn xóa vĩnh viễn bài viết này";
        }
        bootbox.confirm(text, function (result) {
            if (result == true) {
                Utils.Loading();
                $.ajax({
                    type: 'POST',
                    url: appPath + 'Manager/ArtUpdateStatus',
                    data: {
                        ArtId: id,
                        Status: status
                    },
                    //contentType: "application/json; charset=utf-8",
                    //dataType: "html",
                    success: function (data) {
                        Utils.UnLoading();
                        if (data.ResponseCode > 0) {
                            bootbox.alert(data.Msg, function () {
                                location.reload();
                            });
                        }
                        else {
                            bootbox.alert(data.Msg);
                            return;
                        }
                    },
                    error: function () {
                        bootbox.alert("Hệ thống bận");
                    }
                });
            }
        }); 
    },

    UpdateOrderHot: function (id, type) {

        bootbox.confirm("Bạn có chắc thay đổi vị trí của bài viết", function (result) {
            if (result == true) {
                Utils.Loading();
                $.ajax({
                    type: 'POST',
                    url: appPath + 'Manager/ArtUpdateOrderHot',
                    data: {
                        ArtId: id,
                        type: type
                    },
                    //contentType: "application/json; charset=utf-8",
                    //dataType: "html",
                    success: function (data) {
                        Utils.UnLoading();
                        if (data.ResponseCode > 0) {
                            bootbox.alert(data.Msg, function () {
                                location.reload();
                            });
                        }
                        else {
                            bootbox.alert(data.Msg);
                            return;
                        }
                    },
                    error: function () {
                        bootbox.alert("Hệ thống bận");
                    }
                });
            }
        });
    }
}

window.AlbumCtrl = {
    MoreImg: function () {
        var total_img = $("#total_img").val();
        total_img++;
        var html = '<div class="form-group box_img' + total_img + '">';
        html += '<label id="labanh" for="txtImageUrl" class="control-label col-md-1">Ảnh ' + total_img + ' (750x420) *</label>';
        html += '<div class="col-md-8"><div class="input-icon right">';
        html += '<input type="text" id="txtImage' + total_img + '" name="txtImage" value="" class="form-control" placeholder="" /> </div> </div>';
        html += '<div class="col-md-1"><a href="javascript:;" onclick="upload_image(' + total_img + ')"><i class="fa fa-file"></i></a></div>';
        html += '<img src="" id="img_show' + total_img + '" style="max-width: 100px; max-height: 50px" /></div>';

        html += '<div class="form-group box_img' + total_img + '">';
        html += '<label for="txtTitle" class="control-label col-md-1">Chú thích:  </label><div class="col-md-8"><div class="input-icon right"><textarea id="txtDescription' + total_img + '" name="txtDescription" maxlength="150" placeholder="Tối đa 150 ký tự" class="form-control" rows="3"></textarea></div></div></div>';

        $(".list_img_album").append(html);
        $("#total_img").val(total_img);
    },

    BtnBack: function (id) {
        location.href = appPath + "Manager/ImgManager?ArtId=" + id;
    },

    RemoveImg: function () {
        var total_img = $("#total_img").val();
        if (total_img == 1) {
            $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
            $("#ErrorMessage").text("Album ảnh phải có ít nhất 1 ảnh");
            $(".alert-danger").show("slow");
            $(".alert-success").hide("slow");
            setTimeout(function () { $(".alert-danger").hide("slow"); }, 2000);
            return;
        }
        $(".box_img" + total_img).remove();
        total_img--;
        $("#total_img").val(total_img);
    },

    SaveImgAlbum: function (id) {
        var total_img = $("#total_img").val();
        var imgStr = "";
        for (var i = 1; i <= total_img; i++) {
            if ($("#txtImage" + i).val() == "") {
                $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
                $("#ErrorMessage").text("Bạn chưa nhập đường dẫn ảnh " + i + " trong Album");
                $(".alert-danger").show("slow");
                $(".alert-success").hide("slow");
                setTimeout(function () { $(".alert-danger").hide("slow"); }, 2000);
                return;
            }

            if ($("#txtDescription" + i).val().length > 150) {
                $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
                $("#ErrorMessage").text("Chú thích tại ảnh " + i + " không được quá 150 ký tự");
                $(".alert-danger").show("slow");
                $(".alert-success").hide("slow");
                setTimeout(function () { $(".alert-danger").hide("slow"); }, 2000);
                return;
            }

            imgStr += $("#txtImage" + i).val() + "," + $("#txtDescription" + i).val() + ";";
        }

        Utils.Loading();
        $.ajax({
            type: 'POST',
            url: appPath + 'Manager/AlbumAddImage',
            data: {
                artId: id,
                file_img: imgStr
            },
            //contentType: "application/json; charset=utf-8",
            //dataType: "html",
            success: function (data) {
                Utils.UnLoading();
                console.log(data);
                if (data.ResponseCode > 0) {
                    $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
                    //$("#ErrorMessage").text(data.Msg);
                    $(".alert-danger").hide("slow");
                    $(".alert-success").show("slow");
                    setTimeout(function () {
                        AlbumCtrl.BtnBack(id);
                    }, 2000);
                }
                else {
                    $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
                    $("#ErrorMessage").text(data.Msg);
                    $(".alert-danger").show("slow");
                    $(".alert-success").hide("slow");
                    setTimeout(function () { $(".alert-danger").hide("slow"); }, 2000);
                    return;
                }
            },
            error: function () {
                bootbox.alert("Hệ thống bận");
            }
        });
    },

    GetList: function (id, page) {
        Utils.Loading();
        $.ajax({
            type: 'GET',
            url: appPath + 'Manager/ImgGetList',
            data: {
                ArtId: id,
                toprow: $("#txtTopRow").val(),
                status: $("#txtStatus").val(),
                page: page == null ? 1 : page
            },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (data) {
                Utils.UnLoading();
                $("#list_data").html(data);
            }
        });
    },

    GetFormUpdate: function (ArtId, ImgId) {
        Utils.Loading();
        $.ajax({
            type: 'GET',
            url: appPath + 'Manager/ImgUpdate',
            data: {
                ArtId: ArtId,
                ImgId: ImgId
            },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (data) {
                Utils.UnLoading();
                $("BODY").css("overflow", "hidden");
                $("BODY").append(data);
            }
        });
    },

    RemoveFormUpdate: function () {
        $("#PopupUpdate").remove();
        $("#blackGround").remove();
        document.body.style.overflow = null;
    },

    SaveUpdateImg: function (id, status) {

        
        if(status == 0)
        {
            bootbox.confirm("Bạn có chắc bỏ ảnh này trong album", function (result) {
                if (result == true) {
                    AlbumCtrl.UpdateImgFunc(id, status, null, null);
                }
            });
        }
        else {
            if ($("#txtDes").val().length > 150) {
                $("#textError").html("Chú thích ảnh không được quá 150 ký tự");
                return;
            }
            AlbumCtrl.UpdateImgFunc(id, status, $("#img_update").val(), $("#txtDes").val());
        }
        
    },

    UpdateImgFunc: function (id, status, file_img, des) {
        
        Utils.Loading();
        $.ajax({
            type: 'POST',
            url: appPath + 'Manager/ImageSaveUpdate',
            data: {
                ImgId: id,
                file_img: file_img,
                Status: status,
                des: des
            },
            //contentType: "application/json; charset=utf-8",
            //dataType: "html",
            success: function (data) {
                Utils.UnLoading();
                console.log(data);
                if (data.ResponseCode > 0) {
                    AlbumCtrl.RemoveFormUpdate();
                    bootbox.alert("Cập nhật dữ liệu thành công!", function () {
                        location.reload();
                    });
                }
                else {
                    $("#textError").html(data.Msg);
                    return;
                }
            },
            error: function () {
                bootbox.alert("Hệ thống bận");
            }
        });
    }
}

window.EventCtrl = {
    GetListEvent: function (page) {
        Utils.Loading();
        $.ajax({
            type: 'GET',
            url: appPath + 'Manager/EventGetList',
            data: {
                eventName: $("#txtEventName").val(),
                status: 1,
                fromdate: $("#txtFromDate").val(),
                todate: $("#txtToDate").val(),
                page: page == null ? 1 : page
            },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (data) {
                Utils.UnLoading();
                $("#list_data").html(data);
            }
        });
    },

    BtnBack: function () {
        location.href = appPath + "Manager/EventRunning";
    },

    MoreSMS: function () {
        var TotalSMS = $("#TotalSMS").val();
        TotalSMS++;
        var htmlx = '<div id="FormSMS_' + TotalSMS + '">';

        htmlx += '<div class="form-group"><label for="txtTitle" class="control-label col-md-2">Cú pháp (*):  </label><div class="col-md-8"><div class="input-icon right"><input type="text" id="txtCommandcode_' + TotalSMS + '" name="txtCommandcode" value="" class="form-control" placeholder="Cú pháp" /></div></div></div>';

        $("#TotalSMS").val(TotalSMS);
        $("#SMSEvent").append(htmlx);
    },

    RemoveSMS: function () {
        var TotalSMS = $("#TotalSMS").val();
        if (TotalSMS == 1) {
            bootbox.alert("Bạn không thể bỏ bớt đầu số tin nhắn");
            return;
        }
        $("#FormSMS_" + TotalSMS).css("visiable", "hidden");
        $("#FormSMS_" + TotalSMS).remove();
        TotalSMS--;
        $("#TotalSMS").val(TotalSMS);
    },

    SaveEvent: function (id) {

        if ($("#txtStartTime").val() == "") {
            $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
            $("#ErrorMessage").text("Mời bạn nhập ngày bắt đầu chương trình");
            $(".alert-danger").show("slow");
            $(".alert-success").hide("slow");
            setTimeout(function () { $(".alert-danger").hide("slow"); }, 2000);
            return;
        } 
        var fromDateTimeArr = $("#txtStartTime").val().split(' ');
        var fromDateArr = fromDateTimeArr[0].split('/');
        var startTime = fromDateArr[2] + "-" + fromDateArr[1] + "-" + fromDateArr[0] + " " + fromDateTimeArr[1];

        if ($("#txtEndTime").val() == "") {
            $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
            $("#ErrorMessage").text("Mời bạn nhập ngày kết thúc chương trình");
            $(".alert-danger").show("slow");
            $(".alert-success").hide("slow");
            setTimeout(function () { $(".alert-danger").hide("slow"); }, 2000);
            return;
        } 
        var toDateTimeArr = $("#txtEndTime").val().split(' ');
        var toDateArr = toDateTimeArr[0].split('/');
        var endTime = toDateArr[2] + "-" + toDateArr[1] + "-" + toDateArr[0] + " " + toDateTimeArr[1];

        var TotalSMS = $("#TotalSMS").val();
        var detail = ""; 
        if ($("#txtPrice").val() == "") {
            $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
            $("#ErrorMessage").text("Bạn chưa nhập giá tiền của tin nhắn");
            $(".alert-danger").show("slow");
            $(".alert-success").hide("slow");
            setTimeout(function () { $(".alert-danger").hide("slow"); }, 2000);
            return;
        } 
        if ($("#txtPrice").val() < 0) {
            $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
            $("#ErrorMessage").text("Giá trị mỗi tin nhắn không được nhỏ hơn 0");
            $(".alert-danger").show("slow");
            $(".alert-success").hide("slow");
            setTimeout(function () { $(".alert-danger").hide("slow"); }, 2000);
            return;
        }

        for (var i = 1; i <= TotalSMS; i++) {
            if ($("#txtCommandcode_" + i).val() == "") {
                $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
                $("#ErrorMessage").text("Bạn chưa nhập cú pháp tin nhắn");
                $(".alert-danger").show("slow");
                $(".alert-success").hide("slow");
                setTimeout(function () { $(".alert-danger").hide("slow"); }, 2000);
                return;
            }
            if ($("#txtCommandcode_" + i).val().indexOf(",") > -1) {
                $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
                $("#ErrorMessage").text("Cú pháp tin nhắn không hợp lệ");
                $(".alert-danger").show("slow");
                $(".alert-success").hide("slow");
                setTimeout(function () { $(".alert-danger").hide("slow"); }, 2000);
                return;
            } 

            detail += $("#txtCommandcode_" + i).val() + "," + $("#txtServiceID").val() + "," + $("#txtPrice").val() + ";";
        }
         

        var param = {
            EventID: id,
            EventName: $('#txtEventName').val(),
            EventDetail: detail,
            StartDate: startTime,
            EndDate: endTime,
            Porpose: $('#txtPorpose').val(),
            HostUnit: $("#txtHostUnit").val(),
            CoordUnit: $("#txtCoordUnit").val(),
            LegalDoc: '',
            LegalFile: $("#txtLegalFile").val(),
            Revenue: $("#txtRevenue").val(),
            Result: $("#txtResult").val(),
            Image: $("#txtImage").val(),
            Detail: '',
            IsDone: $("#cbxIsDone").is(":checked") ? 1 : 0,
            IsSetted: $("#cbxIsSetted").is(":checked") ? 1 : 0,
            Status: 1,
        }
        Utils.Loading();
        $.ajax({
            type: 'POST',
            url: appPath + 'Manager/EventInUpFunction',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: true,
            success: function (data) {
                Utils.UnLoading();
                if (data.ResponseCode > 0) {
                    $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
                    $(".alert-success").show("slow");
                    $(".alert-danger").hide("slow");
                    setTimeout(function () {
                        $(".alert-success").hide("slow");
                        location.href = appPath + "Manager/EventRunning";
                    }, 2000);
                }
                else {
                    $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
                    $("#ErrorMessage").text(data.Msg);
                    $(".alert-danger").show("slow");
                    $(".alert-success").hide("slow");
                    setTimeout(function () { $(".alert-danger").hide("slow"); }, 2000);
                }
            }
        });
    },

    ListReport: function () {
        Utils.Loading();
        $.ajax({
            type: 'GET',
            url: appPath + 'Manager/ReportEventGetList',
            data: {
                eventId: $("#txtEventId").val(),
                fromdate: $("#txtFromDate").val(),
                todate: $("#txtToDate").val(),
            },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (data) {
                Utils.UnLoading();
                $("#list_data").html(data);
            }
        });
    },

    RemoveEvent: function (id) {
        var param = {
            EventID: id
        }
        Utils.Loading();
        $.ajax({
            type: 'POST',
            url: appPath + 'Manager/EventRemove',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: true,
            success: function (data) {
                Utils.UnLoading();
                if (data.ResponseCode > 0) {
                  
                    bootbox.alert("Xóa thành công", function () {
                        location.reload();
                    });
                }
                else {
                    bootbox.alert(data.Msg);
                    return;
                }
            }
        });
    },

    ListEventRunning: function (page) {
        Utils.Loading();
        $.ajax({
            type: 'GET',
            url: appPath + 'Manager/EventListRunning',
            data: {
                eventName: $("#txtEventName").val(),
                page: page == null ? 1 : page
            },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (data) {
                Utils.UnLoading();
                $("#list_data").html(data);
            }
        });
    },

    ListEventDone: function (year, page) {
        Utils.Loading();
        $.ajax({
            type: 'GET',
            url: appPath + 'Manager/EventListDone',
            data: {
                eventName: $("#txtEventName").val(),
                year: year,
                page: page == null ? 1 : page
            },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (data) {
                Utils.UnLoading();
                $("#list_data").html(data);
            }
        });
    },
}

window.SuggetCtrl = {
    GetList: function (page) {
        Utils.Loading();
        $.ajax({
            type: 'GET',
            url: appPath + 'Manager/SuggestGetList',
            data: {
                mobile: $("#txtMobile").val(),
                mail: $("#txtEmail").val(),
                status: $("#txtStatus").val(),
                fromdate: $("#txtFromDate").val(),
                todate: $("#txtToDate").val(),
                page: page == null ? 1 : page
            },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (data) {
                Utils.UnLoading();
                $("#list_data").html(data);
            }
        });
    },

    BtnBack: function(){
        location.href = appPath + "Manager/SuggestManager";
    },

    SendMail: function (id) {

        var param = {
            suggetid: id,
            from_email: $("#txtEmailSend").val(),
            password: $("#txtPassword").val(),
            recive_mail: $("#txtEmailReceive").val(),
            cc_email: $("#txtEmailCc").val(),
            subject: $("#txtSubject").val(),
            mailcontent: CKEDITOR.instances['txtContent'].getData(),
        }

        Utils.Loading();
        $.ajax({
            type: "POST",
            url: appPath + 'Manager/SuggestSendMail',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: true,
            data: JSON.stringify(param),
            success: function (data) {
                Utils.UnLoading();
                if (data.Response == -9) {
                    $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
                    $("#ErrorMessage").text(data.msg);
                    $(".alert-danger").show("slow");
                    $(".alert-success").hide("slow");
                    setTimeout(function () {
                        $(".alert-danger").hide("slow");
                        window.location.href = data.url;
                    }, 2000);

                }
                else if (data.Response <= 0) {
                    $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
                    $("#ErrorMessage").text(data.msg);
                    $(".alert-danger").show("slow");
                    $(".alert-success").hide("slow");
                    setTimeout(function () {
                        $(".alert-danger").hide("slow");
                    }, 2000);
                }
                else {
                    $("html,body").animate({ scrollTop: $('#TemplateContent').offset().top }, 'fast');
                    $(".alert-success").show("slow");
                    $(".alert-danger").hide("slow");
                    setTimeout(function () {
                        $(".alert-success").hide("slow");
                        location.href = appPath + "Manager/SuggestManager";
                    }, 2000);
                }
            }
        });
    },

    GetContentAns: function (id) {
        Utils.Loading();
        $.ajax({
            type: 'GET',
            url: appPath + 'Manager/SuggestViewAnswer',
            data: {
                ID: id,
            },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (data) {
                Utils.UnLoading();
                $("BODY").css("overflow", "hidden");
                $("BODY").append(data);
            }
        });
    },

    RemovePopup: function () {
        $("#PopupForm").remove();
        $("#blackGround").remove();
        document.body.style.overflow = null;
    },

    RemoveAns: function (id) {
        Utils.Loading();
        $.ajax({
            //dataType: "text",
            type: "POST",
            url: appPath + 'Manager/SuggestNotAns',
            data: {
                suggetid: id, 
            },
            success: function (data) {
                Utils.UnLoading();
                if (data.Response > 0) {
                    location.reload();
                }
                else {
                    bootbox.alert(data.msg, function () {
                        return;
                    });
                } 
            }
        });
    }
}