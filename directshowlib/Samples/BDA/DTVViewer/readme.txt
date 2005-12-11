DTVViewer - A basic Digital TV (BDA) viewer sample

This sample demonstrate how to use BDA to display Digital TV into a Windows Form.

This application ask to the user tuning parameters. You have to know most of them to have a working Tune Request.

DVB-T parameters directly dependent to the emiter your root antenna is pointing. Each contries broadcasting DVB-T TVs have a website with those informations. In worst case, http://www.dvb.org/ can be a good start...

DVB-S parameters depend on the satellite pointed by your dish antenna. This website http://www.lyngsat.com/ can be really useful!

Files in this project :

StartUp.cs : A StartUp class with the application main entry point. Windows XP enhanced UI is activated here.

MainForm.cs : The application main form. Nothing DirectShow specific, just plain .Net stuff. This form use the BDAGraphBuilder class to drive the DirectShow graph and do the tuning throw a class implementing ITuningSelector.

ITuningSelector.cs : An interface to vitualize the tuning process. In this application, tuning parameters are asked to the user but this task could easily be replaced by data coming from a DB, an XML file or whatever.

DVBSTuning.cs & DVBTTuning.cs : Two Windows Forms designed to ask to the user the tuning parameters like the Carrier Frequency, etc. They also implemente the ITuningSelector interface (see later). With this architecture, it's easy to add support to ATSC or DVB-C networks.

BDAGraphBuilder.cs : This is here the real Magic is! This class build the BDA graph and tune it with Tune Requests provided by DVBSTuning or DVBTTuning forms.

