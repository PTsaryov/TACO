INSERT INTO [TB_TACO_SecurityRole] ( [SecurityRole], [RoleDescription], [CreatedBy]) 
VALUES ('Employee', 'Employee', '1'),
('GlobalAdmin', 'Global Admin', '1'),
('TeamAdmin', 'Team Admin', '1'),
('TeamLead', 'Team Lead', '1');

INSERT INTO [TB_TACO_Department] ( [DepartmentName], [DepartmentDescription], [StartDate], [ExpiryDate], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) 
VALUES ('Sales', 'generate real-time infrastructures', '2018-06-30T03:44:29.000', '2020-02-13T05:14:20.000', 1, '2018-08-11T07:22:14.000', 1, '2018-06-01T08:54:18.000');



INSERT INTO [TB_TACO_Area] ([DepartmentId], [AreaName], [AreaDescription], [StartDate], [ExpiryDate], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) 
VALUES (1, 'Area_A', 'disintermediate proactive ROI', '2018-12-12T15:56:47.000', '2020-02-13T05:14:20.000', 2, '2018-08-13T01:14:58.000', 3, '2018-07-02T15:55:44.000');


Insert into TB_TACO_Schedule (ScheduleName, ScheduleDescription, ScheduleTime, CreatedBy, CreatedOn)
Values
('Regular Hours', '7 hours 15 minutes', 435, '1', '2017-11-17T23:08:41.000'),
('Part-time','Variable', 0, '1', '2017-11-17T23:08:41.000'),
('Compressed Work Week I', '8 hours 4 minutes', 484, '1', '2017-11-17T23:08:41.000'),
('Compressed Work Week II', '9 hours 4 minutes', 544, '1', '2017-11-17T23:08:41.000'),
('Earned Time Off', '7 hours 35 minutes', 455, '1', '2017-11-17T23:08:41.000');

Insert into TB_TACO_Attendance (AttendanceCode, AttendanceDescription, Units, CreatedBy, CreatedOn)
Values
('VAC', 'Vacation', 'days', 1, '2017-11-17T23:08:41.000'),
('FWT', 'Flexible Work', 'hours', 1, '2017-11-17T23:08:41.000'),
('ETO', 'Earned Time Off', 'days', 1, '2017-11-17T23:08:41.000');

Insert into TB_TACO_Overtime (OvertimeCode, OvertimeDescription, Units, CreatedBy, CreatedOn)
Values
('BNK', 'Banked Time', 'hours', 1, '2017-11-17T23:08:41.000'),
('OT', 'Overtime', 'hours', 1, '2017-11-17T23:08:41.000');

INSERT into [TB_TACO_Unit] ([AreaId], [UnitName], [UnitDescription], [StartDate], [ExpiryDate], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) 
VALUES (1, 'Unit_A', 'Tall Ironweed', '2018-07-05T17:59:47.000', '2020-02-28T04:27:35.000', 4, '2018-11-20T21:02:52.000', 1, '2018-12-17T10:42:55.000');

INSERT [TB_TACO_Team] ([UnitId], [TeamName], [TeamDescription], [StartDate], [ExpiryDate], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) 
VALUES (1, 'Voltsillam', 'transform robust technologies', '2018-12-01T09:46:43.000', '2020-02-13T05:14:20.000', 1, '2018-11-01T00:09:29.000', 3, '2018-05-07T14:01:43.000');


INSERT INTO [TB_TACO_Category] ([CategoryName], [CategoryDescription], [StartDate], [ExpiryDate], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) 
VALUES ('Stronghold', 'nasa.gov', '2017-06-07T09:11:49.000', '2020-06-17T13:28:26.000', 1, '2019-02-06T03:51:04.000', 4, '2018-04-03T15:25:27.000');


INSERT INTO [TB_TACO_Holiday] ([HolidayName], [HolidayDate], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) 
VALUES ('Boxing Day', '2019-12-26T11:32:13.000', 4, '2017-04-13T13:16:47.000', 4, '2018-08-31T03:43:19.000'),
('Waitangi Day', '2019-02-06T11:39:11.000', 1, '2018-01-29T13:54:29.000', 3, '2018-11-24T04:43:50.000'),
('Good Friday', '2019-04-19T11:47:18.000', 3, '2018-02-11T03:14:32.000', 1, '2018-05-20T18:26:26.000'),
('Labour Day', '2019-09-12T17:19:05.000', 1, '2017-05-09T09:15:33.000', 3, '2018-05-07T08:33:31.000'),
('Easter Monday', '2019-04-22T05:38:20.000', 4, '2017-07-17T21:57:12.000', 3, '2018-04-02T23:00:06.000'),
('New Year''s Day', '2020-01-01T16:43:07.000', 4, '2018-02-14T04:02:15.000', 2, '2018-05-07T13:34:58.000'),
('Christmas Day', '2019-12-25T00:43:35.000', 2, '2017-04-02T01:11:51.000', 1, '2018-11-26T18:06:39.000'),
('Canada Day', '2019-06-01T13:37:23.000', 4, '2017-09-07T07:29:40.000', 1, '2018-03-12T15:12:19.000'),
('Christmas Eve', '2019-12-24T08:23:15.000', 3, '2018-01-20T21:46:15.000', 4, '2018-07-19T07:48:14.000');

INSERT INTO [TB_TACO_Position] ([PositionName], [PositionDescription], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) 
VALUES ('Admin', 'Administration', 4, '2019-01-05T04:15:23.000', 3, '2019-01-20T20:38:14.000');

INSERT INTO [TB_TACO_Project] ([CategoryId], [ProjectName], [ProjectDescription], [StartDate], [EndDate], [Priority], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) 
VALUES (1, 'global', 'Fetal anemia and thrombocytopenia, second trimester', '2017-04-26T19:16:50.000', '2022-05-27T01:24:54.000', 'Medium', 1, '2017-03-29T02:57:55.000', 1, '2018-02-08T10:17:26.000');


INSERT [TB_TACO_Employee] ([PositionId], [EmployeeNumber], [SecurityRoleId],[FirstName], [LastName], [HireDate], [TerminationDate], [Birthdate], [EmergencyContactName], [EmergencyContactPhone], [Station], [Computer], [Phone], [Email], [ScheduleId], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) 
VALUES (1, 'Administrator', 2, 'Global', 'Admin', '2019-01-01T05:45:17.000', NULL, '1990-05-18T02:43:59.000', 'Mycah Roxbee', '(884)171-9444', 'S82011', 'S8262XR', '(620)672-5467', 'mroxbee0@upenn.edu', 4, 1, '2016-05-12T06:29:50.000', 4, '2017-12-18T15:00:27.000'),
(1, 'teamlead1', 4, 'Team', 'Lead', '2019-01-01T06:39:32.000', NULL, '1990-09-20T05:21:08.000', 'Mandie Lovatt', '(644)672-9238', 'T63011S', 'H95131', '(688)148-1858', 'mlovatt1@hostgator.com', 1, 2, '2016-11-09T04:51:05.000', 2, '2017-11-05T20:19:40.000'),
(1, 'teamadmin1', 3, 'Team', 'Admin', '2019-01-01T05:41:59.000', NULL, '1991-10-25T02:02:48.000', 'Lethia Bloggett', '(884)171-9444', 'S27329D', 'S9361', '(644)672-9238', 'lbloggett2@imdb.com', 2, 2, '2016-04-12T11:41:10.000', 1, '2018-01-21T05:36:21.000'),
(1, 'employee1', 1, 'A', 'Employee', '2017-07-13T02:37:41.000', NULL, '1991-11-07T12:06:23.000', 'Sammy Gianuzzi', '(688)148-1858', 'H11232', 'S62035', '(644)672-9238', 'sgianuzzi3@hostgator.com', 1, 2, '2016-12-29T06:09:13.000', 4, '2018-02-19T00:52:46.000');


INSERT INTO [TB_TACO_TeamMember] ([TeamId], [EmployeeId], [StartDate], [EndDate], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) 
VALUES (1, 1, '2018-03-05T21:23:08.000', '2020-01-29T11:33:13.000', 4, '2017-10-24T01:05:23.000', 1, '2018-02-15T13:48:28.000'),
(1, 2, '2017-06-01T08:42:16.000', '2019-08-02T00:09:14.000', 2, '2017-09-03T10:13:03.000', 3, '2018-03-05T03:17:40.000'),
(1, 3, '2017-05-12T17:41:25.000', '2019-10-03T10:46:41.000', 1, '2017-11-26T17:46:26.000', 1, '2018-02-20T02:16:03.000'),
(1, 4, '2018-02-18T05:10:57.000', '2020-04-12T23:26:16.000', 1, '2017-06-21T02:06:18.000', 4, '2018-01-21T00:03:44.000');

