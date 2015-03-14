//Initial bind
$(document).ready(function () {
    BindControlEvents();
});

//Re-bind for callbacks
var prm = Sys.WebForms.PageRequestManager.getInstance();

prm.add_endRequest(function () {
    BindControlEvents();
});

function BindControlEvents() {
    $(".txtDate").inputmask('dd/mm/yyyy');

    $('.formMain').validate({
        errorPlacement: function (error, element) {
            //error.appendTo(element.parent("td").next("td"));
            error.insertAfter(element);
        }
    });


    $.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            var re = new RegExp(regexp);
            return this.optional(element) || re.test(value);
        }
    );

    if ($('.txtUsername').length) {
        $('.txtUsername').rules("add", {
            required: true,
            minlength: 6,
            maxlength: 100,
            regex: /^\s*[a-zA-Z0-9_]+\s*$/,
            messages: {
                required: 'Vui lòng nhập tên đăng nhập!',
                minlength: 'Độ dài tên đăng nhập phải lớn hơn 6 ký tự!',
                maxlength: 'Độ dài tên đăng nhập phải nhỏ hơn 100 ký tự!',
                regex: 'Tên đăng nhập chỉ gồm chữ, số và dấu gạch dưới'
            }
        });
    }

    if ($('.txtCurrentPassword').length) {
        $('.txtCurrentPassword').rules("add", {
            required: true,
            maxlength: 100,
            messages: {
                required: 'Vui lòng nhập mật khẩu hiện tại!',
                maxlength: 'Độ dài câu trả lời phải nhỏ hơn 100 ký tự!'
            }
        });
    }

    if ($('.txtPassword').length) {
        $('.txtPassword').rules("add", {
            required: true,
            minlength: 6,
            maxlength: 100,
            messages: {
                required: 'Vui lòng nhập mật khẩu!',
                minlength: 'Độ dài mật khẩu phải lớn hơn 6 ký tự!',
                maxlength: 'Độ dài mật khẩu phải nhỏ hơn 100 ký tự!'
            }
        });
    }

    if ($('.txtConfirmPassword').length) {
        $('.txtConfirmPassword').rules("add", {
            required: true,
            minlength: 6,
            maxlength: 100,
            equalTo: '.txtPassword',
            messages: {
                required: 'Vui lòng nhập mật khẩu xác nhận!',
                minlength: 'Độ dài mật khẩu xác nhận phải lớn hơn 6 ký tự!',
                maxlength: 'Độ dài mật khẩu xác nhận phải nhỏ hơn 100 ký tự!',
                equalTo: 'Mật khẩu xác nhận không đúng!'
            }
        });
    }

    if ($('.txtNewPassword').length) {
        $('.txtNewPassword').rules("add", {
            required: true,
            minlength: 6,
            maxlength: 100,
            messages: {
                required: 'Vui lòng nhập mật khẩu mới!',
                minlength: 'Độ dài mật khẩu mới phải lớn hơn 6 ký tự!',
                maxlength: 'Độ dài mật khẩu mới phải nhỏ hơn 100 ký tự!'
            }
        });
    }

    if ($('.txtConfirmNewPassword').length) {
        $('.txtConfirmNewPassword').rules("add", {
            required: true,
            minlength: 6,
            maxlength: 100,
            equalTo: '.txtNewPassword',
            messages: {
                required: 'Vui lòng nhập mật khẩu xác nhận!',
                minlength: 'Độ dài mật khẩu xác nhận phải lớn hơn 6 ký tự!',
                maxlength: 'Độ dài mật khẩu xác nhận phải nhỏ hơn 100 ký tự!',
                equalTo: 'Mật khẩu xác nhận không đúng!'
            }
        });
    }


    if ($('.txtEmail').length) {
        $('.txtEmail').rules("add", {
            required: true,
            maxlength: 50,
            email: true,
            messages: {
                required: 'Vui lòng nhập địa chỉ Email!',
                email: 'Email không hợp lệ!',
                maxlength: 'Độ dài email phải nhỏ hơn 50 ký tự!'
            }
        });
    }

    if ($('.txtAddress').length) {
        $('.txtAddress').rules("add", {
            required: true,
            maxlength: 50,
            messages: {
                required: 'Vui lòng nhập địa chỉ!',
                maxlength: 'Độ dài địa chỉ phải nhỏ hơn 50 ký tự'
            }
        });
    }

    if ($('.txtFullName').length) {
        $('.txtFullName').rules("add", {
            required: true,
            maxlength: 200,
            messages: {
                required: 'Vui lòng nhập họ và tên!',
                maxlength: 'Độ dài họ tên phải nhỏ hơn 200 ký tự'
            }
        });
    }

    if ($('.txtMobile').length) {
        $('.txtMobile').rules("add", {
            required: true,
            maxlength: 50,
            number: true,
            messages: {
                required: 'Vui lòng nhập số điện thoại!',
                maxlength: 'Độ dài số điện thoại phải nhỏ hơn 50 ký tự!',
                number: 'Số điện thoại chỉ chứ ký tự số!'
            }
        });
    }

    if ($('.ddlProvince').length) {
        $('.ddlProvince').rules("add", {
            required: true,
            messages: {
                required: 'Vui lòng chọn Tỉnh/ Thành phố!'
            }
        });
    }

    if ($('.ddlBank').length) {
        $('.ddlBank').rules("add", {
            required: true,
            messages: {
                required: 'Vui lòng chọn ngân hàng!'
            }
        });
    }

    if ($('.txtAccountNo').length) {
        $('.txtAccountNo').rules("add", {
            required: true,
            maxlength: 20,
            messages: {
                required: 'Vui lòng nhập số tài khoản chuyển!',
                maxlength: 'Độ dài số tài khoản chuyển phải nhỏ hơn 20 ký tự!'
            }
        });
    }

    if ($('.txtOrderNo').length) {
        $('.txtOrderNo').rules("add", {
            required: true,
            maxlength: 50,
            messages: {
                required: 'Vui lòng nhập mã đơn hàng!',
                maxlength: 'Độ dài mã đơn hàng phải nhỏ hơn 50 ký tự!',
            }
        });
    }

    if ($('.txtAmount').length) {
        $('.txtAmount').rules("add", {
            required: true,
            maxlength: 13,
            number: true,
            messages: {
                required: 'Vui lòng nhập số tiền!',
                maxlength: 'Độ dài số tiền phải nhỏ hơn 13 ký tự!',
                number: 'Số tiền chỉ chứ ký tự số!'
            }
        });
    }

    if ($('.ddlWebsiteGroup').length) {
        $('.ddlWebsiteGroup').rules("add", {
            required: true,
            messages: {
                required: 'Vui lòng chọn nhóm website!'
            }
        });
    }

    if ($('.ddlWebsite').length) {
        $('.ddlWebsite').rules("add", {
            required: true,
            messages: {
                required: 'Vui lòng chọn website!'
            }
        });
    }

    
    if ($('.ddlOrigin').length) {
        $('.ddlOrigin').rules("add", {
            required: true,
            messages: {
                required: 'Vui lòng chọn xuất xứ!'
            }
        });
    }

    if ($('.txtLinkProduct').length) {
        $('.txtLinkProduct').rules("add", {
            required: true,
            maxlength: 1000,
            messages: {
                required: 'Vui lòng nhập link sản phẩm!',
                maxlength: 'Độ dài link sản phẩm phải nhỏ hơn 1000 ký tự!'
            }
        });
    }

    if ($('.txtPriceWeb').length) {
        $('.txtPriceWeb').rules("add", {
            required: true,
            maxlength: 13,
            messages: {
                required: 'Vui lòng nhập giá web!',
                maxlength: 'Độ dài giá web phải nhỏ hơn 13 ký tự!'
            }
        });
    }

    if ($('.txtPriceWebOff').length) {
        $('.txtPriceWebOff').rules("add", {
            required: true,
            maxlength: 13,
            messages: {
                required: 'Vui lòng nhập giá web off!',
                maxlength: 'Độ dài giá web off phải nhỏ hơn 13 ký tự!'
            }
        });
    }

    if ($('.txtShipUsa').length) {
        $('.txtShipUsa').rules("add", {
            required: true,
            maxlength: 13,
            messages: {
                required: 'Vui lòng nhập Ship Mỹ!',
                maxlength: 'Độ dài Ship Mỹ phải nhỏ hơn 13 ký tự!'
            }
        });
    }

    if ($('.txtShipUsaVn').length) {
        $('.txtShipUsaVn').rules("add", {
            required: true,
            maxlength: 13,
            messages: {
                required: 'Vui lòng nhập Ship Mỹ - Việt!',
                maxlength: 'Độ dài Ship Mỹ - Việt phải nhỏ hơn 13 ký tự!'
            }
        });
    }

    if ($('.txtTaxUsa').length) {
        $('.txtTaxUsa').rules("add", {
            required: true,
            maxlength: 13,
            messages: {
                required: 'Vui lòng nhập thuế Mỹ!',
                maxlength: 'Độ dài thuế Mỹ phải nhỏ hơn 13 ký tự!'
            }
        });
    }

    if ($('.txtTaxImport').length) {
        $('.txtTaxImport').rules("add", {
            required: true,
            maxlength: 13,
            messages: {
                required: 'Vui lòng nhập thuế nhập!',
                maxlength: 'Độ dài thuế nhập phải nhỏ hơn 13 ký tự!'
            }
        });
    }

    if ($('.txtEffort').length) {
        $('.txtEffort').rules("add", {
            required: true,
            maxlength: 13,
            messages: {
                required: 'Vui lòng nhập công!',
                maxlength: 'Độ dài công phải nhỏ hơn 13 ký tự!'
            }
        });
    }

    if ($('.txtQuantity').length) {
        $('.txtQuantity').rules("add", {
            required: true,
            maxlength: 13,
            messages: {
                required: 'Vui lòng nhập SL!',
                maxlength: 'Độ dài SL nhỏ hơn 13 ký tự!'
            }
        });
    }    

    if ($('.txtProductName').length) {
        $('.txtProductName').rules("add", {
            required: true,
            maxlength: 50,
            messages: {
                required: 'Vui lòng nhập tên sản phẩm!',
                maxlength: 'Độ dài tên sản phẩm nhỏ hơn 50 ký tự!'
            }
        });
    }

    if ($('.txtTitleFeedback').length) {
        $('.txtTitleFeedback').rules("add", {
            required: true,
            maxlength: 50,
            messages: {
                required: 'Vui lòng nhập tiêu đề thông điệp!',
                maxlength: 'Độ dài tiêu đề thông điệp phải nhỏ hơn 50 ký tự!'
            }
        });
    }

    if ($('.txtContentFeedback').length) {
        $('.txtContentFeedback').rules("add", {
            required: true,
            maxlength: 50,
            messages: {
                required: 'Vui lòng nhập nội dung thông điệp!',
                maxlength: 'Độ dài nội dung thông điệp phải nhỏ hơn 50 ký tự!'
            }
        });
    }

    if ($('.txtLink1').length) {
        $('.txtLink1').rules("add", {
            required: true,
            maxlength: 200,
            messages: {
                required: 'Vui lòng nhập link sản phẩm!',
                maxlength: 'Độ dài link sản phẩm nhỏ hơn 200 ký tự!'
            }
        });
    }
    
    if ($('.ddlGroupProduct').length) {
        $('.ddlGroupProduct').rules("add", {
            required: true,
            messages: {
                required: 'Vui lòng chọn loại sản phẩm!'
            }
        });
    }

    if ($('.ddlCategory').length) {
        $('.ddlCategory').rules("add", {
            required: true,
            messages: {
                required: 'Vui lòng chọn nhóm sản phẩm!'
            }
        });
    }

    $(".btnOrderLink").click(function () {
        if ($('.txtDate').val() == '') {
            $('.dateError').html('Ngày không được để trống!');
            return false;
        }
    });

    $('.doubleNumber').keypress(function (e) {
        var charCode = (e.which) ? e.which : e.keyCode;
        if (charCode > 31 && charCode != 45 && charCode != 46 && (charCode < 48 || charCode > 57)) {
            return false;
        } else {
            if (charCode == 45) {
                var pattern = /^-{1}/;
                var money = $('.doubleNumber').val();
                if (pattern.test(money)) {
                    return false;
                }
            }
        }
    });

    $('.doubleNumber').keyup(function () {
        var value = $(this).val();
        value = ReplaceAll(value, ',', '');
        value = numberFormat(value);
        $(this).val(value);
    });
    
    if ($('.rdLogin input:checked').attr("checked") == 'checked')
    {    
        $('.pnLogin').css("display","block");
        $('.pnRegister').css("display","none");
    }

    if ($('.rdRegister input:checked').attr("checked") == 'checked')
    {     
        $('.pnLogin').css("display","none");
        $('.pnRegister').css("display","block");
    }


    $('.rdLogin').click(function () {                  
        $('.pnLogin').css("display","block");
        $('.pnRegister').css("display","none");
        $('.rdLogin input:checked').attr("checked","checked");
        $('.rdRegister input:checked').removeAttr("checked");
    });

    $('.rdRegister').click(function () {
        $('.pnLogin').css("display","none");
        $('.pnRegister').css("display","block");
        $('.rdRegister input:checked').attr("checked","checked");
        $('.rdLogin input:checked').removeAttr("checked");
    });            
}

function numberFormat(nStr) {
    nStr += '';
    x = nStr.split(',');
    x1 = x[0];
    x2 = x.length > 1 ? ',' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1))
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    return x1 + x2;
}

function ReplaceAll(Source, stringToFind, stringToReplace) {
    var temp = Source;
    var index = temp.indexOf(stringToFind);
    while (index != -1) {
        temp = temp.replace(stringToFind, stringToReplace);
        index = temp.indexOf(stringToFind);
    }
    return temp;
}