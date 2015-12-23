# black\_screen #
## Summary ##

The black\_screen command sets the player to black screen mode. If the player is in playback mode, playback is interrupted.

## Syntax ##

| **Parameter**         | **Value(s)**   | **Example**        | **API Version(s)** |
|:----------------------|:---------------|:-------------------|:-------------------|
| [cmd](Cmd.md)           | black\_screen  | cmd=black\_screen  | ≥ 1                |
| [[timeout](Timeout.md)] | {n}            | timeout=20         | ≥ 1                |

## Examples ##

Set the device to black screen mode:

`http://dune/cgi-bin/do?cmd=black_screen`