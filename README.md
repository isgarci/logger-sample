# sample-logger

## Table of contents
* [General info](#general-info)
* [Projects](#projects)

## General info
This project is a draft for a logger module, customizable, and with option for Flush and NonFlush logging.
	
## Projects
Logger : Contains the model definition classes and basic interfaces.<br/>
CustomLogger: Contains classes with a customized implementation of the logger, based in this case in the Serilog framework.<br/>
Base client: Console client to run logging with flush and non flush, using default parameters.<br/>
Test: Contain unit tests to run logging with flush and non flush, using testing parameters. <br/>
