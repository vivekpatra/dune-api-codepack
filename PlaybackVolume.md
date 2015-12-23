# playback\_volume #

## Summary ##

`playback_volume` indicates the playback volume percentage.

## Input ##

| **Parameter** | **Value(s)**         |
|:--------------|:---------------------|
| cmd           | set\_playback\_state |
| volume        | {n}                  |
| [[timeout](timeout.md)] | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="playback_volume" value="{n}"/>
  ...
</command_result>
```

## Values ##

|           | **Volume** |
|:----------|:-----------|
| **Minimum** | `0`        |
| **Maximum** | `150`      |
| **Default** | `100`      |

## Notes ##

  * A value up to 100 can be set using [set\_playback\_state](SetPlaybackStateCommand.md). Values up to 150 can be reached by repeatedly sending volume up.

  * Changing `playback_volume` resets [playback\_mute](PlaybackMute.md).