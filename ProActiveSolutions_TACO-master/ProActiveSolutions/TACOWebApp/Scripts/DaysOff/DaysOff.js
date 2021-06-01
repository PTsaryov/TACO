$(document).ready(function () {
    ChangeButtonColorsAccordingToReason();
    ChangeColorForWeekends();
    GetCodes();

    //Listens for the click to launch the modal
    $('[id*="_ButtonLaunchModal_"]').click(function () {
        var absenceCodes;
        ShowModal($(this));
        var btnValue = $(this).val();
        //Method initialized the value the user sees
        //on the radio button list when they open the modal.
        RadioButtonInitialValue(btnValue);
    });

    //When modal is on show
    $('#myModal').on('show.bs.modal', function (e) {
        //declaring variables
        var origin = $(e.relatedTarget);
        var formContainer = $('#form-container');
        var radioButtonContainer = $('#radio-buttons-container');
        var bookButton = $('#Book');
        var nextButton = $('#Next');
        var errors = $('#errors');
        var errorContainer = $('<div id="error-container">')
        var employeeId = e.relatedTarget.closest("tr").find("td:first input:first").val();

        //BUTTON EVENTS STARTS HERE
        $("#Save").off('click');
        $("#Save").click(origin, updateButton);
        //Function to change the value of the button where the click came from
        //if the user changes the value in the radio button list
        function updateButton(event) {
            var answer;
            var newAbsenceCode;
            var newValue = $("input[type='radio'][name='code']:checked").val();
            var origin = event.data;
            var timesheetDetailsId = origin.data('button');
            newAbsenceCode = newValue;
            //var timesheetDetailsId = origin.attr("data-button").toString();


            //checks if the user changed the value in the radio button list
            //if the new value is equal to none, it will delete that day off record.
            if (newValue === " ") {
                answer = confirm("Selecting NONE and saving changes will delete this day off. Are you sure you want to continue?");
                if (answer) {
                    $.ajax(
                        {
                            url: "BookDaysOff.aspx/DeleteDayOff",
                            type: 'POST',
                            data: JSON.stringify({ Id: timesheetDetailsId }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function () {
                                alert("Successfully Deleted");
                                location.reload(true);
                            },
                            error: function (xhr) {
                                var exception = JSON.parse(xhr.responseText);
                                alert(exception.Message);

                            }
                        });

                    $('.modal').modal('hide');

                }
            } else {
                //if the new value is anothe absence code, it will update the day off
                answer = confirm("Saving changes will update the reason for your day off. Are you sure you want to continue?");
                if (answer) {
                    $.ajax(
                        {
                            url: "BookDaysOff.aspx/UpdateTimesheetDetails",
                            type: 'POST',
                            data: JSON.stringify({ Id: timesheetDetailsId, Code: newValue, EmployeeId: employeeId }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function () {
                                alert("Successfully Updated");
                                location.reload(true);

                            },
                            error: function (xhr, status, error) {
                                var exception = JSON.parse(xhr.responseText);
                                alert(exception.Message);
                            }
                        });
                    $('.modal').modal('hide');

                }

                //maybe throw validation here if they do not have enough reason for their day off
            }


        }

        //When the user clicks on an empty button
        //a 'NEXT' button is shown instead of the regular 'Save Changes'
        //This listens for the clicks on the next button
        $("#Next").off('click');
        $('#Next').click(function (evt) {
            clickOrigin = $(evt.target);
            var radioButtonSelected = $("input[type='radio'][name='code']:checked").val();
            var dayOffForm;
            //checks the value of the radio button selected.
            //Depending on the value, different forms will be rendered.
            if (radioButtonSelected === " ") {
                //TODO ONLY RUNS ONCE
                errorContainer.append(['<p><strong>You must select a reason for your absence.</strong></p>'].join(' '));
                errors.html(errorContainer);

            } else if (radioButtonSelected === "ETO") {
                clickOrigin.addClass('hidden');
                bookButton.removeClass('hidden');
                formContainer.removeClass('hidden');
                radioButtonContainer.addClass('hidden');
                errors.addClass('hidden');
                $('.modal-title').html('Please choose one:');
                 dayOffForm = $('<div id="radio-button-group">');
                //I DONT KNOW WHY THESE LOOK HUGE
                dayOffForm.append([
                    '<div class="selected-reason>',
                    '<p class="reason"><strong>You have selected: </strong>' + radioButtonSelected + '</p>',
                    '</div>',
                    '<div class="radio-button">',
                    '<input  id="AM" value="AM" name="am-or-pm" type="radio"/>',
                    '<label class="control-label" for="AM">AM</label>',
                    '</div>',
                    '<div class="radio-button">',
                    '<input  id="PM" value="PM" name="am-or-pm" type="radio"/>',
                    '<label class="control-label" for="PM">PM</label>',
                    '</div>',
                    '<div class="radio-button">',
                    '<input id="FullDay" value="FullDay" name="am-or-pm" type="radio"/>',
                    '<label class="control-label" for="FullDay">Full day</label>',
                    '</div>'
                ]);
                formContainer.html(dayOffForm);
            } else {

                clickOrigin.addClass('hidden');
                bookButton.removeClass('hidden');
                formContainer.removeClass('hidden');
                radioButtonContainer.addClass('hidden');
                errors.addClass('hidden');
                $('.modal-title').html('Please fill out the form below:');
                dayOffForm = $('<div id="absence-form">');
                dayOffForm.append([
                    '<div class="selected-reason>',
                    '<p class="reason"><strong>You have selected: </strong>' + radioButtonSelected + '</p>',
                    '</div>',
                    '<div class="form-group form-inline">',
                    '<label class="control-label" for="start-date"><strong>Start date</strong><span class="required"> *</span></label>',
                    '<input class="form-control" id="start-date" name="start-date"  type="text" />',
                    '</div>',
                    '<div class="form-group form-inline">',
                    '<label class="control-label" for="end-date"><strong>End date</strong><span class="required"> *</span></label>',
                    '<input class="form-control" id="end-date" name="end-date"  type="text"/>',
                    '</div>'

                ]);

                formContainer.html(dayOffForm);

                var id = origin.attr('id');
                var btnId = id.slice(-2);
                var btnDay = btnId.replace(/[_\W]+/g, " ").trim();
                var dayNumber = (parseInt(btnDay, 10)) + 1;
                //Turning inputs to datepickers.
                $("#start-date").datepicker({
                    stepMonths: 0 //inihibits the user from booking a day off in the wrong calendar.
                });
                $('#start-date').datepicker("setDate", new Date(GetSelectedYear(), GetSelectedMonth(), dayNumber));
                $("#end-date").datepicker({
                    stepMonths: 0
                });
                $('#end-date').datepicker("setDate", new Date(GetSelectedYear(), GetSelectedMonth(), dayNumber));
                $("#end-date").attr('readOnly', 'true');
                $("#start-date").attr('readOnly', 'true');

            }

        });

        //listens for the close click in the modal to reset all the forms
        $('#Close').click(function () {
            formContainer.addClass('hidden');
            radioButtonContainer.removeClass('hidden');
            nextButton.removeClass('hidden');
            bookButton.addClass('hidden');
        })

        //listen for bookbutton and do validation
        $("#Book").off('click');
        $('#Book').click(function (evt) {
            var origin = e.relatedTarget.attr('id');
            var absenceCodeSelected = $("input[type='radio'][name='code']:checked").val();
            if (absenceCodeSelected === "ETO") {
                if ($("input[name='am-or-pm']:checked").length) {
                    var month = GetSelectedMonth();
                    var year = GetSelectedYear();
                    var answer;
                    var originId = origin.slice(-2);
                    var day = originId.replace(/[_\W]+/g, " ").trim();
                    var dayNumber = parseInt(day, 10) + 1;
                    var date = new Date(year, month, dayNumber);
                    var time = $("input[name='am-or-pm']:checked").val();
                    var startTime, endTime;

                    if (time === "AM") {
                        startTime = new Date(date.setHours(date.getHours() + 8));
                        endTime = new Date(date.setHours(date.getHours() + 4));
                        answer = confirm("Saving changes will book a day off using ETO. Are you sure you want to continue?");

                        if (answer) {
                            //var tempObj = { Code: absenceCodeSelected, Time: time, Start:};
                            $.ajax(
                                {
                                    url: "BookDaysOff.aspx/CreateNewDayOff",
                                    type: 'POST',
                                    data: JSON.stringify({ Code: absenceCodeSelected, StartTime: startTime, EndTime: endTime, EmployeeId: employeeId }),
                                    dataType: "json",
                                    contentType: "application/json; charset=utf-8",
                                    success: function () {
                                        alert("Successfully booked a day off.");
                                        location.reload(true);

                                    },
                                    error: function (xhr) {
                                        var exception = JSON.parse(xhr.responseText);
                                        alert(exception.Message);

                                    }
                                });
                            $('.modal').modal('hide');
                        }
                        else {
                            //error here
                        }

                    } else if (time === "PM") {
                        startTime = new Date(date.setHours(date.getHours() + 13));
                        endTime = new Date(date.setHours(date.getHours() + 3));

                        answer = confirm("Saving changes will book a day off using ETO. Are you sure you want to continue?");
                        if (answer) {
                            $.ajax(
                                {
                                    url: "BookDaysOff.aspx/CreateNewDayOff",
                                    type: 'POST',
                                    data: JSON.stringify({ Code: absenceCodeSelected, StartTime: startTime, EndTime: endTime, EmployeeId: employeeId }),
                                    dataType: "json",
                                    contentType: "application/json; charset=utf-8",
                                    success: function () {
                                        alert("Successfully booked a day off.");
                                        location.reload(true);

                                    },
                                    error: function (xhr) {
                                        var exception = JSON.parse(xhr.responseText);
                                        alert(exception.Message); alert(JSON.stringify(xhr));

                                    }
                                });
                            $('.modal').modal('hide');
                        }
                    } else {
                        startTime = new Date(date.setHours(date.getHours() + 8));
                        endTime = new Date(date.setHours(date.getHours() + 8));

                        answer = confirm("Saving changes will book a day off using ETO. Are you sure you want to continue?");
                        if (answer) {
                            $.ajax(
                                {
                                    url: "BookDaysOff.aspx/CreateNewDayOff",
                                    type: 'POST',
                                    data: JSON.stringify({ Code: absenceCodeSelected, StartTime: startTime, EndTime: endTime, EmployeeId: employeeId }),
                                    dataType: "json",
                                    contentType: "application/json; charset=utf-8",
                                    success: function () {
                                        alert("Successfully booked a day off.");
                                        location.reload(true);

                                    },
                                    error: function (xhr) {
                                        var exception = JSON.parse(xhr.responseText);
                                        alert(exception.Message);

                                    }
                                });
                            $('.modal').modal('hide');

                            //maybe off here
                        }
                    }

                }
            } else {

                var startDate, endDate, today;
                startDate = new Date($('#start-date').val());
                endDate = new Date($('#end-date').val());
                today = new Date();

                var startTime = new Date(startDate.setHours(startDate.getHours() + 8));
                var endTime = new Date(endDate.setHours(startDate.getHours() + 8));

                console.log(startTime, endTime);
                if (startDate.getFullYear() < today.getFullYear()) {
                    //append errors
                    console.log("can't book a day off in the past");
                }
                if (startDate > endDate) {
                    //does not append yet
                    errorContainer.append(['<p><strong>You must select a reason for your absence.</strong></p>'].join(' '));
                    errors.html(errorContainer);
                } else {
                    answer = confirm("Saving changes will book a day off. Are you sure you want to continue?");
                    if (answer) {
                        $.ajax(
                            {
                                url: "BookDaysOff.aspx/CreateNewDayOff",
                                type: 'POST',
                                data: JSON.stringify({ Code: absenceCodeSelected, StartTime: startTime, EndTime: endTime, EmployeeId: employeeId }),
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                success: function () {
                                    alert("Successfully booked a day off.");
                                    location.reload(true);

                                },
                                error: function (xhr) {
                                    var exception = JSON.parse(xhr.responseText);
                                    alert(exception.Message);

                                }
                            });
                        $('.modal').modal('hide');
                    }
                }

            }
        });

    });

    function GetCodes() {
        $.ajax({
            type: "POST",
            url: "BookDaysOff.aspx/GetAbsenceCodes",
            contentType: "application/json; charset=utf-8",
            dataType: "json", // Data returned as JSON string
            success: function (response) {
                absenceCodes = JSON.parse(response.d);
                RadioButtonListInsideModal(absenceCodes);


            },
            // if failed to retriev will send detailed error message
            failure: function () {
                var exception = JSON.parse(xhr.responseText);
                alert(exception.Message);
            }
        });
        return false;
    }
    function ShowModal(buttonClicked) {
        $('#myModal').modal('show', buttonClicked);
    };
    //Jquery functin to build radio button list and set initial value
    function RadioButtonListInsideModal(absenceCodes) {
        $('#myModal').on('show.bs.modal', function (event) {
            var radioButtonContainer = $('#radio-buttons-container');
            var group = $('<div id="radio-button-group">');
            console.log(absenceCodes);
            $.each(absenceCodes, function (key, val) {
                group.append(['<div class="radio-buttons">',
                    '<input type="radio" id="' + val.Code + '" name="code" value="' + val.Code + '">',
                    '<label for="' + val.Code + '">' + val.Code + '</label>',
                    '</div>']);
            })
            group.append(['<div class="radio-buttons">',
                '<input type="radio" id=" " name="code" value=" ">',
                '<label for="None"> None </label>',
                '</div>']);
            radioButtonContainer.html(group);
            var origin = $(event.relatedTarget);
            var modalSave = $('#Save');
            var modalNext = $('#Next');

            if (origin.val() === " ") {

                modalSave.addClass('hidden');
                modalNext.removeClass('hidden');

            }
            else {

                modalSave.removeClass('hidden');
                modalNext.addClass('hidden');
            }

        });

    };

    //sets value of radio button list to the absence code
    function RadioButtonInitialValue(btnValue) {
        $('#radio-button-group').find('input:radio').each(function () {
            if (this.id === btnValue) {
                $(this).prop('checked', true);
            }

        });
    }
    //set colors
    function ChangeButtonColorsAccordingToReason() {

        $('input[data-id="launchModal"]').each(function () {
            if ($(this).val() === 'ETO') {
                $(this).css('background', ' #fe8a71');
            }
            else if ($(this).val() === 'VAC') {
                $(this).css('background', '#f6cd61');
            }
            else if ($(this).val() === 'FWT') {
                $(this).css('background', '#35a79c');
            }
            else if ($(this).val() === 'BNK') {
                $(this).css('background', '#e0a899');
            } else if ($(this).val() === 'OFF') {
                $(this).css('background', '#54647c');
            }

        });


    }
    function GetSelectedMonth() {
        var month = ($("#MainContent_DropdownMonths").prop('selectedIndex'));
        return month;

    }
    function GetSelectedYear() {
        var year = $("#MainContent_DropdownYears option:selected").text();
        return year;
    }

    function ChangeColorForWeekends() {

        //console.log(year);
        //var isWeekend = (day === 6) || (day === 0);
        //var txt = 'Header 2';
        //var column = $('table tr th').each(function () {
        //     //return $(this).text() === txt;

        //    var date = new Date(year + month + $(this).text());
        //    console.log(date);
        //}).index();
        //$('table tr:first span').each(function (idx, obj) {

        //    var day = ($('table tr:first span').text());
        //    //return $(this).text() === txt;
        //    //console.log($(this));
        //    ////var day = $(this).find('span').text();
        //    //var date = new Date(year,month,day);

        //    //console.log(year,month,day);
        //});


        //if (column > -1) {
        //    $('table tr').each(function () {
        //        $(this).find('td').eq(column).css('background-color', '#eee');
        //    });
        //}


    }


});