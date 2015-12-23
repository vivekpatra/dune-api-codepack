# status #
## Summary ##

The `status` command retrieves the device's state without affecting it in any way.

## Syntax ##

| **Parameter**         | **Value(s)** | **Example**  | **API Version(s)** |
|:----------------------|:-------------|:-------------|:-------------------|
| [cmd](Cmd.md)           | status       | cmd=status   | ≥ 1                |
| [[timeout](Timeout.md)] | {n}          | timeout=20   | ≥ 1                |

In its simplest form, the output looks like this:

```
<?xml version="1.0" ?>
<command_result>
	<param name="protocol_version" value="3"/>
	<param name="player_state" value="navigator"/>
</command_result>
```

Depending on the player/playback state, the returned [command results](CommandResults.md) may include additional parameters. Each one of them will be thoroughly discussed on its own page.

## Examples ##

Get the current player status:

`http://dune/cgi-bin/do?cmd=status`