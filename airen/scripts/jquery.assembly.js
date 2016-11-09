(function ($) {
    //组件初始化
    $.assembly = function (doc) {
        if (!doc) { doc = ""; }

        //百度编辑器
        $(doc + " .ueditor ").each(function () {
            var name = $(this).attr("name");
            var guid = new GUID();
            var id = guid.newGUID();
            var width = "";
            var height = "";
            if ($(this).attr("width")) {
                width = " width:" + $(this).attr("width") + ";";
            }
            if ($(this).attr("height")) {
                height = " height:" + $(this).attr("height") + ";";
            }

            $(this).before('<script id="' + id + '" name="' + name + '" style=" ' + width + ' ' + height + ' " type="text/plain">' + $(this).text() + '</script>');
            $(this).remove();
            UE.getEditor(id);

        });

        $(doc + " .dictionary-combotree ").each(function () {
            var name = $(this).attr("name");
            var id = $(this).attr("id") ? $(this).attr("id") : new GUID().newGUID();
            var indexed = $(this).attr("indexed");
            var value = $(this).val();
            var width = "";
            if ($(this).attr("width")) {
                width = " width:" + $(this).attr("width") + ";";
            }
            $(this).before('<select id="' + id + '" name="' + name + '" style=" ' + width + '"></select>');
            $(this).remove();
            $('#' + id).combotree({
                url: '/Dictionary/TreeList?indexed=' + indexed,
                onLoadSuccess: function () {
                    $('#' + id).combotree("setValue", value);
                }
            });
        });

        $(doc + " .datagrid-selector").each(function () {

            var name = $(this).attr("name");
            var id = $(this).attr("id") ? $(this).attr("id") : new GUID().newGUID();
            var value = "";
            var text = "";
            var url = $(this).attr("url");
            var tools = undefined;
            var textName = $(this).attr("textName");
            var width = "width:200px;";
            var height = "height:20px;";
            var line_height = "line-height:20px;";
            var title = "选择";
            if ($(this).attr("width")) {
                width = " width:" + $(this).attr("width") + ";";
            }
            if ($(this).attr("height")) {
                height = " height:" + $(this).attr("height") + ";";
                line_height = " line-height:" + $(this).attr("height") + ";";
            }
            if ($(this).attr("title")) {
                title = $(this).attr("title");
            }
            if ($(this).attr("tools")) {
                tools = $(this).attr("tools");
            }
            if ($(this).val()) {
                value = $(this).val();
            }
            if ($(this).attr("text")) {
                text = $(this).attr("text");
            }


            var html = '<div class="textbox " id="' + id + '_div" style="' + width + height + '">';
            html += '<a class="textbox-button textbox-button-right l-btn l-btn-small" style="' + height + line_height + ' padding-right:10px; padding-left:5px; right: 0px;">选择</a>';
            html += '<input name="' + textName + '" type="text" style="' + width + height + ' padding:0px;" class="textbox-text" value="' + text + '" readonly="readonly" >';
            html += '<input type="hidden" id="' + id + '" name="' + name + '" value="' + value + '" />';
            html += '</div>';

            var dialogHtml = '<div class="easyui-panel" data-options="border:false,fit:true" style=" padding:5px;"><div class="easyui-layout" data-options="border:false,fit:true"><div region="west" title="选择列表" split="true" style="width:60%">';
            dialogHtml += '<table id="' + id + '_l"></table>';
            dialogHtml += '</div><div title="已选择项" region="center" id="dictionary-manage-edit">';
            dialogHtml += '<table id="' + id + '_r"></table>';
            dialogHtml += '</div></div></div>';

            $(this).before(html);
            $(this).remove();
            // $.parser.parse($(this).before());

            $("#" + id + "_div a").click(function () {
                var dialog = $("<div></div>").dialog({
                    title: title,
                    width: 850,
                    height: 450,
                    modal: true,
                    content: dialogHtml,
                    buttons: [{
                        text: '确认',
                        iconCls: 'icon-accept',
                        handler: function () {
                            var texts = "";
                            var values = "";
                            var rows = $("#" + id + "_r").datagrid("getRows");
                            for (var i = 0; i < rows.length; i++) {
                                texts += texts == "" ? (rows[i].Title) : ("," + rows[i].Title);
                                values += values == "" ? (rows[i].Value) : ("," + rows[i].Value);
                            }
                            $("#" + id + "_div input[type='text']").val(texts);
                            $("#" + id + "_div input[type='hidden']").val(values);
                            dialog.dialog('destroy');
                        }
                    }, {
                        text: '取消',
                        iconCls: 'icon-crossout',
                        handler: function () {
                            dialog.dialog('destroy');
                        }
                    }],
                    onClose: function () {
                        dialog.dialog('destroy');
                    }
                });

                $("#" + id + "_l").datagrid({
                    url: url,
                    tools: tools,
                    fit: true,
                    fitColumns: true,
                    rownumbers: true,
                    border: false,
                    pagination: true,
                    pageSize: 20,
                    pageList: [10, 20, 50, 100],
                    striped: true,
                    cache: false,
                    queryParams: {},
                    onClickRow: function (rowIndex, rowData) {
                        $("#" + id + "_r").datagrid("appendRow", rowData);
                    },
                    columns: [[
                        {
                            field: "Name",
                            title: "名称",
                            width: 100
                        }
                    ]]
                });

                $("#" + id + "_r").datagrid({
                    fit: true,
                    fitColumns: true,
                    striped: true,
                    border: false,
                    rownumbers: true,
                    onClickRow: function (rowIndex, rowData) {
                        $("#" + id + "_r").datagrid("deleteRow", rowIndex);
                    },
                    columns: [[
                        {
                            field: "Title",
                            title: "名称",
                            width: 100
                        }
                    ]]
                });

                if ($("#" + id + "_div input[type='hidden']").val()) {
                    var texts = $("#" + id + "_div input[type='text']").val().split(",");
                    var values = $("#" + id + "_div input[type='hidden']").val().split(",");
                    for (var i = 0; i < values.length; i++) {
                        $("#" + id + "_r").datagrid('appendRow', {
                            Title: texts[i],
                            Value: values[i]
                        });
                    }
                }


            });

        });


        $(doc + " .attachment-manager").each(function () {
            var id = $(this).attr("id") ? $(this).attr("id") : new GUID().newGUID();
            var name = $(this).attr("name") ? $(this).attr("id") : id;
            var value = $(this).val() ? $(this).val() : "";
            var width = "width:200px;";
            var height = "height:20px;";
            var line_height = "line-height:20px;";
            var indexed = $(this).attr("Indexed") ? $(this).attr("Indexed") : "";

            if ($(this).attr("width")) {
                width = " width:" + $(this).attr("width") + ";";
            }
            if ($(this).attr("height")) {
                height = " height:" + $(this).attr("height") + ";";
                line_height = " line-height:" + $(this).attr("height") + ";";
            }


            var html = '<div class="textbox " id="' + id + '_div" style="' + width + height + '">';
            html += '<a class="textbox-button textbox-button-right l-btn l-btn-small" style="' + height + line_height + ' padding-right:10px; padding-left:5px; right: 0px;">管理</a>';
            html += '<input type="text" style="' + width + height + ' padding:0px;" class="textbox-text" value="数据加载中.." readonly="readonly" >';
            html += '<input type="hidden" id="' + id + '" name="' + name + '" value="' + value + '" />';
            html += '</div>';

            $(this).before(html);
            $(this).remove();

            var valueInput = $("#" + id + "_div input[type='hidden']");
            var textInput = $("#" + id + "_div input[type='text']");

            if (value != "") {
                $.ajax({
                    url: "/Attachment/GetName",
                    data: { ids: value },
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        textInput.val(data.Data);
                    }, error: function () {
                        textInput.val("");
                    }
                });
            } else {
                textInput.val("");
            }

            $("#" + id + "_div a").click(function () {

                var href = "/Attachment/List";
                if (indexed == "") {
                    href += "?ids=" + valueInput.val();
                } else {
                    href += "?indexed=" + indexed;
                }
                var dialog = $("<div></div>").dialog({
                    href: href,
                    title: "附件列表管理",
                    width: 800,
                    height: 500,
                    modal: true,
                    buttons: [{
                        text: '确认',
                        iconCls: 'icon-accept',
                        handler: function () {
                            dialog.dialog('close');
                        }
                    }],
                    onClose: function () {
                        
                        var texts = "";
                        var values = "";
                        var rows = $("#attachment_list").datagrid("getRows");
                        for (var i = 0; i < rows.length; i++) {
                            texts += texts == "" ? (rows[i].Name) : ("," + rows[i].Name);
                            values += values == "" ? (rows[i].ID) : ("," + rows[i].ID);
                        }
                        textInput.val(texts);
                        valueInput.val(values);

                        dialog.dialog('destroy');
                        $("#attachment_add_dialog").dialog('destroy');
                    }
                });
            });



        });

    }
})(jQuery);