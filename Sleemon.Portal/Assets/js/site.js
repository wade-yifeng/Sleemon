config = {
    PAGE_SIZE: 10,
    MAX_QUESTION_COUNT: 50,
    MAX_CHOICE_COUNT: 4
};

function htmlEncode(str) {
    return String(str)
        .replace(/&/g, '&amp;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
}

function htmlDecode(value) {
    return String(value)
        .replace(/&quot;/g, '"')
        .replace(/&#39;/g, "'")
        .replace(/&lt;/g, '<')
        .replace(/&gt;/g, '>')
        .replace(/&amp;/g, '&');
}

function delay(callback, seconds) {
    setTimeout(callback, seconds || 50);
}

//toast消息弹框
function showInfo(info) {
    $('#pin_toast').html(info);
    var container = $('#pin_toast_container');
    container.stop().fadeIn(1000, function () {
        setTimeout(function () {
            container.fadeOut(1000);
        }, 1000);
    });
}

/***************问卷管理***************/
function initWaterfall(isPreAnimationNeeded) {
    if (isPreAnimationNeeded) {
        $('.row').height(150);
    }
    delay(function () {
        var row = $('.row');
        var colWidth = ($('.row').width() - 90) * 0.3333333333;
        row.waterfall({
            align: 'left',
            itemCls: 'col-sm-4',
            minCol: 3,
            colWidth: colWidth,
            gutterWidth: 30,
            gutterHeight: 15
        });
    }, 500);
}

function refreshWaterfall() {
    delay(function () {
        $('.row').waterfall('reLayout');
    });
}

function showEditor(isNew) {
    var editor = $('#editor');
    editor.find('h4').text(isNew ? '新增问题' : '编辑问题');
    editor.modal();
}

function hideEditor() {
    $('#editor').modal('hide');
}

function removeQuestion(index) {
    $('.col-sm-4').eq(index).remove();
}

/***************资讯管理***************/
function initUEditor() {
    delay(function () {
        UE.delEditor('editor');
        var ue = UE.getEditor('editor');
        ue.addListener('contentChange', function (editor) {
            context = ue.getContent();
            $("#context").val(context);
            $('#content').html(context ? context : '还未编辑正文');
        });
    });
}

function setUEditor(content) {
    var html = htmlDecode(content);
    delay(function () {
        UE.getEditor('editor').setContent(html);
        $('#content').html(context ? context : '还未编辑正文');
    }, 100);
}