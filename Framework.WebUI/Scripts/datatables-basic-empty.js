jQuery(function() {
    // Setting datatable defaults
    jQuery.extend(jQuery.fn.dataTable.defaults, {
        bSort       : false,
        responsive  : true,
        bPaginate   : false,
        bInfo       : false,
        dom         : '<"datatable-scroll"t><"datatable-footer"ip>',
        language    : {
            paginate: { 'first': 'First', 'last': 'Last', 'next': '&rarr;', 'previous': '&larr;' }
        },
        drawCallback: function () {
            jQuery(this).find('tbody tr').slice(-3).find('.dropdown, .btn-group').addClass('dropup');
        },
        preDrawCallback: function () {
            jQuery(this).find('tbody tr').slice(-3).find('.dropdown, .btn-group').removeClass('dropup');
        }
    });
    // Basic datatable
    jQuery('.datatable-basic').DataTable();
});