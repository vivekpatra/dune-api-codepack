# Introduction #
Quite frankly, the IP control overview ([link](http://dune-hd.com/firmware/ip_control/dune_ip_control_overview.txt)) is not that very useful. It lacks a lot of information about supported commands, command results and command parameters. Which is why I've created this!

This page is meant to be complete reference, covering all aspects of the API. I hope you enjoy reading it as much as I enjoyed making it. But if anything, at least let it be useful.

The page layout is designed to show all the fundamentals, with links to more detailed information where appropriate. This allows you to very quickly find the information that you need while writing your own code library.


---


# IP Control Protocol #

The IP control protocol (hereafter known as _the API_) is built on a push technology called [long polling](http://en.wikipedia.org/wiki/Push_technology#Long_polling). It is not real push code because a client request is always required in order for the server to return data. This is why the API has a status command that does nothing except return the player status.

You will notice that I often use an abstracted version of a parameter name/value, such as `{n}` instead of a real number. These are of course not recognized by the API, but emphasize the command semantics.

## Definitions ##

  * **API** (Application programming interface): the set of utilities that together form the IP control protocol.
  * **Commands**: HTTP requests which target a certain API function, causing the device to perform one or more actions.
  * **Command results**: an XML-formatted document that typically contains a status code, as well as information pertaining to the current device status.


---


# Commands #

[Go to main article](Commands.md)


---


# Command Results #

[Go to main article](CommandResults.md)


---


# Final Notes #

In code, you may want to get status updates at regular intervals. If you do this, do not set the interval too low to avoid crippling the device. Symptoms include choppy playback and slow menu animations.

This documentation uses simple HTTP GET requests  for simplicity in every example, but this is "abusing" the HTTP protocol. The proper way is to use GET requests for commands that retrieve information (i.e. `cmd=status` and `cmd=get_text`). Use POST requests for commands that (can) alter the player or playback state (i.e. every other command).