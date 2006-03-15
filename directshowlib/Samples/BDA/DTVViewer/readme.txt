DTVViewer - A basic Digital TV (BDA) viewer sample

This sample demonstrates how to use BDA to display Digital TV into a Windows Form.

This application asks the user for tuning parameters. You have to know most of them to have a working Tune Request.

DVB-T parameters are directly dependent on the emiter to which your root antenna is pointing. Each country's broadcasting DVB-T TVs have a website with that informations. Worst case, http://www.dvb.org/ can be a good start...

DVB-S parameters depend on the satellite pointed to by your dish antenna. This website http://www.lyngsat.com/ can be really useful!

Files in this project :

StartUp.cs : A StartUp class with the application main entry point. Windows XP enhanced UI is activated here.

MainForm.cs : The application main form. Nothing DirectShow specific, just plain .Net stuff. This form uses the BDAGraphBuilder class to drive the DirectShow graph and do the tuning through a class implementing ITuningSelector.

ITuningSelector.cs : An interface to vitualize the tuning process. In this application, tuning parameters are requested from the user but this task could easily be replaced by data coming from a DB, an XML file or whatever.

DVBSTuning.cs & DVBTTuning.cs : Two Windows Forms designed to ask the user the tuning parameters like the Carrier Frequency, etc. They also implement the ITuningSelector interface (see later). With this architecture, it's easy to add support to ATSC or DVB-C networks.

BDAGraphBuilder.cs : This is here the real Magic is! This class builds the BDA graph and tunes it with Tune Requests provided by DVBSTuning or DVBTTuning forms.

