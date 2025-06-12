

var obj =
{
    "ToDateStr": null,
    "FromDateStr": null,
    "pageIndex": 1,
    "pageSize": 10
}
let isPicker = false;
var _comment =
{

    Listing: function () {
        obj.pageIndex = obj.pageIndex++;
        $.ajax({
            url: "/Comment/ListComment",
            type: "Post",
            data: { searchModel: obj },
            success: function (result) {

                $('#Comment-content').html(result);
                $('#selectPaggingOptions').val(obj.pageSize).attr("selected", "selected");
                window.scrollTo(0, 0);
            }
        });
    },
    LoadComment: function (input) {


        $.ajax({
            url: "/Comment/ListComment",
            type: "Post",
            data: { searchModel: input },
            success: function (result) {

                $('#Comment-content').html(result);
                $('#selectPaggingOptions').val(obj.pageSize).attr("selected", "selected");
                window.scrollTo(0, 0);
            }
        });
    },
  

    OnPanging: function (value) {
        obj.pageIndex = value;
        _comment.LoadComment();
    },
    onSelectPageSize: function () {
        obj.pageIndex = 1;
        obj.pageSize = $("#selectPaggingOptions").find(':selected').val()
        _comment.LoadComment(obj);
    },
    SearchData() {
        var model =
        {
            "ToDateStr": null,
            "FromDateStr": null,
            "pageIndex": 1,
            "pageSize": 10
        }
        //model.FromDateStr = _global_function.GetDayText($('#fromDate').data('daterangepicker').startDate._d);
        //model.ToDateStr = _global_function.GetDayText($('#fromDate').data('daterangepicker').endDate._d);
        if (isPicker == false) {
            model.FromDateStr = null;
            model.ToDateStr = null;
        }
        else {
            model.FromDateStr = _global_function.GetDayText($('#fromDate').data('daterangepicker').startDate._d, true);
            model.ToDateStr = _global_function.GetDayText($('#fromDate').data('daterangepicker').endDate._d, true);
        }
        _comment.LoadComment(model);
    },
}

$(document).ready(function () {

    _comment.LoadComment(obj);
    //--scroll event
    $(window).scroll(function () {
        if ($(window).scrollTop() >= $('#table-comment').offset().top + $('#table-comment').outerHeight() - window.innerHeight) {
            _comment.LoadComment(obj)
        }
    });
    $('input[name="datetimeOrder"]').daterangepicker({
        //timePicker: true,
        //startDate: '',
        //endDate: '',
        maxDate: new Date(),
        //locale: {
        //    format: 'DD-MM-YYYY'
        //},
        //autoUpdateInput: true,
        autoUpdateInput: false,
        locale: {
            cancelLabel: 'Clear'
        }
    });
    $('input[name="datetimeOrder"]').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('DD/MM/YYYY') + ' - ' + picker.endDate.format('DD/MM/YYYY'));
        isPicker = true;
    });
});
