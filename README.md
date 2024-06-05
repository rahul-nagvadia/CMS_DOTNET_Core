# College Management System (.NET Core)

The College Management System is a comprehensive and efficient software solution designed to streamline the administrative and academic processes within educational institutions.  

## Features

- Teacher data danagement (By Admin)
- Student data danagement (By Admin)
- Department management (By Admin)
- Course management (By Admin)
- Student marks management (By Teacher)
- Updation and deletion functionality for above all
- Students can view results


## Installation

One can directly download the zip file of the project from https://github.com/bhargavp2004/webportal

1) Extract the zip file
2) Open Visual Studio 2022 IDE
3) Click on open project or solution
4) Open extracted folder and from that select CollegeMgmtSystem.sln file
5) Dependencies to be installed :
   - Microsoft.EntityFrameworkCore.SqlServer
   - Microsoft.EntityFrameworkCore.Tools
   - Microsoft.EntityFrameworkCore.Design
   - Microsoft.EntityFrameworkCore
6) Run following commands :
```bash
Add-Migration Init 
```
```bash
Update-Database
```
(Init is name of the migration so you can give any name)


 ## Authors

- [@bhargavp2004](https://github.com/bhargavp2004)
- [@rahul-nagvadia](https://github.com/rahul-nagvadia)

- ## Running web application

1) Application is having previledge that admin is the only person who can add teachers and student so until and unless you don't have data in aspnetusers table you won't be able to proceed further. So first remove [Authorize] tag declared above the controllers and add roles then add admin to the system and again put all the tags.
2) Now click on run button.
