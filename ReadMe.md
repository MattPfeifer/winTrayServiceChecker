This is a simple application that runs in the tray.

Configure by entering URLs in the appSettings.json file.

You may also configure how often the services are checked by changing the Interval field (default is 10 minutes)

When a service is down, a notification will be sent as a toast.

The icon in the tray will display
  Green: if all services are up and running
    Red: if at least one service is not running

Right click on the Tray in the icon to view the status of each service.
    Click an individual service to launch the corresponding URL in your default browser.
    Click 'View Logs' to see the history of service checks.
    Click 'Exit' to close the application


The application is best run by placing a shortcut in your startup folder:
    \\\\Users\\{UserName}\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup


Change Log
2023.01.12
-----------
Added refresh option to menu
Added installer
Added help link to confluence