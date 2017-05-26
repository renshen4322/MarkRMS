//获取该组织所有角色；
function SelRoles(id) {

    $("#hide_selOrgnization").val(id);

    $("#organizationList").empty();

    $.ajax({
        url: "/Organization/GetRolesByOrganization",
        type: "post",
        dataType: "json",
        async: false,
        data: { Id: id },
        success: function (result) {

            if (result.Success) {

                $.each(result.Data, function (id, item) {

                    $("#organizationList").append(' <tr><td><span style="cursor:pointer;" data_source=' + item.Id + ' class="roleList_edit">' + item.Name + '</span></td><td>' + item.Description + '</td><td><span data_source=' + item.Id + ' class="roleList_edit actionBtn active">修改</span><span data_source=' + item.Id + ' class="roleList_del actionBtn">删除</span></td> </tr>');

                });
            } else {

                if (result.Code < 0) { window.location.href = result.Data; }

            }
        }
    });
}

//获取权限列表;
function GetAuthorityNames() {

    var authorityNames = new Array();

    $("label[name='lab_author']").each(function () {

        if ($(this).hasClass("checkedActive")) {
            authorityNames.push($(this).attr("data_source"));
        }

    });

    return authorityNames;
}

$(function () {

    $("#treeview").jstree({
        'core': {
            'data': {
                'url': '/Organization/GetOrganizationList',
                'data': {}
            }
        },
        "multiple": true,
        "plugins": ["contextmenu", "wholerow"],
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
                                                 
                                                var dataObj = eval("(" + result.Message + ")");

                                                $.alert({
                                                    title: '友情提示!',
                                                    content: dataObj.modelState.organizationId[0]

                                                });
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

        SelRoles(e.node.id);

    }).bind("loaded.jstree", function (e, data) {
        //默认加载成功后点击根组织；
        var inst = data.instance;
        var obj = inst.get_node(e.target.firstChild.firstChild.lastChild);
        SelRoles(obj.id);
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

    $("#authList").on('click', 'li', function () {

        $(this).find('label').toggleClass('checkedActive');
    });

    //添加角色
    $("#add_Role").bind("click", function () {

        $("#txt_roleName").val("").removeClass("inputError").next().html("");

        $("#txt_roleDes").val("");

        $("#authList").empty();

        $(".submitBtn").attr('id', "btn_add");

        $.ajax({
            url: "/Organization/GetAuthorityList",
            type: "post",
            dataType: "json",
            data: {},
            success: function (result) {

                if (result.Success) {

                    var html = "";
                    $.each(result.Data, function (id, item) {

                        html += ('<li>  <label name="lab_author" data_source=' + item.Name + '></label><span>' + item.DisplayName + '</span></li>');

                    });

                    $("#authList").append(html);

                } else {

                    if (result.Code < 0) { window.location.href = result.Data; }

                }
            }
        });
    });

    //提交添加角色
    $("#myModal").delegate("#btn_add", "click", function () {

        var roleName = $.trim($("#txt_roleName").val());

        var roleDes = $.trim($("#txt_roleDes").val());

        var orgId = $("#hide_selOrgnization").val();

        var names = GetAuthorityNames();

        if (roleValidate.NameValidate()) {
            $.ajax({
                url: "/Organization/CreateRole",
                type: "post",
                dataType: "json",
                data: { Names: names, OrganizationId: orgId, Name: roleName, Description: roleDes },
                traditional: true,
                success: function (result) {

                    if (result.Success) {

                        $('#myModal').modal('hide');

                        $.alert({
                            title: '友情提示',
                            content: '添加成功',
                        });

                        SelRoles($("#hide_selOrgnization").val());

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

    //编辑角色
    $("#organizationList").delegate(".roleList_edit", "click", function () {

        var id = $(this).attr("data_source");

        $.ajax({
            url: "/Organization/GetRoleById",
            type: "post",
            dataType: "json",
            data: { Id: id, },
            success: function (result) {

                if (result.Success) {

                    $("#txt_roleName").val(result.Data.Name).removeClass("inputError").next().html("");

                    $("#txt_roleDes").val(result.Data.Description);

                    $("#authList").empty();

                    $("#authList").append('<input type="hidden" id="hid_roleId" value=' + id + ' />');

                    var html = "";

                    $.each(result.Data.AuthorList, function (id, item) {

                        var ischeck = item.IsSelected == true ? "class='checkedActive'" : "";

                        html += ('<li>  <label name="lab_author" ' + ischeck + ' data_source=' + item.Name + '></label><span>' + item.DisplayName + '</span></li>');

                    });

                    $("#authList").append(html);

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

    //提交编辑角色
    $("#myModal").delegate("#btn_edit", "click", function () {

        var roleId = $("#hid_roleId").val();

        var roleName = $.trim($("#txt_roleName").val());

        var roleDes = $.trim($("#txt_roleDes").val());

        var names = GetAuthorityNames();

        if (roleValidate.NameValidate()) {

            $.ajax({
                url: "/Organization/EditRole",
                type: "post",
                dataType: "json",
                data: { Names: names, Id: roleId, Name: roleName, Description: roleDes },
                success: function (result) {
                    if (result.Success) {

                        $('#myModal').modal('hide');

                        $.alert({
                            title: '友情提示',
                            content: '修改成功',
                        });
                        SelRoles($("#hide_selOrgnization").val());
                    } else {

                        if (result.Code < 0) { window.location.href = result.Data; }

                        $.alert({
                            title: '友情提示',
                            content: result.Message,
                        });
                    }
                }
            });

        };

    });

    //删除角色
    $("#organizationList").delegate(".roleList_del", "click", function () {

        var roleId = $(this).attr("data_source");

        $.confirm({
            title: '友情提示!',
            content: '确定删除角色吗？',
            confirm: function () {

                $.ajax({
                    url: "/Organization/DelRole",
                    type: "post",
                    dataType: "json",
                    data: { Id: roleId },
                    success: function (result) {
                        if (result.Success) {

                            SelRoles($("#hide_selOrgnization").val());

                        } else {

                            if (result.Code < 0) { window.location.href = result.Data; }

                            $.alert({
                                title: '友情提示!',
                                content: result.Message

                            });

                        }
                    }
                });
            }
        });
    });
});