# standby #
## Summary ##

The standby command sets the player to standby mode. If the player is in playback mode, playback is interrupted.


## Syntax ##

| **Parameter**         | **Value(s)** | **Example**   | **API Version(s)** |
|:----------------------|:-------------|:--------------|:-------------------|
| [cmd](Cmd.md)           | standby      | cmd=standby   | ≥ 1                |
| [[timeout](Timeout.md)] | {n}          | timeout=20    | ≥ 1                |

## Examples ##

Set the device to standby mode:

`http://dune/cgi-bin/do?cmd=standby`