function UrlRoot() {
    var UrlRoot = appPath;
    return UrlRoot;
}

function MediaUrl() {
    var mediaUrl = appPath;
    return mediaUrl;
}
// ReSharper disable once ThisInGlobalContext
var UrlRoot = this.UrlRoot();

function ResetTextBox() {
    $("input:text").val("");
}
///----------------chức năng bộ phận người dùng

// danh sách tree Business
function ListBusinessTree() {
    location.href = UrlRoot + "Business/ManagerBusiness";
};


// Lấy thông tin UserMerchant ( Bộ Phân người dùng)
function ViewUserMerchantData(id) {
    Utils.Loading();
    $.ajax({
        type: 'GET',
        url: UrlRoot + "Home/GetUserMerchantInfo",
        data: {
            'id': id
        },
        success: function (data) {
            Utils.UnLoading();
            $("#TemplateContent").html(data);
            $("html,body").animate({ scrollTop: 0 }, 'fast');
        }
    });
}




///---------------Chức Năng User
//Danh sách User
function ListUsers() {
    location.href = UrlRoot + "Home/ManagerUser";
};

//Danh sách Phí chuyển khoản
function ListTransferFee() {
    location.href = UrlRoot + "AdmBusinessPolicy/TransferFee";
};

// Danh sách thương nhân
function ListBusiness() {
    location.href = UrlRoot + "Business/Business";
};


// Lấy thông tin User
function ViewUserData(id) {
    Utils.Loading();
    $.ajax({
        type: 'GET',
        url: UrlRoot + "Home/GetUserInfo",
        data: {
            'id': id
        },
        success: function (data) {
            Utils.UnLoading();
            $("#TemplateContent").html(data);
            $("html,body").animate({ scrollTop: 0 }, 'fast');
        }
    });
}
//Update Status User
function UpdateStatusUser(id, status) {
    var type = "Vô hiệu hóa";
    if (status == 0) {
        type = "Kích hoạt";
    }
    bootbox.confirm("Bạn muốn " + type + " Tài khoản này ?", function (conf) {
        if (conf == 1) {
            var urlUpstatus = UrlRoot + "Home/UpdateActiveUser";
            Utils.Loading();
            $.ajax({
                type: 'POST',
                url: urlUpstatus,
                data: { id: id },
                success: function (data) {
                    Utils.UnLoading();
                    if (data.ResponseCode >= 0) {
                        bootbox.alert(data.Description);
                        setTimeout(function () {
                            ListUsers();
                        }, 1000);
                    }
                    else {
                        bootbox.alert(data.Description);
                        return;
                    }
                },
                error: function () {
                    Utils.UnLoading();
                }
            });
        }
        return;
    });
};

///-------------End Chức năng User



///------------- Chức năng Hệ Thống

//Chuc nang
function LoadFunction(url) {
    Utils.Loading();
    $.ajax({
        type: 'GET',
        url: UrlRoot + url,
        data: {
        },
        success: function (data) {
            Utils.UnLoading();
            $("#TemplateContent").html(data);
            $("html,body").animate({ scrollTop: 0 }, 'fast');
        }
    });
}
//Danh sách ứng dụng
function ListFunction() {
    location.href = UrlRoot + "Home/ManagerFunction";
};
//Sắp xếp chức năng
function FunctionOrder() {
    location.href = UrlRoot + "Home/FunctionOrder";
};
// View Detail Function
function ViewDataFunction(id) {
    Utils.Loading();
    $.ajax({
        type: 'GET',
        url: UrlRoot + "Home/GetFunctionInfo",
        data: {
            'id': id
        },
        success: function (data) {
            Utils.UnLoading();
            $("#TemplateContent").html(data);
            $("html,body").animate({ scrollTop: 0 }, 'fast');
        }
    });
}

/// ------------- End Chức năng Hệ thống





///------------- Chức năng Bộ phận ( MerChant)

//Danh sách Bộ Phận ( Merchant )
function ListMerchant() {
    location.href = UrlRoot + "Merchants/ManagerMerchant";
};

// Lấy Thông Tin Merchant
function ViewMerchantData(MerchantId) {
    //debugger;
    Utils.Loading();
    $.ajax({
        type: 'GET',
        url: UrlRoot + "Merchants/GetinfoMerchant",
        data: {
            'MerchantId': MerchantId
        },
        success: function (data) {
            Utils.UnLoading();
            $("#TemplateContent").html(data);
            $("html,body").animate({ scrollTop: 0 }, 'fast');
        }
    });
}

//Update Status Merchant ( Bộ Phận )
function UpdateStatusMerchant(MerchantId, status) {
    var type = "Vô hiệu hóa";
    if (status == 0) {
        type = "Kích hoạt";
    }
    bootbox.confirm("Bạn muốn " + type + " Bộ Phận này ?", function (conf) {
        if (conf == 1) {
            var urlUpstatus = UrlRoot + "Merchants/UpdateStatusMerchant";
            Utils.Loading();
            $.ajax({
                type: 'POST',
                url: urlUpstatus,
                data: { MerchantId: MerchantId },
                success: function (data) {
                    Utils.UnLoading();
                    if (data.ResponseCode >= 0) {
                        bootbox.alert(data.Description);
                        setTimeout(function () {
                            ListMerchant();
                        }, 1000);
                    }
                    else {
                        bootbox.alert(data.Description);
                        return;
                    }
                },
                error: function () {
                    Utils.UnLoading();
                }
            });
        }
        return;
    });
};

/// ----------- End Chức năng Bộ Phận



/// ----------- Dịch vụ - Service

//Danh sách dịch vụ
function ListService() {
    location.href = UrlRoot + "Services/ManagerService";
};
function ListServiceCondition(group, merchant, status) {
    param = {
        groupService: group,
        merchantID: merchant,
        status: status
    };
    var urlRequestAns = UrlRoot + "Services/ManagerService";
    Utils.Loading();
    $.ajax({
        type: 'POST',
        url: urlRequestAns,
        data: param,
        async :true,
        success: function (data) {
            $("#TemplateContent").html(data);
            $("html,body").animate({ scrollTop: 0 }, 'fast');
            Utils.UnLoading();
        },
        error: function () {
            alert("error load fail !");
            Utils.UnLoading();
        }
    });
}
//Update Status Service
function UpdateStatusService(id, status) {
    var type = "vô hiệu hóa";
    if (status == 0) {
        type = "kích hoạt";
    }
    bootbox.confirm("Bạn muốn " + type + " trạng thái dịch vụ ?", function (conf) {
        if (conf == 1) {
            var urlUpstatus = UrlRoot + "Home/UpdateActiveService";
            Utils.Loading();
            $.ajax({
                type: 'GET',
                url: urlUpstatus,
                data: { id: id },
                success: function (data) {
                    Utils.UnLoading();
                    if (data.status == 1) {
                        bootbox.alert(data.msg);
                        ListService();
                    }
                    else {
                        bootbox.alert(data.msg);
                        return;
                    }
                },
                error: function () {
                    Utils.UnLoading();
                }
            });
        }
        return;
    });

};

/// ---------- End Dịch vụ - Service



/// -----------Log Hệ thống

//Danh sách ErrorLog
function ListErrorLog() {
    Utils.Loading();
    $.ajax({
        type: 'GET',
        url: UrlRoot + "Home/ManagerErrorLog",
        data: {
        },
        success: function (data) {
            Utils.UnLoading();
            $("#TemplateContent").html(data);
            $("html,body").animate({ scrollTop: 0 }, 'fast');
        }
    });
};

// -----------End Hệ thống



// Lấy Thông tin Service
function ViewServiceData(ServiceId) {
    var param = {
        ServiceId: ServiceId,
        GroupService: 0,
        filterGroup: $("#ddlgroupService").val(),
        filterMerchant: $("#ddlmerchantId").val(),
        filterStatus: $('#ddlActive').val()
    };
    Utils.Loading();
    $.ajax({
        type: 'GET',
        url: UrlRoot + "Services/GetinfoService",
        data: param,
        success: function (data) {
            Utils.UnLoading();
            $("#TemplateContent").html(data);
            $("html,body").animate({ scrollTop: 0 }, 'fast');
        }
    });
}
// Lấy thông tin dịch vụ
function ViewDataService(id) {
    Utils.Loading();
    $.ajax({
        type: 'GET',
        url: UrlRoot + "Home/GetServiceInfo",
        data: {
            'id': id
        },
        success: function (data) {
            Utils.UnLoading();
            $("#TemplateContent").html(data);
            $("html,body").animate({ scrollTop: 0 }, 'fast');
        }
    });
}

// View Phân quyền người dùng
function ViewGrantUser(uid) {
    Utils.Loading();
    $.ajax({
        type: 'GET',
        url: UrlRoot + "Home/GetGrantUser",
        data: {
            'userid': uid
        },
        success: function (data) {
            Utils.UnLoading();
            $("#TemplateContent").html(data);
            $("html,body").animate({ scrollTop: 0 }, 'fast');
        }
    });
}
// View Phân Quyền Nhóm Người Dùng
function GetGrantGroups(GroupID) {
    Utils.Loading();
    $.ajax({
        type: 'GET',
        url: UrlRoot + "Home/GetGrantGroups",
        data: {
            'GroupID': GroupID
        },
        success: function (data) {
            Utils.UnLoading();
            $("#TemplateContent").html(data);
            $("html,body").animate({ scrollTop: 0 }, 'fast');
        }
    });
}
// View Phân quyền dịch vụ
function ViewGrantSerive(uid) {
    Utils.Loading();
    $.ajax({
        type: 'GET',
        url: UrlRoot + "Home/GetGrantService",
        data: {
            'userid': uid
        },
        success: function (data) {
            Utils.UnLoading();
            $("#TemplateContent").html(data);
            $("html,body").animate({ scrollTop: 0 }, 'fast');
        }
    });
}

function ViewReportBalance() {

    Utils.Loading();
    var urlRequestAns = UrlRoot + "ReportStatic/GeneralBalanceReport";
    $.ajax({
        type: 'GET',
        url: urlRequestAns,
        success: function (data) {
            $("#ReportBalance").html(data);
            Utils.UnLoading();
        },
        error: function () {
            Utils.UnLoading();
            bootbox.alert("Hệ thống đang bận. Xin thử lại sau !");
        }
    });
};

function ViewGrantSource(uid) {
    Utils.Loading();
    $.ajax({
        type: 'GET',
        url: UrlRoot + "Home/ViewGrantSource",
        data: {
            'userid': uid
        },
        success: function (data) {
            Utils.UnLoading();
            $("#TemplateContent").html(data);
            $("html,body").animate({ scrollTop: 0 }, 'fast');
        }
    });
}
function ViewChangePass() {
    Utils.Loading();
    $.ajax({
        type: 'GET',
        url: UrlRoot + "Home/ChangePassword",
        data: {},
        success: function (data) {
            Utils.UnLoading();
            $("#changepass").html(data);
        }
    });
}
function ResetTextBox() {
    $("input:text").val("");
}




function formatlabel(num) {
    var isNegative = false;
    var formattedNumber;
    if (num < 0) {
        isNegative = true;
    }
    num = Math.abs(num)
    if (num >= 1000000000) {
        formattedNumber = (num / 1000000000).toFixed(2).replace(/\.0$/, '') + ' Tỷ';
    } else if (num >= 1000000) {
        formattedNumber = (num / 1000000).toFixed(2).replace(/\.0$/, '') + 'm';
    } else if (num >= 1000) {
        formattedNumber = (num / 1000).toFixed(2).replace(/\.0$/, '') + 'k';
    } else {
        formattedNumber = num;
    }
    if (isNegative) { formattedNumber = '-' + formattedNumber; }
    return formattedNumber;
}

function legendFormatter(label, series) {
    return "<div style='font-size:8pt; text-align:center; padding:2px; color:white;'>" + label + "<br/>" + Math.round(series.percent) + "%</div>";
}
function labelFormatter(label, series) {
    var IndexToMonth = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var month = IndexToMonth[new Date(label).getMonth()];
    var year = new Date(label).getFullYear();
    return "<div style='font-size:8pt; text-align:center; padding:2px; color:white;'>" + year + ' ' + month + "<br/>" + Math.round(series.percent) + "% </div>";
}
function legendLabel(label) {
    var IndexToMonth = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var month = IndexToMonth[new Date(label).getMonth()];
    var year = new Date(label).getFullYear();
    return "<div style='font-size:8pt;'>" + year + ' ' + month + "</div>";
}

function recursive(bool, data, inputCheckAll, inputCheck) {
    if (data == null || data == undefined || data.length == 0)
        return;
    if (bool == null || bool == undefined)
        bool = true;
    $.each(data, function () {
        if ($(this).treegrid('isLeaf')) {
            if (bool) {
                $(this).find('input.' + inputCheck).prop('checked', true);
                $(this).find('input.IsView').prop('checked', true);
            }
            else {
                $(this).find('input.' + inputCheck).prop('checked', false);
            }
        }
        else {
            var item = $(this).find('input.' + inputCheckAll);
            if (bool) {
                item.prop('checked', true);
                $(this).find('input.CheckAllView').prop('checked', true);
            }
            else {
                item.prop('checked', false);
            }
            var id = $(this).find('input.' + inputCheckAll).attr('id');
            var listchild = $('.treegrid-' + id).treegrid('getChildNodes');
            recursive(bool, listchild, inputCheckAll, inputCheck);
        }
    });
}
function parentCheck($parent, $check, inputCheckAll, inputCheck) {
    if ($parent == null || $parent == undefined)
        return;
    var parent = $parent.treegrid('getParentNode');

    var id = $parent.treegrid('getNodeId');
    var parentId = $('.treegrid-' + id).treegrid('getParentNodeId');
    if (!$check) {
        $parent.find(inputCheckAll).prop('checked', false);
        if (parent != null && parent != undefined)
            parentCheck(parent, $check, inputCheckAll, inputCheck);
        else return;
    }
    else {
        var check = true;
        var children = $parent.treegrid('getChildNodes');
        $.each(children, function () {
            var $this = $(this);
            var checkChild = false;
            if ($this.treegrid('isLeaf')) {
                checkChild = $this.find(inputCheck).prop('checked');
            }
            else {
                checkChild = $this.find(inputCheckAll).prop('checked');
            }
            if (!checkChild) {
                check = false;
            }
        });
        $parent.find(inputCheckAll).prop('checked', check);
        if (parent != null && parent != undefined)
            parentCheck(parent, $check, inputCheckAll, inputCheck);
        else return;
    }
}


function rowspanDTable(oSettings,divId) {
    if (oSettings.aiDisplay.length <= 0)
        return;
    $("#"+divId+" tr td").removeAttr("hidden").removeAttr("rowspan");

    for (i = 0; i < oSettings.nTBody.childElementCount; i++) {
        for (j = 0; j < oSettings.nTBody.rows[i].childElementCount; j++) {
            var count = 1;
            var cellIndex = parseInt(oSettings.nTBody.rows[i].cells[j]._DT_CellIndex.column);

            if (oSettings.aoColumns[cellIndex].sType != "num" && oSettings.aoColumns[cellIndex].sType != "num-fmt") {
                if (i == 0) {
                    for (index = i + 1; index < oSettings.nTBody.childElementCount; index++) {
                        if (oSettings.nTBody.rows[i].cells[j].textContent != oSettings.nTBody.rows[index].cells[j].textContent)
                            break;
                        count++;
                    }
                    if (count > 1) {
                        $('#'+divId+' tbody tr:eq(' + i + ') td:eq(' + j + ')').attr("rowspan", count);
                    }
                }
                else {
                    if (oSettings.nTBody.rows[i - 1].cells[j].textContent == oSettings.nTBody.rows[i].cells[j].textContent)
                        $('#' + divId + ' tbody tr:eq(' + i + ') td:eq(' + j + ')').attr("hidden", "true");
                    else {
                        for (index = i + 1; index < oSettings.nTBody.childElementCount; index++) {
                            if (oSettings.nTBody.rows[i].cells[j].textContent != oSettings.nTBody.rows[index].cells[j].textContent)
                                break;
                            count++;
                        }
                        if (count > 1) {
                            $('#' + divId + ' tbody tr:eq(' + i + ') td:eq(' + j + ')').attr("rowspan", count);
                        }
                    }
                }
            }
            else {
                //var number = oSettings.nTBody.rows[i].cells[j].textContent;
                //var numberHtml = Utils.formatMoney(number);
                $('#' + divId + ' tbody tr:eq(' + i + ') td:eq(' + j + ')').attr("align", "right").text(oSettings.nTBody.rows[i].cells[j].textContent.toString().trim());
            }
        }

    }
}

var saveAs = saveAs || function (e) { "use strict"; if (typeof navigator !== "undefined" && /MSIE [1-9]\./.test(navigator.userAgent)) { return } var t = e.document, n = function () { return e.URL || e.webkitURL || e }, r = t.createElementNS("http://www.w3.org/1999/xhtml", "a"), i = "download" in r, o = function (e) { var t = new MouseEvent("click"); e.dispatchEvent(t) }, a = /Version\/[\d\.]+.*Safari/.test(navigator.userAgent), f = e.webkitRequestFileSystem, u = e.requestFileSystem || f || e.mozRequestFileSystem, s = function (t) { (e.setImmediate || e.setTimeout)(function () { throw t }, 0) }, c = "application/octet-stream", d = 0, l = 500, w = function (t) { var r = function () { if (typeof t === "string") { n().revokeObjectURL(t) } else { t.remove() } }; if (e.chrome) { r() } else { setTimeout(r, l) } }, p = function (e, t, n) { t = [].concat(t); var r = t.length; while (r--) { var i = e["on" + t[r]]; if (typeof i === "function") { try { i.call(e, n || e) } catch (o) { s(o) } } } }, v = function (e) { if (/^\s*(?:text\/\S*|application\/xml|\S*\/\S*\+xml)\s*;.*charset\s*=\s*utf-8/i.test(e.type)) { return new Blob(["\ufeff", e], { type: e.type }) } return e }, y = function (t, s, l) { if (!l) { t = v(t) } var y = this, m = t.type, S = false, h, R, O = function () { p(y, "writestart progress write writeend".split(" ")) }, g = function () { if (R && a && typeof FileReader !== "undefined") { var r = new FileReader; r.onloadend = function () { var e = r.result; R.location.href = "data:attachment/file" + e.slice(e.search(/[,;]/)); y.readyState = y.DONE; O() }; r.readAsDataURL(t); y.readyState = y.INIT; return } if (S || !h) { h = n().createObjectURL(t) } if (R) { R.location.href = h } else { var i = e.open(h, "_blank"); if (i == undefined && a) { e.location.href = h } } y.readyState = y.DONE; O(); w(h) }, b = function (e) { return function () { if (y.readyState !== y.DONE) { return e.apply(this, arguments) } } }, E = { create: true, exclusive: false }, N; y.readyState = y.INIT; if (!s) { s = "download" } if (i) { h = n().createObjectURL(t); r.href = h; r.download = s; setTimeout(function () { o(r); O(); w(h); y.readyState = y.DONE }); return } if (e.chrome && m && m !== c) { N = t.slice || t.webkitSlice; t = N.call(t, 0, t.size, c); S = true } if (f && s !== "download") { s += ".download" } if (m === c || f) { R = e } if (!u) { g(); return } d += t.size; u(e.TEMPORARY, d, b(function (e) { e.root.getDirectory("saved", E, b(function (e) { var n = function () { e.getFile(s, E, b(function (e) { e.createWriter(b(function (n) { n.onwriteend = function (t) { R.location.href = e.toURL(); y.readyState = y.DONE; p(y, "writeend", t); w(e) }; n.onerror = function () { var e = n.error; if (e.code !== e.ABORT_ERR) { g() } }; "writestart progress write abort".split(" ").forEach(function (e) { n["on" + e] = y["on" + e] }); n.write(t); y.abort = function () { n.abort(); y.readyState = y.DONE }; y.readyState = y.WRITING }), g) }), g) }; e.getFile(s, { create: false }, b(function (e) { e.remove(); n() }), b(function (e) { if (e.code === e.NOT_FOUND_ERR) { n() } else { g() } })) }), g) }), g) }, m = y.prototype, S = function (e, t, n) { return new y(e, t, n) }; if (typeof navigator !== "undefined" && navigator.msSaveOrOpenBlob) { return function (e, t, n) { if (!n) { e = v(e) } return navigator.msSaveOrOpenBlob(e, t || "download") } } m.abort = function () { var e = this; e.readyState = e.DONE; p(e, "abort") }; m.readyState = m.INIT = 0; m.WRITING = 1; m.DONE = 2; m.error = m.onwritestart = m.onprogress = m.onwrite = m.onabort = m.onerror = m.onwriteend = null; return S }(typeof self !== "undefined" && self || typeof window !== "undefined" && window || this.content); if (typeof module !== "undefined" && module.exports) { module.exports.saveAs = saveAs } else if (typeof define !== "undefined" && define !== null && define.amd != null) { define([], function () { return saveAs }) }
var SelectService = {
    serviceId: 0,
    ServiceName: "Chọn dịch vụ cha"
};
var SelectProduct = {
    productID: 0,
    ProductName: "Chọn sản phẩm"
};
// Lấy ra list Service theo groupService và merchant
function bindTreeService() {
    var groupService = $("#ddlGroupService").val();
    var merchant = $("#ddlMerchant").val();
 
    SelectService.serviceId = 0;
    SelectService.ServiceName = "Chọn dịch vụ cha";

    $("#contextParentSv").val(SelectService.ServiceName);
    $("#dllParentId").val(SelectService.serviceId);// lấy id

    $.ajax({
        type: 'GET',
        url: UrlRoot + "Services/GetTreeService",
        data: { groupService: groupService, merchant: merchant },
        success: function (data) {
            if (data.status > 0 && data.item.length > 0) {
                $('#tree').treeview({
                    color: "#428bca",
                    enableAction: false,
                    nodeIcon: "glyphicon glyphicon-shopping-cart",
                    collapseIcon: "glyphicon glyphicon-chevron-down",
                    expandIcon: "glyphicon glyphicon-chevron-right",
                    selectedColor: "white",
                    data: data.item,
                    levels: 1,
                    onNodeSelected: function (event, data) {
                        SelectService.serviceId = data.serviceId;
                        SelectService.ServiceName = data.text;

                        $("#contextParentSv").val(SelectService.ServiceName);//lôi giá trị ra button
                        $("#dllParentId").val(SelectService.serviceId);// lấy id
                    },
                    onNodeUnselected: function (event, data) {
                        SelectService.serviceId = 0;
                        SelectService.ServiceName = "Chọn dịch vụ cha";

                        $("#contextParentSv").val(SelectService.ServiceName);
                        $("#dllParentId").val(SelectService.serviceId);// lấy id
                    }
                });
            }
        }
    });
};

// Lấy ra list Service theo groupService và merchant
function bindTreeProduct() {
    var Group = $("#ddlGroupService").val();
    var merchant = $("#ddlMerchant").val();

    if (Group == 6 || Group == 0 || Group == null || Group == undefined) {
        $("#selectProduct").hide();
        return;
    }
    if (merchant == null || merchant == 0 || merchant == undefined) {
        $("#selectProduct").hide();
        return;
    }
    $("#selectProduct").show();
    SelectProduct.productID = 0;
    SelectProduct.ProductName = "Chọn sản phẩm";

    $("#contextProduct").val(SelectProduct.ProductName);
    $("#dllProductId").val(SelectProduct.productID);// lấy id

    $.ajax({
        type: 'GET',
        url: UrlRoot + "Services/GetTreeService",
        data: { groupService: 6, merchant: merchant, isGetProduct : 1 },
        success: function (data) {
            if (data.status > 0 && data.item.length > 0) {
                $('#treeProduct').treeview({
                    color: "#428bca",
                    enableAction: false,
                    nodeIcon: "glyphicon glyphicon-shopping-cart",
                    collapseIcon: "glyphicon glyphicon-chevron-down",
                    expandIcon: "glyphicon glyphicon-chevron-right",
                    selectedColor: "white",
                    data: data.item,
                    levels: 1,
                    onNodeSelected: function (event, data) {
                        SelectProduct.productID = data.serviceId;
                        SelectProduct.ProductName = data.text;
                        $("#contextProduct").val(SelectProduct.ProductName);//lôi giá trị ra button
                        $("#dllProductId").val(SelectProduct.productID);// lấy id
                    },
                    onNodeUnselected: function (event, data) {
                        SelectProduct.productID = 0;
                        SelectProduct.ProductName = "Chọn sản phẩm";

                        $("#contextProduct").val(SelectProduct.ProductName);
                        $("#dllProductId").val(SelectProduct.productID);// lấy id
                    }
                });
            }
        }
    });

};


window.CommonCtrl = {
    LoadFunction: function (url) {
        Utils.Loading();
        $.ajax({
            type: 'GET',
            url: appPath + url,
            data: {
            },
            success: function (data) {
                Utils.UnLoading();
                $("#TemplateContent").html(data);
                $("html,body").animate({ scrollTop: 0 }, 'fast');
            }
        });
    },
}