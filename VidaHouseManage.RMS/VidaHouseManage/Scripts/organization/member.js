//设置组织下的成员
function SelMembers(id) {

    $("#hide_selOrgnization").val(id);

    $("#meberList").empty();

    Collect.GetList(1, "meberList", "pageList", id);

}

//获取该组织下的角色列表;
function GetRoleIds() {

    var roleIds = new Array();

    $("label[name='lab_role']").each(function () {

        if ($(this).hasClass("checkedActive")) {

            roleIds.push($(this).attr("data_source"));
        }

    });

    return roleIds;

}

$(function () {
   

    $(document).delegate('#paging>a', 'click', function () {

        var current = $(this).attr("data-current");

        var id = $("#hide_selOrgnization").val();

        if (!current) {
            Collect.GetList($(this).attr("rel"), "meberList", "pageList", id);
        }
    });

    $("#treeview").jstree({
        'core': {
            'data': {
                'url': '/Member/GetOrganizationList',
                'data': {}
            }
        },
        "multiple": true,
        "plugins": ["contextmenu","wholerow"],
        "contextmenu": {
            "items": {
                "create": null,
                "rename": null,
                "remove": null,
                "ccp": null,
                "新建子组织": {
                    "label": "新建子组织",
                    "action": function (data) {

                        var inst = $.jstree.reference(data.reference),

                        obj = inst.get_node(data.reference);

                        $("#txt_organizationName").val("").removeClass("inputError").next().html("");

                        $("#txt_organizationDes").val("");

                        $("#hid_organizationId").val(obj.id);

                        $(".submitBtn").attr('id', "btn_add");

                        $('#myOrganization').modal('show');
                    }
                },
                "编辑组织": {
                    "label": "编辑组织",
                    "action": function (data) {
                        var inst = $.jstree.reference(data.reference),

                        obj = inst.get_node(data.reference);

                        $("#hid_organizationId").val(obj.id);

                        $("#txt_organizationName").val(obj.text).removeClass("inputError").next().html("");

                        $("#txt_organizationDes").val(obj.a_attr.Description);

                        $(".submitBtn").attr('id', "btn_edit");

                        $('#myOrganization').modal('show');
                    }
                },
                "删除组织": {
                    "label": "删除组织",
                    "action": function (data) {
                        var inst = $.jstree.reference(data.reference),
                        obj = inst.get_node(data.reference);
                        $.confirm({
                            title: '友情提示!',
                            content: '确定删除组织吗？',
                            confirm: function () {

                                $.ajax({
                                    url: "/Organization/DelOrganization",
                                    type: "post",
                                    dataType: "json",
                                    data: { id: obj.id },
                                    success: function (result) {
                                        if (result.Success) {
                                            $("#treeview").jstree("refresh");
                                        } else {
                                            if (result.Code < 0) { window.location.href = result.Data; }
                                            else {
                                                alert(result.Message);
                                            }
                                        }
                                    }
                                });
                            }
                        });
                    }
                }
            }
        }

    }).bind("activate_node.jstree", function (obj, e) {  //点击事件触发

        SelMembers(e.node.id);

    }).bind("loaded.jstree", function (e, data) {

        var inst = data.instance;

        var obj = inst.get_node(e.target.firstChild.firstChild.lastChild);

        SelMembers(obj.id);
    });

    //添加组织；
    $("#myOrganization").delegate("#btn_add", "click", function () {

        var name = $.trim($("#txt_organizationName").val());

        var id = $("#hid_organizationId").val();

        var des = $.trim($("#txt_organizationDes").val());

        if (organizationValidate.NameValidate()) {
            $.ajax({
                url: "/Organization/AddOrganization",
                type: "post",
                dataType: "json",
                data: { Id: id, Name: name, Description: des },
                success: function (result) {
                    if (result.Success) {

                        $("#treeview").jstree("refresh");

                        $('#myOrganization').modal('hide');

                    } else {

                        if (result.Code < 0) { window.location.href = result.Data; }

                    }
                }
            });

        }
    });

    //修改组织
    $("#myOrganization").delegate("#btn_edit", "click", function () {

        var name = $.trim($("#txt_organizationName").val());

        var id = $("#hid_organizationId").val();

        var des = $.trim($("#txt_organizationDes").val());

        if (organizationValidate.NameValidate()) {

            $.ajax({
                url: "/Organization/EditOrganization",
                type: "post",
                dataType: "json",
                data: { Id: id, Name: name, Description: des },
                success: function (result) {
                    if (result.Success) {

                        $("#treeview").jstree("refresh");

                        $('#myOrganization').modal('hide');
                    } else {

                        if (result.Code < 0) { window.location.href = result.Data; }

                    }
                }
            });
        }

    });


    $("#roleList").on('click', 'li', function () {

        $(this).find('label').toggleClass('checkedActive');
    });

    //添加成员
    $("#add_Member").bind("click", function () {

        var organzationId = $("#hide_selOrgnization").val();

        $("#username").val("").removeClass("inputError").next().html("");

        $("#phone").val("").removeClass("inputError").next().html("");

        $("#email").val("").removeClass("inputError").next().html("");

        $.ajax({
            url: "/Member/GetRolesByOrganizationId",
            type: "post",
            dataType: "json",
            async: false,
            data: { Id: organzationId },
            success: function (result) {

                if (result.Success) {

                    $("#roleList").empty();

                    $("#memberBtn").empty();

                    var html = "";
                    $.each(result.Data, function (id, item) {

                        html += ('<li>  <label name="lab_role" data_source=' + item.Id + '></label><span>' + item.Name + '</span></li>');

                    });

                    $("#roleList").append(html);

                    $(".submitBtn").attr('id', "btn_add");

                } else {
                    if (result.Code < 0) { window.location.href = result.Data; }
                }
            }
        });
    });

    //提交添加成员
    $("#myModal").on("click", "#btn_add", function () {

        var username = $.trim($("#username").val());

        var phone = $.trim($("#phone").val());

        var email = $.trim($("#email").val());

        var roleIds = GetRoleIds();

        var organizationId = $("#hide_selOrgnization").val();

        if (memberValidate.NameValidate() && memberValidate.PhoneValidate() && memberValidate.EmailValidate()) {
            $.ajax({
                url: "/Member/AddMember",
                type: "post",
                dataType: "json",
                async: false,
                data: { Ids: roleIds, UserName: username, PhoneNumber: phone, Email: email, OrganizationId: organizationId },
                success: function (result) {

                    if (result.Success) {

                        var origanizationId = $("#hide_selOrgnization").val();

                        SelMembers(origanizationId);

                        $("#username").val("");

                        $("#phone").val("");

                        $("#email").val("");

                        $('#myModal').modal('hide');

                    } else {

                        if (result.Code < 0) { window.location.href = result.Data; }

                        $.alert({
                            title: '友情提示',
                            content: result.Message,
                        });
                    }
                }
            });
        }

    });

    //编辑成员
    $("#meberList").delegate(".member_edit", "click", function () {

        var id = $(this).attr("data_source");

        $.ajax({
            url: "/Member/GetMemberById",
            type: "post",
            dataType: "json",
            async: false,
            data: { Id: id },
            success: function (result) {

                if (result.Success) {

                    $("#roleList").empty();

                    $("#username").val(result.Data.UserName).removeClass("inputError").next().html("");

                    $("#phone").val(result.Data.PhoneNumber).removeClass("inputError").next().html("");

                    $("#email").val(result.Data.Email).removeClass("inputError").next().html("");

                    var html = "";

                    $.each(result.Data.Roles, function (id, item) {

                        var ischeck = item.IsSelected == true ? "class='checkedActive'" : "";

                        html += ('<li>  <label name="lab_role" ' + ischeck + ' data_source=' + item.Id + '></label><span>' + item.Name + '</span></li>');

                    });

                    $("#roleList").append(html);

                    $("#roleList").append('<input type="hidden" id="hid_memberId" value=' + id + ' />');

                    $(".submitBtn").attr('id', "btn_edit");

                    $('#myModal').modal('show');

                } else {

                    if (result.Code < 0) { window.location.href = result.Data; }

                    $.alert({
                        title: '友情提示',
                        content: result.Message,
                    });
                }

            }
        });

    });

    //提交编辑成员
    $("#myModal").delegate("#btn_edit", "click", function () {

        var username = $.trim($("#username").val());

        var phone = $.trim($("#phone").val());

        var email = $.trim($("#email").val());

        var Id = $("#hid_memberId").val();

        var roleIds = GetRoleIds();

        if (memberValidate.NameValidate() && memberValidate.PhoneValidate() && memberValidate.EmailValidate()) {

            $.ajax({
                url: "/Member/EditMember",
                type: "post",
                dataType: "json",
                async: false,
                data: { Id: Id, Ids: roleIds, UserName: username, PhoneNumber: phone, Email: email },
                success: function (result) {

                    if (result.Success) {

                        var origanizationId = $("#hide_selOrgnization").val();

                        SelMembers(origanizationId);

                        $("#usename").val("");

                        $("#phone").val("");

                        $("#email").val("");

                        $('#myModal').modal('hide');

                    } else {

                        if (result.Code < 0) { window.location.href = result.Data; }

                        $.alert({
                            title: '友情提示',
                            content: result.Message,
                        });
                    }
                }
            });
        }

    });

    //删除成员
    $("#meberList").delegate(".member_del", "click", function () {

        var id = $(this).attr("data_source");

        $.confirm({
            title: '友情提示!',
            content: '确定删除成员吗？',
            confirm: function () {

                $.ajax({
                    url: "/Member/DeleteMember",
                    type: "post",
                    dataType: "json",
                    async: false,
                    data: { Id: id },
                    success: function (result) {

                        if (result.Success) {

                            var origanizationId = $("#hide_selOrgnization").val();

                            SelMembers(origanizationId);
                        } else {

                            if (result.Code < 0) { window.location.href = result.Data; }
                        }

                    }
                });
            }
        });
    });
});