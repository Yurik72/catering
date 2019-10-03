
$(document).on('change', "input[cp-name|='dayselect']", function () { 
   
        if (this.checked) {
             //alert("click");
        }
         //var dishid = parseInt($(this).parent().find("#item_DishId").val());
        // var dishdate = $(this).parent().find("#item_Date").val();
         var dishid = parseInt($(this).parent().find("#item_DishId").val());
         var dishdate = $(this).parent().find("#item_Date").val();
        //alert(JSON.stringify({ DishId: dishid, daydate: 0, enabled: this.checked }));
        $.ajax({
            type: "POST",
            data: { DishId: dishid, daydate: dishdate, enabled: this.checked },

            url: "/DayDishes/SaveDayDish",
            success: function (data) {
               // alert(data);
            }
        });
    });