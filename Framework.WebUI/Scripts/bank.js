var firstPageClick = true;

jQuery(function () {
    jQuery('#btn-search').on('click', function () {
        firstPageClick = true;
        var code = encodeURIComponent(jQuery('#BankCode').val());
        var name = encodeURIComponent(jQuery('#BankName').val());
        searchBank(code, name, null, true);
    });
    jQuery('#btn-search').click();
});

function createPaging(recreate) {
    if (recreate && jQuery('#pager').data("twbs-pagination")) {
        jQuery('#pager').twbsPagination('destroy');
    }
    var pageCount = jQuery('#total-page').text();
    if (!pageCount || pageCount === "0") return false;
    jQuery('#pager').twbsPagination({
        totalPages      : pageCount,
        itemOnPage      : 8,
        currentPage     : 1,
        cssStyle        : '',
        prev            : 'Prev',
        first           : 'First',
        last            : 'Last',
        onInit          : function () {
            // fire first page loading
        },
        onPageClick     : function (event, page) {
            if (firstPageClick) {
                firstPageClick = false;
                return;
            }
            var code = encodeURIComponent(jQuery('#BankCode').val());
            var name = encodeURIComponent(jQuery('#BankName').val());
            searchBank(code, name, page, false);
        }
    });
    return false;
}

function searchBank(code, name, page, recreatePaging) {
    blockUI();
    var url = searchUrl + '/?code=' + code + '&name=' + name;
    if (page) {
        url = url + '&page=' + page;
    }
    jQuery("#search-wrapper").load(url, function () {
        jQuery('.datatable-empty').DataTable();
        createPaging(recreatePaging);
        registerBtnDelete();
        unblockUI();
    });
}

function registerBtnDelete() {
    jQuery('.btn-delete').on('click', function() {
        var id = jQuery(this).attr('id-value');
        DeleteBank(id);
    });
}

function DeleteBank(id) {
    jQuery.ajax({
        method  : "GET",
        url     : deleteUrl + "/" + id
    })
    .done(function() {
        jQuery('#btn-search').click();
     })
    .fail(function(data) {
        console.log(data);
    });
    return false;
}

function blockUI() {
    var wrapper = jQuery('#main-wrapper');
    jQuery(wrapper).block({
        message     : '<i class="icon-spinner6 spinner"></i>',
        overlayCSS  : {
            backgroundColor : '#fff',
            opacity         : 0.85,
            cursor          : 'wait'
        },
        css: {
            border          : 0,
            padding         : 0,
            backgroundColor : 'none'
        }
    });
}

function unblockUI() {
    var wrapper = jQuery('#main-wrapper');
    jQuery(wrapper).unblock();
}
