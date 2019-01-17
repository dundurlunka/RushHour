$(document).ready(function () {
    let startDate = $("#datepickerStartDate").val();
    let endDate = $("#datepickerEndDate").val();

    initDatepickers();

    if (startDate != '01-Jan-01 12:00:00 AM') {
        let parsedStartDate = new Date(Date.parse(startDate));
        let parsedEndDate = new Date(Date.parse(endDate));
        console.log(parsedStartDate);
        $('#datepickerStartDate').data('daterangepicker').setStartDate(parsedStartDate);
        $('#datepickerEndDate').data('daterangepicker').setStartDate(parsedEndDate);
    }
})

function initDatepickers() {
    $("#datepickerStartDate").daterangepicker({
        singleDatePicker: true,
        timePicker: true,
        startDate: moment().startOf('hour').add(1, 'hour'),
        timePicker24Hour: true,
        opens: 'left',
        "locale": {
            "format": "DD/MM/YYYY HH:mm",
            "separator": " - ",
            "applyLabel": "Apply",
            "cancelLabel": "Cancel",
            "fromLabel": "From",
            "toLabel": "To",
            "customRangeLabel": "Custom",
            "weekLabel": "W",
            "daysOfWeek": [
                "Su",
                "Mo",
                "Tu",
                "We",
                "Th",
                "Fr",
                "Sa"
            ],
            "monthNames": [
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December"
            ],
            "firstDay": 1
        },
    });

    $("#datepickerEndDate").daterangepicker({
        singleDatePicker: true,
        timePicker: true,
        startDate: moment().startOf('hour').add(2, 'hour'),
        timePicker24Hour: true,
        opens: 'left',
        "locale": {
            "format": "DD/MM/YYYY HH:mm",
            "separator": " - ",
            "applyLabel": "Apply",
            "cancelLabel": "Cancel",
            "fromLabel": "From",
            "toLabel": "To",
            "customRangeLabel": "Custom",
            "weekLabel": "W",
            "daysOfWeek": [
                "Su",
                "Mo",
                "Tu",
                "We",
                "Th",
                "Fr",
                "Sa"
            ],
            "monthNames": [
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December"
            ],
            "firstDay": 1
        },
    });
}