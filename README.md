# Central Logging Service * *these docs are not complete* *
## What is this?
The central logging service is an all in one solution for storing log messages in a standard format in one location from distributed .NET systems known here as `Publishing Systems`. The system also includes a windows service which monitors the log message database and sends alerts to users as defined by dynamic rules setup via a built-in front-end.

![CLS Admin Dashboard Image](https://i.imgur.com/C6tYYh2.png)

## User Alerts
Users can setup alerts via the front end, these alerts are completely customisable using the alert query builder shown in the `Setting up an Alert` section. An usage example here is that a certain user may want to be notified whenever a particular service logs an error, this can be extended to whenever a particular service logs an error at a certain time, certain day of the week, in a certain staging environment etc etc. For a full list of options see the `Alert Options Breakdown` section.

### Setting up an Alert
#### 1. Register a subscriber
Navigate to the subscribers page then click `+ New Subscriber` and you'll be presented with the following form to complete: 
![register subscriber form](https://i.imgur.com/OjzJN9i.png)

#### 2. Creating the Alert
Navigate to the alerts page then click `+ New Alert` and you'll be presented with the following form to complete, `I've filled out the one below with an example scenario whereby we want to be alerted when any Publishing System sends an error into the database between 1am and 3am`: 

![create alert form](https://i.imgur.com/6rMS6i1.png)

#### 3. Receiving an Alert
You'll receive an email whenever the criteria of the alert are met and it'll look something like this:

![Alert Email Image](https://i.imgur.com/4x3UuRS.png)

#### 4. Alert History
You'll notice that on the email you receive there is a link at the bottom which will take you to a page where you can view a breakdown of the alert that happened.

![Alert History Page](https://i.imgur.com/yx6jsSy.png)

#### Alert Options Breakdown
| Alert Type Name | Description |
| --------------- | ----------- |
| TimeOfDay | The time of day that the log message arrived into the database |
| MessageSeverity | The severity of the log message, can be Debug, Info, Warn, Error or Fatal |
| PublishingSystemName | The name of the system that logged the message |
| EnvironmentType | The staging environment of the publishing system that logged the message, can be DEV, SIT, UAT or LIVE. |
| NumberOfMessages | `(To be used in conjunction with TimeWindowMinutes)` This allows the user to be alerted when a number of messages are logged in a certain window of time e.g. 100 messages in 5 minutes. |
| TimeWindowMinutes | See `NumberOfMessages` description above. |
| DayOfWeek | The day of the week that the message was logged. |

# Development Requirements
- Visual Studio 2017
- .NET Framework 4.7.2
- Microsoft SQL Server 2016 or higher
- Email account to send alerts (project is configured for gmail initially, you can create a new gmail account for use with this application to get up and running fast, then set the gmail account to be compatible with "[less-safe apps](https://myaccount.google.com/lesssecureapps)" or alternatively use an email from microsoft, yahoo or your own domain).

## Software Stack
- Web Frontend - .NET Framework MVC, JavaScript, JQuery, CSS (To be re-written in Angular 2 & CSS w/ SASS).
- Rest of the Solution is .NET Framework 4.7.2 (To be re-written in .NET Core for interoperability).

## Getting up and running fast for development
1. Create a (preferably) empty SQL Server database,
2. Clone the solution in Visual Studio,
3. Right click the solution & click "Restore NuGet Packages" then Press F6 to build the solution,
4. Right click CLS.DatabaseGenerator & click "Set as Startup Project",
5. Navigate to the App.config in the CLS.DatabaseGenerator project and edit the connection string to point at the database you created in step 1,
6. Run the database manager and it will create the tables & metadata in your target database,

7a. Navigate to the `CLS.Core` project and open the App.config file and edit the following:
```
<connectionStrings>
    <add name="CLSDbEntities" connectionString="metadata=res://*/Data.CLSDbModel.csdl|res://*/Data.CLSDbModel.ssdl|res://*/Data.CLSDbModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=(LocalDb)\CentralLoggingService;initial catalog=CentralLoggingService;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
</connectionStrings>
```
7b. Particularly this part:
```
connection string=&quot;data source=(LocalDb)\CentralLoggingService;initial catalog=CentralLoggingService;integrated security=True;
```
7c. Keeping the formatting, edit it to point at the database you created in step 1,  
8. Right click the solution and click properties and set the startup projects as shown:
![project setup example](https://i.imgur.com/6jaNTJf.png?1)

9. Edit the App.config in the projects which will run at start up (seen in screenshot above)

10. Run the projects with 'F5'.

## Solution Breakdown (Development)
| Concept                | Solution Project            |
| ---------------------- | --------------------------- |
| Database Creation      | `CLS.DatabaseGenerator`     |
| Log Message Dispatcher | `CLS.SenderConsole`         |
| Alerts Engine          | `CLS.WindowsServiceConsole` |
| Web Front End          | `CLS.Web`                   |

#### Database Creation
The `CLS.DatabaseGenerator` will allow you to quickly get up and running with the solution, it must be run before anything else to create the required objects & static data in the database.

#### Log Message Dispatcher
The `CLS.SenderConsole` will periodically send randomised log messages into the target database in order to aid in development. In production, the `CLS.Sender` is simply a library which will be inclduded in the systems you want to log from.

#### Alerts Engine
The `CLS.WindowsServiceConsole` will run as a background process, periodically scanning the messages which have been sent into the database to see if any subscribed users should be alerted.

#### Web Front End
The `CLS.Web` project is the front end for both development & production and can be published as such.
