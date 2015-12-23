# playback\_state #
## Summary ##

`playback_state` indicates the current playback state.

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="playback_state" value="{value}"/>
  ...
</command_result>
```

## Values ##

| **Playback State** |
|:-------------------|
| playing            |
| initializing       |
| paused             |
| seeking            |
| stopped            |
| deinitializing     |
| buffering          |
| finished           |

## Notes ##

  * There are no values for "rewinding" or "fast forwarding", even though they are arguably different playback states.

  * It is almost impossible to observe the "finished" playback state as the current playback state. Typically, the playback state changes to "stopped" even before a status command can reach the server.