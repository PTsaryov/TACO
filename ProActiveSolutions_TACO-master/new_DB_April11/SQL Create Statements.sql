Create database Taco
Use Taco
ALTER TABLE TB_TACO_Position
DROP Constraint FK1_TACO_Position_Employee_Created;
ALTER TABLE TB_TACO_Position
DROP Constraint FK2_TACO_Position_Employee_Modified;
ALTER TABLE TB_TACO_Schedule
DROP Constraint FK1_TACO_Schedule_Employee_Created;
ALTER TABLE TB_TACO_Schedule
DROP Constraint FK2_TACO_Schedule_Employee_Modified;

GO
Use Taco
Drop Table TB_TACO_AllocatedTimeLog;
Drop table TB_TACO_AttendanceEntitlement;
Drop table TB_TACO_AllocatedTimeDetail;
Drop table TB_TACO_ProjectTeam;
Drop Table TB_TACO_Request;
Drop Table TB_TACO_TimesheetDetail;
Drop Table TB_TACO_Timesheet;
Drop Table TB_TACO_TeamMember;
Drop Table TB_TACO_Employee;
Drop Table TB_TACO_Attendance;
Drop Table TB_TACO_Overtime;
Drop Table TB_TACO_Holiday;
Drop Table TB_TACO_Project;
Drop Table TB_TACO_Category;
Drop Table TB_TACO_Position;
Drop Table TB_TACO_Team;
Drop Table TB_TACO_Unit;
Drop Table TB_TACO_Area;
Drop Table TB_TACO_Department;
Drop Table TB_TACO_Schedule;
Drop Table TB_TACO_SecurityRole;

Go
Create Table TB_TACO_SecurityRole(
	SecurityRoleId	int identity(1,1) not null Constraint PK_TACO_SecurityRole primary key clustered,
	SecurityRole	varchar(50) not null,
	RoleDescription varchar(250) not null,
	RoleDeactivated	bit not null Constraint DF_SecurityRole_Deactivated default 'FALSE',
	CreatedBy		int		not null,
	CreatedOn		datetime	not null Constraint DF_SecurityRole_CreatedOn default Getdate(),
	ModifiedBy		int		null,
	ModifiedOn		datetime	null
)

Create Table TB_TACO_Department(
	DepartmentId	int identity(1,1) not null Constraint PK_TACO_Department primary key clustered,
	DepartmentName	varchar(100) not null,
	DepartmentDescription varchar(250) not null,
	StartDate		datetime	not null Constraint DF_Department_StartDate default Getdate(),
	ExpiryDate		datetime	not null Constraint DF_Department_ExpiryDate default 9999-12-31,
	CreatedBy		int		not null,
	CreatedOn		datetime	not null Constraint DF_Department_CreatedOn default Getdate(),
	ModifiedBy		int		null,
	ModifiedOn		datetime	null
)
Create Table TB_TACO_Area(
	AreaId			int identity(1,1) not null Constraint PK_TACO_Area primary key clustered,
	DepartmentId	int		not null Constraint FK1_TACO_Area_Department references TB_TACO_Department(DepartmentId) Index IX1_TACO_Area_Department nonclustered,
	AreaName		varchar(100) not null,
	AreaDescription varchar(250) not null,
	StartDate		datetime	not null Constraint DF_Area_StartDate default Getdate(),
	ExpiryDate		datetime	not null Constraint DF_Area_ExpiryDate default 9999-12-31,
	CreatedBy		int		not null,
	CreatedOn		datetime	not null Constraint DF_Area_CreatedOn default Getdate(),
	ModifiedBy		int		null,
	ModifiedOn		datetime	null

)
Create Table TB_TACO_Unit(
	UnitId		int identity(1,1) not null Constraint PK_TACO_Unit primary key clustered,
	AreaId		int		not null Constraint FK1_TACO_Unit_Area references TB_TACO_Area(AreaId) Index IX1_TACO_Unit_Area nonclustered,
	UnitName	varchar(100) not null,
	UnitDescription varchar(250) not null,
	StartDate		datetime	not null Constraint DF_Unit_StartDate default Getdate(),
	ExpiryDate		datetime	not null Constraint DF_Unit_ExpiryDate default 9999-12-31,
	CreatedBy		int		not null,
	CreatedOn		datetime	not null Constraint DF_Unit_CreatedOn default Getdate(),
	ModifiedBy		int		null,
	ModifiedOn		datetime	null
)
Create Table TB_TACO_Team (
	TeamId			int		identity(1,1) not null Constraint PK_TACO_DepartmentTeam primary key clustered,
	UnitId			int		not null Constraint FK1_TACO_Team_Unit references TB_TACO_Unit(UnitId) Index IX1_TACO_Team_Unit nonclustered,
	TeamName		varchar(100)	not null,
	TeamDescription		varchar(250)	not null,
	StartDate		datetime	not null Constraint DF_Team_StartDate default Getdate(),
	ExpiryDate		datetime	not null Constraint DF_Team_ExpiryDate default 9999-12-31,
	CreatedBy		int		not null,
	CreatedOn		datetime	not null Constraint DF_DepartmentTeam_CreatedOn default Getdate(),
	ModifiedBy		int		null,
	ModifiedOn		datetime	null
)
Create Table TB_TACO_Position (
	PositionId	 		int		identity(1,1)	not null Constraint PK_TACO_Position primary key clustered,
	PositionName		varchar(100)	not null,
	PositionDescription	varchar(250)	not null,
	PositionDeactivated	bit not null Constraint DF_Position_Deactivated default 'FALSE',
	CreatedBy		int		not null,
	CreatedOn		datetime	not null Constraint DF_Position_CreatedOn default Getdate(),
	ModifiedBy		int		null,
	ModifiedOn		datetime	null
)
Create Table TB_TACO_Category (
	CategoryId			int		identity(1,1)	not null Constraint PK_TACO_Category primary key clustered,
	CategoryName		varchar(100)	not null,
	CategoryDescription	varchar(250)	not null,
	StartDate		datetime	not null Constraint DF_Category_StartDate default Getdate(),
	ExpiryDate		datetime	not null Constraint DF_Category_ExpiryDate default 9999-12-31,
	CreatedBy		int		not null,
	CreatedOn		datetime	not null Constraint DF_Category_CreatedOn default Getdate(),
	ModifiedBy		int		null,
	ModifiedOn		datetime	null
)
Create Table TB_TACO_Project (
	ProjectId		int		identity(1,1)	not null Constraint PK_TACO_Project primary key clustered,
	CategoryId		int		not null Constraint FK1_TACO_Project_Category references TB_TACO_Category(CategoryId) Index IX1_TACO_Project_Category nonclustered,
	ProjectName		varchar(100)	not null,
	ProjectDescription	varchar(250)	not null,
	StartDate		datetime	not null Constraint DF_Project_StartDate default Getdate(),
	EndDate			datetime	not null Constraint DF_Project_EndDate default 9999-12-31,
	Priority		varchar(15)	not null Constraint CK_Priority check (Priority = 'Low' or Priority = 'Medium' or Priority = 'High') Constraint DF_Project_Priority default 'Low',
	ProjectColor	varchar(10) not null Constraint DF_Project_ProjectColor default '#ff0000',
	CreatedBy		int		not null,
	CreatedOn		datetime	not null Constraint DF_Project_CreatedOn default Getdate(),
	ModifiedBy		int		null,
	ModifiedOn		datetime	null
)
Create Table TB_TACO_Holiday (
	HolidayId		int		identity(1,1)	not null Constraint PK_TACO_Holiday primary key clustered,
	HolidayName		varchar(100)	not null,
	HolidayDate		datetime	not null,
	HolidayDeactivated	bit not null Constraint DF_Holiday_Deactivated default 'FALSE',
	CreatedBy		int		not null,
	CreatedOn		datetime	not null Constraint DF_Holiday_CreatedOn default Getdate(),
	ModifiedBy		int		null,
	ModifiedOn		datetime	null
)
Create Table TB_TACO_Attendance (
	AttendanceId			int		identity(1,1)	not null Constraint PK_TACO_Attendance primary key clustered,
	AttendanceCode			varchar(10)	not null,
	AttendanceDescription	varchar(250)		not null,
	AttendanceDeactivated		bit not null Constraint DF_Attendance_Deactivated default 'FALSE',
	Units			varchar(5)	not null Constraint CK_Attendance check (Units = 'Days' or Units = 'Hours'),
	CreatedBy		int		not null,
	CreatedOn		datetime	not null Constraint DF_Attendance_CreatedOn default Getdate(),
	ModifiedBy		int		null,
	ModifiedOn		datetime	null
)
Create Table TB_TACO_Overtime (
	OvertimeId			int		identity(1,1)	not null Constraint PK_TACO_Overtime primary key clustered,
	OvertimeCode		varchar(10)	not null,
	OvertimeDescription	varchar(250)		not null,
	OvertimeDeactivated		bit not null Constraint DF_Overtime_Deactivated default 'FALSE',
	Units			varchar(5)	not null Constraint CK_Overtime_Units check (Units = 'Days' or Units = 'Hours'),
	Color			varchar(10) not null Constraint DF_Overtime_Color default '#ff0000',
	CreatedBy		int		not null,
	CreatedOn		datetime	not null Constraint DF_Overtime_CreatedOn default Getdate(),
	ModifiedBy		int		null,
	ModifiedOn		datetime	null
)
Create Table TB_TACO_Schedule (
	ScheduleId			int		identity(1,1)	not null Constraint PK_TACO_Schedule primary key clustered,
	ScheduleName		varchar(100)	not null,
	ScheduleDescription		varchar(250)	not null,
	ScheduleTime			int	not null,
	ScheduleDeactivated		bit not null Constraint DF_Schedule_Deactivated default 'FALSE',
	CreatedBy		int		not null,
	CreatedOn		datetime	not null Constraint DF_Schedule_CreatedOn default Getdate(),
	ModifiedBy		int		null,
	ModifiedOn		datetime	null
)
Create Table TB_TACO_Employee (
	EmployeeId		int		identity(1,1)	not null Constraint PK_TACO_Employee primary key clustered,
	PositionId		int	not null Constraint FK1_TACO_Employee_Position references TB_TACO_Position(PositionId) Index IX1_TACO_Employee_Position nonclustered,
	ScheduleId		int not null Constraint FK2_TACO_Employee_Schedule references TB_TACO_Schedule(ScheduleId) Index IX3_TACO_Employee_Schedule nonclustered,
	SecurityRoleId	int not null Constraint FK5_TACO_Employee_SecurityRole references TB_TACO_SecurityRole Index IX4_TACO_Employee_SecurityRole nonclustered,
	EmployeeNumber	varchar(50)		not null,
	FirstName		varchar(100)		not null,
	LastName		varchar(100)		not null,
	HireDate		dateTime		not null	Constraint DF_Employee_HireDate default Getdate(),
	TerminationDate	dateTime    null,
	Birthdate		dateTime	not null,
	EmergencyContactName	varchar(100) null,
	EmergencyContactPhone	varchar(15) null,
	Station 	varchar(50) not null,
	Computer	varchar(50) not null,
	Phone	varchar(15) not null Constraint CK_Phone check (Phone like ('([0-9][0-9][0-9])[0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]')),
	Email	varchar(250) not null Constraint CK_Email check (Email Like '%_@_%._%'),
	CreatedBy		int		not null,
	CreatedOn		datetime	not null Constraint DF_Employee_CreatedOn default Getdate(),
	ModifiedBy		int		null,
	ModifiedOn		datetime	null
)
Create Table TB_TACO_TeamMember(
	TeamMemberId	int identity(1,1) not null Constraint PK_TACO_TeamMembers primary key clustered,
	TeamId			int not null constraint FK1_TACO_TeamMember_Team references TB_TACO_Team(TeamId) Index IX1_TACO_TeamMember_Team nonclustered,
	EmployeeId		int not null constraint FK2_TACO_TeamMember_Employee references TB_TACO_Employee(EmployeeId) Index IX2_TACO_TeamMember_Employee nonclustered,
	StartDate		datetime	not null Constraint DF_TeamMember_StartDate default Getdate(),
	EndDate			datetime	not null Constraint DF_TeamMember_EndDate default 9999-12-31,
	CreatedBy		int		not null,
	CreatedOn		datetime	not null Constraint DF_TeamMember_CreatedOn default Getdate(),
	ModifiedBy		int		null,
	ModifiedOn		datetime	null
)
Create Table TB_TACO_Timesheet (
	TimesheetId		int		identity(1,1)	not null Constraint PK_TACO_Timesheet primary key clustered,
	EmployeeId		int		not null Constraint FK1_TACO_Timesheet_Employee references TB_TACO_Employee(EmployeeId) Index IX1_TACO_Timesheet_Employee nonclustered,
	StartDate		datetime	not null Constraint DF_Timesheet_StartDate default Getdate(),
	EndDate			datetime	not null Constraint DF_Timesheet_EndDate default 9999-12-31
)
Create Table TB_TACO_TimesheetDetail (
	TimesheetDetailId	int		identity(100,1)	not null Constraint PK_TACO_TimesheetDetail primary key clustered,
	TimesheetId		int		not null Constraint FK1_TACO_TimesheetDetail_Timesheet references TB_TACO_Timesheet(TimesheetId) Index IX1_TACO_TimesheetDetail_Timesheet nonclustered,
	ProjectId		int		null Constraint FK2_TACO_TimesheetDetail_Project references TB_TACO_Project(ProjectId) Index IX2_TACO_TimesheetDetail_Project nonclustered,
	AttendanceId	int		null Constraint FK3_TACO_TimesheetDetail_Attendance references TB_TACO_Attendance(AttendanceId) Index IX3_TACO_TimesheetDetail_Attendance nonclustered,
	HolidayId		int		null Constraint FK4_TACO_TimesheetDetail_Holiday references TB_TACO_Holiday(HolidayId) Index IX4_TACO_TimesheetDetail_Holiday nonclustered,
	OvertimeId		int		null Constraint FK7_TACO_TimesheetDetail_Overtime references TB_TACO_Overtime(OvertimeId) Index IX5_TACO_TimesheetDetail_Overtime nonclustered,
	StartDateTime	datetime	not null,
	EndDateTime		datetime	not null,
	ArchiveDelete	bit		not null Constraint DF_TimesheetDetail_ArchiveDelete default 'FALSE',
	CreatedBy		int		not null,
	CreatedOn		datetime	not null Constraint DF_TimesheetDetails_CreatedOn default Getdate(),
	ModifiedBy		int			null,
	ModifiedOn		datetime	null
)
Create Table TB_TACO_Request (
	RequestId	int		identity(1,1)	not null Constraint PK_TACO_Request primary key clustered,
	EmployeeId	int		not null Constraint FK1_TACO_Request_Employee references TB_TACO_Employee(EmployeeId) Index IX1_TACO_Request_Employee nonclustered,
	OvertimeId	int		not null Constraint FK2_TACO_Request_Overtime references TB_TACO_Overtime(OvertimeId) Index IX2_TACO_Request_Overtime nonclustered,
	TotalTime	decimal(7,2)	not null,
	Status		varchar(10) not null Constraint CK_Request_Status check (Status = 'Pending' or Status = 'Approved' or Status = 'Denied') Constraint DF_Request_Status default 'Pending',
	Date		dateTime	not null,
	Comment		varchar(250)	null,
	CreatedBy		int			not null,
	CreatedOn		datetime	not null Constraint DF_Request_CreatedOn default Getdate(),
	ModifiedBy		int			null,
	ModifiedOn		datetime	null
)
Create Table TB_TACO_ProjectTeam (
	ProjectTeamId	int		identity(1,1)	not null Constraint PK_TACO_ProjectTeam primary key clustered,
	EmployeeId		int		not null Constraint FK1_TACO_ProjectTeam_Employee references TB_TACO_Employee(EmployeeId) Index IX1_TACO_ProjectTeam_Employee nonclustered,
	ProjectId		int		not null Constraint FK2_TACO_ProjectTeam_Project references TB_TACO_Project(ProjectId) Index IX2_TACO_ProjectTeam_Project nonclustered,
	StartDate		datetime	not null Constraint DF_ProjectTeam_StartDate default Getdate(),
	EndDate			datetime	not null Constraint DF_ProjectTeam_EndDate default 9999-12-31,
	CreatedBy		int			not null,
	CreatedOn		datetime	not null Constraint DF_ProjectTeam_CreatedOn default Getdate(),
	ModifiedBy		int			null,
	ModifiedOn		datetime	null
)
Create Table TB_TACO_AllocatedTimeDetail (
	AllocatedTimeId	int		identity(1,1)	not null Constraint PK_TACO_AllocatedTimeDetail primary key clustered,
	ProjectTeamId	int		not null Constraint FK1_TACO_AllocatedTimeDetail_ProjectTeam references TB_TACO_ProjectTeam(ProjectTeamId) Index IX1_TACO_AllocatedTimeDetail_ProjectTeam nonclustered,
	AllocatedMonth	int		not null,
	AllocatedYear	int		not null,
	AllocatedDays	int		not null Constraint CK_AllocatedTimeDetail_AllocatedDays check (AllocatedDays >= 0),
	CreatedBy		int		not null,
	CreatedOn		datetime	not null Constraint DF_AllocatedTimeDetail_CreatedOn default Getdate(),
	ModifiedBy		int			null,
	ModifiedOn		datetime	null
)
Create Table TB_TACO_AttendanceEntitlement (
	AttendanceEntitlementId	int		identity(1,1)	not null Constraint PK_TACO_AttendanceEntitlement primary key clustered,
	AttendanceId	int		not null Constraint FK1_TACO_AttendanceEntitlement_Attendance references TB_TACO_Attendance(AttendanceId) Index IX1_TACO_AttendanceEntitlement_Attendance nonclustered,
	EmployeeId		int		not null Constraint FK2_TACO_AttendanceEntitlement_Employee references TB_TACO_Employee(EmployeeId) Index IX2_TACO_AttendanceEntitlement_Employee nonclustered,
	TotalTime		decimal(7,2)	not null Constraint DF_AttendanceEntitlement_TotalTime default 0,
	CreatedBy		int			not null,
	CreatedOn		datetime	not null Constraint DF_AttendanceEntitlement_CreatedOn default Getdate(),
	ModifiedBy		int			null,
	ModifiedOn		datetime	null
)

Create Table TB_TACO_AllocatedTimeLog (
	AllocatedTimeLogId	int		identity(1,1)	not null Constraint PK_TACO_AllocatedTimeLog primary key clustered,
	AllocatedTimeId		int		not null Constraint FK1_TACO_AllocatedTimeLog_AllocatedTimeDetail references TB_TACO_AllocatedTimeDetail(AllocatedTimeId) Index IX1_TACO_AllocatedTimeLog_AllocatedTimeDetail nonclustered,
	AllocatedTimeLogged	int		not null,
	CreatedBy			int		not null,
	CreatedOn			datetime	not null
)

Go
Alter Table TB_TACO_AllocatedTimeLog
Add Constraint FK2_TACO_AllocatedTimeLog_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_AttendanceEntitlement
Add Constraint FK3_TACO_AttendanceEntitlement_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_AttendanceEntitlement
Add Constraint FK4_TACO_AttendanceEntitlement_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_AllocatedTimeDetail
Add Constraint FK3_TACO_AllocatedTimeDetail_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_AllocatedTimeDetail
Add Constraint FK4_TACO_AllocatedTimeDetail_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_ProjectTeam
Add Constraint FK3_TACO_ProjectTeam_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_ProjectTeam
Add Constraint FK4_TACO_ProjectTeam_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_Request
Add Constraint FK3_TACO_Request_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_Request
Add Constraint FK4_TACO_Request_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_TimesheetDetail
Add Constraint FK5_TACO_TimesheetDetail_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_TimesheetDetail
Add Constraint FK6_TACO_TimesheetDetail_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_TeamMember
Add Constraint FK3_TACO_TeamMember_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_TeamMember
Add Constraint FK4_TACO_TeamMember_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_Employee
Add Constraint FK3_TACO_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_Employee
Add Constraint FK4_TACO_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_Schedule
Add Constraint FK1_TACO_Schedule_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_Schedule
Add Constraint FK2_TACO_Schedule_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_Attendance
Add Constraint FK1_TACO_Attendance_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_Attendance
Add Constraint FK2_TACO_Attendance_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_Overtime
Add Constraint FK1_TACO_Overtime_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_Overtime
Add Constraint FK2_TACO_Overtime_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_Holiday
Add Constraint FK1_TACO_Holiday_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_Holiday
Add Constraint FK2_TACO_Holiday_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_Project
Add Constraint FK2_TACO_Project_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_Project
Add Constraint FK3_TACO_Project_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_Category
Add Constraint FK1_TACO_Category_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_Category
Add Constraint FK2_TACO_Category_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_Position
Add Constraint FK1_TACO_Position_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_Position
Add Constraint FK2_TACO_Position_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_Team
Add Constraint FK2_TACO_Team_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_Team
Add Constraint FK3_TACO_Team_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_Unit
Add Constraint FK2_TACO_Unit_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_Unit
Add Constraint FK3_TACO_Unit_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_Area
Add Constraint FK2_TACO_Area_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_Area
Add Constraint FK3_TACO_Area_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_Department
Add Constraint FK1_TACO_Department_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_Department
Add Constraint FK2_TACO_Department_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

Alter Table TB_TACO_SecurityRole
Add Constraint FK1_TACO_SecurityRole_Employee_Created 
Foreign Key (CreatedBy) references TB_TACO_Employee(EmployeeId);
Alter Table TB_TACO_SecurityRole
Add Constraint FK2_TACO_SecurityRole_Employee_Modified
Foreign Key (ModifiedBy) references TB_TACO_Employee(EmployeeId);

