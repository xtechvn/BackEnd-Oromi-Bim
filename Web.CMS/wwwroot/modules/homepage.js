$(document).ready(function () {

    homepage.Init()
})


var homepage = {
    Init: function () {
        homepage.DynamicBind()
    },
    DynamicBind: function () {
        $('body').on('click', '#banner-main .col-md-3 .delete,#banner-sub .col-md-3 .delete,#banner-trend-main .col-md-3 .delete,#banner-trend-sub .col-md-3 .delete ', function () {
            var element = $(this)
            element.closest('.col-md-3').find('.magnific_popup').hide()
            element.closest('.col-md-3').find('.choose').show()
            element.closest('.col-md-3').find('img').attr('src', '')

        })
        $('body').on('change', '.col-md-3 .banner-file,.col-md-2 .banner-file', function () {
            var element = $(this)
            console.log('Change event triggered for:', this.id); // In ra ID của input bị thay đổi

            homepage.AddImage(element)
        })


        $('body').on('click', '#save-progress', function () {

            homepage.Summit()
        })
        $('body').on('click', '#cancel-progress', function () {
            var title = 'Xác nhận hủy';
            var description = 'Các chỉnh sửa sẽ không được lưu lại, Bạn có chắc chắn không?';

            _msgconfirm.openDialog(title, description, function () {
                setTimeout(function () {
                    window.location.reload();
                }, 1000);
            });


        })
        $('body').on('click', '.choose', function (e) {
            e.preventDefault()
            var element = $(this)

            element.closest('.img-upload-col').find('input').trigger('click')


        })
    },
    AddImage: function (element) {
        var ImageExtension = ['jpeg', 'jpg', 'png', 'bmp']

        if ($.inArray(element.val().split('.').pop().toLowerCase(), ImageExtension) == -1) {
            _msgalert.error("Vui lòng chỉ upload các định dạng sau: " + ImageExtension.join(', '));
            return
        }

        $(element[0].files).each(function (index, item) {

            var reader = new FileReader();
            reader.onload = function (e) {
                var src = e.target.result;

                element.closest('.img-upload-col').find('.magnific_popup').find('img').attr('src', src)
            }
            element.closest('.img-upload-col').find('.magnific_popup').show()
            element.closest('.img-upload-col').find('.choose').hide()
            reader.readAsDataURL(item);
            return false;
        });
        element.val(null)

    },
    POSTSynchorus: function (url, model) {
        var data = undefined
        $.ajax({
            url: url,
            type: "POST",
            data: model,
            success: function (result) {
                data = result;
            },
            error: function (err) {
                console.log(err)
            },
            async: false
        });
        return data
    },
    CheckIfImageVideoIsLocal: function (data) {
        if (data != undefined && (data.includes("data:image") || data.includes("data:video") || data.includes("base64,"))) {
            return true
        }
        else {
            return false
        }
    },
    Summit: function () {
        var banner_main = []
        $('#banner-main .img-upload-col').each(function (index, item) {

            var element_image = $(this)
            if (element_image.find('img').length > 0) {
                var data_src = element_image.find('img').attr('src')
                if (data_src == null || data_src == undefined || data_src.trim() == '') {
                    banner_main.push(
                        {
                            Id: element_image.attr('data-id'),
                            CodeValue: element_image.attr('data-pos'),
                            OrderNo: element_image.attr('data-pos'),
                            Description: ""
                        })
                    return true
                }
                if (homepage.CheckIfImageVideoIsLocal(data_src)) {
                    var result = homepage.POSTSynchorus('/Product/SummitImages', { data_image: data_src })
                    if (result != undefined && result.data != undefined && result.data.trim() != '') {
                        banner_main.push(
                            {
                                Id: element_image.attr('data-id'),
                                CodeValue: element_image.attr('data-pos'),
                                OrderNo: element_image.attr('data-pos'),
                                Description: result.data
                            })
                    } else {
                        banner_main.push({
                            Id: element_image.attr('data-id'),
                            CodeValue: element_image.attr('data-pos'),
                            OrderNo: element_image.attr('data-pos'),
                            Description: data_src
                        })
                    }
                }
                else {
                    banner_main.push({
                        Id: element_image.attr('data-id'),
                        CodeValue: element_image.attr('data-pos'),

                        OrderNo: element_image.attr('data-pos'),
                        Description: data_src
                    })
                }
            }
        })
        var banner_sub = []
        $('#banner-sub .img-upload-col').each(function (index, item) {
            var element_image = $(this)
            if (element_image.find('img').length > 0) {
                var data_src = element_image.find('img').attr('src')
                if (data_src == null || data_src == undefined || data_src.trim() == '') {
                    banner_sub.push(
                        {
                            Id: element_image.attr('data-id'),
                            CodeValue: element_image.attr('data-pos'),
                            OrderNo: element_image.attr('data-pos'),
                            Description: ""
                        })
                    return true

                }
                if (homepage.CheckIfImageVideoIsLocal(data_src)) {
                    var result = homepage.POSTSynchorus('/Product/SummitImages', { data_image: data_src })
                    if (result != undefined && result.data != undefined && result.data.trim() != '') {
                        banner_sub.push(
                            {
                                Id: element_image.attr('data-id'),
                                CodeValue: element_image.attr('data-pos'),
                                OrderNo: element_image.attr('data-pos'),
                                Description: result.data
                            })
                    } else {
                        banner_sub.push({
                            Id: element_image.attr('data-id'),
                            CodeValue: element_image.attr('data-pos'),

                            OrderNo: element_image.attr('data-pos'),
                            Description: data_src
                        })
                    }
                }
                else {
                    banner_sub.push({
                        Id: element_image.attr('data-id'),
                        CodeValue: element_image.attr('data-pos'),

                        OrderNo: element_image.attr('data-pos'),
                        Description: data_src
                    })
                }
            }
        })
        //var trending_main = []
        //$('#banner-trend-main .img-upload-col').each(function (index, item) {

        //    var element_image = $(this)
        //    if (element_image.find('img').length > 0) {
        //        var data_src = element_image.find('img').attr('src')
        //        if (data_src == null || data_src == undefined || data_src.trim() == '') {
        //            trending_main.push(
        //                {
        //                    Id: element_image.attr('data-id'),
        //                    CodeValue: element_image.attr('data-pos'),
        //                    OrderNo: element_image.attr('data-pos'),
        //                    Description: ""
        //                })
        //            return true
        //        }
        //        if (homepage.CheckIfImageVideoIsLocal(data_src)) {
        //            var result = homepage.POSTSynchorus('/Product/SummitImages', { data_image: data_src })
        //            if (result != undefined && result.data != undefined && result.data.trim() != '') {
        //                trending_main.push(
        //                    {
        //                        Id: element_image.attr('data-id'),
        //                        CodeValue: element_image.attr('data-pos'),
        //                        OrderNo: element_image.attr('data-pos'),
        //                        Description: result.data
        //                    })
        //            } else {
        //                trending_main.push({
        //                    Id: element_image.attr('data-id'),
        //                    CodeValue: element_image.attr('data-pos'),
        //                    OrderNo: element_image.attr('data-pos'),
        //                    Description: data_src
        //                })
        //            }
        //        }
        //        else {
        //            trending_main.push({
        //                Id: element_image.attr('data-id'),
        //                CodeValue: element_image.attr('data-pos'),

        //                OrderNo: element_image.attr('data-pos'),
        //                Description: data_src
        //            })
        //        }
        //    }
        //    return false
        //})
        //var trending_sub = []
        //$('#banner-trend-sub .img-upload-col').each(function (index, item) {

        //    var element_image = $(this)
        //    if (element_image.find('img').length > 0) {
        //        var data_src = element_image.find('img').attr('src')
        //        if (data_src == null || data_src == undefined || data_src.trim() == '') {
        //            trending_sub.push(
        //                {
        //                    Id: element_image.attr('data-id'),
        //                    CodeValue: element_image.attr('data-pos'),
        //                    OrderNo: element_image.attr('data-pos'),
        //                    Description: ""
        //                })
        //            return true
        //        }
        //        if (homepage.CheckIfImageVideoIsLocal(data_src)) {
        //            var result = homepage.POSTSynchorus('/Product/SummitImages', { data_image: data_src })
        //            if (result != undefined && result.data != undefined && result.data.trim() != '') {
        //                trending_sub.push(
        //                    {
        //                        Id: element_image.attr('data-id'),
        //                        CodeValue: element_image.attr('data-pos'),
        //                        OrderNo: element_image.attr('data-pos'),
        //                        Description: result.data
        //                    })
        //            } else {
        //                trending_sub.push({
        //                    Id: element_image.attr('data-id'),
        //                    CodeValue: element_image.attr('data-pos'),
        //                    OrderNo: element_image.attr('data-pos'),
        //                    Description: data_src
        //                })
        //            }
        //        }
        //        else {
        //            trending_sub.push({
        //                Id: element_image.attr('data-id'),
        //                CodeValue: element_image.attr('data-pos'),

        //                OrderNo: element_image.attr('data-pos'),
        //                Description: data_src
        //            })
        //        }
        //    }
        //})
        var vnpay = {
            Id: $('#vnpay').attr('data-id'),
            CodeValue: $('#vnpay input').val(),
            OrderNo: 1,
            Description: 'Triết khấu VNPAY đối với sản phẩm'
        }
        var request = { banner_main: banner_main, banner_sub: banner_sub, vnpay: vnpay }
        let url = "/homepage/summit";
        _global_function.AddLoading()
        _ajax_caller.post(url, request, function (result) {
            _global_function.RemoveLoading()

            if (result.is_success) {
                _msgalert.success(result.message);

            } else {
                _msgalert.error(result.message);
            }

        });
    },
    DeleteById: function (id) {
        var request = { id: id }
        let url = "/homepage/DeleteById";
        _global_function.AddLoading()
        _ajax_caller.post(url, request, function (result) {
            _global_function.RemoveLoading()

            if (result.is_success) {
                _msgalert.success(result.message);

            } else {
                _msgalert.error(result.message);
            }

        });
    }
}