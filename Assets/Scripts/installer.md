# Creating the installer
To create the installer, you will need to download and install [InnoSetup](https://jrsoftware.org/isinfo.php "InnoSetup") and you also need to [build the project](building.md). Use InnoSetup to open the file called "SokobanInnoSetupScript.iss" that is included with the source code. Now you need to set some fields to the correct values. <br/>
Set the:
* "Licence File" field to the location of the licence.txt file
* "OutputDir" to where you want the installer to be created
* "SetupIconFile" to the location of the icon file
* First "Source" field to the location of the .exe file in your build 
* Second "Source" field to the location of your "Builds" folder.<br/>
Then you only need to press the compile button and an installer will be created in your selected "OutputDir".
[<img src="innosetuptutorial.jpg" width="1000"/>](innosetuptutorial.jpg)<br/>