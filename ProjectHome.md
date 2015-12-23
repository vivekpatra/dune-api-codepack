# Introduction #

Welcome to the official project page of the Dune API codepack.

Dune API codepack is a code library designed to wrap and extend the IP Control Protocol exposed by IP control-enabled Dune HD player models based on Sigma Designs chips 864x/865x/867x.



For more information on where to purchase a Dune HD device, visit http://dune-hd.com.

# Who should use this library? #

Originally developed for personal use, any .NET developer can now benefit from my work. Everything is provided completely free of charge (see licensing for more information).

All code is written in Visual Basic.NET, and can be compiled into a single DLL. The Visual Studio project currently targets the .NET 4 client profile, but an upgrade to .NET 4.5 is just a matter of time now. This makes Visual Studio 2012 a minimum requirement.

# API #

The IP Control protocol is an always-on HTTP service which operates on your Dune player (port 80).

Official API documentation: http://dune-hd.com/firmware/ip_control/dune_ip_control_overview.txt.

The overview is largely based on version 1, but contain notes about features introduced in version 2 and version 3.

Alternative API documentation: http://code.google.com/p/dune-api-codepack/wiki/ApiReference

# Code library #
Most of the code is self-documenting, so you can figure out how to use it while you write code. I always use descriptive names according to Microsoft's naming conventions and set IntelliSense tooltips where appropriate.

Click [here](http://code.google.com/p/dune-api-codepack/wiki/HelloWorld) for a code quickstart.
You can also find MSDN-style documentation [here](http://dl.dropbox.com/u/14454764/DuneAPICodePack/Help/Index.html).

To access the code repository, you must first install [Git](http://git-scm.com/), and optionally a graphical user interface for Git (e.g. [Git Extensions](http://code.google.com/p/gitextensions/)). Explaining how to use Git is not within the scope of this page, but is a fairly straightforward process.

Pull code from the [master](http://code.google.com/p/dune-api-codepack/source/browse/?name=master) branch for production releases (or download the archive which I've provided on the Downloads page). For the latest developments you can pull from the [develop](http://code.google.com/p/dune-api-codepack/source/browse/?name=develop) branch.
The develop branch generally contains code that's deemed unstable, although no guarantees are made for code on the master branch.

# Versioning #

Dune player firmware versions are usually simple date representations (yymmdd\_hhmm). The IP control protocol version is increased by 1 for every public release. This code library follows a similar versioning strategy.

The latest tested and supported player firmware is `121018_0846`.

The latest tested and supported API version is `3`.

The latest code library version is `3.2012.0807`.


All code is fully backwards compatible, but there is currently no mechanism in place to check whether a specific command is supported by the target Dune device. For example: changing the playback volume is not possible in API version 1, but this is ignored on the client side. Instead, the request will go through and the device will return a command\_error which then triggers a code exception. I might change that in the future, as there are certain circumstances under which a command error is not triggered, possibly causing very subtle bugs.

| **API version**| **Minimum firmware version**|
|:---------------|:----------------------------|
|1               |110127\_2105\_beta           |
|2               |111122\_0159\_beta           |
|3               |120531\_2200\_beta           |
|4               |TBA                          |


# License #
This software is licensed under the LGPL, meaning that it's free for both private and commercial use.