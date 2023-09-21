- This is more personal preference and wholey depends on the Code Standard at the company. I would have the inteface and the implementation kept in seperate folders.

!!! I dont have the user story so im not sure if the only thing the user has done is the 'Add' function or also had to write the 'GetAll' function, 
  but given they are in the same controller I'm gonna aassume they had to add the 'GetAll' function too, 
  therefore had to write the table scheme and had to add the entries to the View. So the following comments are based on them having to write the whole thing. 

- Update the db to have the hours as a double and the date as a DateTime.

- Add a new model call TimesheetEntryModel that replaces TimesheetEntry in the controller to ensure that we have seperate models to reflect the database models 
  and the model from the user.
  
- Update the html to have type attributes to each different input, i.e. Date has a date type, hours has a number type, first name, last name and project have text type.

- Add validation message's for the different inputs

- I would add restrictions to the 'TimesheetEntryModel' Class to ensure that the information is valid:
	- make them all required.
	- max and min lengths for first name, last name, project.
	- ensuring that the text written in the first name, last name and project text inputs are string only.
	- making sure the hours input is double.

- Add a check on line 25 in the 'TimesheetController' to validate the model state.

- Add another data set in context and save the information of the 'TimesheetEntry', as once we add the Timesheet in the reference for the linked timesheet is being cleared. 
  If the spec said thats how the data should be saved. If not I would only save the 'TimesheetEntry' Items and collate them depending on what the user wants, i.e.
  if the user only wants to see total hours worked by project then you can group using the project and sum up the total hours. If they want it by user then group by first and last name etc.

- Line 22 in the 'TimesheetRepository' there is a random ';' that has been left in accidently.

- In the test the 'timesheet' variable can be re-written as:
	var timesheet = new Timesheet
    {
        Id = 1,
        TimesheetEntry = new TimesheetEntry()
        {
            Id = 1,
            Date = "01/09/2023",
            Project = "Test Project",
            FirstName = "Test",
            LastName = "Test",
            Hours = "7.5"
        },
        TotalHours = "7.5",
    };

 - Additionally there should be a couple more tests written such as: 
    - add a call to get the timesheets and make sure that thee one that has been insterted is the same as the details as you sent to be inserted. 
    - one to check that if you add timesheets with the same Id then there would be an error thrown saying the key needs to be unique
    - one to add multiple timesheets and to check they have been saved and are the same as what was sent to be saved. 
