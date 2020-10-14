# Getting Started

This guide assumes that you have installed:
 - Revit >= 2019
 - .NetStandard >= 2.0
 - .Net Framework >= 4.7.2
 - Visual Studio 2019

## Download Visual Studio Templates

Click here to download the templates

## Install the Templates

> First, make sure Visual Studio is not running, then unzip the contents on *%userprofile%\Documents\Visual Studio 2019* folder.

![alt text](../images/GettingStarted_1.jpg "Visual Studio Templates Folder")

> The resulting folder structure will be something like this for the ProjectTemplates folder:

![alt text](../images/GettingStarted_2.jpg "Visual Studio Templates Folder")

## Launch Visual Studio

> Launch Visual Studio and click on **"Create a new project"**

![alt text](../images/GettingStarted_3.jpg "Visual Studio Templates Folder")

> On Platform drop down menu, choose Revit and then pick **"Onbox MVC - APP"**

![alt text](../images/GettingStarted_4.jpg "Visual Studio Templates Folder")

> Type your project name and click **"Create"**

![alt text](../images/GettingStarted_5.jpg "Visual Studio Templates Folder")

## Launch your App

> Sometimes Visual Studio will complain when you try to build the solution for the first time, this happens, apperently because of a bug on PropertyChanged.Fody when referenced by Nuget. If you get build errors in the next step, click on Build -> Clean Solution and try to Run the Solution again. Once the packages are downloaded you should not have this issue anymore.

> This step also assumes that your Revit is installed on the default C:/Program Files/ Revit folder, if not, please change this patch on Visual Studio: Project Properties -> Debug tab.

> Choose a Revit Version on the Solution Configuration Drop down menu and hit **"Start"**

![alt text](../images/GettingStarted_6.jpg "Visual Studio Templates Folder")

> Accept the loading of the Addin:

![alt text](../images/GettingStarted_7.jpg "Visual Studio Templates Folder")

> After Launching Revit, open or create a project and you will see a new Ribbon tab with two buttons:

![alt text](../images/GettingStarted_8.jpg "Visual Studio Templates Folder")