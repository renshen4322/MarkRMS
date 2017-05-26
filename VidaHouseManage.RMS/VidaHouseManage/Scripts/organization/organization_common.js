/************组织********************/
var organizationValidate = {

    NameValidate: function () {
        var name = $.trim($("#txt_organizationName").val());

        if (name == "") {

            $("#txt_organizationName").addClass("inputError").next().html("组织名称不能为空");

            return false;
        }
        if (name.length > 50) {

            $("#txt_organizationName").addClass("inputError").next().html("最大长度50");

            return false;
        }
        return true;
    }

};

$("#txt_organizationName").bind("blur", function () {

    var name = $.trim($(this).val());

    if (name == "") {

        $(this).addClass("inputError").next().html("组织名称不能为空");

        return false;
    }
    if (name.length > 50) {

        $(this).addClass("inputError").next().html("最大长度50");

        return false;
    }

}).bind("focus", function () {

    $(this).removeClass("inputError").next().html("");

});

$("#txt_organizationDes").bind("keyup", function () {

    var organizationDes = $.trim($(this).val());

    if (organizationDes.length > 500) {

        $(this).val(organizationDes.substring(0, 500));
    }
}).bind("blur", function () {

    var organizationDes = $(this).val();

    if (organizationDes.length > 500) {

        $(this).val(organizationDes.substring(0, 500));
    }
});
/***************end*************************/

/************组织角色**********************/

var roleValidate = {

    NameValidate: function () {

        var name = $.trim($("#txt_roleName").val());

        if (name == "") {

            $("#txt_roleName").addClass("inputError").next().html("角色名称不能为空");

            return false;
        }
        if (name.length > 50) {

            $("#txt_roleName").addClass("inputError").next().html("最大长度50");

            return false;

        }
        return true;

    }
};

$("#txt_roleName").bind("blur", function () {

    var name = $.trim($(this).val());

    if (name == "") {
        $(this).addClass("inputError").next().html("角色名称不能为空");

        return false;
    }
    if (name.length > 50) {

        $(this).addClass("inputError").next().html("最大长度50");

        return false;
    }

}).bind("focus", function () {

    $(this).removeClass("inputError").next().html("");

});;

$("#txt_roleDes").bind("keyup", function () {

    var roleDes = $(this).val();

    if (roleDes.length > 500) {

        $(this).val(roleDes.substring(0, 500));
    }
}).bind("blur", function () {

    var roleDes = $(this).val();

    if (roleDes.length > 500) {

        $(this).val(roleDes.substring(0, 500));
    }

});
/***************end**************************/


