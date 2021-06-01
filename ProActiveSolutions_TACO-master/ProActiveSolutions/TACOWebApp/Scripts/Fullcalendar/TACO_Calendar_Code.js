
$(document).ready(function () {
    //Global variables
    var $calendar = $('#calendar');
    var projectAndOvertime = "";
    const DATE_FORMAT = "DD-MM-YYYY HH:mm:ss";

    /**
     * Initial data request from database
     * Created By: Anton Drantiev
     * Created On: February 16, 2019
     * Modified By: Anton Drantiev
     * Modified On: April 06, 2019
     * 
     * @function getAllEventsInitCalendar() - Initialize calendar and All currently saved projects and overtime requests.
     * for the logged-in user.
     * @param selectedIdValue - Employee ID Passed to code behind by ajax request. 
     * @function initCalendar() - Creates the clendar object with all its properties.
     * accepts 1 parameter: 
     * @param projectAndOvertime - Array to populte the calendar with events if 
     * no parameter passed the calendar will strat empty.
     * @function saveButtonClick()  - Inithializes the onClick event listner for the save events button.
     * @function ajaxReadData(URL,firstFunction,employeeId,secondFunction,errorMessage) - Retrives the data about the 
     * project/overtime the user assigned to at the moment and populates them in the draggable panel at
     * the front page. Accepts 5 parameters:
     * @param URL - The address to the method at the code behind (Default.aspx.cs page) with the method name 
     * right after it (e.g. "Default.aspx/GetAssignedProjects").
     * @param firstFunction - @function generateEventsElement/generateRequestsElems - Generates all the elements in the html page.
     * @param employeeId - The id of the logged in employee.
     * @param secondFunction - @function initExtEvents/initAbbsences - Assigns the front end properties to each events
     * @param errorMessage - Desired error message in case of failure.
     */

    getAllEventsInitCalendar();

    function getAllEventsInitCalendar() {
        // Check drop down if logged in as employye will be undefiend
        // and will load the employees calendar from the code behind
        var ddlEmployees = $("[id*=DropdonEmployeeLIst]");
        selectedIdValue = ddlEmployees.val();
       
        if (selectedIdValue === null || selectedIdValue === undefined) {
            selectedIdValue = "0";
        }
        console.log(selectedIdValue);
        $.ajax({
            type: "POST",
            url: "Default.aspx/GetProjectsAndOvertimes",
            data: JSON.stringify({ id: selectedIdValue }),
            contentType: "application/json; charset=utf-8",
            dataType: "json", // Data returned as JSON string
            success: function (response) {
                projectAndOvertime = JSON.parse(response.d);//parse JSON to array
                // Init Calendar
                initCalendar(projectAndOvertime);
                // Remove web cache, add new source and render
                $('#calendar').fullCalendar('removeEvents');
                $('#calendar').fullCalendar('addEventSource', projectAndOvertime);
                $('#calendar').fullCalendar('rerenderEvents');
                // Init event listner to save button
                saveButtonClick();
                // Recalculate requested overtime
                ajaxReadData("Default.aspx/CalculateOvertimeTotals", generateOvertimeToatls, selectedIdValue, null, "An error occured while calculating overtime totals");
                // Retriev and populate projects
                ajaxReadData("Default.aspx/GetAssignedProjects", generateEventsElements, selectedIdValue, initExtEvents, "An error occured while retrieving assigned projects data");
                // Retriev and populate overtimes
                ajaxReadData("Default.aspx/GetAssignedOvertimes", generateRequestsElem, selectedIdValue, initOvertimes, "An error occured while retrieving overtime data");

            },
            // if failed to retriev will send detailed error message
            failure: function () {
                alert("An error occured while loading your calendar");
            }
        });
    }

    /**
     * On click listner for employees dropd down
     * that will be seen only if logged in as global admin
     * Created By: Anton Drantiev
     * Created On: April 02, 2019
     * Modified By: Anton Drantiev
     * Modified On: April 06, 2019
     */

    $("[id*=fetchEmployee]").click(function () {
        var ddlEmployees = $("[id*=DropdonEmployeeLIst]");
        var selectedIdValue = ddlEmployees.val();
        console.log(selectedIdValue);
        $.ajax({
            type: "POST",
            url: "Default.aspx/GetProjectsAndOvertimes",
            data: JSON.stringify({ id: selectedIdValue }),
            contentType: "application/json; charset=utf-8",
            dataType: "json", // Data returned as JSON string
            success: function (response) {
                projectAndOvertime = JSON.parse(response.d);//parse JSON to array
                // Init Calendar
                initCalendar(projectAndOvertime);
                // Remove web cache, add new source and render
                $('#calendar').fullCalendar('removeEvents');
                $('#calendar').fullCalendar('addEventSource', projectAndOvertime);
                $('#calendar').fullCalendar('rerenderEvents');
                // Init event listner to save button
                saveButtonClick();
                // Recalculate requested overtime
                ajaxReadData("Default.aspx/CalculateOvertimeTotals", generateOvertimeToatls, selectedIdValue, null, "An error occured while calculating overtime totals");
                // Retriev and populate projects
                ajaxReadData("Default.aspx/GetAssignedProjects", generateEventsElements, selectedIdValue, initExtEvents, "An error occured while retrieving assigned projects data");
                // Retriev and populate overtimes
                ajaxReadData("Default.aspx/GetAssignedOvertimes", generateRequestsElem, "0", initOvertimes, "An error occured while retrieving overtime data");

            },
            // if failed to retriev will send detailed error message
            failure: function () {
                alert("An error occured while loading your calendar");
            }
        });
        return false;
    });

    /**
    * Fullcalendar initialization.
    * Created By: Anton Drantiev
    * Created On: February 16, 2019
    * Modified By: Anton Drantiev
    * Modified On: April 02, 2019
    * 
    * @param {projectAndOvertime} - Array which contains all currnetly save projects and overtime data.
    * @param {dateFormat} - Global variebal that assigns the same date time format to any date time parse, 
    * the format is "DD-MM-YYYY HH:mm:ss"
    */

    function initCalendar(projectAndOvertime) {
        $calendar.fullCalendar({
            defaultView: 'agendaWeek', // Calendar type (month, week, day)
            header: {
                left: 'prev,next today', // Buttons to navigate between  previous/next month, week or day, 
                // and 'today' buttn to go back to current days' week or month.
                center: 'title',         // Title if in month selected will diplay 'month name, year' = 'April 2019',
                // week selected 'month name, date number of first day of the week - 
                // month name, date number of last day of the week' = 'Mar 31 - Apr 6, 2019'
                // day selected 'month name, day date number, year' = 'March 31, 2019'.
                right: 'month,agendaWeek,agendaDay' // Butons to chnge the display of the calendar 'month' = fullmonth view
                // 'week' = fullweek view and 'day' full day view.
            },
            editable: true, // Events can be edited within the calendar.
            droppable: true, // this allows things to be dropped onto the calendar.
            drop: function (date) {
                var endTime = moment(date).add(2, 'hours'); // Dropped event end time set to 2 hours from start
                var droppedEvent = $(this).data('event');
                var newId = droppedEvent.id += 1; // Increment the temporary id by 1, use to avoid duplicate temp id's
                droppedEvent = { // Set dropped event properties that are inherited from the event object in the panel
                    id: newId,
                    title: droppedEvent.title,
                    start: moment.utc(date).format(DATE_FORMAT), // Using moment.js open source library to convert date to UTC time format.
                    end: moment.utc(endTime).format(DATE_FORMAT)

                };

                $calendar.fullCalendar('updateEvent', droppedEvent); // Update event data (that exist in the front end) after dropped.

            },
            eventLimit: false, // If number is assigned that will limit the amount of events diplayed.
            eventSources: [{ events: projectAndOvertime }], //Events data source (string array) passed to the calendar for rendering (can be more than one source).
            allDaySlot: false, // To avoid events assigned for full day, if true the assigned event will be a 24 hour event.
            slotDuration: '00:15:00',
            slotLabelInterval: "00:15:00",
            eventClick: function (event, element) { // Triggers a modal popup display on event click.
                var endTime; // Validate that events has end time, if not assign 2 hours from start time
                if (event.end === null) {
                    endTime = moment(event.start).add(2, 'hours');
                } else {
                    endTime = event.end;
                }

                $('.modal').modal('show');// Display modal.
                // Set the values input boxes in the modal to the clicked event values.
                $('.modal').find('#title').val(event.title).attr("disabled", "disabled"); //Title is not editable
                $('.modal').find('#starts-at').val(moment(event.start).format("HH:mm"));
                $('.modal').find('#ends-at').val(moment(endTime).format("HH:mm"));

                deleteEvetModal(event); //Trigger on click function when delete button clicked, and pass the clicked event data.
                saveEventModal(event); //Trigger on click function when save button clicked, and pass the clicked event data.
            },
            height: 800, // Calendar hieght in pixels.
            eventColor: '#f37736'// Default event color (if color doesn't load from db)

        });
    }

    /**
    * Generate event elements in the draggable panel
    * and assign all attributes and class 
    * Created By: Anton Drantiev
    * Created On: February 18, 2019
    * Modified By: Anton Drantiev
    * Modified On: March 01, 2019
    * 
    * @param {[currentEmployeeProjects]} - Current employees projects array.
    */

    function generateEventsElements(currentEmployeeProjects) {
        $(".events").html(''); // Clear previous events
        var eventsContainer = $(".events");// Targeted div
        var text = "";
        var blockDiv;  // Used in the for loop
        // Loop through projects
        $(currentEmployeeProjects).each(function (i, item) {
            var bgColor = 'background: ' + item.color + ';';//Background color

            blockDiv = $("<div>").addClass("fc-event").attr('id', item.ProjectId)
                .attr('style', bgColor).attr('data-color', item.color).appendTo(eventsContainer);
            text = item.title;
            $('<span>').text(text).appendTo(blockDiv);
        });
    }

    /**
    * Generate overtime elements in the draggable panel
    * and assign all attributes and class
    * Created By: Anton Drantiev
    * Created On: February 26, 2019
    * Modified By: Anton Drantiev
    * Modified On: March 16, 2019
    * 
    * @param {[currentEmployeeOvertimes]} - Current employees projects array.
    */

    function generateRequestsElem(currentEmployeeOvertimes) {
        $(".requests").html('');// Clear previous events
        var container = $(".requests");// Targeted div
        var text = "";
        var blockDiv;  // Used in the for loop
        // Loop through overtimes
        $(currentEmployeeOvertimes).each(function (i, item) {
            var bgColor = 'background: ' + item.color + ';';

            blockDiv = $("<div>").addClass("fc-event").attr('id', item.id)
                .attr('style', bgColor).attr('data-color', item.color).appendTo(container);
            text = item.title;
            $('<span>').text(text).appendTo(blockDiv);
        });
    }

    /**
    * Assigns the front end properties to each event(project).
    * The properties will be held within each event object
    * Created By: Anton Drantiev
    * Created On: February 18, 2019
    * Modified By: Anton Drantiev
    * Modified On: March 01, 2019
    * 
    * @param {'.events .fc-event'} - Targeted element.
    * @type {eventObject} - Consists of: title, id (temporary), overtimeId, 
    * attendance, stick (makes sure it stays where is was dropped) and color.
    * @type {draggble} - Makes the object draggable from the pannel.
    */

    function initExtEvents() {
        $('.events .fc-event').each(function () {
            // Store eventObject data so the calendar knows to render an event upon drop
            $(this).data('event', {
                title: $.trim($(this).text()), // Use the element's text as the event title
                id: parseInt($(this).attr('id')), // Use for deleting events that are with 
                // undefined id and for events that have not been saved yet to database
                ProjectId: parseInt($(this).attr('id')),
                flag: true, // True if event was not saved to database
                stick: true,
                color: $(this).data("color")
            });
            // Make the event draggable using jquery ui
            $(this).draggable({
                zIndex: 999,
                revert: true,      // will cause the event to go back to its
                revertduration: 0  // original position after the drag and the duration back to 0
            });
        });
    }

    /**
    * Assigns the front end properties to each event(overtime).
    * The properties will be held within each event object
    * Created By: Anton Drantiev
    * Created On: February 26, 2019
    * Modified By: Anton Drantiev
    * Modified On: March 18, 2019
    * 
    * @param {'.requests .fc-event'} - Targeted element.
    * @type {eventObject} - Consists of: title, id (temporary), overtimeId, 
    * flag, stick (makes sure it stays where is was dropped) and color.
    * @type {draggble} - Makes the object draggable from the pannel.
    */

    function initOvertimes() {
        $('.requests .fc-event').each(function () {
            // Store eventObject data so the calendar knows to render an event upon drop
            $(this).data('event', {
                title: $.trim($(this).text()), // Use the element's text as the event title
                id: parseInt($(this).attr('id')),// Use for deleting events that are with 
                // undefined id and for events that have not been saved yet to database
                overtimeId: parseInt($(this).attr('id')),
                overtime: true, // True if event was not saved to database
                stick: true,
                color: $(this).data("color")
            });
            // Make the event draggable using jquery ui
            $(this).draggable({
                zIndex: 999,
                revert: true,      // Will cause the event to go back to its
                revertduration: 0  // original position after the drag and the duration back to 0
            });
        });
    }

    /**
    * Saves to database all updated and newly created events.
    * Created By: Anton Drantiev
    * Created On: March 06, 2019
    * Modified By: Anton Drantiev
    * Modified On: March 18, 2019
    * 
    * @function saveEvents() - triggered when the save button clicked, will loop
    * through all the events in the callendar and sort it before sending to back end.
    * Using off/on click methods to unload any cache from the previous request
    */

    function saveButtonClick() {
        $('.row-buttons').off('click', '#save').on('click', '#save', function () {
            // Get all the events currently in the calendar.
            var events = $calendar.fullCalendar('clientEvents');
            var listToCreate = [];
            var listToUpdate = [];
            var listToRequest = [];
            var endTime;
            // Loop and sort the events
            $(events).each(function () {
                if (this.overtime && this.flag === undefined) { // Check if overtime flag is true and project flag is false (add it as overtime event to create).
                    if (this.end === null) {
                        endTime = moment(this.start._d).add(2, 'hours'); // If dropped but end time was not assigned, add 2 hours from strat time.
                        endTime = moment.utc(endTime).format(DATE_FORMAT);
                    } else {
                        endTime = moment.utc(this.end._d).format(DATE_FORMAT);
                    }
                    listToRequest.push(
                        {
                            ProjectId: this.overtimeId,
                            overtime: false,
                            start: moment.utc(this.start._d).format(DATE_FORMAT),
                            end: endTime
                        });
                }
                if (!this.flag && !this.overtime) { // Check if project flag is false and overtime flag is false (add it as an event to update).
                    listToUpdate.push(
                        {
                            id: this.id,
                            start: moment(this.start._i).format(DATE_FORMAT),
                            end: moment(this.end._i).format(DATE_FORMAT)
                        });
                }
                if (this.flag && this.overtime === undefined) { // Check if project flag is true and overtime flag is false (add it as project event to create).
                    if (this.end === null) {
                        endTime = moment(this.start._d).add(2, 'hours');
                        endTime = moment.utc(endTime).format(DATE_FORMAT);
                    } else {
                        endTime = moment.utc(this.end._d).format(DATE_FORMAT);
                    }
                    listToCreate.push(
                        {
                            ProjectId: this.ProjectId,
                            flag: false,
                            start: moment.utc(this.start._d).format(DATE_FORMAT),
                            end: endTime
                        });
                }
            });
            // check drop down to update/create the selected employee calandar
            var ddlEmployees = $("[id*=DropdonEmployeeLIst]");
            var selectedId = ddlEmployees.val();
            if (selectedId === undefined) {
                selectedId = "0";
            }
            //prompt to confirm before changes take place
            var confirmPrompt = confirm("Saving changes");
            if (confirmPrompt) {
                //send to code behind to create new overtime events
                ajaxWriteData("Default.aspx/CreateNewRequest", JSON.stringify(listToRequest), selectedId, "An error occured while creating new request");
                //send to code behind to create new project events
                ajaxWriteData("Default.aspx/CreateNewEvents", JSON.stringify(listToCreate), selectedId, "An error occured while adding a new project");
                //send to code behind to update
                ajaxWriteData("Default.aspx/UpdateEvents", JSON.stringify(listToUpdate), selectedId, "An error occured while updating a project");
                alert("Saved successfully");

            } else {
                //clear all changes
                $(events).each(function () {
                    if (this.id <= 100) {
                        $calendar.fullCalendar('removeEvents', this.id);
                    }
                });
            }

        });
    }


    /* Modal functionalty
    -----------------------------------------------------------------*/

    /**
    * Saves changes made in the modal but only at the front end
    * Created By: Anton Drantiev
    * Created On: March 16, 2019
    * Modified By: Anton Drantiev
    * Modified On: April 06, 2019
    * 
    * @function saveEventModal() - Will save updated start and end time for the selected event.
    * @param selectedEvent - Events array passed from initCalendar when event is selected.
    * Using off/on click methods to unload any cache from the previous request.
    */

    function saveEventModal(selectedEvent) {
        $('.modal').off('click', '#save-event').on('click', '#save-event', function () {
            var eventData = ""; // Temp event holder
            var title = $('#title').val(); // Get title from modal
            // Adding time to the selected date(reformated for IE11)
            // and creating date + time formated using MomentJs, for the browser and calendar to recognize
            var currdate = moment(selectedEvent.start).format("MM-DD-YYYY");
            var start = $('#starts-at').val();
            var end = $('#ends-at').val();
            var toDateStart = new Date(currdate.toString() + " " + start);
            var toDateEnd = new Date(currdate.toString() + " " + end);
            var combinedStart = moment(toDateStart).format();
            var combinedEnd = moment(toDateEnd).format();
            // Time validation
            var timeIsValid = validateTime(start, end);
            console.log(selectedEvent);
            if (title && timeIsValid && selectedEvent.id < 100) {
                if (selectedEvent.ProjectId) {
                    eventData = {
                        id: selectedEvent.id,
                        ProjectId: selectedEvent.ProjectId,
                        flag: true,
                        title: selectedEvent.title,
                        start: combinedStart,
                        end: combinedEnd,
                        color: selectedEvent.color
                    };
                } else {
                    eventData = {
                        id: selectedEvent.id,
                        overtimeId: selectedEvent.overtimeId,
                        overtime: true,
                        title: selectedEvent.title,
                        start: combinedStart,
                        end: combinedEnd,
                        color: selectedEvent.color
                    };
                }

                // Refetching events
                $calendar.fullCalendar('removeEvents', eventData.id);
                $calendar.fullCalendar('renderEvent', eventData, true); // third param (stick? = true) will 
                //make the event to stay at the same loaction on the calendar
                alert("Successfully Updated");
            } else {
                eventData = {
                    id: selectedEvent.id,
                    ProjectId: selectedEvent.ProjectId,
                    title: selectedEvent.title,
                    start: combinedStart,
                    end: combinedEnd,
                    color: selectedEvent.color
                };
                // Refetching events
                $calendar.fullCalendar('removeEvents', eventData.id);
                $calendar.fullCalendar('renderEvent', eventData, true); // third param (stick? = true) will 
                //make the event to stay at the same loaction on the calendar
                alert("Successfully Updated");
            }
            // Unselect selected event
            $calendar.fullCalendar('unselect');
            // Clear modal inputs
            $('.modal').find('input').val('');
            // hide modal
            $('.modal').modal('hide');
        });
    }

    /**
    * Delete selected event from database
    * Created By: Anton Drantiev
    * Created On: March 17, 2019
    * Modified By: Anton Drantiev
    * Modified On: April 06, 2019
    * 
    * @function deleteEvetModal() - Will save updated start and end time for the selected event.
    * @param selectedEvent - Events array passed from initCalendar when event is selected.
    */

    function deleteEvetModal(selectedEvent) {
        //var events = $CALENDAR.fullCalendar('clientEvents');
        var eventToDelete = null;
        // Adding time to the selected date(reformated for IE11)
        // and creating date + time formated using MomentJs, for the browser and calendar to recognize
        var currdate = moment(selectedEvent.start).format("MM-DD-YYYY");
        var end = $('#ends-at').val();
        var toDateEnd = new Date(currdate.toString() + " " + end);
        var combinedEnd = moment(toDateEnd).format();
        // Event object to delete
        eventToDelete = {
            id: selectedEvent.id,
            title: selectedEvent.title,
            start: String(moment.utc(selectedEvent.start._d).format(DATE_FORMAT)),
            end: String(moment.utc(combinedEnd).format(DATE_FORMAT))
        };
        //newly dropped events that was not saved and does not have id from database yet, exists only in front end
        if (eventToDelete.id < 100) {
            $('.modal').off('click', '#delete-event').on('click', '#delete-event', function () {
                $calendar.fullCalendar('removeEvents', selectedEvent.id);
                // hide modal
                $('.modal').modal('hide');
                alert("Successfully Deleted");
            });

        } else {
            var jsonData = JSON.stringify(eventToDelete);
            // Using off / on click methods to unload any cache from the previous request
            $('.modal').off('click', '#delete-event').on('click', '#delete-event', function () {
                $calendar.fullCalendar('removeEvents', selectedEvent.id);
                $.ajax(
                    {
                        url: "Default.aspx/DeleteEvent",
                        type: 'POST',
                        data: JSON.stringify({ eventsJson: jsonData }
                        ),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function () {
                            alert("Successfully Deleted");
                        },
                        error: function (xhr) {
                            alert(JSON.stringify(xhr));
                        }
                    });
                // hide modal
                $('.modal').modal('hide');
            });
            // Reinitialize calendar for changes to take place
            getAllEventsInitCalendar();
        }

    }

    /**
    * Cancel button will clear all unsaved events
    * Created By: Anton Drantiev
    * Created On: March 29, 2019
    * Modified By: Anton Drantiev
    * Modified On: April 06, 2019
    * 
    * Using off/on click methods to unload any cache from the previous request
    */

    $('.row-buttons').off('click', '#cancel').on('click', '#cancel', function () {
        var events = $calendar.fullCalendar('clientEvents');
        $(events).each(function () {
            if (this.id <= 100) { // limit of id up to a 100 per dragged and unsaved event
                $calendar.fullCalendar('removeEvents', this.id);
            }
        });
    });


    /* Helper functions
    -----------------------------------------------------------------*/


    /**
    * Asynchronous JavaScript and XML request to read from database
    * Created By: Anton Drantiev
    * Created On: April 01, 2019
    * Modified By: Anton Drantiev
    * Modified On: April 06, 2019
    * 
    * @param url - The address to the method at the code behind (Default.aspx.cs page) with the method name
    * right after it (e.g. "Default.aspx/GetAssignedProjects").
    * @param firstFunction - (Optional) Any function that will manipulate the retrieved data.
    * @param selectedValue - The employee id.
    * @param secondFunction - (Optional) Any function that will not manipulate the retrieved data.
    * @param errorMessage - Desired error message in case of failure.
    */

    function ajaxReadData(url, firstFunction, selectedValue, secondFunction, errorMessage) {
        var dataSource = "";
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify({ id: selectedValue }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                dataSource = JSON.parse(data.d);
                if (typeof firstFunction === "undefined" || firstFunction === null) {
                    firstFunction = null;
                } else {
                    firstFunction(dataSource);
                }
                if (typeof secondFunction === "undefined" || secondFunction === null) {
                    secondFunction = null;
                } else {
                    secondFunction();
                }
            },
            failure: function () {
                alert(errorMessage);
            }
        });
        return dataSource;
    }

    /**
    * Asynchronous JavaScript and XML call to write to database
    * Created By: Anton Drantiev
    * Created On: April 01, 2019
    * Modified By: Anton Drantiev
    * Modified On: April 06, 2019
    * 
    * @param url - The address to the method at the code behind (Default.aspx.cs page) with the method name
    * right after it (e.g. "Default.aspx/createNewEvents").
    * @param dataSource - The data as a string array.
    * @param selectedId - The employee id.
    * @param errorMessage - Desired error message in case of failure.
    */

    function ajaxWriteData(url, dataSource, selectedId, errorMessage) {
        $.ajax(
            {
                url: url,
                type: 'POST',
                data: JSON.stringify({ eventsJson: dataSource, id: selectedId }
                ),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    // Reinitialize calendar for changes to take place
                    getAllEventsInitCalendar();
                },
                error: function () {
                    alert(errorMessage);
                }

            });
    }

   /**
   * Time validation
   * Created By: Anton Drantiev
   * Created On: March 25, 2019
   * Modified By: Anton Drantiev
   * Modified On: April 06, 2019
   * 
   * Will check for format of (24 hours), null, and if start time is after end time.
   * @param start - Start time.
   * @param end - End time.
   */

    function validateTime(start, end) {
        var startParts = start.split(':');
        var endParts = end.split(':');
        if (!/^\d{2}:\d{2}$/.test(start) ||
            !/^\d{2}:\d{2}$/.test(end) ||
            startParts[0] > 23 || startParts[1] > 59 ||
            endParts[0] > 23 || endParts[1] > 59) {
            alert("Time entered is in incorrect format (e.g. 23:59 or 00:01)");
            return false;
        } else if (startParts[0] > endParts[0] ||
            startParts[0] === endParts[0] && startParts[1] > endParts[1]) {
            alert("Start time can't be greter then end time");
            return false;
        }
        return true;
    }


   /**
   * Generate elements and populate with overtime totals
   * Created By: Anton Drantiev
   * Created On: April 02, 2019
   * Modified By: Anton Drantiev
   * Modified On: April 06, 2019
   * @param totalsData - calculated overtime totals by type of overtime.
   */

    function generateOvertimeToatls(totalsData) {
        $(".overtime-totals").html('');// Clear previous events
        var container = $(".overtime-totals");// Targeted div
        var blockDiv;  // Used in the for loop
        // Loop through overtimes
        $(totalsData).each(function (i, item) {
            var time = item.Total.split(':');
            var timeNoSeconds = time[0] + ':' + time[1];
            blockDiv = $("<div>").attr('id', item.OvertimeId).appendTo(container);
            $('<span>').text(item.OvertimeDescription + " total requested: " + timeNoSeconds).appendTo(blockDiv);
        });
    }


   /**
   * Return to the calendar of the logged in global admin, team lead or team admin
   * Created By: Anton Drantiev
   * Created On: April 10, 2019
   * Modified By: Anton Drantiev
   * Modified On: April 11, 2019
   */

    $('#backToMyCalendar').click(function () {
        var ddlEmployees = $("[id*=DropdonEmployeeLIst]");
        ddlEmployees.val("0");
        console.log(ddlEmployees.val());
        getAllEventsInitCalendar();
    });
});

