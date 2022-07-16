# BPR-Mod-Config

<br />A vehicle and (soon-to-be) map mod configurator for Burnout Paradise Remastered (PC)<br />
-By CoolguyTC<br />

<br />**DISCLAMER:**<br />
This program only currently supports vehicle mods that overwrite existing vehicles. There is currently no method within the program to add extra vehicles through modifying the "VEHICLELIST.BIN" file. However support may come in the future. As of right now the program will only overwrite files withing the game's VEHICLE folder so folders such as the mods folder etc will not be touched.

---
**Installation**<br />
---
**Prerequisites:**<br />
Please ensure that before running the program you have these things set up:
1. A copy of Burnout Paradise Remastered (from origin or steam).
2. You have made a complete backup of the game's files stored in a seperate folder. (This will be used to remove mods when neccessary)
3. You have a folder set up for vehicle mods. (For formating see **"How do I setup the vehicle mod folder?"** below)
4. You have a folder set up for map mods. (Formating doesn't exist yet. There will be more information when the feature is added.)
5. You have Noesis installed and the "Burnout Paradise Packer / Unpacker" plugin installed. (See installation info below)<br /><br />

**Noesis:**<br />
Noesis is required in order to interact with the game's files. There is a link to the website to download below.<br />
DGIorio's Noesis plugins are also required. Their download link is in the **#modding-tools** channel in the Burnout Hints discord server (link at the bottom of the page).<br />
Noesis: https://www.richwhitehouse.com/index.php?content=inc_projects.php&showproject=91<br /><br />

**Completing the Setup:**<br />
Unzip the file found in the releases section into an empty folder. From there follow the instructions on screen to complete the setup. Make sure to copy directories directly from file explorer in the format:<br />
C:\FOLDER1\FOLDER2&nbsp;&nbsp;(and so on)<br />
(When asked to reset your files it is highly recommended that you do. Otherwise the program will have no way to tell what mods you currently have installed)<br />
**[This WILL NOT effect any other folders in the game's directory]**

---
**Build Instructions**
---
For those who wish to build it themselves it should be as simple as downloading the source and running the "BPR Mod Manager.sln" file.
From there you can view the source code. Next publish it in the Self-contained deployment mode with "Produce single file" checked. Then put that exe alongside the extracted "DependancyFiles.zip" so that the DATA file is in the same directory as the exe.


---
**To-Do**
---
- Create fast vehicle apply.
- Make sure error handling is complete.
- Create map configurator.
- Make a GUI version with vehicle images :D
- Make presets.
- Highlight file conflicts.

---
**Questions and Answers**
---

Q) Where do I put the files?

>Pretty much anywhere.
The program will ask you to input directories for different folders when started.


<br />Q) How do I setup the vehicle mod folder?

>Within the folder you assigned as the 'vehicle mods' folder your mods should look like this:

![image](https://user-images.githubusercontent.com/95531273/179301454-dd70cb46-6039-432e-92d3-b8ebde470c05.png)

>Where 'CAR NAME X' is the name of the modded car (Car names do not have to be all-caps).
And each individual car folder should look like this:

![image](https://user-images.githubusercontent.com/95531273/179301772-392b24e1-d631-46f5-9c9e-ce4a3d23d304.png)

>There should be 1-3 '.BIN' files in there from the car mod that you have downloaded.


<br />Q) Is there going to be a version that isn't in the command line?

>Yes, but I want to get it complete first before I move it to a GUI based system.


<br />Q) When is support for presets and map mods coming?

>At some point. (hopefully)


<br />Q) I still need help!

>For any further questions you can @ me on the Burnout Hints discord or DM me.<br />
Burnout Hints discord: https://discord.gg/dMyuRBq<br />
My discord is 'ThomasMLGEngine#3576'
