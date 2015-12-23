# main\_screen #
## Summary ##

The main\_screen command sets the player to the start-up screen. If the player is in playback mode, playback is interrupted.

## Syntax ##

| **Parameter**         | **Value(s)**  | **Example**       | **API Version(s)** |
|:----------------------|:--------------|:------------------|:-------------------|
| [cmd](Cmd.md)           | main\_screen  | cmd=main\_screen  | ≥ 1                |
| [[timeout](Timeout.md)] | {n}           | timeout=20        | ≥ 1                |

## Examples ##

Set the device to the main screen:

`http://dune/cgi-bin/do?cmd=main_screen`