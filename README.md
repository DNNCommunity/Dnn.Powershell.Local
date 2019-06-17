# Dnn.Powershell.Local

Powershell library for common DNN tasks to be used locally on the server

## Intent

The intent for this project is to help quickly create a DNN environment with content that can be used for testing. 
Adding users and roles and spreading those users across those roles can be done with this library easily and quickly.

## Use

This dll can be used as a Powershell module as follows:

``` ps
import-module C:\Path\to\Dnn.Powershell.Local.dll -DisableNameChecking
```

The included commands will then be available to use in Powershell:

- Use-Dnn
- Add-Role
- Add-Roles
- Add-User
- Add-Users

For help on commands use Get-Help like so:

``` ps
get-help use-dnn -full
```

## Environment

All commands interacting with DNN require you to set the environment to the right DNN installation. This is done using:

``` ps
Use-Dnn C:\Path\to\YourDnn
```

This will look for a web.config at that place and query SQL server from the connection string found in that web.config. The used DNN
installation will remain active until you change it using ```Use-Dnn``` again.
