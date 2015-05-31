if (!Modernizr.inputtypes.date) {

    $(function () {

        $(".datecontrol").datepicker({ format: "dd.mm.yyyy" });

    });

}