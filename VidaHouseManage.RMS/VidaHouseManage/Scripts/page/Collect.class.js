var Collect = {

    paging: {
        StepNum: 7
    },
    MinSize: function (pageindex) {
        return (parseInt(pageindex) - Collect.paging.StepNum) < 1 ? 1 : (parseInt(pageindex) - Collect.paging.StepNum);
    },
    MaxSize: function (pageindex, totalPage) {
        return (parseInt(pageindex) + Collect.paging.StepNum) > totalPage ? totalPage : (parseInt(pageindex) + Collect.paging.StepNum);
    },
    PagingHtml: function (pageindex, totalPage) {

        var html = "<div id=\"paging\" class=\"paging right clearfix\">";

        var MinSize = Collect.MinSize(pageindex);
        var MaxSize = Collect.MaxSize(pageindex, totalPage);

        if (pageindex > 1) {
            html = html + "<a href=\"javascript:void(0);\"rel=\"1\">首页</a>";
            html = html + "<a href=\"javascript:void(0);\"rel=\"" + (pageindex - 1) + "\">上一页</a>";
        } else {
            html = html + "<a href=\"javascript:void(0);\"rel=\"1\">首页</a>";
            html = html + "<a href=\"javascript:void(0);\"rel=\"1\">上一页</a>";
        }

        for (var i = MinSize; i <= MaxSize; i++) {
            if (pageindex == i) {
                html = html + "<a href=\"javascript:void(0);\" rel=\"" + i + "\" data-current=\"true\" class=\"hover\" >" + pageindex + "</a>";
            }
            else {
                html = html + "<a href=\"javascript:void(0);\"rel=\"" + i + "\">" + i + "</a>";
            }
        }
        if (pageindex < totalPage) {
            html = html + "<a href=\"javascript:void(0);\"rel=\"" + (parseInt(pageindex) + 1) + "\">下一页</a>";
            html = html + "<a href=\"javascript:void(0);\" rel=\"" + totalPage + "\" >末页</a>";
        } else {

            html = html + "<a href=\"javascript:void(0);\" rel=\"" + totalPage + "\" >下一页</a>";
            html = html + "<a href=\"javascript:void(0);\" rel=\"" + totalPage + "\" >末页</a>";
        }

        html = html + "</div>";

        return html;
    },
    GetList: function (pageindex, objId, pageId, organizationId) {

        var obj = $("#" + objId);

        var pageobj = $("#" + pageId);

        if (pageindex == undefined || pageindex < 1)
            pageindex = 1;

        var param = {
            OrganizationId: organizationId,
            Page: pageindex,
            PageSize:this.paging.StepNum
        };

        $.ajax({
            url: "/Member/MemberList",
            type: "post",
            dataType: "json",
            async: false,
            data: param,
            success: function (result) {

                var commenthtml = "";

                if (result.Success) {

                    $.each(result.Data, function (id, item) {

                        var template = '<tr><td><span style="cursor:pointer;" data_source=' + item.Id + ' class="member_edit">' + item.UserName + '</span></td><td>' + item.PhoneNumber + '</td> <td>' + item.Email + '</td> <td><span data_source=' + item.Id + ' class="member_edit actionBtn active">修改</span><span data_source=' + item.Id + ' class="member_del actionBtn">删除</span></tr>';

                        commenthtml = commenthtml + template;
                    });
                    if (result.TotalPages > 1) {

                        obj.html(commenthtml);

                        pageobj.html(Collect.PagingHtml(pageindex, result.TotalPages));
                    } else {
                        obj.html(commenthtml);
                    }

                } else {

                    if (result.Code < 0) { window.location.href = result.Data; }

                    obj.html(commenthtml);
                }
            }
        });
    }
};

