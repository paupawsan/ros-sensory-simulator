# Sensors Simulator Project

## Prerequisites
### Install .NET SDK
 * How to install tutorial page
   > https://www.microsoft.com/net/learn/get-started-with-dotnet-tutorial

 * Or in Ubuntu 18.04 we can directly install as follows
  
        1. wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb
        2. sudo dpkg -i packages-microsoft-prod.deb
        3. sudo apt-get install apt-transport-https
        4. sudo apt-get update
        5. sudo apt-get install dotnet-sdk-2.1


## How to build steps
```
// Clone the project.
// a private key needed to access private git repository
// please download it here https://www.dropbox.com/s/ryhx21uqr94ffcs/idrsa_pass_is_robotics?dl=0
// and put in ~/.ssh directory.
// please do chmod 400 for the key
// use this robotics passphrase for the key password.
1. user$ git clone git@github.com:paupawsan/ros-sensory-simulator.git
Or just download the project here


// Change directory to ros-sensory-simulator
2. user$ cd ros-sensory-simulator

// Build the project
3. PC:ros-sensory-simulator user$ dotnet build SensorsSimulator.sln

// Create sensors folder
4. PC:ros-sensory-simulator user$ mkdir -p ROSBoard/bin/Debug/netcoreapp2.0/sensors

// For Steps 5 and 6 below can be executed later after the project run or now.
// Since the system support plug n play of new sensors virtually
// Just need to copy the dll of compiled sensors into ROSBoard/bin/Debug/netcoreapp2.0/sensors directory
// Note: PortID will be incremented and it will be assigned for each newlly detected sensor in runtime.
5. PC:ros-sensory-simulator user$ cp AnalogOneSensor/bin/Debug/netcoreapp2.0/AnalogOneSensor.dll ROSBoard/bin/Debug/netcoreapp2.0/sensors/
6. PC:ros-sensory-simulator user$ cp DigitalSensor/bin/Debug/netcoreapp2.0/DigitalSensor.dll ROSBoard/bin/Debug/netcoreapp2.0/sensors/

// Run the project
7. PC:ros-sensory-simulator user$ dotnet run --project ROSBoard/
``` 

# Software Design 
## Overview
The system is simple design to virtually mimic the sensors attachment on SystemBoard.
As sensor can be attached into the board and get clock signal from the SystemBoard for data process synchronization.
 