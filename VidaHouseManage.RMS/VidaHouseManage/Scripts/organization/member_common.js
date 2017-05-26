/************成员********************/
var memberValidate = {

    phoneRule: /^1[34578]\d{9}|09\d{8}$/,

    emailRule: /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/,

    NameValidate: function () {

        var name = $.trim($("#username").val());

        if (name == "") {

            $("#username").addClass("inputError").next().html("用户名不能为空");

            return false;
        }
        if (name.length > 50) {

            $("#username").addClass("inputError").next().html("最大长度50");

            return false;

        }

        return true;
    },

    PhoneValidate: function () {

        var phone = $.trim($("#phone").val());

        if (phone == "") {
            $("#phone").addClass("inputError").next().html("手机号不能为空");

            return false;
        }

        if (!(memberValidate.phoneRule.test(phone))) {

            $("#phone").addClass("inputError").next().html("手机号格式错误");

            return false;
        }

        return true;
    },

    EmailValidate: function () {

        var email = $.trim($("#email").val());

        if (email == "") {
            $("#email").addClass("inputError").next().html("Email不能为空");
            return false;
        }

        if (!(memberValidate.emailRule.test(email))) {
            $("#email").addClass("inputError").next().html("Email格式错误");
            return false;
        }

        return true;
    }
};

$("#username").bind("blur", function () {

    var name = $.trim($(this).val());

    if (name == "") {

        $(this).addClass("inputError").next().html("用户名不能为空");

        return false;
    }
    if (name.length > 50) {

        $(this).addClass("inputError").next().html("用户名最大长度50");

        return false;
    }
}).bind("focus", function () {

    $(this).removeClass("inputError").next().html("");

});

$("#phone").bind("blur", function () {

    var phone = $.trim($(this).val());

    if (phone == "") {

        $(this).addClass("inputError").next().html("手机号不能为空");

        return false;
    }

    if (!(memberValidate.phoneRule.test(phone))) {

        $(this).addClass("inputError").next().html("手机号格式错误");

        return false;
    }
}).bind("focus", function () {

    $(this).removeClass("inputError").next().html("");

});

$("#email").bind("blur", function () {

    var email = $.trim($(this).val());

    if (email == "") {

        $(this).addClass("inputError").next().html("Email不能为空");

        return false;
    }

    if (!(memberValidate.emailRule.test(email))) {

        $(this).addClass("inputError").next().html("Email格式错误");
    }
}).bind("focus", function () {

    $(this).removeClass("inputError").next().html("");

});

/***************end*************************/



